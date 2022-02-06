import clsx from "clsx";
import Select from "react-select";
import { useApiFetch } from "../api";

const transform = (elements) =>
  elements.map((c) => ({
    value: c.identifier,
    label: c.displayName,
  }));
export default function EntityFieldInputReference({ baseApiUrl, field, value, hasErrors, onChange }) {
  const get = { method: "GET" };
  const [loading, elements] = useApiFetch(baseApiUrl, get, [], transform);
  return (
    !loading && (
      <Select
        name={field.name}
        required={field.required}
        classNamePrefix="react-select"
        className={clsx({ "is-invalid": hasErrors })}
        placeholder={field.placeholder}
        options={elements}
        value={elements.filter((s) => s.value === value)}
        onChange={(s) => onChange({ target: { name: field.name, value: s.value } })}
      />
    )
  );
}
