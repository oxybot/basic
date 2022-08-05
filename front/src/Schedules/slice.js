import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("schedules/getAll", async (sortOptions) => {
  if(sortOptions == null) {
    sortOptions = ["noSort", "None"];
  }
  const response = await apiFetch("Schedules?sortKey=" + sortOptions[1] + "&sortValue=" + sortOptions[0] + "&filter=" + sortOptions[2], { method: "GET" });
  return response;
});

export const refresh = (sortValue, sortKey, search = null) => (dispatch, getState) => {
  const sortOptions = [sortValue, sortKey, search];
  const { connected, loading } = schedulesState(getState());
  if (connected && !loading) {
      dispatch(getAll(sortOptions));
  }
};

export const slice = createSlice({
  name: "schedules",
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

export const schedulesState = (state) => state.schedules;

export default slice.reducer;
