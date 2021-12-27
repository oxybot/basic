import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiUrl, retries } from "../api";

const initialState = {
  connected: false,
  loading: false,
  values: [],
};

function useApiFetch(url, options) {
  const uri = typeof url === "string" ? apiUrl(url) : url;
  return retries(() => fetch(apiUrl(uri), options)).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      console.error("Can't retrieve data");
      console.log(response);
      throw new Error("Can't retrieve data");
    }
  });
}

export const getAll = createAsyncThunk("clients/getAll", async () => {
  const response = await useApiFetch("Clients", { method: "GET" });
  return response;
});

export const refresh = () => (dispatch, getState) => {
  const { connected, loading } = clientsState(getState());
  if (connected && !loading) {
    dispatch(getAll());
  }
};

export const clientsSlice = createSlice({
  name: "clients",
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

export const { disconnect } = clientsSlice.actions;

export const clientsState = (state) => state.clients;

export default clientsSlice.reducer;
