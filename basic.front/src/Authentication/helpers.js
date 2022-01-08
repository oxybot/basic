import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { authenticationState, connect, setRoles, setUser } from ".";
import { apiFetch } from "../api";

export function useInRole() {
  const { roles } = useSelector(authenticationState);

  return (...acceptedRoles) => {
    // Missing initialization or no acceptedRoles
    if (roles === null) {
      return false;
    } else if (acceptedRoles === null || acceptedRoles.length === 0) {
      return true;
    } else if (acceptedRoles.length === 1 && acceptedRoles[0] === null) {
      return true;
    }

    // Standard cases
    if (acceptedRoles.length === 1 && Array.isArray(acceptedRoles)) {
      return roles.filter((r) => acceptedRoles[0].includes(r.code)).length > 0;
    } else {
      return roles.filter((r) => acceptedRoles.includes(r.code)).length > 0;
    }
  };
}

export function usePersistedAuthentication() {
  const dispatch = useDispatch();
  const { expire } = useSelector(authenticationState);
  const expireIsSet = expire !== undefined && expire !== null;
  const now = Date.now();
  const delay = 5 * 60 * 1000; // 5 minutes in milliseconds
  const renew = expire - delay < now;

  useEffect(() => {
    if (expireIsSet && renew) {
      apiFetch("Auth/renew", { method: "POST" }).then((response) => {
        if (response.ok) {
          response.json().then((response) => {
            window.cookieStore.set("access-token", response.accessToken);
            dispatch(connect(response));
            apiFetch("Users/me", { method: "GET" }).then((response) => dispatch(setUser(response)));
            apiFetch("Roles/mine", { method: "GET" }).then((response) => dispatch(setRoles(response)));
          });
        }
      });
    }
  }, [dispatch, expireIsSet, renew]);
}
