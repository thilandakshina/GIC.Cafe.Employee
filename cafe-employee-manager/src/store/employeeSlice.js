import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { employeeApi } from '../api/employeeApi';

export const fetchEmployees = createAsyncThunk(
  'employees/fetchEmployees',
  async (selectedCafe) => {
    return await employeeApi.getAll(selectedCafe);
  }
);

export const fetchEmployee = createAsyncThunk(
  'employees/fetchEmployee',
  async (id) => {
    return await employeeApi.getById(id);
  }
);

export const createEmployee = createAsyncThunk(
  'employees/createEmployee',
  async (employeeData) => {
    return await employeeApi.create(employeeData);
  }
);


export const updateEmployee = createAsyncThunk(
  'employees/updateEmployee',
  async ({ id, data }) => {
    return await employeeApi.update(id, data);
  }
);

export const deleteEmployee = createAsyncThunk(
  'employees/deleteEmployee',
  async (id) => {
    await employeeApi.delete(id);
    return id;
  }
);

const employeeSlice = createSlice({
  name: 'employees',
  initialState: {
    items: [],
    currentEmployee: null,
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
      .addCase(createEmployee.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createEmployee.fulfilled, (state, action) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createEmployee.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })
      .addCase(fetchEmployees.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchEmployees.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchEmployees.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message;
      })
      .addCase(fetchEmployee.fulfilled, (state, action) => {
        state.currentEmployee = action.payload;
      })
      .addCase(updateEmployee.fulfilled, (state, action) => {
        const index = state.items.findIndex(emp => emp.id === action.payload.id);
        if (index !== -1) {
          state.items[index] = action.payload;
        }
      })
      .addCase(deleteEmployee.fulfilled, (state, action) => {
        state.items = state.items.filter(emp => emp.id !== action.payload);
      });
  },
});

export const { clearError } = employeeSlice.actions;
export default employeeSlice.reducer;