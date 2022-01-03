import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  connected: false,
  token: null,
  expire: null,
};

export const authenticationSlice = createSlice({
  name: "authentication",
  initialState,
  reducers: {
    connect: (state, action) => {
      console.log(action.payload);
      state.connected = true;
      state.token = action.payload.token;
      state.expire = action.payload.expireIn;
    },
    disconnect: (state) => {
      state.connected = false;
      state.token = null;
      state.expire = null;
    },
  },
});

export const { connect, disconnect } = authenticationSlice.actions;

export const authenticationState = (state) => state.authentication;

export default authenticationSlice.reducer;
