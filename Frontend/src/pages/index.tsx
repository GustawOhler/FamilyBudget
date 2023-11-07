import { FunctionComponent, useContext, useEffect, useState } from "react";

import { AuthContext } from "@/components/AuthRequiredContent";
import { Budget } from "@/types/budget";
import BudgetSticker from "@/components/BudgetSticker";
import Head from "next/head";
import { getBudgetsForUser } from "@/APIConnection/ApiConnector";

const Index: FunctionComponent = () => {
  const { userDetails } = useContext(AuthContext);
  const [budgets, setBudgets] = useState<Budget[]>([]);

  useEffect(() => {
    const getData = async () => {
      let budgets = await getBudgetsForUser(userDetails!.id, userDetails!.token);
      setBudgets(budgets);
    };
    if (userDetails) {
      getData();
    }
  }, [userDetails]);

  return (
    <>
      <Head>
        <title>Family Budget app</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container-xl py-xl-3 d-flex flex-column justify-content-start align-items-center transparent-pane rounded h-100 overflow-auto">
        <div className="row w-100 g-3">
          {budgets.map((budget) => (
            <BudgetSticker id={budget.Id} name="Test" admin={budget.Admin} members={budget.Members} key={budget.Id} />
          ))}
        </div>
      </main>
    </>
  );
};

export default Index;
