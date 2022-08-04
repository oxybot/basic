import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("events/getAll", async (sortOptions) => {
  if(sortOptions == null) {
    sortOptions = [0, "None"];
  }
  const response = await apiFetch("Events?sortKey=" + sortOptions[1] + "&sortValue=" + sortOptions[0], { method: "GET" });
  return response;
});

export const refresh = (sortValue, sortKey) => (dispatch, getState) => {
  const sortOptions = [sortValue, sortKey];
  const { connected, loading } = eventsState(getState());
  if (connected && !loading) {
      dispatch(getAll(sortOptions));
  }
};

export const slice = createSlice({
  name: "events",
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

export const eventsState = (state) => state.events;

export default slice.reducer;
