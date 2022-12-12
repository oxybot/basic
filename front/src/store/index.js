import { applyMiddleware, configureStore } from "@reduxjs/toolkit";
import alertsReducer from "../Alerts/slice";
import authenticationReducer from "../Authentication/slice";
import balancesReducer from "../Balances/slice";
import eventCategoriesReducer from "../EventCategories/slice";
import eventsReducer from "../Events/slice";
import globalDaysOffReducer from "../GlobalDaysOff/slice";
import productsReducer from "../Products/slice";
import scheduleReducer from "../Schedules/slice";
import usersReducer from "../Users/slice";

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
      balances: balancesReducer,
      eventCategories: eventCategoriesReducer,
      events: eventsReducer,
      globalDaysOff: globalDaysOffReducer,
      products: productsReducer,
      schedules: scheduleReducer,
      users: usersReducer,
    },
  },
  middlewareEnhancer
);
