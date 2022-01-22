import Select from "react-select";
import { useApiFetch } from "../api";

export default function EntityFieldInputReference({ baseApiUrl, field, value, onChange }) {
  const [loading, elements] = useApiFetch(baseApiUrl, { method: "GET" }, [], (elements) =>
    elements.map((c) => ({
      value: c.identifier,
      label: c.displayName,
    }))
  );
  return (
    !loading && (
      <Select
        name={field.name}
        required={field.required}
        classNamePrefix="react-select"
        placeholder={field.placeholder}
        options={elements}
        value={elements.filter((s) => s.value === value)}
        onChange={(s) => onChange({ target: { name: field.name, value: s.value } })}
      />
    )
  );
}
