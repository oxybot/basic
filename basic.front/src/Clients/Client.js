import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { IconEdit, IconChevronRight, IconChevronLeft } from "@tabler/icons";
import { retries, apiUrl, getDefinition } from "../api";
import { objectMap, groupBy } from "../helpers";
import ClientContractList from "../ClientContracts/ClientContractList";

function Client() {
    const { clientId } = useParams();
    const [entity, setEntity] = useState({});
    const [definition, setDefinition] = useState(null);

    useEffect(() => {
        retries(() => fetch(apiUrl("Clients", clientId), { method: "GET" }))
            .then(response => response.json())
            .then(response => setEntity(response))
            .catch(err => console.log(err));
    }, [clientId]);

    useEffect(() => {
        getDefinition("Client")
            .then(definition => setDefinition(definition))
            .catch(err => console.log(err));
    }, []);

    return (
        <>
            <div className="page-header">
                <div className="row align-items-center">
                    <div className="col-auto ms-auto d-print-none">
                        <div className="d-flex">
                            <Link to="/clients" className="btn btn-outline-primary btn-icon d-none d-lg-block">
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
                            <Link to="edit" className="btn btn-primary btn-icon d-md-none" aria-label="Edit">
                                <IconEdit />
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
            <div className="page-body">
                <div className="card-tabs">
                    <ul className="nav nav-tabs">
                        <li className="nav-item"><a href="#tab-top-1" className="nav-link active" data-bs-toggle="tab">Details</a></li>
                        <li className="nav-item"><a href="#tab-top-2" className="nav-link" data-bs-toggle="tab">Contracts</a></li>
                        <li className="nav-item"><a href="#tab-top-3" className="nav-link" data-bs-toggle="tab">Invoices</a></li>
                    </ul>
                    <div className="tab-content">
                        <div id="tab-top-1" className="card tab-pane show active">
                            <div className="card-body">
                                {definition && objectMap(groupBy(definition.fields, x => x.group), (fields, group, index) => (
                                    <div key={index} className="mb-3">
                                        <div className="card">
                                            {group !== "null" && (
                                                <div className="card-header">
                                                    <h3 className="card-title">
                                                        {group}
                                                    </h3>
                                                </div>
                                            )}
                                            <div className="card-body">
                                                {fields.map((field, index) => (
                                                    <div key={index} className="mb-3">
                                                        <div className="small text-muted">{field.displayName}</div>
                                                        <div className="lead">{entity[field.name] || "-"}</div>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                        <div id="tab-top-2" className="card tab-pane">
                            <div className="card-body">
                                <ClientContractList />
                            </div>
                        </div>
                        <div id="tab-top-3" className="card tab-pane">
                            <div className="card-body">
                                <div className="card-title">Invoices</div>
                                <p>
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit. Adipisci, alias aliquid distinctio dolorem expedita, fugiat hic magni molestiae molestias odit.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Client;
