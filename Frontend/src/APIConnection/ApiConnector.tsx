import { Budget } from "@/types/budget";
import { User } from "@/types/user";
import { UserDetails } from "@/components/AuthRequiredContent";
import moment from "moment";
import { stringify } from "querystring";

const BASE_URL = "http://127.0.0.1:5064";

function GetUserMock(): User {
  return {
    id: 1,
    UserName: "Testing user",
  };
}

function GetBudgetMock(): Budget {
  return {
    Admin: GetUserMock(),
    Balance: 100,
    BalanceChanges: [],
    Id: 1,
    Members: [GetUserMock()],
    Name: "Very important Budget",
  };
}

export async function logInApp(username: string, password: string): Promise<UserDetails> {
  // const rawResponse = await fetch(BASE_URL + "/api/authentication/login", {
  //   method: "POST",
  //   mode: "cors",
  //   body: JSON.stringify({
  //     username,
  //     password,
  //   }),
  //   headers: {
  //     "Content-Type": "application/json",
  //   },
  // });
  // const jsonResponse: UserDetails = await rawResponse.json();
  const jsonResponse: UserDetails = {
    expirationDate: moment().add(30, "m").toDate(),
    id: 1,
    token: "Randomstringtoken",
  };
  return jsonResponse;
}

export async function getBudgetsForUser(userId: number, token: string): Promise<Budget[]> {
  // const rawResponse = await fetch(BASE_URL + `/api/users/${userId}/budgets`, {
  //   method: "GET",
  //   mode: "cors",
  //   headers: {
  //     authorization: `Bearer ${token}`,
  //   },
  // });
  // const jsonResponse: { budgets: Budget[] } = await rawResponse.json();
  const jsonResponse = {
    budgets: [GetBudgetMock(), GetBudgetMock(), GetBudgetMock(), GetBudgetMock()],
  };
  return jsonResponse.budgets;
}

export async function getBudgetDetails(budgetId: number, token: string): Promise<Budget> {
  // const rawResponse = await fetch(BASE_URL + `/api/budgets/${budgetId}`, {
  //   method: "GET",
  //   mode: "cors",
  //   headers: {
  //     authorization: `Bearer ${token}`,
  //   },
  // });
  // const jsonResponse: Budget = await rawResponse.json();
  const jsonResponse = GetBudgetMock();
  return jsonResponse;
}

export async function getUserById(id: number): Promise<User> {
  return GetUserMock();
}

export async function searchUsers(username: string): Promise<User[]> {
  const jsonResponse = [
    GetUserMock(),
    {
      id: 2,
      UserName: "Testing user 2",
    },
  ];
  return jsonResponse;
}
