import clsx from "clsx";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, Navigate } from "react-router-dom";
import { apiFetch, useDefinition } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import { refresh } from "./slice";

import EntityFieldEdit from "../Generic/EntityFieldEdit";
import EntityFieldInput from "../Generic/EntityFieldInput";
import EntityFieldLabel from "../Generic/EntityFieldLabel";
import { groupBy, objectMap } from "../helpers";

export function UserNewLdap() {
    const dispatch = useDispatch();
    const definition = useDefinition("UserForEdit");
    const texts = {
        title: "Users",
        subTitle: "Add a new Ldap user",
    };
    const errors = [];

    const [search, setSearch] = useState("");
    const [results, setResults] = useState([]);

    function handleSearch() {
        dispatch(refresh());
    }

    async function handleChange(event) {
        const value = event.target.value;
        setSearch(value);
        const response = await apiFetch("Users", { method: "GET" });
        setResults(response);
    }

    function t(code) {
        const text = texts[code];
        return text;
    }

    return (
        <form onSubmit={handleSearch} noValidate={true}>
            <MobilePageTitle back="./..">
                <div className="navbar-brand flex-fill">{t("title")}</div>
                <button type="submit" className="btn btn-primary">
                    {t("form-action")}
                </button>
            </MobilePageTitle>
            <div className="page-header d-none d-lg-block">
                <div className="row align-items-center">
                    <div className="col">
                        <h2 className="page-title">{t("title")}</h2>
                        <div className="text-muted mt-1">{t("subTitle")}</div>
                    </div>
                    <div className="col-auto ms-auto d-print-none">
                        <div className="d-flex">
                            <Link to="./.." className="btn btn-link me-3">
                                Cancel
                            </Link>
                            <button type="submit" className="btn btn-primary">
                                {t("form-action")}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div className="page-body">
                <div className="row row-cards">
                    {errors[""] && <div className="alert show fade alert-danger">{errors[""]}</div>}
                    <div className="card col-lg-12">
                        {/* <div className="card-header">
                            <h3 className="card-title">{group}</h3>
                        </div> */}
                        <div className="card-body">
                            <label htmlFor="search" className="form-label required">Search</label>
                            <div className="mb-3"><input
                                type="text"
                                className={clsx("form-control")}
                                required={true}
                                id="search"
                                name="search"
                                placeholder="My Best Employee"
                                value={search}
                                onChange={handleChange}
                            />
                            </div>
                        </div>
                    </div>
                    <div className="card col-lg-12">
                        {results.map((result, index) =>  (
                            <button key={index} onClick={console.log({result})}>
                                {index} - {result.email}
                            </button>
                        ))}
                    </div>
                </div>
            </div>
        </form>);
}
