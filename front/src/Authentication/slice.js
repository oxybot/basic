import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  connected: false,
  authenticated: false,
  token: null,
  expire: null,
  user: null,
  roles: null,
};

export const authenticationSlice = createSlice({
  name: "authentication",
  initialState,
  reducers: {
    connect: (_, action) => {
      return {
        ...initialState,
        connected: true,
        token: action.payload.token,
        expire: action.payload.expire,
      };
    },
    setUser: (state, action) => {
      state.user = action.payload;
      if (state.user !== null && state.roles !== null) {
        state.authenticated = true;
      }
    },
    setRoles: (state, action) => {
      state.roles = action.payload;
      if (state.user !== null && state.roles !== null) {
        state.authenticated = true;
      }
    },
    disconnect: () => ({ ...initialState })
  },
});

export const { connect, setUser, setRoles, disconnect } = authenticationSlice.actions;

export const authenticationState = (state) => state.authentication;

export default authenticationSlice.reducer;
