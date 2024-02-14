import { FunctionComponent, useContext, useEffect, useState } from "react";

import AddNewBudgetButton from "@/components/AddNewBudgetButton";
import { AuthContext } from "@/components/AuthRequiredContent";
import { Budget } from "@/types/budget";
import BudgetSticker from "@/components/BudgetSticker";
import GoBackArrow from "@/components/BackArrow";
import Head from "next/head";
import UserOverlay from "@/components/UserOverlay";
import { getBudgetsForUser } from "@/APIConnection/ApiConnector";

const Index: FunctionComponent = () => {
  const { userDetails } = useContext(AuthContext);
  const [budgets, setBudgets] = useState<Budget[]>([]);
  const [pageNumber, setPageNumber] = useState(0);
  const budgetsOnPage = 8;

  useEffect(() => {
    const getData = async () => {
      let budgets = await getBudgetsForUser(userDetails!.id, userDetails!.token, pageNumber, budgetsOnPage);
      setBudgets(budgets);
    };
    if (userDetails) {
      getData();
    }
  }, [userDetails, pageNumber]);

  return (
    <>
      <Head>
        <title>Family Budget app</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <div className="container-xl py-xl-3 d-flex flex-column justify-content-start align-items-center transparent-pane rounded h-100 overflow-auto">
        <div className="row w-100 g-3">
          {budgets.map((budget) => (
            <BudgetSticker budget={budget} key={budget.Id} />
          ))}
        </div>
        <div className="row w-100 d-flex flex-row justify-content-center py-2">
          {/* <div className="col d-flex flex-column align-items-center py-2"> */}
          <div className="col-auto d-flex flex-column justify-content-center">
            <button
              type="button"
              className={`btn btn-secondary btn-lg py-0 ${pageNumber <= 0 ? "disabled" : ""}`}
              onClick={() => setPageNumber(pageNumber - 1)}
            >
              <i className="bi bi-arrow-left" />
            </button>
          </div>
          <div className="col-auto">
            <AddNewBudgetButton />
          </div>
          <div className="col-auto d-flex flex-column justify-content-center">
            <button
              type="button"
              className="btn btn-secondary btn-lg py-0"
              onClick={() => setPageNumber(pageNumber + 1)}
            >
              <i className="bi bi-arrow-right" />
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default Index;
