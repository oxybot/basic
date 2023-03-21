import { IconCurrencyEuro } from "@tabler/icons";
import clsx from "clsx";
import EntityFieldInputColor from "./EntityFieldInputColor";
import EntityFieldInputImage from "./EntityFieldInputImage";
import EntityFieldInputReference from "./EntityFieldInputReference";
import EntityFieldInputSchedule from "./EntityFieldInputSchedule";
import Select from "./Select";

export default function EntityFieldInput({ field, value, hasErrors, onChange }) {
  switch (field.type) {
    case "date":
      return (
        <input
          type="date"
          className={clsx("form-control", { "is-invalid": hasErrors })}
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value ?? ""}
          onChange={onChange}
        />
      );

    case "hours":
      return (
        <div className={clsx("input-group", { "is-invalid": hasErrors })}>
          <input
            type="text"
            className={clsx("form-control", { "is-invalid": hasErrors })}
            required={field.required}
            id={field.name}
            name={field.name}
            placeholder={field.placeholder}
            value={value}
            onChange={onChange}
          />
          <span className="input-group-text">hour(s)</span>
        </div>
      );

    case "ref/category":
      return (
        <EntityFieldInputReference
          baseApiUrl="EventCategories"
          field={field}
          value={value}
          hasErrors={hasErrors}
          onChange={onChange}
        />
      );

    case "ref/client":
      return (
        <EntityFieldInputReference
          baseApiUrl="Clients"
          field={field}
          value={value}
          hasErrors={hasErrors}
          onChange={onChange}
        />
      );

    case "ref/product":
      return (
        <EntityFieldInputReference
          baseApiUrl="Products"
          field={field}
          value={value}
          hasErrors={hasErrors}
          onChange={onChange}
        />
      );

    case "ref/user":
      return (
        <EntityFieldInputReference
          baseApiUrl="Users"
          field={field}
          value={value}
          hasErrors={hasErrors}
          onChange={onChange}
        />
      );

    case "ref/eventtimemapping":
      const options = [
        { value: "TimeOff", label: "Time-off" },
        { value: "Active", label: "Active" },
        { value: "Informational", label: "Informational" },
      ];
      return (
        <Select
          name={field.name}
          required={field.required}
          className={clsx({ "is-invalid": hasErrors })}
          placeholder={field.placeholder}
          options={options}
          defaultValue={options.filter((s) => s.value === value)}
          onChange={(s) => onChange({ target: { name: field.name, value: s && s.value } })}
        />
      );

    case "image":
      return <EntityFieldInputImage field={field} value={value} hasErrors={hasErrors} onChange={onChange} />;

    case "schedule":
      return <EntityFieldInputSchedule field={field} value={value} hasErrors={hasErrors} onChange={onChange} />;

    case "color":
      return <EntityFieldInputColor field={field} value={value} hasErrors={hasErrors} onChange={onChange} />;

    case "boolean":
      function handleChange(e) {
        onChange({ target: { name: field.name, value: e.target.checked } });
      }

      return (
        <div className={clsx("form-check form-switch", { "is-invalid": hasErrors })}>
          <input
            className="form-check-input"
            type="checkbox"
            role="switch"
            id={field.name}
            name={field.name}
            checked={value}
            onChange={handleChange}
          />
          <label className="form-check-label" htmlFor={field.name}>
            {field.placeholder || "Yes"}
          </label>
        </div>
      );

    case "string":
      return (
        <input
          type="text"
          className={clsx("form-control", { "is-invalid": hasErrors })}
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value}
          onChange={onChange}
        />
      );

    case "int":
      return (
        <input
          type="number"
          className={clsx("form-control", { "is-invalid": hasErrors })}
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value}
          onChange={onChange}
        />
      );

    case "password":
      return (
        <input
          type="password"
          className={clsx("form-control", { "is-invalid": hasErrors })}
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          onChange={onChange}
        />
      );

    case "currency":
      return (
        <div className="input-icon">
          <input
            type="text"
            className={clsx("form-control", { "is-invalid": hasErrors })}
            required={field.required}
            id={field.name}
            name={field.name}
            placeholder={field.placeholder}
            value={value}
            onChange={onChange}
          />
          <span className="input-icon-addon">
            <IconCurrencyEuro />
          </span>
        </div>
      );

    default:
      console.warn("Unknown field type: " + field.type);
      return (
        <input
          type="text"
          className={clsx("form-control", { "is-invalid": hasErrors })}
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value}
          onChange={onChange}
        />
      );
  }
}
