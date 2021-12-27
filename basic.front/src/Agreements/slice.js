import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("agreements/getAll", async () => {
  const response = await apiFetch("Agreements", { method: "GET" });
  return response;
});

export const refresh = () => (dispatch, getState) => {
  const { connected, loading } = agreementsState(getState());
  if (connected && !loading) {
    dispatch(getAll());
  }
};

export const slice = createSlice({
  name: "agreements",
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

export const { disconnect } = slice.actions;

export const agreementsState = (state) => state.agreements;

export default slice.reducer;
