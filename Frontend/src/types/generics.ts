type OptionType = {
  value: number;
  label: string;
};

type OptionSearchFn = (input: string) => Promise<OptionType[]>;
