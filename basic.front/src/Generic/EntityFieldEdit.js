import { IconCurrencyEuro } from "@tabler/icons";
import dayjs from "dayjs";
import Select from "react-select";
import { useApiFetch } from "../api";

function EntityInputReference({ baseApiUrl, field, value, onChange }) {
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

function EntityInputImage({ field, value, onChange }) {
  function handleRemove() {
    onChange({ target: { name: field.name, value: null } });
  }

  function handleFileChange(e) {
    if (e.target.files.length === 0) {
      // Do nothing: User clicked 'cancel'
      return;
    }

    const file = e.target.files[0];
    const reader = new FileReader();
    reader.onload = function (e) {
      const data = e.target.result.replace(/^data:.+;base64,/, "");
      onChange({ target: { name: field.name, value: { mimeType: file.type, data: data } } });
    };
    reader.readAsDataURL(file);
  }

  return (
    <div className="d-flex">
      {value && <img className="avatar avatar-lg me-2" alt="" src={`data:${value.mimeType};base64,${value.data}`} />}
      {!value && <div className="avatar avatar-lg me-2"></div>}
      <div className="d-flex flex-column align-items-start justify-content-between">
        <input
          type="file"
          className="form-control"
          name={field.name}
          required={field.required}
          accept="image/*"
          onChange={handleFileChange}
        />
        {value && (
          <button type="button" className="btn btn-secondary mt-1" onClick={handleRemove}>
            Remove
          </button>
        )}
      </div>
    </div>
  );
}

export default function EntityFieldEdit({ field, value, onChange }) {
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

    case "ref/category":
      return <EntityInputReference baseApiUrl="EventCategories" field={field} value={value} onChange={onChange} />;

    case "ref/client":
      return <EntityInputReference baseApiUrl="Clients" field={field} value={value} onChange={onChange} />;

    case "ref/product":
      return <EntityInputReference baseApiUrl="Products" field={field} value={value} onChange={onChange} />;

    case "ref/user":
      return <EntityInputReference baseApiUrl="Users" field={field} value={value} onChange={onChange} />;

    case "ref/eventtimemapping":
      return (
        <select class="form-select" id={field.name} name={field.name} value={value} onChange={onChange}>
          <option value="Active">Active</option>
          <option value="StandardTimeOff">Standard Time-off</option>
          <option value="ExtraTimeOff">Extra Time-off</option>
        </select>
      );

    case "image":
      return <EntityInputImage field={field} value={value} onChange={onChange} />;

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
