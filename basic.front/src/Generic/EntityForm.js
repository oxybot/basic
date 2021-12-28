import { IconCurrencyEuro } from "@tabler/icons";
import clsx from "clsx";
import dayjs from "dayjs";
import { Link } from "react-router-dom";
import Select from "react-select";
import { useApiFetch } from "../api";
import { groupBy, objectMap } from "../helpers";
import MobilePageTitle from "./MobilePageTitle";

function EntityInputClient({ field, value, onChange }) {
  const [loading, clients] = useApiFetch("Clients", { method: "GET" }, [], (clients) =>
    clients.map((c) => ({
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
        options={clients}
        value={clients.filter((s) => s.value === value)}
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
    <div class="d-flex">
      {value && <img className="avatar avatar-lg me-2" src={`data:${value.mimeType};base64,${value.data}`} />}
      {!value && <div className="avatar avatar-lg me-2"></div>}
      <div class="d-flex flex-column align-items-start justify-content-between">
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

function EntityInput({ field, value, onChange }) {
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

    case "ref/client":
      return <EntityInputClient field={field} value={value} onChange={onChange} />;

    case "image":
      return <EntityInputImage field={field} value={value} onChange={onChange} />;

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

export default function EntityForm({
  definition,
  entity,
  texts,
  handleChange,
  handleSubmit,
  full = false,
  validated = false,
}) {
  function t(code) {
    const text = texts[code];
    if (typeof text === "function") {
      return text(entity);
    } else {
      return text;
    }
  }

  return (
    <form
      onSubmit={handleSubmit}
      noValidate={true}
      className={clsx({ "container-xl": full, "was-validated": validated })}
    >
      <MobilePageTitle back="./..">
        <div className="navbar-brand flex-fill">{t("title")}</div>
        <button type="submit" className="btn btn-primary">
          {t("form-action")}
        </button>
      </MobilePageTitle>
      <div className="page-header d-none d-lg-block">
        <div className="row align-items-center">
          <div className="col">
            <h2 className="page-title">{t("title")}</h2>
            <div className="text-muted mt-1">{t("subTitle")}</div>
          </div>
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link to="./.." className="btn btn-link me-3">
                Cancel
              </Link>
              <button type="submit" className="btn btn-primary">
                {t("form-action")}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <div className="row row-cards">
          {definition &&
            objectMap(
              groupBy(definition.fields, (x) => x.group),
              (fields, group, index) => (
                <div key={index} className="col-lg-12">
                  <div className="card">
                    {group !== "null" && (
                      <div className="card-header">
                        <h3 className="card-title">{group}</h3>
                      </div>
                    )}
                    <div className="card-body">
                      {fields.map((field, index) => (
                        <div key={index} className="mb-3">
                          <label
                            htmlFor={field.name}
                            className={clsx("form-label", {
                              required: field.required,
                            })}
                          >
                            {field.displayName}
                          </label>
                          <EntityInput field={field} value={entity[field.name] || ""} onChange={handleChange} />
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              )
            )}
        </div>
      </div>
    </form>
  );
}
