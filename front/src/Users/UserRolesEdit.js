import { Link, useLoaderData, useNavigate, useParams, useRevalidator } from "react-router-dom";
import { apiFetch, useApiFetch } from "../api";
import { useDispatch } from "react-redux";
import { useState } from "react";
import { addWarning } from "../Alerts/slice";
import clsx from "clsx";
import MobilePageTitle from "../Generic/MobilePageTitle";
import EntityFieldEdit from "../Generic/EntityFieldEdit";

const transform = (entity) => {
  return entity.roles.map((role) => role.code).reduce((a, v) => ({ ...a, [v]: true }), {});
};

export function UserRolesEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { userId } = useParams();
  const [, roles] = useApiFetch("roles", { method: "GET" });

  const dispatch = useDispatch();
  const navigate = useNavigate();
  const user = useLoaderData();
  const [entity, setEntity] = useState(transform(user));
  const [errors, setErrors] = useState([]);

  const texts = {
    title: user.displayName,
    subTitle: "Edit the Roles of a user",
    "form-action": "Update",
  };

  function t(code) {
    const text = texts[code];
    if (typeof text === "function") {
      return text(entity);
    } else {
      return text;
    }
  }

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value === "" ? null : event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const convert = (entity) => {
    return Object.keys(entity).reduce((a, k) => (entity[k] ? [...a, k] : a), []);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    apiFetch(["users", userId, "roles"], {
      method: "PUT",
      body: JSON.stringify(convert(entity)),
    })
      .then(() => {
        navigate("./..");
        revalidator.revalidate();
      })
      .catch((err) => {
        if (err && err.message) {
          dispatch(addWarning("Invalid information", "Please review the values provided."));
        } else if (typeof err === "string") {
          dispatch(addWarning("Invalid information", "Please review the values provided"));
        } else {
          setErrors(err);
        }
      });
  };

  return (
    <form onSubmit={handleSubmit} noValidate={true} className={clsx({ "container-xl": full })}>
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
        {errors[""] && <div className="alert show fade alert-danger">{errors[""]}</div>}
        <div className="card mb-3 col-lg-12">
          <div className="card-body">
            {roles &&
              roles.values.map((role, index) => {
                const field = { type: "boolean", name: role.code, displayName: role.code };
                const fieldErrors = errors[role.code] || [];
                return (
                  <EntityFieldEdit
                    key={index}
                    field={field}
                    entity={entity}
                    errors={fieldErrors}
                    onChange={handleChange}
                  />
                );
              })}
          </div>
        </div>
      </div>
    </form>
  );
}
