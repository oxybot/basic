import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function eventCategoriesReducer(state = initialState, action) {
  switch (action.type) {
    case "eventCategories/disconnect": {
      return { ...state, connected: false };
    }
    case "eventCategories/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "eventCategories/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "eventCategories/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "eventCategories/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "eventCategories/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = eventCategoriesState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "eventCategories/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = eventCategoriesState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "eventCategories/retrieveAll/pending" });

    try {
      const response = await apiFetch(`EventCategories${params}`, { method: "GET" });
      dispatch({ type: "eventCategories/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "eventCategories/retrieveAll/rejected" });
    }
  };

export const eventCategoriesState = (state) => state.eventCategories;
