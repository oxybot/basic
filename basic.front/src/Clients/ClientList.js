import { IconPlus, IconSearch } from "@tabler/icons";
import pluralize from "pluralize";
import { Link, useOutlet, useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import EntityList from "../Generic/EntityList";
import MobilePageTitle from "../Generic/MobilePageTitle";

export default function ClientList() {
  const outlet = useOutlet();
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const [loading, elements] = useApiFetch("Clients", { method: "GET" }, []);

  return (
    <div className="container-xl">
      <div className="row">
        <div className={outlet ? "d-none d-lg-block col-lg-6" : "col-12"}>
          <MobilePageTitle>
            <div className="navbar-brand flex-fill">Clients</div>
            <Link
              to="new"
              className="btn btn-primary btn-icon"
              aria-label="Add client"
            >
              <IconPlus />
            </Link>
          </MobilePageTitle>
          <div className="page-header d-none d-lg-block">
            <div className="row align-items-center">
              <div className="col">
                <h2 className="page-title">Clients</h2>
                <div className="text-muted mt-1">
                  {elements
                    ? pluralize("entry", elements.length, true)
                    : "- entry"}
                </div>
              </div>
              <div className="col-auto ms-auto d-print-none">
                <div className="d-flex">
                  <div className="me-3">
                    <div className="input-icon">
                      <input
                        type="text"
                        className="form-control"
                        placeholder="Search&hellip;"
                      />
                      <span className="input-icon-addon">
                        <IconSearch />
                      </span>
                    </div>
                  </div>
                  <Link to="new" className="btn btn-primary d-none d-md-block">
                    <IconPlus />
                    Add client
                  </Link>
                  <Link
                    to="new"
                    className="btn btn-primary btn-icon d-md-none"
                    aria-label="Add client"
                  >
                    <IconPlus />
                  </Link>
                </div>
              </div>
            </div>
          </div>
          <div className="page-header d-lg-none">
            <div className="text-muted">
              {elements ? pluralize("entry", elements.length, true) : "- entry"}
            </div>
          </div>
          <div className="page-body">
            <div className="card">
              <EntityList
                loading={loading}
                definition={definition}
                entities={elements}
                baseTo="/clients"
                selectedId={clientId}
              />
            </div>
          </div>
        </div>
        {outlet && <div className="col-12 col-lg-6">{outlet}</div>}
      </div>
    </div>
  );
}
