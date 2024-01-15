import { FunctionComponent } from "react";
import { User } from "@/types/user";
import { useRouter } from "next/router";

type BudgetStickerProps = {
  id: number;
  name: string;
  admin: User;
  members: Array<User>;
};

const BudgetSticker: FunctionComponent<BudgetStickerProps> = ({ id, name, admin, members }) => {
  return (
    <div className="col-6">
      <div className="rounded bg-secondary d-flex flex-column justify-content-start align-items-center standard-box-shadow py-2">
        <h3>{name}</h3>
        <h5>Actual budget: 20 $</h5>
        <div>
          Members:
          {members.map((member, i) => (
            <p key={i}>{member.UserName}</p>
          ))}
        </div>
        <div className="row my-3 w-100">
          <div className="col d-flex flex-column align-items-start">
            <p className="mb-0 align-self-center">Last 5 incomes:</p>
            <p className="mb-0">
              01.10.2022 <span className="text-success">+100</span> Payday
            </p>
          </div>
          <div className="col d-flex flex-column align-items-center">
            <p className="mb-0">Last 5 expenses:</p>
            <p className="mb-0">
              01.10.2022 <span className="text-danger">-50</span> Shopping
            </p>
          </div>
        </div>
        <a type="button" className="btn btn-primary rounded standard-box-shadow" href={`/budgets/${id}/details/`}>
          Details
        </a>
      </div>
    </div>
  );
};

export default BudgetSticker;
