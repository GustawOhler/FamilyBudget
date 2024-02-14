import "@/styles/customBootstrap.scss";
import "@/styles/common.scss";
import "bootstrap-icons/font/bootstrap-icons.css";

import { useEffect, useState } from "react";

import type { AppProps } from "next/app";
import AuthRequiredContent from "@/components/AuthRequiredContent";
import ErrorBoundary from "@/components/ErrorBoundary";
import GoBackArrow from "@/components/BackArrow";
import { Roboto } from "next/font/google";
import UserOverlay from "@/components/UserOverlay";
import { useRouter } from "next/router";

const font = Roboto({ subsets: ["latin-ext"], weight: ["300", "400", "500", "700"] });

export default function App({ Component, pageProps }: AppProps) {
  // const router = useRouter();
  // const [history, setHistory] = useState([router.asPath]);

  // useEffect(() => {
  //   const handleRouteChange = (url: string) => {
  //     setHistory((history) => [...history, url]);
  //   };

  //   router.events.on("routeChangeComplete", handleRouteChange);
  //   return () => {
  //     router.events.off("routeChangeComplete", handleRouteChange);
  //   };
  // }, [router.events]);

  return (
    <main className={`${font.className} vh-100 p-2 p-xl-4`}>
      <GoBackArrow />
      <UserOverlay />
      <ErrorBoundary>
        <AuthRequiredContent>
          <Component {...pageProps} />
        </AuthRequiredContent>
      </ErrorBoundary>
    </main>
  );
}
