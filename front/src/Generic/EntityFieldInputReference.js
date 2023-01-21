import clsx from "clsx";
import { useApiFetch } from "../api";
import Select from "./Select";

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
        className={clsx({ "is-invalid": hasErrors })}
        placeholder={field.placeholder}
        options={elements}
        defaultValue={elements.filter((s) => s.value === value)}
        onChange={(s) => onChange({ target: { name: field.name, value: s && s.value } })}
      />
    )
  );
}
