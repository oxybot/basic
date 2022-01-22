import EntityFieldInput from "./EntityFieldInput";
import EntityFieldLabel from "./EntityFieldLabel";

export default function EntityFieldEdit({ field, entity, onChange }) {
  return (
    <div className="mb-3">
      <EntityFieldLabel field={field} />
      <EntityFieldInput field={field} value={entity[field.name] || ""} onChange={onChange} />
    </div>
  );
}
