import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("balances/getAll", async (sortOptions) => {
  if(sortOptions == null) {
    sortOptions = ["noSort", "User"];
  }
  const response = await apiFetch("Balances?sortKey=" + sortOptions[1] + "&sortValue=" + sortOptions[0] + "&filter=" + sortOptions[2], { method: "GET" });
  return response;
});

export const refresh = (sortValue, sortKey, search = null) => (dispatch, getState) => {
  const sortOptions = [sortValue, sortKey, search];
  const { connected, loading } = balancesState(getState());
  if (connected && !loading) {
      dispatch(getAll(sortOptions));
  }
};

export const balancesSlice = createSlice({
  name: "balances",
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

export const { disconnect } = balancesSlice.actions;

export const balancesState = (state) => state.balances;

export default balancesSlice.reducer;
