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

function EntityInputSchedule({ field, value = [], onChange }) {
  const complex = Array.isArray(value) && value.length > 7;
  const prefix = field.name;
  const days = [0, 1, 2, 3, 4, 5, 6];

  function handleCheck(e) {
    const max = e.target.checked ? 14 : 7;
    if (value.length < max) {
      let updated = [...value];
      updated.push(0, 0, 0, 0, 0, 0, 0);
      onChange({ target: { name: field.name, value: updated } });
    } else {
      let updated = value.slice(0, 7);
      onChange({ target: { name: field.name, value: updated } });
    }
  }

  function handleChange(e, d) {
    let updated = [...value];
    updated[d] = Number(e.target.value);
    onChange({ target: { name: field.name, value: updated } });
  }

  return (
    <>
      <div className="form-check form-switch">
        <input
          className="form-check-input"
          type="checkbox"
          role="switch"
          id={prefix + "-complex"}
          checked={complex}
          onChange={handleCheck}
        />
        <label className="form-check-label" htmlFor={prefix + "-complex"}>
          Odd/Even weeks mapping
        </label>
      </div>
      <div className="schedule">
        <div className="row g-0">
          {days.map((d) => {
            const dayLabel = dayjs().day(d).format("dddd").toLowerCase();
            const dayShort = dayjs().day(d).format("ddd");

            return (
              <div key={d} className="col">
                <label htmlFor={prefix + "-" + dayLabel} className="form-label text-center mb-1">
                  {dayShort}
                </label>
                <input
                  type="text"
                  className="form-control text-center"
                  id={prefix + "-" + dayLabel}
                  value={value[d] || 0}
                  onChange={(e) => handleChange(e, d)}
                />
              </div>
            );
          })}
        </div>
        {complex && (
          <div className="row mt-1 g-0">
            {days.map((d) => {
              const dayLabel = dayjs().day(d).format("dddd").toLowerCase();

              return (
                <div key={d} className="col">
                  <input
                    type="text"
                    className="form-control text-center"
                    id={prefix + "-" + dayLabel}
                    value={value[7 + d] || "0"}
                    onChange={(e) => handleChange(e, 7 + d)}
                  />
                </div>
              );
            })}
          </div>
        )}
      </div>
    </>
  );
}

const colors = [
  "blue",
  "azure",
  "indigo",
  "purple",
  "pink",
  "red",
  "orange",
  "yellow",
  "lime",
  "green",
  "teal",
  "cyan",
];
function EntityInputColor({ field, value, onChange }) {
  return (
    <div className="row g-2">
      {colors.map((color) => (
        <div key={color} className="col-auto">
          <label className="form-colorinput">
            <input
              name={field.name}
              type="radio"
              value={"bg-" + color}
              className="form-colorinput-input"
              checked={"bg-" + color === value}
              onChange={onChange}
            />
            <span className={"form-colorinput-color bg-" + color} title={color}></span>
          </label>
        </div>
      ))}
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
      return <EntityInputReference baseApiUrl="EventCategories" field={field} value={value} onChange={onChange} />;

    case "ref/client":
      return <EntityInputReference baseApiUrl="Clients" field={field} value={value} onChange={onChange} />;

    case "ref/product":
      return <EntityInputReference baseApiUrl="Products" field={field} value={value} onChange={onChange} />;

    case "ref/user":
      return <EntityInputReference baseApiUrl="Users" field={field} value={value} onChange={onChange} />;

    case "ref/eventtimemapping":
      return (
        <select className="form-select" id={field.name} name={field.name} value={value} onChange={onChange}>
          <option value="Active">Active</option>
          <option value="StandardTimeOff">Standard Time-off</option>
          <option value="ExtraTimeOff">Extra Time-off</option>
        </select>
      );

    case "image":
      return <EntityInputImage field={field} value={value} onChange={onChange} />;

    case "schedule":
      return <EntityInputSchedule field={field} value={value} onChange={onChange} />;

    case "color":
      return <EntityInputColor field={field} value={value} onChange={onChange} />;

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
