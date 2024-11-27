import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { cafeApi } from '../api/cafeApi';

export const fetchCafes = createAsyncThunk(
  'cafes/fetchCafes',
  async (locationFilter) => {
    return await cafeApi.getAll(locationFilter);
  }
);

export const fetchCafe = createAsyncThunk(
  'cafes/fetchCafe',
  async (id) => {
    return await cafeApi.getById(id);
  }
);

export const createCafe = createAsyncThunk(
  'cafes/createCafe',
  async (cafeData) => {
    return await cafeApi.create(cafeData);
  }
);

export const updateCafe = createAsyncThunk(
  'cafes/updateCafe',
  async ({ id, data }) => {
    return await cafeApi.update(id, data);
  }
);

export const deleteCafe = createAsyncThunk(
  'cafes/deleteCafe',
  async (id) => {
    await cafeApi.delete(id);
    return id;
  }
);

const cafeSlice = createSlice({
  name: 'cafes',
  initialState: {
    items: [],
    currentCafe: null,
    loading: false,
    error: null,
  },
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCafes.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCafes.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
        state.error = null;
      })
      .addCase(fetchCafes.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })
      .addCase(fetchCafe.fulfilled, (state, action) => {
        state.currentCafe = action.payload;
        state.error = null;
      })
      .addCase(createCafe.fulfilled, (state, action) => {
        state.items.push(action.payload);
        state.error = null;
      })
      .addCase(updateCafe.fulfilled, (state, action) => {
        const index = state.items.findIndex(cafe => cafe.id === action.payload.id);
        if (index !== -1) {
          state.items[index] = action.payload;
        }
        state.error = null;
      })
      .addCase(deleteCafe.fulfilled, (state, action) => {
        state.items = state.items.filter(cafe => cafe.id !== action.payload);
        state.error = null;
      });
  },
});

export const { clearError } = cafeSlice.actions;
export const getAllCafes = (state) => state.cafes.items;
export default cafeSlice.reducer;