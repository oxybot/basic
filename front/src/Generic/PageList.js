import { IconFilter, IconPlus } from "@tabler/icons";
import clsx from "clsx";
import pluralize from "pluralize";
import { Link, useOutlet } from "react-router-dom";
import { useInRole } from "../Authentication";
import EntityList from "./EntityList";
import MobilePageTitle from "./MobilePageTitle";

export default function PageList({
  listClassName,
  definition,
  loading,
  elements,
  selectedId,
  texts,
  newRole = null,
  sorting,
  setSorting,
  filters = null,
  filtered = false,
}) {
  const outlet = useOutlet();
  const isInRole = useInRole();

  return (
    <div className="container-xl">
      <div className="row">
        <div className={outlet ? "d-none d-lg-block col-lg-6" : "col-12"}>
          <MobilePageTitle>
            <div className="navbar-brand flex-fill">{texts.title}</div>
            {isInRole(newRole) && (
              <>
                <Link to="new" className="btn btn-primary btn-icon" aria-label={texts.add}>
                  <IconPlus />
                </Link>
              </>
            )}
          </MobilePageTitle>
          <div className="page-header d-none d-lg-block">
            <div className="row align-items-center">
              <div className="col">
                <h2 className="page-title">{texts.title}</h2>
                <div className="text-muted mt-1">
                  {elements ? pluralize("entry", elements.length, true) : "- entry"}
                </div>
              </div>
              <div className="col-auto ms-auto d-print-none">
                <div className="d-flex">
                  {isInRole(newRole) && (
                    <>
                      <Link to="new" className="ms-3 btn btn-primary d-none d-md-block">
                        <IconPlus />
                        {texts.add}
                      </Link>
                      <Link to="new" className="ms-3 btn btn-primary btn-icon d-md-none" aria-label={texts.add}>
                        <IconPlus />
                      </Link>
                    </>
                  )}
                </div>
              </div>
            </div>
          </div>
          <div className="mt-3 d-lg-none d-flex flex-row justify-content-between align-items-start">
            <div className="text-muted">{elements ? pluralize("entry", elements.length, true) : "- entry"}</div>
            {filters && (
              <button
                className={clsx("btn btn-icon", filtered ? "btn-secondary" : "btn-outline-secondary")}
                data-bs-toggle="collapse"
                data-bs-target="#filters"
                aria-expanded="false"
                aria-controls="filters"
              >
                <IconFilter />
              </button>
            )}
          </div>
          {filters && (
            <div className="collapse d-lg-none" id="filters">
              <div className="card card-body">{filters}</div>
            </div>
          )}
          <div className="page-body">
            <div className="card">
              <EntityList
                className={listClassName}
                loading={loading}
                definition={definition}
                elements={elements}
                baseTo={""}
                selectedId={selectedId}
                sorting={sorting}
                setSorting={setSorting}
              />
            </div>
          </div>
        </div>
        {outlet && <div className="col-12 col-lg-6">{outlet}</div>}
      </div>
    </div>
  );
}
