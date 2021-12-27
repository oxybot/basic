import { configureStore } from "@reduxjs/toolkit";
import clientsReducer from "../Clients/slice";

export const store = configureStore({
  reducer: {
    clients: clientsReducer,
  },
});
