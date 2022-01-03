import clsx from "clsx";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { apiFetch, apiUrl } from "../api";
import LayoutTheme from "../LayoutTheme";
import { connect, setRoles, setUser } from "./slice";

export function SignIn() {
  const dispatch = useDispatch();
  const [loading, setLoading] = useState(true);
  const [credentials, setCredentials] = useState({});
  const [error, setError] = useState(null);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setError(null);
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
          headers: { authorization: "Bearer " + cookie.value },
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
        if (response.status === 401) {
          setError("Invalid credentials");
        } else {
          setError("System error - try again later");
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
          <form className="card card-md" onSubmit={handleSubmit} method="get" autoComplete="off">
            <div className="card-body">
              <h2 className="card-title text-center mb-4">Login to your account</h2>
              <div className="mb-3">
                <label className="form-label" htmlFor="username">
                  Username
                </label>
                <input
                  id="username"
                  name="username"
                  type="username"
                  onChange={handleChange}
                  tabIndex="1"
                  className="form-control"
                  placeholder="Enter username"
                />
              </div>
              <div className="mb-2">
                <label className="form-label" htmlFor="password">
                  Password
                  <span className="form-label-description">
                    <a href="./forgot-password.html">I forgot password</a>
                  </span>
                </label>
                <input
                  id="password"
                  name="password"
                  type="password"
                  onChange={handleChange}
                  tabIndex="2"
                  className="form-control"
                  placeholder="Password"
                  autoComplete="off"
                />
              </div>
              <div className="mb-2">
                <label className="form-check">
                  <input type="checkbox" className="form-check-input" tabIndex="3" />
                  <span className="form-check-label">Remember me on this device</span>
                </label>
              </div>
              <div className="form-footer">
                <button type="submit" className="btn btn-primary w-100" tabIndex="4">
                  Sign in
                </button>
              </div>
            </div>
          </form>
          <div className="m-3 text-center">
            <div className={clsx({ "card bg-danger text-white": error })}>
              <div className="card-body px-3 py-2">{error || ""}&nbsp;</div>
            </div>
          </div>
        </div>
      </div>
    )
  );
}
