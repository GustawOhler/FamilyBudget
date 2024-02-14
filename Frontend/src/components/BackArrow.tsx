import { Button } from "react-bootstrap";
import { useRouter } from "next/router";

const GoBackArrow = () => {
  const router = useRouter();
  return (
    <Button
      className="position-fixed top-0 start-0 fs-3 m-4 text-primary lh-1"
      onClick={() => {
        router.back();
      }}
      type="button"
      variant="transparent"
    >
      <i className="bi bi-arrow-left fs-1"></i>
    </Button>
  );
};

export default GoBackArrow;
