import { Budget } from "@/types/budget";
import { Category } from "@/types/category";
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
    balanceChanges: [],
    Id: 1,
    Members: [GetUserMock()],
    Name: "Very important Budget",
  };
}

export async function logInApp(username: string, password: string): Promise<UserDetails> {
  const rawResponse = await fetch(BASE_URL + "/api/authentication/login", {
    method: "POST",
    mode: "cors",
    body: JSON.stringify({
      username,
      password,
    }),
    headers: {
      "Content-Type": "application/json",
    },
  });
  const jsonResponse: UserDetails = await rawResponse.json();
  return jsonResponse;
}

export async function getBudgetsForUser(
  userId: number,
  token: string,
  pageNumber: number,
  pageSize: number = 20,
  name?: string
): Promise<Budget[]> {
  let queryObject: Record<string,string> = {
    pageNumber: pageNumber.toString(),
    pageSize: pageSize.toString(),
  };
  if (name) {
    queryObject["name"] = name;
  }

  const rawResponse = await fetch(BASE_URL + `/api/users/${userId}/budgets?` + new URLSearchParams(queryObject), {
    method: "GET",
    mode: "cors",
    headers: {
      authorization: `Bearer ${token}`,
    },
  });
  const jsonResponse: { budgets: Budget[] } = await rawResponse.json();
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

export async function searchCategories(name: string): Promise<Category[]> {
  const jsonResponse = [
    {
      id: 2,
      name: "Testing category",
    },
  ];
  return jsonResponse;
}
