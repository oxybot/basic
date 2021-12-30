import { configureStore } from "@reduxjs/toolkit";
import agreementsReducer from "../Agreements/slice";
import balancesReducer from "../Balances/slice";
import clientsReducer from "../Clients/slice";
import eventCategoriesReducer from "../EventCategories/slice";
import productsReducer from "../Products/slice";
import usersReducer from "../Users/slice";

export const store = configureStore({
  reducer: {
    agreements: agreementsReducer,
    balances: balancesReducer,
    clients: clientsReducer,
    products: productsReducer,
    users: usersReducer,
    eventCategories: eventCategoriesReducer,
  },
});
