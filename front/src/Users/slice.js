import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiFetch } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

export const getAll = createAsyncThunk("users/getAll", async () => {
  const response = await apiFetch("Users", { method: "GET" });
  return response;
});

export const refresh = () => (dispatch, getState) => {
  const { connected, loading } = usersState(getState());
  if (connected && !loading) {
    dispatch(getAll());
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
