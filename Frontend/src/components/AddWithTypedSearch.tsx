import { FunctionComponent, useState } from "react";

import AsyncSelect from "react-select/async";
import { SingleValue } from "react-select";
import { User } from "@/types/user";
import { searchUsers } from "@/APIConnection/ApiConnector";

interface AddButtonWithTypedSearchProps {
  searchFn: OptionSearchFn;
  addFn: (id: number) => void;
}

const AddButtonWithTypedSearch: FunctionComponent<AddButtonWithTypedSearchProps> = ({ searchFn, addFn }) => {
  const [isSearching, setSearchingState] = useState(false);

  const loadOptions = async (inputValue: string) => {
    let users = await searchFn(inputValue);
    return users;
  };

  const onChange = (newValue: SingleValue<OptionType>) => {
    if (newValue) {
      addFn(newValue.value);
      setSearchingState(false);
    }
  };

  if (isSearching) {
    return (
      <AsyncSelect
        loadOptions={loadOptions}
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
