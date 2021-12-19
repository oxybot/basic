import { useState, useEffect } from "react";
import { apiUrl, getDefinition, retries } from "../api";
import { groupBy, objectMap } from "../helpers";
import clsx from "clsx";
import { Link, useNavigate, useParams } from "react-router-dom";

export default function ClientEdit() {
  const navigate = useNavigate();
  const { clientId } = useParams();
  const [entity, setEntity] = useState(null);
  const [definition, setDefinition] = useState(null);

  useEffect(() => {
    getDefinition("ClientForEdit")
      .then((definition) => setDefinition(definition))
      .catch((err) => {
        console.log(err);
      });
  }, []);

  useEffect(() => {
    retries(() => fetch(apiUrl("Clients", clientId), { method: "GET" }))
      .then((response) => response.json())
      .then((response) => setEntity(response))
      .catch((err) => console.log(err));
  }, [clientId]);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity((values) => ({ ...values, [name]: value }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    fetch("https://localhost:7268/Clients/" + clientId, {
      method: "PUT",
      headers: {
        "content-type": "application/json",
        accept: "application/json",
      },
      body: JSON.stringify(entity),
    })
      .then((response) => response.json())
      .then((response) => {
        console.log(response);
        navigate("./..");
      })
      .catch((err) => {
        alert(err);
        console.log(err);
      });
  };

  return (
    <div className="container-xl">
      <form onSubmit={handleSubmit}>
        <div className="page-header">
          <div className="row align-items-center">
            <div className="col">
              <h2 className="page-title">{entity && entity.displayName}</h2>
              <div className="text-muted mt-1">Edit a Client</div>
            </div>
            <div className="col-auto ms-auto d-print-none">
              <div className="d-flex">
                <Link to="./.." className="btn btn-link me-3">
                  Cancel
                </Link>
                <button type="submit" className="btn btn-primary">
                  Save
                </button>
              </div>
            </div>
          </div>
        </div>
        <div className="page-body">
          <div className="row row-cards">
            {entity &&
              definition &&
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
                              value={entity[field.name] || ""}
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
      </form>
    </div>
  );
}
