import { BalanceChange } from "./balanceChange";
import { User } from "./user";

export type Budget = {
  Id: number;
  Balance: number;
  Admin: User;
  Members: User[];
  balanceChanges: BalanceChange[];
  Name: string;
};
