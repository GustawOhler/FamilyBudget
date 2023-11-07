import { FunctionComponent } from "react";
import { Spinner } from "react-bootstrap";

interface LoadingProps {
  isVisible: boolean;
}

const LoadingPopup: FunctionComponent<LoadingProps> = ({ isVisible }) => {
  return (
    <div
      className={`${
        isVisible ? "d-flex" : "d-none"
      } z-2 vh-100 vw-100 position-absolute top-0 start-0 flex-row justify-content-center align-items-center bg-black bg-opacity-50`}
    >
      <div className="p-5 d-flex flex-column align-items-center bg-third text-white rounded fs-3">
        <Spinner animation="border" />
        <span>Ładowanie...</span>
      </div>
    </div>
  );
};

export default LoadingPopup;
