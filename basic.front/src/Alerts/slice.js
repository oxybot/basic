import { createSlice } from "@reduxjs/toolkit";

const initialState = [];

function generateId() {
  const min = 1;
  const max = 1000000;
  return Math.floor(Math.random() * (max - min + 1) + min);
}

export function addWarning(title, message) {
  return (dispatch, _) => {
    dispatch(hideAllAlerts());
    dispatch(addAlert({ identifier: generateId(), category: "warning", title, message }));
  };
}

export function addError(title, message) {
  return (dispatch, _) => {
    dispatch(hideAllAlerts());
    dispatch(addAlert({ identifier: generateId(), category: "danger", title, message }));
  };
}

export const alertsSlice = createSlice({
  name: "alerts",
  initialState,
  reducers: {
    addAlert: (state, action) => {
      return [...state, { ...action.payload, status: "show" }];
    },
    removeAlert: (state, action) => {
      var updated = state.filter((e) => e.identifier !== action.payload);
      return updated;
    },
    hideAlert: (state, action) => {
      const index = state.findIndex((a) => a.identifier === action.payload);
      if (index >= 0) {
        state[index].status = "hide";
      }
    },
    hideAllAlerts: (state) => {
      state.forEach((a) => (a.status = "hide"));
    },
  },
});

export const { addAlert, removeAlert, hideAlert, hideAllAlerts } = alertsSlice.actions;

export const alertsState = (state) => state.alerts;

export default alertsSlice.reducer;
