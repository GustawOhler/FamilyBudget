import { Head, Html, Main, NextScript } from "next/document";

import GoBackArrow from "@/components/BackArrow";
import UserOverlay from "@/components/UserOverlay";

export default function Document() {
  return (
    <Html lang="pl">
      <Head></Head>
      <body>
        <Main />
        <NextScript />
      </body>
    </Html>
  );
}
