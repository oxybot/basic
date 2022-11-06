import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function agreementsReducer(state = initialState, action) {
  switch (action.type) {
    case "agreements/disconnect": {
      return { ...state, connected: false };
    }
    case "agreements/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "agreements/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "agreements/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "agreements/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "agreements/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = agreementsState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "agreements/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = agreementsState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "agreements/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Agreements${params}`, { method: "GET" });
      dispatch({ type: "agreements/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "agreements/retrieveAll/rejected" });
    }
  };

export const agreementsState = (state) => state.agreements;
