import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("balances/getAll", async (sortOptions) => {
  const sortKey = sortOptions?.sortKey || "";
  const sortValue = sortOptions?.sortValue || "";
  const filter = sortOptions?.filter || "";
  const response = await apiFetch(`Balances?sortKey=${sortKey}&sortValue=${sortValue}&filter=${filter}`, {
    method: "GET",
  });
  return response;
});

export const refresh =
  (sortKey, sortValue, filter = null) =>
  (dispatch, getState) => {
    const sortOptions = { sortKey, sortValue, filter };
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
