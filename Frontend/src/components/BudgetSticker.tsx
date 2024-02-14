import { BalanceChangeType } from "@/types/balanceChange";
import { Budget } from "@/types/budget";
import { FunctionComponent } from "react";
import { User } from "@/types/user";
import moment from "moment";
import { useRouter } from "next/router";

type BudgetStickerProps = {
  budget: Budget;
};

const BudgetSticker: FunctionComponent<BudgetStickerProps> = ({ budget }) => {
  return (
    <div className="col-6">
      <div className="rounded bg-secondary d-flex flex-column justify-content-start align-items-center standard-box-shadow py-2">
        <h3>{budget.Name}</h3>
        <h5>Actual budget: 20 $</h5>
        <div>
          Members:
          {budget.Members.map((member, i) => (
            <p key={i}>{member.UserName}</p>
          ))}
        </div>
        <div className="row my-3 w-100">
          <div className="col d-flex flex-column align-items-start">
            <p className="mb-0 align-self-center">Last 5 incomes:</p>
            {budget.balanceChanges.filter((bc) => bc.Type == BalanceChangeType.Income).map((bc, i) => {
              console.log("Jestes")
              if (i < 5) {
                return (
                  <p className="mb-0" key={bc.Id}>
                    {moment(bc.DateOfChange).format("DD.MM.YYYY")} <span className="text-success">+{bc.Amount}</span> {bc.Name}
                  </p>
                );
              }
            })}
          </div>
          <div className="col d-flex flex-column align-items-center">
            <p className="mb-0">Last 5 expenses:</p>
            {budget.balanceChanges.filter((bc) => bc.Type == BalanceChangeType.Expense).map((bc, i) => {
              if (i < 5) {
                return (
                  <p className="mb-0" key={bc.Id}>
                    {moment(bc.DateOfChange).format("DD.MM.YYYY")} <span className="text-danger">{bc.Amount}</span> {bc.Name}
                  </p>
                );
              }
            })}
          </div>
        </div>
        <a
          type="button"
          className="btn btn-primary rounded standard-box-shadow"
          href={`/budgets/${budget.Id}/details/`}
        >
          Details
        </a>
      </div>
    </div>
  );
};

export default BudgetSticker;
