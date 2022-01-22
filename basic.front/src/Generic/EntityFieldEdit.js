import EntityFieldInput from "./EntityFieldInput";
import EntityFieldLabel from "./EntityFieldLabel";

export default function EntityFieldEdit({ field, entity, errors, onChange }) {
  const hasErrors = errors && errors.length > 0;
  return (
    <div className="mb-3">
      <EntityFieldLabel field={field} />
      <EntityFieldInput field={field} value={entity[field.name] || ""} hasErrors={hasErrors} onChange={onChange} />
      {errors &&
        errors.map((error, index) => (
          <div key={index} className="invalid-feedback">
            {error}
          </div>
        ))}
    </div>
  );
}
