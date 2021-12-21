import { IconPlus, IconSearch } from "@tabler/icons";
import pluralize from "pluralize";
import { Link, useOutlet } from "react-router-dom";
import { useApiAgreements } from "../api";
import AgreementList from "./AgreementList";
import MobilePageTitle from "../Generic/MobilePageTitle";

export default function Agreements() {
  const outlet = useOutlet();
  const [loading, agreements] = useApiAgreements();

  return (
    <>
      <MobilePageTitle>
        <div className="navbar-brand">Agreements</div>
      </MobilePageTitle>
      <div className="container-xl">
        <div className="row">
          <div className={outlet ? "d-none d-lg-block col-lg-6" : "col-12"}>
            <div className="page-header">
              <div className="row align-items-center">
                <div className="col">
                  <h2 className="page-title">Agreements</h2>
                  <div className="text-muted mt-1">
                    {pluralize("entry", agreements.length, true)}
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
                      Add agreement
                    </Link>
                    <Link
                      to="new"
                      className="btn btn-primary btn-icon d-md-none"
                      aria-label="Add agreement"
                    >
                      <IconPlus />
                    </Link>
                  </div>
                </div>
              </div>
            </div>
            <div className="page-body">
              <div className="card">
                <AgreementList loading={loading} agreements={agreements} />
              </div>
            </div>
          </div>
          {outlet && <div className="col-12 col-lg-6">{outlet}</div>}
        </div>
      </div>
    </>
  );
}
