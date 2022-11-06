import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function usersReducer(state = initialState, action) {
  switch (action.type) {
    case "users/disconnect": {
      return { ...state, connected: false };
    }
    case "users/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "users/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "users/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "users/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "users/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = usersState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "users/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = usersState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "users/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Users${params}`, { method: "GET" });
      dispatch({ type: "users/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "users/retrieveAll/rejected" });
    }
  };

export const usersState = (state) => state.users;
