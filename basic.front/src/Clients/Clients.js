import { useState, useEffect } from "react";
import { IconPlus, IconSearch, IconChevronUp, IconLoader } from "@tabler/icons";
import { Link, useOutlet, useNavigate, useParams } from "react-router-dom";
import pluralize from "pluralize";
import { retries, apiUrl } from "../api";
import clsx from "clsx";
import MobilePageTitle from "../Generic/MobilePageTitle";

export default function Clients() {
  const outlet = useOutlet();
  const navigate = useNavigate();
  const { clientId } = useParams();

  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    retries(() => fetch(apiUrl("Clients"), { method: "GET" }))
      .then((response) => response.json())
      .then((response) => {
        setClients(response);
        setLoading(false);
      })
      .catch((err) => console.log(err));
  }, []);

  return (
    <>
      <MobilePageTitle>
        <div className="navbar-brand">Clients</div>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="row">
          <div className={outlet ? "d-none d-lg-block col-lg-6" : "col-12"}>
            <div className="page-header">
              <div className="row align-items-center">
                <div className="col">
                  <h2 className="page-title">Clients</h2>
                  <div className="text-muted mt-1">
                    {clients
                      ? pluralize("entry", clients.length, true)
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
                    <Link
                      to="new"
                      className="btn btn-primary d-none d-md-block"
                    >
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
            <div className="page-body">
              <div className="card">
                <div className="table-responsive">
                  <table className="table card-table table-vcenter text-nowrap datatable table-hover">
                    <thead>
                      <tr>
                        <th>
                          Display Name <IconChevronUp />
                        </th>
                        <th>Full Name</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr className={loading ? "" : "d-none"}>
                        <td colSpan="2">
                          <IconLoader /> Loading...
                        </td>
                      </tr>
                      {clients.map((client) => (
                        <tr
                          key={client.identifier}
                          className={clsx({
                            "table-active": client.identifier === clientId,
                          })}
                          onClick={() =>
                            navigate("/clients/" + client.identifier)
                          }
                        >
                          <td>{client.displayName}</td>
                          <td>{client.fullName}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
          {outlet && <div className="col-12 col-lg-6">{outlet}</div>}
        </div>
      </div>
    </>
  );
}
