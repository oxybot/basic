import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";
import { useEffect } from "react";
import { EntityList } from "../Generic/EntityList";
import { sortValue, sortKey } from "../Generic/EntityList";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("users/getAll", async (sortValue, sortKey) => {
  console.log("getAll: " + sortValue);
  const response = await apiFetch("Users?sortKey=" + "UserName" + "&sortValue=" + sortValue, { method: "GET" });
  return response;
});

export const refresh = (sortValue, sortKey) => (dispatch, getState) => {
  const { connected, loading } = usersState(getState());
  console.log("refresh : " + sortValue);
  if (connected && !loading) {
      dispatch(getAll(sortValue, sortKey));
  }
};

export const usersSlice = createSlice({
  name: "users",
  initialState,
  reducers: {
    disconnect: (state) => {
      state.connected = false;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(getAll.pending, (state) => {
        state.connected = true;
        state.loading = true;
      })
      .addCase(getAll.fulfilled, (state, action) => {
        state.loading = false;
        state.values = action.payload;
      })
      .addCase(getAll.rejected, (state) => {
        state.connected = false;
        state.loading = false;
        state.values = [];
      });
  },
});

export const { disconnect } = usersSlice.actions;

export const usersState = (state) => state.users;

export default usersSlice.reducer;
