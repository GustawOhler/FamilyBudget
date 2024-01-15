import { Dropdown, ListGroup } from "react-bootstrap";

import { AuthContext } from "./AuthRequiredContent";
import { USER_DETAILS } from "@/common/cookieKeys";
import { useContext } from "react";
import { useCookies } from "react-cookie";
import { useRouter } from "next/router";

export default function UserOverlay() {
  const router = useRouter();
  const currentUser = useContext(AuthContext);
  const [cookies, setCookie, removeCookie] = useCookies([USER_DETAILS]);

  const handleLogout = () => {
    removeCookie(USER_DETAILS);
    router.push("/login");
  };

  return (
    <Dropdown className="position-fixed top-0 end-0 m-4 z-2">
      <Dropdown.Toggle variant="transparent" className="fs-3 lh-1" id="user-panel-toggle">
        <i className="bi bi-person-circle fs-1"></i>
      </Dropdown.Toggle>
      <Dropdown.Menu className="bg-white bg-opacity-50">
        <Dropdown.Item disabled={true} className="text-primary">
          {/* {currentUser?.userDetails} */}
          Test user
        </Dropdown.Item>
        <Dropdown.Divider></Dropdown.Divider>
        <Dropdown.Item onClick={handleLogout}>Wyloguj się</Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );
}
