import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { retries, apiUrl } from "../api";
import EntityDetail from "../Generic/EntityDetail";

export default function Agreement() {
  const { agreementId } = useParams();
  const [entity, setEntity] = useState({});

  useEffect(() => {
    retries(() => fetch(apiUrl("Agreements", agreementId), { method: "GET" }))
      .then((response) => response.json())
      .then((response) => setEntity(response))
      .catch((err) => console.log(err));
  }, [agreementId]);

  return (
    <>
      <div className="page-header">
        <div className="row align-items-center">
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link
                to="/agreements"
                className="btn btn-outline-primary btn-icon d-none d-lg-block"
              >
                <IconChevronRight />
              </Link>
              <Link
                to="/agreements"
                className="btn btn-outline-primary d-lg-none"
              >
                <IconChevronLeft />
                Back
              </Link>
            </div>
          </div>
          <div className="col">
            <h2 className="page-title">
              {entity.internalCode} - {entity.title}
            </h2>
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
              <a
                href="#tab-top-1"
                className="nav-link active"
                data-bs-toggle="tab"
              >
                Details
              </a>
            </li>
          </ul>
          <div className="tab-content">
            <div id="tab-top-1" className="card tab-pane show active">
              <div className="card-body">
                <EntityDetail
                  definitionName="AgreementForView"
                  entity={entity}
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
