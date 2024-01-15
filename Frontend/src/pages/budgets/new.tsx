import { ChangeEvent, FunctionComponent, useContext, useEffect, useState } from "react";

import { AuthContext } from "@/components/AuthRequiredContent";
import Head from "next/head";
import LoadingPopup from "@/components/LoadingPopup";
import { useRouter } from "next/router";

interface NewBudgetPageProps {}

const NewBudgetPage: FunctionComponent<NewBudgetPageProps> = () => {
  const [budgetName, setBudgetName] = useState("");
  const [startingBalance, setStartingBalance] = useState("");
  const [loading, setLoading] = useState(false);
  const [isShowErrorPopup, setShowErrorPopup] = useState(false);
  const [errorPopupMessage, setErrorPopupMessage] = useState<undefined | string>(undefined);
  const router = useRouter();
  const { userDetails } = useContext(AuthContext);

  async function onSubmit() {
    setLoading(true);
    try {
      //Add new budget logic
      router.back();
    } catch (error: unknown) {
      console.log(error);
    }
    setLoading(false);
  }

  return (
    <>
      <Head>
        <title>Family Budget app - add new budget</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container w-25 h-100 py-xl-3 d-flex flex-column justify-content-center align-items-center overflow-auto">
        <div className="row w-100 g-3 transparent-pane rounded p-3">
          <div className="col">
            <form className="h-100 d-flex flex-column justify-content-center">
              <h3>Add new budget</h3>
              <div className="mb-3">
                <label htmlFor="budgetName" className="form-label">
                  Name
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="budgetName"
                  value={budgetName}
                  onChange={(e) => setBudgetName(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="budgetStartingBalance" className="form-label">
                  Starting balance
                </label>
                <input
                  type="number"
                  className="form-control"
                  id="budgetStartingBalance"
                  value={startingBalance}
                  onChange={(e) => setStartingBalance(e.target.value)}
                />
              </div>
              <div className="d-flex flex-row justify-content-center">
                <button className="btn btn-primary" type="button" onClick={() => onSubmit()}>
                  Submit
                </button>
              </div>
            </form>
          </div>
        </div>
        <LoadingPopup isVisible={loading} />
      </main>
    </>
  );
};

export default NewBudgetPage;
