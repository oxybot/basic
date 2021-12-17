import { useState, useEffect } from "react";
import { IconPlus, IconSearch, IconChevronUp, IconLoader } from "@tabler/icons";
import { Link, useOutlet, useNavigate } from "react-router-dom";
import pluralize from "pluralize";
import { retries, apiUrl } from "../api";

function Clients() {
    let outlet = useOutlet();
    let navigate = useNavigate();
    let withOutlet = outlet !== null && outlet.props.children !== null;

    let [clients, setClients] = useState([]);
    let [loading, setLoading] = useState(true);
    useEffect(() => {
        retries(() => fetch(apiUrl("Clients"), { method: "GET" }))
            .then(response => response.json())
            .then(response => {
                setClients(response);
                setLoading(false);
            })
            .catch(err => console.log(err));
    }, []);

    return (
        <div className="container-xl">
            <div className="row">
                <div className={withOutlet ? "d-none d-lg-block col-lg-6" : "col-12"}>
                    <div className="page-header">
                        <div className="row align-items-center">
                            <div className="col">
                                <h2 className="page-title">Clients</h2>
                                <div className="text-muted mt-1">{pluralize("entry", clients.length, true)}</div>
                            </div>
                            <div className="col-auto ms-auto d-print-none">
                                <div className="d-flex">
                                    <div className="me-3">
                                        <div className="input-icon">
                                            <input type="text" className="form-control" placeholder="Search&hellip;" />
                                            <span className="input-icon-addon">
                                                <IconSearch />
                                            </span>
                                        </div>
                                    </div>
                                    <Link to="new" className="btn btn-primary d-none d-md-block">
                                        <IconPlus />
                                        Add client
                                    </Link>
                                    <Link to="new" className="btn btn-primary btn-icon d-md-none" aria-label="Add client">
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
                                            <th>Display Name <IconChevronUp /></th>
                                            <th>Full Name</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr className={loading ? "" : "d-none"}>
                                            <td colSpan="2"><IconLoader /> Loading...</td>
                                        </tr>
                                        {clients.map(client => (
                                            <tr key={client.identifier} onClick={() => navigate("/clients/" + client.identifier)}>
                                                <td>
                                                    {client.displayName}
                                                </td>
                                                <td>
                                                    {client.fullName}
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                            <div className="card-footer">
                                <div className="btn-group">
                                    <button className="btn btn-primary">Active</button>
                                    <button className="btn btn-outline-primary">Archived</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {withOutlet &&
                    <div className="col-12 col-lg-6">
                        {outlet}
                    </div>
                }
            </div>
        </div>
    );
}

export default Clients;
