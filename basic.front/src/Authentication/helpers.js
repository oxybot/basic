import { useSelector } from "react-redux";
import { authenticationState } from ".";

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
