import { useParams, Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { apiUrl, useApiFetch } from "../api";
import AgreementInitializedList from "../Agreements/AgreementInitializedList";
import EntityDetail from "../Generic/EntityDetail";
import MobilePageTitle from "../Generic/MobilePageTitle";

export default function Client() {
  const { clientId } = useParams();
  const get = {
    method: "GET",
  };
  const [, entity] = useApiFetch(apiUrl("Clients", clientId), get, {});
  const [, links] = useApiFetch(apiUrl("Clients", clientId, "links"), get, {});

  return (
    <>
      <MobilePageTitle back="/clients">
        <div className="navbar-brand flex-fill">{entity.displayName}</div>
        <Link to="edit" className="btn btn-primary">
          <IconEdit />
          Edit
        </Link>
      </MobilePageTitle>
      <div className="page-header d-none d-lg-block">
        <div className="row align-items-center">
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link
                to="/clients"
                className="btn btn-outline-primary btn-icon d-none d-lg-block"
              >
                <IconChevronRight />
              </Link>
              <Link to="/clients" className="btn btn-outline-primary d-lg-none">
                <IconChevronLeft />
                Back
              </Link>
            </div>
          </div>
          <div className="col">
            <h2 className="page-title">{entity.displayName}</h2>
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
            <li className="nav-item">
              <a href="#tab-top-2" className="nav-link" data-bs-toggle="tab">
                Agreements
                <span className="badge bg-green ms-2">{links.agreements}</span>
              </a>
            </li>
          </ul>
          <div className="tab-content">
            <div id="tab-top-1" className="card tab-pane show active">
              <div className="card-body">
                <EntityDetail definitionName="ClientForView" entity={entity} />
              </div>
            </div>
            <div id="tab-top-2" className="card tab-pane">
              <div className="card-body">
                <AgreementInitializedList />
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
