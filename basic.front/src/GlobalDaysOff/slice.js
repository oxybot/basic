import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("global-daysoff/getAll", async () => {
  const response = await apiFetch("GlobalDaysOff", { method: "GET" });
  return response;
});

export const refresh = () => (dispatch, getState) => {
  const { connected, loading } = globalDaysOffState(getState());
  if (connected && !loading) {
    dispatch(getAll());
  }
};

export const globalDaysOffSlice = createSlice({
  name: "globalDaysOff",
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

export const { disconnect } = globalDaysOffSlice.actions;

export const globalDaysOffState = (state) => state.globalDaysOff;

export default globalDaysOffSlice.reducer;
