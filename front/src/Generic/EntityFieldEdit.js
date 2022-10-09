import EntityFieldInput from "./EntityFieldInput";
import EntityFieldLabel from "./EntityFieldLabel";

export default function EntityFieldEdit({ field, entity, errors, onChange }) {
  const hasErrors = errors && errors.length > 0;
  const value = entity[field.name] === undefined || entity[field.name] === null ? "" : entity[field.name];

  return (
    <div className="mb-3">
      <EntityFieldLabel field={field} />
      <EntityFieldInput field={field} value={value} hasErrors={hasErrors} onChange={onChange} />
      {errors &&
        errors.map((error, index) => (
          <div key={index} className="invalid-feedback">
            {error}
          </div>
        ))}
      {field.description && <div className="form-text">{field.description}</div>}
    </div>
  );
}
