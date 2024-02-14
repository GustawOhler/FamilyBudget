import { Category } from "./category";

export enum BalanceChangeType {
  Income = 0,
  Expense = 1,
}

export type BalanceChange = {
  Id: number;
  Name?: string;
  Amount: number;
  DateOfChange: Date;
  Type: BalanceChangeType;
  Category: Category;
};
