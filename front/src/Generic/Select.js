import clsx from "clsx";
import Select, { components } from "react-select";

// Override of the standard Option component from react-select
// Supports an optional description
function Option(props) {
  let updated = { ...props };
  if (props.data.description) {
    updated.children = (
      <>
        {props.data.label}
        <br />
        <span className="text-muted">{props.data.description}</span>
      </>
    );
  }

  return <components.Option {...updated} />;
}

export default function MySelect(props) {
  return (
    <Select
      {...props}
      components={{ Option }}
      classNames={{
        option: ({ isFocused, isSelected }) =>
          clsx("p-2", isFocused && "bg-primary theme-dark", isSelected && !isFocused && "bg-primary-50"),
      }}
      styles={{
        option: (baseStyles, state) => ({}),
      }}
      isClearable={true}
      isSearchable={true}
      classNamePrefix="react-select"
    />
  );
}
