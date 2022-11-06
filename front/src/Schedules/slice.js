import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  elements: [],
  sorting: [],
};

export default function schedulesReducer(state = initialState, action) {
  switch (action.type) {
    case "schedules/disconnect": {
      return { ...state, connected: false };
    }
    case "schedules/setSorting": {
      return { ...state, sorting: action.payload };
    }
    case "schedules/retrieveAll/pending": {
      return { ...state, connected: true, loading: true };
    }
    case "schedules/retrieveAll/rejected": {
      return { ...state, connected: false, loading: false };
    }
    case "schedules/retrieveAll/fulfilled": {
      return { ...state, loading: false, elements: action.payload };
    }
    default:
      return state;
  }
}

export const disconnect = () => ({ type: "schedules/disconnect" });
export const setSorting = (s) =>
  function (dispatch, getState) {
    let newValue = s;
    // check if s is an updater
    if (typeof s === "function") {
      const oldValue = schedulesState(getState()).sorting;
      newValue = s(oldValue);
    }
    dispatch({ type: "schedules/setSorting", payload: newValue });
  };

export const retrieveAll = () =>
  async function (dispatch, getState) {
    let params = "";

    const sorting = schedulesState(getState()).sorting;
    if (sorting.length > 0) {
      params = `?sortKey=${sorting[0].id}&sortValue=${sorting[0].desc ? "desc" : "asc"}`;
    }

    dispatch({ type: "schedules/retrieveAll/pending" });

    try {
      const response = await apiFetch(`Schedules${params}`, { method: "GET" });
      dispatch({ type: "schedules/retrieveAll/fulfilled", payload: response });
    } catch {
      dispatch({ type: "schedules/retrieveAll/rejected" });
    }
  };

export const schedulesState = (state) => state.schedules;
