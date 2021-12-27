import { configureStore } from "@reduxjs/toolkit";
import agreementsReducer from "../Agreements/slice";
import clientsReducer from "../Clients/slice";
import productsReducer from "../Products/slice";

export const store = configureStore({
  reducer: {
    agreements: agreementsReducer,
    clients: clientsReducer,
    products: productsReducer,
  },
});