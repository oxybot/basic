import { IconCurrencyEuro } from "@tabler/icons";
import dayjs from "dayjs";
import EntityFieldInputColor from "./EntityFieldInputColor";
import EntityFieldInputImage from "./EntityFieldInputImage";
import EntityFieldInputReference from "./EntityFieldInputReference";
import EntityFieldInputSchedule from "./EntityFieldInputSchedule";

export default function EntityFieldInput({ field, value, onChange }) {
  switch (field.type) {
    case "date":
      return (
        <input
          type="date"
          className="form-control"
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value ? dayjs(value).format("YYYY-MM-DD") : ""}
          onChange={onChange}
        />
      );

    case "number/hours":
      return (
        <div className="input-group">
          <input
            type="text"
            className="form-control"
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
      return <EntityFieldInputReference baseApiUrl="EventCategories" field={field} value={value} onChange={onChange} />;

    case "ref/client":
      return <EntityfieldInputReference baseApiUrl="Clients" field={field} value={value} onChange={onChange} />;

    case "ref/product":
      return <EntityfieldInputReference baseApiUrl="Products" field={field} value={value} onChange={onChange} />;

    case "ref/user":
      return <EntityFieldInputReference baseApiUrl="Users" field={field} value={value} onChange={onChange} />;

    case "ref/eventtimemapping":
      return (
        <select className="form-select" id={field.name} name={field.name} value={value} onChange={onChange}>
          <option value="Active">Active</option>
          <option value="StandardTimeOff">Standard Time-off</option>
          <option value="ExtraTimeOff">Extra Time-off</option>
        </select>
      );

    case "image":
      return <EntityFieldInputImage field={field} value={value} onChange={onChange} />;

    case "schedule":
      return <EntityFieldInputSchedule field={field} value={value} onChange={onChange} />;

    case "color":
      return <EntityFieldInputColor field={field} value={value} onChange={onChange} />;

    case "boolean":
      function handleChange(e) {
        onChange({ target: { name: field.name, value: e.target.checked } });
      }

      return (
        <div className="form-check form-switch">
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
            {field.description || "Yes"}
          </label>
        </div>
      );

    case "string":
      return (
        <input
          type="text"
          className="form-control"
          required={field.required}
          id={field.name}
          name={field.name}
          placeholder={field.placeholder}
          value={value}
          onChange={onChange}
        />
      );

    case "currency":
      return (
        <div className="input-icon">
          <input
            type="text"
            className="form-control"
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
          className="form-control"
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
