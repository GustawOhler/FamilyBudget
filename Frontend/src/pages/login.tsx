import { useContext, useEffect, useState } from "react";

import { AuthContext } from "@/components/AuthRequiredContent";
import Head from "next/head";
import LoadingPopup from "@/components/LoadingPopup";
import { USER_DETAILS } from "@/common/cookieKeys";
import { logInApp } from "@/APIConnection/ApiConnector";
import { useCookies } from "react-cookie";
import { useRouter } from "next/router";

export default function Login() {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(true);
  const [isShowErrorPopup, setShowErrorPopup] = useState(false);
  const [errorPopupMessage, setErrorPopupMessage] = useState<undefined | string>(undefined);
  const router = useRouter();
  const { userDetails } = useContext(AuthContext);
  const [cookies, setCookie] = useCookies([USER_DETAILS]);

  useEffect(() => {
    if (userDetails) {
      router.push("/");
    }
    setLoading(false);
  }, [router, userDetails]);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setLoading(true);
    try {
      let userDetails = await logInApp(login, password);
      setCookie(USER_DETAILS, userDetails);
      router.push("/");
    } catch (error: unknown) {
      console.log(error);
    }
    setLoading(false);
  }

  return (
    <>
      <Head>
        <title>Zaloguj się</title>
        <meta name="description" content="Logowanie do Family Budget" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container w-50 my-5 py-3 d-flex flex-column justify-content-start align-items-center transparent-pane rounded">
        <div className="row w-100 my-3">
          <div className="col d-flex flex-column justify-content-center align-items-center">
            <h2>Zaloguj się</h2>
          </div>
        </div>
        <form className="w-100" onSubmit={onSubmit}>
          <div className="mb-3 row">
            <div className="col d-flex flex-column align-items-center">
              <div className="form-group">
                <label htmlFor="login">Login</label>
                <input
                  type="text"
                  className="form-control"
                  id="login"
                  value={login}
                  onChange={(e) => setLogin(e.target.value)}
                />
              </div>
            </div>
          </div>
          <div className="mb-3 row">
            <div className="col d-flex flex-column align-items-center">
              <div className="form-group">
                <label htmlFor="password">Hasło</label>
                <input
                  type="password"
                  className="form-control"
                  id="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
            </div>
          </div>
          <div className="mb-3 row">
            <div className="col d-flex flex-column align-items-center">
              <button type="submit" className="btn btn-primary">
                Zaloguj się
              </button>
            </div>
          </div>
        </form>
        <LoadingPopup isVisible={loading} />
        {/* 
        <ErrorPopup
          isShow={isShowErrorPopup}
          onCloseModal={() => {
            setShowErrorPopup(false);
          }}
          message={errorPopupMessage}
          asModal={true}
        /> */}
      </main>
    </>
  );
}
