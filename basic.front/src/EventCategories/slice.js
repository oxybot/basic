import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("event-categories/getAll", async () => {
  const response = await apiFetch("EventCategories", { method: "GET" });
  return response;
});

export const refresh = () => (dispatch, getState) => {
  const { connected, loading } = eventCategoriesState(getState());
  if (connected && !loading) {
    dispatch(getAll());
  }
};

export const eventCategoriesSlice = createSlice({
  name: "eventCategories",
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

export const { disconnect } = eventCategoriesSlice.actions;

export const eventCategoriesState = (state) => state.eventCategories;

export default eventCategoriesSlice.reducer;
