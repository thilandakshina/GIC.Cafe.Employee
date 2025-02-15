import { configureStore } from '@reduxjs/toolkit';
import employeeReducer from './employeeSlice';
import cafeReducer from './cafeSlice';

export const store = configureStore({
  reducer: {
    employees: employeeReducer,
    cafes: cafeReducer,
  },
});

export default store;