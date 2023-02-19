import { IconAlertTriangle } from "@tabler/icons";
import Cookies from "js-cookie";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { addError } from "../Alerts/slice";
import { apiFetch, apiUrl, useDefinition } from "../api";
import EntityFieldEdit from "../Generic/EntityFieldEdit";
import LayoutTheme from "../LayoutTheme";
import { connect, setRoles, setUser } from "./slice";

export function SignIn() {
  const dispatch = useDispatch();
  const definition = useDefinition("AuthRequest");
  const [loading, setLoading] = useState(true);
  const [credentials, setCredentials] = useState({});
  const [errors, setErrors] = useState({});

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setCredentials({ ...credentials, [name]: value });
  };

  // Manage the reconnection of a user - or a forced page refresh
  useEffect(() => {
    const token = Cookies.get("access-token");
    if (!token) {
      // No previous connection
      setLoading(false);
    } else {
      fetch(apiUrl("Auth/renew"), {
        method: "POST",
        headers: {
          "content-type": "application/json",
          accept: "application/json",
          authorization: "Bearer " + token,
        },
      }).then(async (response) => {
        if (!response.ok) {
          // previous connection invalid
          Cookies.remove("access-token");
          setLoading(false);
        } else {
          // previous connection valid - autoconnect
          const json = await response.json();
          Cookies.set("access-token", json.accessToken);
          dispatch(connect(json));
          apiFetch("My/User", { method: "GET" }).then((response) => dispatch(setUser(response)));
          apiFetch("My/Roles", { method: "GET" }).then((response) => dispatch(setRoles(response)));
        }
      });
    }
  }, [dispatch]);

  function handleSubmit(e) {
    e.preventDefault();
    setErrors({});

    fetch(apiUrl("Auth"), {
      method: "POST",
      headers: {
        "content-type": "application/json",
        accept: "application/json",
      },
      body: JSON.stringify(credentials),
    }).then((response) => {
      if (!response.ok) {
        if (response.status === 400) {
          response.json().then(setErrors);
        } else {
          dispatch(addError("System error", "The system doesn't behave properly - try again later"));
        }
      } else {
        response.json().then((response) => {
          Cookies.set("access-token", response.accessToken);
          dispatch(connect(response));
          apiFetch("My/User", { method: "GET" }).then((response) => dispatch(setUser(response)));
          apiFetch("My/Roles", { method: "GET" }).then((response) => dispatch(setRoles(response)));
        });
      }
    });
  }

  return (
    !loading &&
    definition && (
      <div className="page page-center">
        <div className="container container-tight py-4">
          <div className="mx-3 d-flex align-items-center">
            <h5 className="basic-title d-flex align-items-center m-0">
              <div className="basic-logo">B</div>
              <div className="basic-text ms-1">asic</div>
            </h5>
            <div className="ms-auto">
              <LayoutTheme className="btn btn-link text-body" />
            </div>
          </div>
          <div className="card card-md">
            <div className="card-body p-4">
              <h2 className="text-center mb-4">Login to your account</h2>
              <form onSubmit={handleSubmit} method="get" autoComplete="off" noValidate>
                {definition.fields.map((field, index) => (
                  <EntityFieldEdit
                    key={index}
                    field={field}
                    errors={errors && errors[field.name]}
                    entity={credentials}
                    onChange={handleChange}
                  />
                ))}
                {errors[""] && (
                  <div className="alert alert-important alert-warning">
                    <div className="d-flex">
                      <div>
                        <IconAlertTriangle className="alert-icon" />
                      </div>
                      <div>{errors[""]}</div>
                    </div>
                  </div>
                )}
                <div className="form-footer">
                  <button type="submit" className="btn btn-primary w-100" tabIndex="4">
                    Sign in
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    )
  );
}
