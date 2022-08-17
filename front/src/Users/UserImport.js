import { IconAtom2 } from "@tabler/icons";
import clsx from "clsx";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { apiFetch } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import { refresh } from "./slice";

export function UserImport() {
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

  function handleSearch() {
    dispatch(refresh());
  }

  function handleChange(event) {
    const value = event.target.value;
    setSearch(value);
  }

  useEffect(() => {
    if (displaySearch !== search && !loading) {
      setLoading(true);
      apiFetch("users/import?searchTerm=" + search, { method: "GET" }).then(
        ({ occurrencesNumber, listOfLdapUsers }) => {
          setResults(listOfLdapUsers);
          setOccurrences(occurrencesNumber);
          setDisplaySearch(search);
        }
      );
      setLoading(false);
    }
  }, [search, displaySearch, loading]);

  function t(code) {
    const text = texts[code];
    return text;
  }

  function importUser(entity) {
    apiFetch("users", {
      method: "POST",
      body: JSON.stringify(entity),
    }).then(() => {
      dispatch(refresh());
    });
  }

  return (
    <>
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
              <form onSubmit={() => handleSearch} noValidate={true}>
                <label htmlFor="search" className="form-label required">
                  Search
                </label>
                <div className="mb-3">
                  <input
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
              </form>
            </div>
          </div>

          {!loading && (
            <div>
              {occurrences} matching user{occurrences < "2" ? "" : "s"}
            </div>
          )}

          <div className="card col-lg-12">
            <ul className="list-group list-group-flush">
              {results.map((result, index) => (
                <li className="list-group-item" key={index}>
                  <div className="d-flex flex-row">
                    <div className={clsx("avatar", "avatar-lg mt-1")}>
                      {!result.avatar && <IconAtom2 />}
                      {result.avatar && (
                        <img alt="" src={`data:${result.avatar.mimeType};base64,${result.avatar.data}`} />
                      )}
                    </div>

                    <div className="ms-3">
                      <div className="lead">{result.displayName}</div>
                      <div className="">{result.title || "-"}</div>
                      <div className="">{result.email || "-"}</div>
                    </div>
                  </div>

                  <div className="d-flex flex-row mt-2">
                    <button
                      className="btn btn-primary ms-auto"
                      disabled={!result.importable}
                      onClick={() => importUser(result)}
                    >
                      Import user
                    </button>
                  </div>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </>
  );
}
