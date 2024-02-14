import { FunctionComponent, useState } from "react";
import Select, { SingleValue } from "react-select";
import { searchCategories, searchUsers } from "@/APIConnection/ApiConnector";

import AsyncSelect from "react-select/async";
import { Category } from "@/types/category";
import Head from "next/head";
import moment from "moment";

interface NewBalanceChangePageProps {}

const NewBalanceChangePage: FunctionComponent<NewBalanceChangePageProps> = () => {
  const [balanceChangeDesc, setBalanceChangeDesc] = useState("");
  const [amount, setAmount] = useState("");
  const [dateOfChange, setDateOfChange] = useState(moment());
  let category: Category;
  let balanceChangeType: number;

  const balanceChangeTypes: OptionType[] = [
    {
      label: "Expense",
      value: 1,
    },
    {
      label: "Income",
      value: 0,
    },
  ];

  const loadCategories = async (inputValue: string) => {
    // TODO
    // let users = await searchFn(inputValue);
    let users = await searchCategories(inputValue);
    let usersOptions: OptionType[] = users.map((c) => ({ label: c.name, value: c.id }));
    return usersOptions;
  };

  const onChange = (newValue: SingleValue<OptionType>) => {
    if (newValue) {
      category = { id: newValue.value, name: newValue.label };
    }
  };

  const onSubmit = () => {};

  return (
    <>
      <Head>
        <title>Family Budget app - add new balance change</title>
        <meta name="description" content="Family Budget - app for planing budget with your family and friends" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <main className="container w-25 h-100 py-xl-3 d-flex flex-column justify-content-center align-items-center overflow-auto">
        <div className="row w-100 g-3 transparent-pane rounded p-3">
          <div className="col">
            <form className="h-100 d-flex flex-column justify-content-center">
              <h3>Add new balance change</h3>
              <div className="mb-3">
                <label htmlFor="balanceChangeDesc" className="form-label">
                  Name or description
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="balanceChangeDesc"
                  value={balanceChangeDesc}
                  onChange={(e) => setBalanceChangeDesc(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="balanceChangeAmount" className="form-label">
                  Balance change amount
                </label>
                <input
                  type="number"
                  className="form-control"
                  id="balanceChangeAmount"
                  value={amount}
                  onChange={(e) => setAmount(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <Select
                  isSearchable={false}
                  options={balanceChangeTypes}
                  placeholder="Type"
                  onChange={(val) => {
                    if (val) {
                      balanceChangeType = val?.value;
                    }
                  }}
                  classNames={{
                    control: () => "bg-body text-dark",
                    container: () => "bg-body text-dark",
                    placeholder: () => "text-dark",
                    menu: () => "bg-body text-dark"
                  }}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="dateOfChange" className="form-label">
                  Date of change
                </label>
                <input
                  type="date"
                  className="form-control"
                  id="dateOfChange"
                  value={dateOfChange.format("YYYY-MM-DD")}
                  onChange={(e) => setDateOfChange(moment(e.target.value, "YYYY-MM-DD"))}
                  max={moment().format("YYYY-MM-DD")}
                />
              </div>
              <div className="mb-3">
                <AsyncSelect
                  loadOptions={loadCategories}
                  defaultOptions={false}
                  onChange={onChange}
                  isSearchable={true}
                  options={balanceChangeTypes}
                  placeholder="Category"
                  classNames={{
                    control: () => "bg-body text-dark",
                    container: () => "bg-body text-dark",
                    placeholder: () => "text-dark",
                    menu: () => "bg-body text-dark"
                  }}
                />
              </div>
              <div className="d-flex flex-row justify-content-center">
                <button className="btn btn-primary" type="button" onClick={() => onSubmit()}>
                  Submit
                </button>
              </div>
            </form>
          </div>
        </div>
        {/* <LoadingPopup isVisible={loading} /> */}
      </main>
    </>
  );
};

export default NewBalanceChangePage;
