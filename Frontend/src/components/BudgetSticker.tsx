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
    <div className="col-4">
      <div className="rounded bg-secondary d-flex flex-column justify-content-start align-items-center standard-box-shadow py-2">
        <h3>{name}</h3>
        <div>Admin: {admin.UserName}</div>
        <div>
          Uczestnicy:
          {members.map((member, i) => (
            <div key={i}>{member.UserName}</div>
          ))}
        </div>
        <a type="button" className="btn btn-primary rounded" href={`/budgets/${id}/details/`}>
          Zobacz szczegóły
        </a>
      </div>
    </div>
  );
};

export default BudgetSticker;
