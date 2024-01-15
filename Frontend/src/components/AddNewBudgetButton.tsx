import { FunctionComponent } from "react";
import router from "next/router";

interface AddNewBudgetButtonProps {}

const AddNewBudgetButton: FunctionComponent<AddNewBudgetButtonProps> = () => {
  return (
    <div className="col d-flex d-flex flex-column align-items-center py-2">
      <button type="button" className="btn btn-secondary btn-lg w-25 py-0" onClick={() => router.push("/budgets/new")}>
        <i className="bi bi-plus fs-2"></i>
      </button>
    </div>
  );
};

export default AddNewBudgetButton;
