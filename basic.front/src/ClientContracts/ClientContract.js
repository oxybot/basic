import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { retries, apiUrl, getDefinition } from "../api";
import { objectMap, groupBy } from "../helpers";

export default function ClientContract() {
  const { contractId } = useParams();
  const [definition, setDefinition] = useState(null);
  const [entity, setEntity] = useState({});

  useEffect(() => {
    getDefinition("ClientContract")
      .then((definition) => setDefinition(definition))
      .catch((err) => console.log(err));
  }, []);

  useEffect(() => {
    retries(() => fetch(apiUrl("ClientContracts", contractId), { method: "GET" }))
      .then((response) => response.json())
      .then((response) => setEntity(response))
      .catch((err) => console.log(err));
  }, [contractId]);

  return (
    <>
      <div className="page-header">
        <div className="row align-items-center">
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link
                to="/clientcontracts"
                className="btn btn-outline-primary btn-icon d-none d-lg-block"
              >
                <IconChevronRight />
              </Link>
              <Link to="/clientcontracts" className="btn btn-outline-primary d-lg-none">
                <IconChevronLeft />
                Back
              </Link>
            </div>
          </div>
          <div className="col">
            <h2 className="page-title">{entity.internalCode} - {entity.title}</h2>
          </div>
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link to="edit" className="btn btn-primary d-none d-md-block">
                <IconEdit />
                Edit
              </Link>
              <Link
                to="edit"
                className="btn btn-primary btn-icon d-md-none"
                aria-label="Edit"
              >
                <IconEdit />
              </Link>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <div className="card-tabs">
          <ul className="nav nav-tabs">
            <li className="nav-item">
              <a href="#tab-top-1" className="nav-link active" data-bs-toggle="tab">
                Details
              </a>
            </li>
          </ul>
          <div className="tab-content">
            <div id="tab-top-1" className="card tab-pane show active">
              <div className="card-body">
                {definition &&
                  objectMap(
                    groupBy(definition.fields, (x) => x.group),
                    (fields, group, index) => (
                      <div key={index} className="mb-3">
                        <div className="card">
                          {group !== "null" && (
                            <div className="card-header">
                              <h3 className="card-title">{group}</h3>
                            </div>
                          )}
                          <div className="card-body">
                            {fields.map((field, index) => (
                              <div key={index} className="mb-3">
                                <div className="small text-muted">
                                  {field.displayName}
                                </div>
                                <div className="lead">
                                  {entity[field.name] || "-"}
                                </div>
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
        </div>
      </div>
    </>
  );
}
