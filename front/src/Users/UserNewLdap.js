import clsx from "clsx";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { apiFetch } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import { refresh } from "./slice";


// MakeLdapConnection

export function UserNewLdap() {

    const dispatch = useDispatch();
    const texts = {
        title: "Users",
        subTitle: "Users from the Active Directory",
    };
    const errors = [];

    const [search, setSearch] = useState("");
    const [displaySearch, setDisplaySearch] = useState("");
    const [occurrences, setOccurrences] = useState("0");
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);

    async function handleSearch() {
        await timeout(3000);
        dispatch(refresh());
    }
    
    async function handleChange(event) {
        const value = event.target.value;
        setSearch(value);
    }

    // To use Timeout : " await timeout([number in milliseconds]); "
    function timeout(delay) {
        return new Promise(res => setTimeout(res, delay));
    }

    useEffect(() => {
        if (displaySearch !== search && !loading) {
            setLoading(true);

            apiFetch("users/ldap?searchTerm=" + search, { method: "GET" })
                .then(({ occurrencesNumber, listOfLdapUsers }) => {
                    setResults(listOfLdapUsers);
                    setOccurrences(occurrencesNumber);
                    setDisplaySearch(search);
                    console.log(listOfLdapUsers);
                })
            setLoading(false);
        }
    }, [search, displaySearch, loading])

    function t(code) {
        const text = texts[code];
        return text;
    }

    function importLdapUser(entity) {

        entity.avatar = {
            "data": entity.avatar,
            "mimeType": "image/jpeg"
          };
    
        apiFetch("users/", {
            method: "POST",
            body: JSON.stringify(entity)
        });
    }

    return (
        <form onSubmit={() => handleSearch} noValidate={true}>
            <MobilePageTitle back="./..">
                <div className="navbar-brand flex-fill">{t("title")}</div>
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
                        </div>
                    </div>
                </div>
            </div>
            <div className="page-body">
                <div className="row row-cards">
                    {errors[""] && <div className="alert show fade alert-danger">{errors[""]}</div>}
                    <div className="card col-lg-12">
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

                    <div hidden={loading}>{occurrences} matching user{occurrences < "2" ? '' : 's'}</div>
                    <div hidden={!loading}>loading</div>


                    <div className="card col-lg-12">
                        {results.map((result, index) => (
                            <div className="ldap-card" hidden={!results} key={index} >
                                
                                <img hidden={result.avatar} className="picture" src="/no-picture.png" alt="user pp" width="100" height="150"></img>
                                <img hidden={!result.avatar} className="picture" src={'data:image/gif;base64,' + result.avatar} alt="user pp" width="100" height="150"></img>

                                <div className="ldap-attributs">
                                    <div className="lead">{result.displayName}</div>
                                    <div className="lead">{result.email !== "" ? result.email : '-'}</div>
                                    <div className="lead">{result.title !== "" ? result.title : '-'}</div>
                                </div>
                                <div className="importable">
                                    <button className="btn btn-primary" disabled={!result.importable} onClick={() => {importLdapUser(result)}}>
                                        Import user
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </form>);
}