import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { addError, addWarning } from "../Alerts/slice";
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

  useEffect(() => {
    window.cookieStore.get("access-token").then(async (cookie) => {
      if (!cookie) {
        // No previous connection
        setLoading(false);
      } else {
        const response = await fetch(apiUrl("Auth/renew"), {
          method: "POST",
          headers: {
            "content-type": "application/json",
            accept: "application/json",
            authorization: "Bearer " + cookie.value,
          },
        });
        if (!response.ok) {
          // previous connection invalid
          window.cookieStore.delete("access-token");
          setLoading(false);
        } else {
          // previous connection valid - autoconnect
          const json = await response.json();
          window.cookieStore.set("access-token", json.accessToken);
          dispatch(connect(json));
          apiFetch("Users/me", { method: "GET" }).then((response) => dispatch(setUser(response)));
          apiFetch("Roles/mine", { method: "GET" }).then((response) => dispatch(setRoles(response)));
        }
      }
    });
  }, [dispatch]);

  function handleSubmit(e) {
    e.preventDefault();

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
          response.json().then((err) => setErrors(err));
        } else if (response.status === 401) {
          dispatch(addWarning("Invalid credentials", "Your username or password seems invalid, review them and retry"));
        } else {
          dispatch(addError("System error", "The system doesn't behave properly - try again later"));
        }
      } else {
        response.json().then((response) => {
          window.cookieStore.set("access-token", response.accessToken);
          dispatch(connect(response));
          apiFetch("Users/me", { method: "GET" }).then((response) => dispatch(setUser(response)));
          apiFetch("Roles/mine", { method: "GET" }).then((response) => dispatch(setRoles(response)));
        });
      }
    });
  }

  return (
    !loading && (
      <div className="page page-center">
        <div className="container-tight py-4">
          <div className="mx-3 d-flex align-items-center">
            <h5 className="basic-title d-flex align-items-center m-0">
              <div className="basic-logo">B</div>
              <div className="basic-text ms-1">asic</div>
            </h5>
            <div className="ms-auto">
              <LayoutTheme className="btn btn-link text-body" />
            </div>
          </div>
          <form className="card card-md" onSubmit={handleSubmit} method="get" autoComplete="off" noValidate>
            <div className="card-body">
              <h2 className="card-title text-center mb-4">Login to your account</h2>
              {definition &&
                definition.fields.map((field, index) => (
                  <EntityFieldEdit
                    key={index}
                    field={field}
                    errors={errors && errors[field.name]}
                    entity={credentials}
                    onChange={handleChange}
                  />
                ))}
              <div className="form-footer">
                <button type="submit" className="btn btn-primary w-100" tabIndex="4">
                  Sign in
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    )
  );
}
