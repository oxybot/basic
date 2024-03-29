import { applyMiddleware, configureStore } from "@reduxjs/toolkit";
import alertsReducer from "../Alerts/slice";
import authenticationReducer from "../Authentication/slice";

const asyncFunctionMiddleware = (storeAPI) => (next) => (action) => {
  // If the "action" is actually a function instead...
  if (typeof action === "function") {
    // then call the function and pass `dispatch` and `getState` as arguments
    return action(storeAPI.dispatch, storeAPI.getState);
  }

  // Otherwise, it's a normal action - send it onwards
  return next(action);
};

const middlewareEnhancer = applyMiddleware(asyncFunctionMiddleware);
export const store = configureStore(
  {
    reducer: {
      authentication: authenticationReducer,
      alerts: alertsReducer,
    },
  },
  middlewareEnhancer
);
