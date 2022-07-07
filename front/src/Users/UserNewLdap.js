import clsx from "clsx";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, Navigate } from "react-router-dom";
import { apiFetch, useDefinition } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import { refresh } from "./slice";


export function UserNewLdap() {
    const dispatch = useDispatch();
    const definition = useDefinition("UserForEdit");
    const texts = {
        title: "Users",
        subTitle: "Add a user from Active Directory",
    };
    const errors = [];

    const [search, setSearch] = useState("");
    const [occurrences, setOccurrences] = useState("");
    const [results, setResults] = useState([]);

    function handleSearch() {
        dispatch(refresh());
    }

    async function handleChange(event) {
        const value = event.target.value;
        setSearch(value);
        const {occurrencesNumber, listOfLdapUsers} = await apiFetch("users/ldap?searchTerm=" + value, { method: "GET" });
        setResults(listOfLdapUsers);
        setOccurrences(occurrencesNumber);
        console.log(listOfLdapUsers);
        console.log(listOfLdapUsers[0]);
    }

    function t(code) {
        const text = texts[code];
        return text;
    }

    // check si un AD user est déjà présent dans la liste de l'app
    // function userAldreadyInApp(userEmail){
    //     const listUsers = apiFetch("users/", { method: "GET" });
    //     listUsers.map((userFromList) => {

    //           if (userFromList.email == userEmail)
    //           {
    //             return true
    //           } 
    //         })
    //     }
        
    // // Affichage du boutton import
    // function importUser(userEmail){
    //     if (userAldreadyInApp(userEmail)) {
    //         return <button type="submit" className="btn btn-primary">
    //                     Import user
    //                 </button>
    //     }
    //     else {
    //         return <div> - </div>
    //     }
    // }

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
                        {occurrences} matching user(s)
                    <div>
                    </div>
                    <div className="card col-lg-12">
                        {results.map((result, index) => (
                            <div key={index} onClick={console.log()}>
                                {result.displayName} - {result.email} 
                                <img src={'data:image/gif;base64,' + result.avatar} alt="user pp" width="100" height="150"></img>

                                <button hidden={result.importable} type="submit" className="btn btn-primary">
                                    Import user
                                </button>
                                
                                <div hidden={!result.importable}>
                                    User already in Basic application
                                </div>
                            </div>
                        ))}
 
                        <button href="newldap?search=" className="btn btn-primary">
                            Link to AD user research
                        </button>

                    </div>
                </div>
            </div>
        </form>);
}
