import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import { CafesPage } from './pages/CafesPage';
import { EmployeesPage } from './pages/EmployeesPage';
import { AddEditCafePage } from './pages/AddEditCafePage';
import { AddEditEmployeePage } from './pages/AddEditEmployeePage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        index: true,
        element: <CafesPage />
      },
      {
        path: 'employees',
        element: <EmployeesPage />
      },
      {
        path: 'employee/edit/:id',
        element: <AddEditEmployeePage />
      },
      {
        path: 'employee/add',
        element: <AddEditEmployeePage />
      },
      {
        path: 'cafe/add',
        element: <AddEditCafePage />
      },
      {
        path: 'cafe/edit/:id',
        element: <AddEditCafePage />
      }
    ]
  }
]);

export default router;