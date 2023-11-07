import { Budget } from "@/types/budget";
import { UserDetails } from "@/components/AuthRequiredContent";
import { stringify } from "querystring";

const BASE_URL = "http://127.0.0.1:5064";

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

export async function getBudgetsForUser(userId: number, token: string): Promise<Budget[]> {
  const rawResponse = await fetch(BASE_URL + `/api/users/${userId}/budgets`, {
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
  const rawResponse = await fetch(BASE_URL + `/api/budgets/${budgetId}`, {
    method: "GET",
    mode: "cors",
    headers: {
      authorization: `Bearer ${token}`,
    },
  });
  const jsonResponse: Budget = await rawResponse.json();
  return jsonResponse;
}
