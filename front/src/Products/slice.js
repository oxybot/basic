import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function productsReducer(state = initialState, action) {
  switch (action.type) {
    case "products/disconnect": {
      return { ...state, connected: false };
    }
    case "products/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "products/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "products/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "products/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "products/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = productsState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "products/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = productsState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "products/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Products${params}`, { method: "GET" });
      dispatch({ type: "products/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "products/retrieveAll/rejected" });
    }
  };

export const productsState = (state) => state.products;
