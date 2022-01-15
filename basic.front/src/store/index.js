import { configureStore } from "@reduxjs/toolkit";
import agreementsReducer from "../Agreements/slice";
import alertsReducer from "../Alerts/slice";
import authenticationReducer from "../Authentication/slice";
import balancesReducer from "../Balances/slice";
import clientsReducer from "../Clients/slice";
import eventCategoriesReducer from "../EventCategories/slice";
import eventsReducer from "../Events/slice";
import globalDaysOffReducer from "../GlobalDaysOff/slice";
import productsReducer from "../Products/slice";
import scheduleReducer from "../Schedules/slice";
import usersReducer from "../Users/slice";

export const store = configureStore({
  reducer: {
    authentication: authenticationReducer,
    agreements: agreementsReducer,
    alerts: alertsReducer,
    balances: balancesReducer,
    clients: clientsReducer,
    eventCategories: eventCategoriesReducer,
    events: eventsReducer,
    globalDaysOff: globalDaysOffReducer,
    products: productsReducer,
    schedules: scheduleReducer,
    users: usersReducer,
  },
});
