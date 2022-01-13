import { IconCircleCheck, IconCircleDashed, IconCircleX, IconSend } from "@tabler/icons";
import clsx from "clsx";
import pluralize from "pluralize";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import Select from "react-select";
import { addError } from "../Alerts/slice";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityFieldEdit from "../Generic/EntityFieldEdit";
import MobilePageTitle from "../Generic/MobilePageTitle";

function EntityFieldLabel({ field }) {
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

function EntityFieldForEdit({ field, entity, onChange }) {
  return (
    <div className="mb-3">
      <EntityFieldLabel field={field} />
      <EntityFieldEdit field={field} value={entity[field.name] || ""} onChange={onChange} />
    </div>
  );
}

function Status({ value, text, message }) {
  return (
    <>
      {" "}
      <div className="d-flex align-items-center">
        {value && <IconCircleCheck className="icon-md text-success" />}
        {!value && message && <IconCircleX className="icon-md text-danger" />}
        {!value && !message && <IconCircleDashed className="icon-md text-secondary" />}
        <div className="ms-1">
          <div className="m-0">{text}</div>
        </div>
      </div>
      {!value && <div className="my-3 lead text-muted">{message}</div>}
    </>
  );
}
export function CalendarRequest() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const definition = useDefinition("CalendarRequest");
  const [entity, setEntity] = useState({});
  const [validated, setValidated] = useState(false);
  const [, categories] = useApiFetch("EventCategories", { method: "GET" }, []);
  const [, check] = useApiFetch("Calendar/check", { method: "POST", body: JSON.stringify(entity) }, null);
  const options = categories.map((e) => ({ value: e.identifier, label: e.displayName }));
  const categoryField = definition && definition.fields.find((f) => f.name === "categoryIdentifier");
  const category = entity.categoryIdentifier && categories.find((c) => c.identifier === entity.categoryIdentifier);

  function handleSubmit(event) {
    event.preventDefault();
    setValidated(true);
    apiFetch("Calendar", {
      method: "POST",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
      })
      .catch((err) => {
        dispatch(addError("Can't send this request", err.status));
      });
  }

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  return (
    <form onSubmit={handleSubmit} noValidate={true} className={clsx("container-xl", { "was-validated": validated })}>
      <MobilePageTitle back="./..">
        <div className="navbar-brand flex-fill">Event request</div>
        <button type="submit" className="btn btn-primary">
          <IconSend /> Send
        </button>
      </MobilePageTitle>
      <div className="page-header d-none d-lg-block">
        <div className="row align-items-center">
          <div className="col">
            <h2 className="page-title">Calendar</h2>
            <div className="text-muted mt-1">Create an event request</div>
          </div>
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link to="./.." className="btn btn-link me-3">
                Cancel
              </Link>
              <button type="submit" className="btn btn-primary">
                <IconSend /> Send
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <div className="row row-cards">
          {definition && (
            <div className="card col-lg-6">
              <div className="card-body">
                <div className="mb-3">
                  <EntityFieldLabel field={categoryField} />
                  <Select
                    name={categoryField.name}
                    required={categoryField.required}
                    classNamePrefix="react-select"
                    placeholder={categoryField.placeholder}
                    options={options}
                    value={options.filter((s) => s.value === entity.categoryIdentifier)}
                    onChange={(s) => handleChange({ target: { name: categoryField.name, value: s.value } })}
                  />
                  {category && (
                    <div className="text-muted mt-1">
                      <strong>Note: </strong>
                      {category.mapping === "Active" && "Considered as working time"}
                      {category.mapping === "StandardTimeOff" && "Considered as standard time-off"}
                      {category.mapping === "ExtraTimeOff" && "Considered as special leaves"}
                    </div>
                  )}
                </div>
                <EntityFieldForEdit
                  field={definition.fields.find((f) => f.name === "startDate")}
                  entity={entity}
                  onChange={handleChange}
                />
                <EntityFieldForEdit
                  field={definition.fields.find((f) => f.name === "endDate")}
                  entity={entity}
                  onChange={handleChange}
                />
              </div>
            </div>
          )}
          {check && (
            <div className="col-lg-6 p-3">
              <Status value={check.requestComplete} text="Request complete" message={check.requestCompleteMessage} />
              <Status value={check.activeSchedule} text="Active schedule" message={check.activeScheduleMessage} />
              <Status value={check.noConflict} text="No conflict" message={check.noConflictMessage} />
              {check.totalHours && (
                <div className="m-3 lead">
                  Request for:
                  <br />
                  {pluralize("hour", check.totalHours, true)}
                  <span> on </span>
                  {pluralize("business day", check.totalDays, true)}
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </form>
  );
}
