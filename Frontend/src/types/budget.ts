import { User } from "./user";

export type Budget = {
  Id: number;
  Balance: number;
  Admin: User;
  Members: User[];
  BalanceChanges: any;
};
