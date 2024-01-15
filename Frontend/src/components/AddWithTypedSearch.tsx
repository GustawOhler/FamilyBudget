import { FunctionComponent, useState } from "react";

import AsyncSelect from "react-select/async";
import { SingleValue } from "react-select";
import { User } from "@/types/user";
import { searchUsers } from "@/APIConnection/ApiConnector";

type IdNamePair = {
  Id: number;
  Name: string;
};

interface AddButtonWithTypedSearchProps {
  searchFn: (input: string) => Promise<IdNamePair[]>;
  addFn: (id: number) => void;
}

const AddButtonWithTypedSearch: FunctionComponent<AddButtonWithTypedSearchProps> = ({ searchFn, addFn }) => {
  const [isSearching, setSearchingState] = useState(false);

  const loadOptions = async (inputValue: string) => {
    let users = await searchUsers(inputValue);
    return users;
  };

  const onChange = (newValue: SingleValue<User>) => {
    if (newValue) {
      addFn(newValue.id);
      setSearchingState(false);
    }
  };

  if (isSearching) {
    return (
      <AsyncSelect
        loadOptions={loadOptions}
        getOptionLabel={(user) => user.UserName}
        getOptionValue={(user) => user.id.toString()}
        defaultOptions={false}
        onChange={onChange}
        className="w-100"
      />
    );
  }
  return (
    <button
      type="button"
      className="btn btn-primary btn-lg py-0"
      onClick={() => {
        setSearchingState(true);
      }}
    >
      <i className="bi bi-plus"></i>
    </button>
  );
};

export default AddButtonWithTypedSearch;
