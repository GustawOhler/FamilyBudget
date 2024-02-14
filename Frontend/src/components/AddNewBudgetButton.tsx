import { FunctionComponent } from "react";
import router from "next/router";

interface AddNewBudgetButtonProps {}

const AddNewBudgetButton: FunctionComponent<AddNewBudgetButtonProps> = () => {
  return (
    <button type="button" className="btn btn-secondary btn-lg py-0" onClick={() => router.push("/budgets/new")}>
      <i className="bi bi-plus fs-2"></i>
    </button>
  );
};

export default AddNewBudgetButton;
