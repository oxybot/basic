import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function balancesReducer(state = initialState, action) {
  switch (action.type) {
    case "balances/disconnect": {
      return { ...state, connected: false };
    }
    case "balances/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "balances/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "balances/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "balances/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "balances/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = balancesState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "balances/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = balancesState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "balances/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Balances${params}`, { method: "GET" });
      dispatch({ type: "balances/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "balances/retrieveAll/rejected" });
    }
  };

export const balancesState = (state) => state.balances;
