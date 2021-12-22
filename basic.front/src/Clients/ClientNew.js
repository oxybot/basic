import { useState } from "react";
import { useDefinition } from "../api";
import { groupBy, objectMap } from "../helpers";
import clsx from "clsx";
import { Link } from "react-router-dom";
import MobilePageTitle from "../Generic/MobilePageTitle";

export default function ClientNew() {
  const [inputs, setInputs] = useState({});
  const definition = useDefinition("ClientForEdit");

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setInputs((values) => ({ ...values, [name]: value }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    fetch("https://localhost:7268/Clients", {
      method: "POST",
      headers: {
        "content-type": "application/json",
        accept: "application/json",
      },
      body: JSON.stringify(inputs),
    })
      .then((response) => response.json())
      .then((response) => {
        console.log(response);
      })
      .catch((err) => {
        alert(err);
        console.log(err);
      });
  };

  return (
    <form onSubmit={handleSubmit}>
      <MobilePageTitle back="/clients">
        <div className="navbar-brand flex-fill">Create Client</div>
        <button type="submit" className="btn btn-primary">
          Create
        </button>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="page-header d-none d-lg-block">
          <div className="row align-items-center">
            <div className="col">
              <h2 className="page-title">Clients</h2>
              <div className="text-muted mt-1">Add a new Client</div>
            </div>
            <div className="col-auto ms-auto d-print-none">
              <div className="d-flex">
                <Link to="./.." className="btn btn-link me-3">
                  Cancel
                </Link>
                <button type="submit" className="btn btn-primary">
                  Create
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
                  <div key={index} className="col-lg-6">
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
                              className={clsx("form-label", {
                                required: field.required,
                              })}
                              htmlFor={field.name}
                            >
                              {field.displayName}
                            </label>
                            <input
                              type="text"
                              className={clsx("form-control", {
                                required: field.required,
                              })}
                              id={field.name}
                              name={field.name}
                              placeholder={field.placeholder}
                              value={inputs[field.name] || ""}
                              onChange={handleChange}
                            />
                          </div>
                        ))}
                      </div>
                    </div>
                  </div>
                )
              )}
          </div>
        </div>
      </div>
    </form>
  );
}
