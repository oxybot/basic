import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function clientsReducer(state = initialState, action) {
  switch (action.type) {
    case "clients/disconnect": {
      return { ...state, connected: false };
    }
    case "clients/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "clients/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "clients/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "clients/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "clients/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = clientsState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "clients/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = clientsState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "clients/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Clients${params}`, { method: "GET" });
      dispatch({ type: "clients/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "clients/retrieveAll/rejected" });
    }
  };

export const clientsState = (state) => state.clients;
