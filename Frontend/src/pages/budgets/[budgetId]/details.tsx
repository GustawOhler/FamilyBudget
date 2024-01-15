import { useContext, useEffect, useState } from "react";

import { AuthContext } from "@/components/AuthRequiredContent";
import { Budget } from "@/types/budget";
import BudgetDetailsComponent from "@/components/BudgetDetails";
import Head from "next/head";
import LoadingPopup from "@/components/LoadingPopup";
import { getBudgetDetails } from "@/APIConnection/ApiConnector";
import { useRouter } from "next/router";

const BudgetDetailsPage = () => {
  const router = useRouter();
  const { budgetId } = router.query;
  const [isLoading, setLoading] = useState<boolean>(false);

  return (
    <>
      <Head>
        <title>Family Budget app</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container-xl py-xl-3 d-flex flex-column justify-content-start align-items-center transparent-pane rounded h-100 overflow-auto">
        <div className="row w-100 g-3">
          <div className="col">
            <BudgetDetailsComponent budgetId={parseInt(budgetId as string)} />
          </div>
        </div>
        <LoadingPopup isVisible={isLoading} />
      </main>
    </>
  );
};

export default BudgetDetailsPage;
