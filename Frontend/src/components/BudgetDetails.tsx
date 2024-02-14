import { FunctionComponent, useContext, useEffect, useState } from "react";
import { getBudgetDetails, getUserById, searchUsers } from "@/APIConnection/ApiConnector";

import AddButtonWithTypedSearch from "./AddWithTypedSearch";
import { AuthContext } from "./AuthRequiredContent";
import { Budget } from "@/types/budget";
import { useRouter } from "next/router";

interface BudgetDetailsComponentProps {
  budgetId: number;
}

const BudgetDetailsComponent: FunctionComponent<BudgetDetailsComponentProps> = ({ budgetId }) => {
  const { userDetails } = useContext(AuthContext);
  const [budget, setBudget] = useState<null | Budget>(null);
  const router = useRouter();

  useEffect(() => {
    const getData = async () => {
      let budget = await getBudgetDetails(budgetId, userDetails!.token);
      setBudget(budget);
    };
    if (budgetId && userDetails) {
      getData();
    }
  }, [budgetId, setBudget, userDetails]);

  let searchUsersForChoose = async (username: string) => {
    let foundUsers = await searchUsers(username);
    let idNameMap = foundUsers.map((u) => {
      return {
        value: u.id,
        label: u.UserName,
      };
    });
    return idNameMap;
  };

  let addNewUserToBudget = async (id: number) => {
    let newUser = await getUserById(id);
    let changedMembers = [...budget!.Members, newUser];
    let changedBudget: Budget = { ...(budget as Budget), Members: changedMembers };
    setBudget(changedBudget);
  };

  return (
    <div className="rounded bg-secondary d-flex flex-column justify-content-start align-items-center standard-box-shadow py-2">
      <h3>{budget?.Name}</h3>
      <h5>Actual balance: {budget?.Balance} $</h5>
      <div className="d-flex flex-column justify-content-start align-items-start align-self-start p-2">
        <p className="mb-0">Members:</p>
        {budget?.Members.map((member, i) => (
          <p className="mb-0" key={i}>
            {member.UserName}
          </p>
        ))}
        <AddButtonWithTypedSearch searchFn={searchUsersForChoose} addFn={addNewUserToBudget} />
      </div>
      <div className="row w-100">
        <div className="col d-flex flex-column align-items-center">
          <h5 className="">Latest changes of balance</h5>
          <table className="table table-hover table-borderless table-striped">
            <thead>
              <tr>
                <th scope="col">Date</th>
                <th scope="col">Category</th>
                <th scope="col">Balance change</th>
                <th scope="col">Description</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <th scope="row">12.12.2023</th>
                <td>Payday</td>
                <td>+10000</td>
                <td>-</td>
              </tr>
              <tr>
                <th scope="row">13.12.2023</th>
                <td>Monthly bills</td>
                <td>-1000</td>
                <td>Electricity</td>
              </tr>
              <tr>
                <th scope="row">20.12.2023</th>
                <td>Shopping</td>
                <td>-9000</td>
                <td>Prada presents</td>
              </tr>
              <tr>
                <td colSpan={12} className="text-center">
                  <button type="button" className="btn btn-primary w-25 py-0" onClick={() => {router.push(`/budgets/${budgetId}/balance-change/new`)}}>
                    <i className="bi bi-plus fs-4"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default BudgetDetailsComponent;
