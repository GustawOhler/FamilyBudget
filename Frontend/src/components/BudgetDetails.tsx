import { FunctionComponent, useContext, useEffect, useState } from "react";
import { getBudgetDetails, getUserById, searchUsers } from "@/APIConnection/ApiConnector";

import AddButtonWithTypedSearch from "./AddWithTypedSearch";
import { AuthContext } from "./AuthRequiredContent";
import { Budget } from "@/types/budget";

interface BudgetDetailsComponentProps {
  budgetId: number;
}

const BudgetDetailsComponent: FunctionComponent<BudgetDetailsComponentProps> = ({ budgetId }) => {
  const { userDetails } = useContext(AuthContext);
  const [budget, setBudget] = useState<null | Budget>(null);

  useEffect(() => {
    const getData = async () => {
      let budget = await getBudgetDetails(budgetId, userDetails!.token);
      setBudget(budget);
    };
    if (budgetId && userDetails) {
      getData();
    }
  }, [budgetId, setBudget, userDetails]);

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
        <AddButtonWithTypedSearch
          searchFn={async (username: string) => {
            let foundUsers = await searchUsers(username);
            let idNameMap = foundUsers.map((u) => {
              return {
                Id: u.id,
                Name: u.UserName,
              };
            });
            return idNameMap;
          }}
          addFn={async (id: number) => {
            let newUser = await getUserById(id);
            let changedMembers = [...budget!.Members, newUser];
            let changedBudget: Budget = { ...(budget as Budget), Members: changedMembers };
            setBudget(changedBudget);
          }}
        />
      </div>
      <div className="row my-3 w-100">
        <div className="col d-flex flex-column align-items-start">
          <p className="mb-0 align-self-center">Last 5 incomes:</p>
          <p className="mb-0">
            01.10.2022 <span className="text-success">+100</span> Payday
          </p>
        </div>
        <div className="col d-flex flex-column align-items-center">
          <p className="mb-0">Last 5 expenses:</p>
          <p className="mb-0">
            01.10.2022 <span className="text-danger">-50</span> Shopping
          </p>
        </div>
      </div>
    </div>
  );
};

export default BudgetDetailsComponent;
