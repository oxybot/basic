import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function eventsReducer(state = initialState, action) {
  switch (action.type) {
    case "events/disconnect": {
      return { ...state, connected: false };
    }
    case "events/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "events/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "events/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "events/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "events/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = eventsState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "events/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = eventsState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "events/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Events${params}`, { method: "GET" });
      dispatch({ type: "events/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "events/retrieveAll/rejected" });
    }
  };

export const eventsState = (state) => state.events;
