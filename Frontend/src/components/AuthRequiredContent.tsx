import { createContext, useEffect, useState } from "react";

import React from "react";
import { USER_DETAILS } from "@/common/cookieKeys";
import { useCookies } from "react-cookie";
import { useRouter } from "next/router";

const ROUTES_WITHOUT_AUTH = ["/login"];

type Props = {
  children: JSX.Element;
};

export type UserDetails = {
  id: number;
  token: string;
  expirationDate: Date;
};

export type UserContext = {
  userDetails: null | UserDetails;
};

export const AuthContext = createContext<UserContext>({ userDetails: null });

export default function AuthRequiredContent({ children }: Props) {
  const [cookies] = useCookies([USER_DETAILS]);
  const [isAuthorized, setAuthorizationState] = useState(false);
  const router = useRouter();

  useEffect(() => {
    if (USER_DETAILS in cookies || ROUTES_WITHOUT_AUTH.includes(router.pathname)) {
      setAuthorizationState(true);
    } else {
      router.push("/login");
    }
  }, [router, cookies]);

  return isAuthorized ? (
    <AuthContext.Provider value={{ userDetails: cookies[USER_DETAILS] }}>{children}</AuthContext.Provider>
  ) : (
    <></>
  );
}
