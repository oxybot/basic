import clsx from "clsx";
import { Link } from "react-router-dom";
import { groupBy, objectMap } from "../helpers";
import EntityFieldEdit from "./EntityFieldEdit";
import MobilePageTitle from "./MobilePageTitle";

export default function EntityForm({
  definition,
  entity,
  texts,
  handleChange,
  handleSubmit,
  full = false,
  validated = false,
  children,
}) {
  function t(code) {
    const text = texts[code];
    if (typeof text === "function") {
      return text(entity);
    } else {
      return text;
    }
  }

  return (
    <form
      onSubmit={handleSubmit}
      noValidate={true}
      className={clsx({ "container-xl": full, "was-validated": validated })}
    >
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
          {definition &&
            objectMap(
              groupBy(definition.fields, (x) => x.group),
              (fields, group, index) => (
                <div key={index} className="card col-lg-12">
                  {group !== "null" && (
                    <div className="card-header">
                      <h3 className="card-title">{group}</h3>
                    </div>
                  )}
                  <div className="card-body">
                    {fields.map((field, index) => (
                      <EntityFieldEdit key={index} field={field} entity={entity} onChange={handleChange} />
                    ))}
                  </div>
                </div>
              )
            )}
          {children}
        </div>
      </div>
    </form>
  );
}
