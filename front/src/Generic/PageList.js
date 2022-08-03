import { IconPlus} from "@tabler/icons";
import pluralize from "pluralize";
import { Link, useOutlet } from "react-router-dom";
import { useInRole } from "../Authentication";
import EntityList from "./EntityList";
import MobilePageTitle from "./MobilePageTitle";

export default function PageList({ definition, loading, elements, selectedId, texts, newRole = null }) {
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
          <div className="page-header d-lg-none">
            <div className="text-muted">{elements ? pluralize("entry", elements.length, true) : "- entry"}</div>
          </div>
          <div className="page-body">
            <div className="card">
              <EntityList
                loading={loading}
                definition={definition}
                entities={elements}
                baseTo={""}
                selectedId={selectedId}
              />
            </div>
          </div>
        </div>
        {outlet && <div className="col-12 col-lg-6">{outlet}</div>}
      </div>
    </div>
  );
}
