import clsx from "clsx";

export default function EntityFieldLabel({ field }) {
  return (
    <label
      htmlFor={field.name}
      className={clsx("form-label", {
        required: field.required,
      })}
    >
      {field.displayName}
    </label>
  );
}
