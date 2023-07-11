import { IconAlertTriangle, IconCheck, IconSend } from "@tabler/icons-react";
import clsx from "clsx";
import pluralize from "pluralize";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { addError } from "../Alerts/slice";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import EntityFieldEdit from "../Generic/EntityFieldEdit";
import EntityFieldLabel from "../Generic/EntityFieldLabel";
import MobilePageTitle from "../Generic/MobilePageTitle";
import AttachmentForm from "../Attachments/AttachmentForm";
import { useInRole } from "../Authentication";
import EntityFieldInputReference from "../Generic/EntityFieldInputReference";

export function CalendarRequest({ full = false }) {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const definition = useDefinition("MyEventRequest");
  const [entity, setEntity] = useState({});
  const [validated, setValidated] = useState(false);
  const [partialStartDate, setPartialStartDate] = useState(false);
  const [partialEndDate, setPartialEndDate] = useState(false);
  const [, categories] = useApiFetch("EventCategories", { method: "GET" }, { values: [] });
  const [, check, errors] = useApiFetch("Calendar/check", { method: "POST", body: JSON.stringify(entity) }, null);
  const categoryField = definition && definition.fields.find((f) => f.name === "categoryIdentifier");
  const category = entity.categoryIdentifier && categories.values.find((c) => c.identifier === entity.categoryIdentifier);
  const isInRole = useInRole();

  function switchPartialStartDate() {
    if (partialStartDate) {
      setEntity({ ...entity, durationFirstDay: null });
    }
    setPartialStartDate(!partialStartDate);
  }

  function switchPartialEndDate() {
    if (partialEndDate) {
      setEntity({ ...entity, durationLastDay: null });
    }
    setPartialEndDate(!partialEndDate);
  }

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

  function handleChange(event) {
    const name = event.target.name;
    const value = event.target.value === "" ? null : event.target.value;
    setEntity({ ...entity, [name]: value });
  }

  return (
    <form onSubmit={handleSubmit} noValidate={true} className={clsx("container-xl", { "was-validated": validated })}>
      <MobilePageTitle back="./..">
        <div className="navbar-brand flex-fill">Event request</div>
        <button type="submit" className="btn btn-primary" disabled={errors}>
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
              <button type="submit" className="btn btn-primary" disabled={errors}>
                <IconSend /> Send
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <div className="row">
          {errors && (
            <div className={clsx("alert show fade alert-danger", { "offset-lg-3 col-lg-6": full })}>
              <div className="d-flex">
                <div>
                  <IconAlertTriangle className="alert-icon" />
                </div>
                <div>
                  <h4 className="alert-title">Please complete the form</h4>
                  <div className="text-muted">{errors[""]}</div>
                </div>
              </div>
            </div>
          )}
          {check && (
            <div className={clsx("alert show fade alert-info", { "offset-lg-3 col-lg-6": full })}>
              <div className="d-flex">
                <div>
                  <IconCheck className="alert-icon" />
                </div>
                <div>
                  <h4 className="alert-title">Valid request for:</h4>
                  <div className="text-muted">
                    {check.totalHours && (
                      <>
                        {pluralize("hour", check.totalHours, true)}
                        <span> on </span>
                        {pluralize("business day", check.totalDays, true)}
                      </>
                    )}
                    {!check.totalHours && pluralize("calendar day", check.totalDays, true)}
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
        <div className="row row-cards">
          {definition && (
            <div className={clsx("card", { "offset-lg-3 col-lg-6": full })}>
              <div className="card-body">
                <div className="mb-3">
                  <EntityFieldLabel field={categoryField} />
                  <EntityFieldInputReference
                    baseApiUrl="EventCategories"
                    field={categoryField}
                    value={entity.categoryIdentifier}
                    hasErrors={errors && errors[categoryField.name]}
                    onChange={handleChange}
                  />
                  {errors &&
                    errors[categoryField.name] &&
                    errors[categoryField.name].map((error, index) => (
                      <div key={index} className="invalid-feedback">
                        {error}
                      </div>
                    ))}
                  {category && (
                    <div className="text-muted mt-1">
                      <strong>Note: </strong>
                      {category.mapping !== "TimeOff" && "Considered as working time"}
                      {category.mapping === "TimeOff" && "Considered as time-off"}
                    </div>
                  )}
                </div>
                <EntityFieldEdit
                  field={definition.fields.find((f) => f.name === "startDate")}
                  entity={entity}
                  errors={errors && errors["startDate"]}
                  onChange={handleChange}
                />

                {entity.startDate && category && category.mapping !== "Informational" && (
                  <>
                    <div className="form-check form-switch">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        role="switch"
                        id="partialStartDate"
                        checked={partialStartDate}
                        onChange={switchPartialStartDate}
                      />
                      <label className="form-check-label text-body" htmlFor="partialStartDate">
                        Partial day
                      </label>
                    </div>
                    {partialStartDate && (
                      <EntityFieldEdit
                        field={definition.fields.find((f) => f.name === "durationFirstDay")}
                        entity={entity}
                        errors={errors && errors["durationFirstDay"]}
                        onChange={handleChange}
                      />
                    )}
                  </>
                )}

                <EntityFieldEdit
                  field={definition.fields.find((f) => f.name === "endDate")}
                  entity={entity}
                  errors={errors && errors["endDate"]}
                  onChange={handleChange}
                />

                {entity.endDate &&
                  category &&
                  category.mapping !== "Informational" &&
                  entity.startDate !== entity.endDate && (
                    <>
                      <div className="form-check form-switch">
                        <input
                          className="form-check-input"
                          type="checkbox"
                          role="switch"
                          id="partialEndDate"
                          checked={partialEndDate}
                          onChange={switchPartialEndDate}
                        />
                        <label className="form-check-label text-body" htmlFor="partialEndDate">
                          Partial day
                        </label>
                      </div>
                      {partialEndDate && (
                        <EntityFieldEdit
                          field={definition.fields.find((f) => f.name === "durationLastDay")}
                          entity={entity}
                          errors={errors && errors["durationLastDay"]}
                          onChange={handleChange}
                        />
                      )}
                    </>
                  )}

                <EntityFieldEdit
                  field={definition.fields.find((f) => f.name === "comment")}
                  entity={entity}
                  errors={errors && errors["comment"]}
                  onChange={handleChange}
                />
                {isInRole("beta") && <AttachmentForm entity={entity} setEntity={setEntity} />}
              </div>
            </div>
          )}
        </div>
      </div>
    </form>
  );
}
