import "@/styles/customBootstrap.scss";
import "@/styles/common.scss";
import "bootstrap-icons/font/bootstrap-icons.css";

import type { AppProps } from "next/app";
import AuthRequiredContent from "@/components/AuthRequiredContent";
import ErrorBoundary from "@/components/ErrorBoundary";
import { Roboto } from "next/font/google";

const font = Roboto({ subsets: ["latin-ext"], weight: ["300", "400", "500", "700"] });

export default function App({ Component, pageProps }: AppProps) {
  return (
    <main className={`${font.className} vh-100 p-2 p-xl-4`}>
      <ErrorBoundary>
        <AuthRequiredContent>
          <Component {...pageProps} />
        </AuthRequiredContent>
      </ErrorBoundary>
    </main>
  );
}
