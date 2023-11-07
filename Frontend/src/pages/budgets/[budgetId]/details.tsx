import { useContext, useEffect, useState } from "react";

import { AuthContext } from "@/components/AuthRequiredContent";
import { Budget } from "@/types/budget";
import Head from "next/head";
import LoadingPopup from "@/components/LoadingPopup";
import { getBudgetDetails } from "@/APIConnection/ApiConnector";
import { useRouter } from "next/router";

const BudgetDetails = () => {
  const router = useRouter();
  const { userDetails } = useContext(AuthContext);
  const { budgetId } = router.query;
  const [budget, setBudget] = useState<null | Budget>(null);
  const [isLoading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    const getData = async () => {
      let budget = await getBudgetDetails(parseInt(budgetId as string), userDetails!.token);
      setBudget(budget);
    };
    if (budgetId && userDetails) {
      getData();
    }
  }, [budgetId, setBudget, userDetails]);

  return (
    <>
      <Head>
        <title>Family Budget app</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container-xl py-xl-3 d-flex flex-column justify-content-start align-items-center transparent-pane rounded h-100 overflow-auto">
        <div className="row w-100 g-3">
          <div className="col">{budget && <p>{budget.Balance}</p>}</div>
        </div>
        <LoadingPopup isVisible={isLoading} />
      </main>
    </>
  );
};

export default BudgetDetails;
