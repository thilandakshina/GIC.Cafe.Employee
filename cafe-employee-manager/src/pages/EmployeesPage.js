import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation, Link as RouterLink } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { AgGridReact } from 'ag-grid-react';
import { 
  Button, 
  Box, 
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Paper,
  Breadcrumbs,
  Link as MuiLink,
  IconButton,
  Tooltip,
  Alert,
  Fade,
  TextField,
  InputAdornment,
  Chip
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import HomeIcon from '@mui/icons-material/Home';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import WorkIcon from '@mui/icons-material/Work';
import PersonIcon from '@mui/icons-material/Person';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-material.css';
import { getGenderLabel } from '../constants/enums';
import { fetchEmployees, deleteEmployee } from '../store/employeeSlice';



export function EmployeesPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch();
  const [deleteId, setDeleteId] = useState(null);
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);
  const [nameFilter, setNameFilter] = useState('');
  const [isDeleting, setIsDeleting] = useState(false);

  const searchParams = new URLSearchParams(location.search);
  const selectedCafe = searchParams.get('cafe');
  const selectedCafeName = searchParams.get('cafename');

  const { items: employees, loading: isLoading } = useSelector(state => state.employees);

  useEffect(() => {
    dispatch(fetchEmployees(selectedCafe));
  }, [dispatch, selectedCafe]);

  const filteredEmployees = employees.filter(employee => 
    employee?.name?.toLowerCase().includes(nameFilter.toLowerCase())
  );

  const handleDelete = async () => {
    setIsDeleting(true);
    try {
      await dispatch(deleteEmployee(deleteId)).unwrap();
      setShowSuccessAlert(true);
      setTimeout(() => setShowSuccessAlert(false), 3000);
    } catch (error) {
      console.error('Error deleting employee:', error);
    } finally {
      setIsDeleting(false);
      setDeleteId(null);
    }
  };
  const handleClearFilter = () => {
    setNameFilter('');
  };

  const handleAddNew = () => {
    navigate('/employee/add'); 
  };

  const columnDefs = [
    { 
      field: 'id', 
      hide: true
    },
    { 
      field: 'employeeId', 
      headerName: 'Employee ID',
      width: 130,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PersonIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'name', 
      headerName: 'Name', 
      flex: 1,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {params.value}
        </Box>
      )
    },
    { 
      field: 'emailAddress',
      headerName: 'Email', 
      flex: 1,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <EmailIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'phoneNumber',
      headerName: 'Phone', 
      width: 150,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <PhoneIcon sx={{ fontSize: 20, color: 'primary.main' }} />
          {params.value}
        </Box>
      )
    },
    { 
      field: 'daysWorked',
      headerName: 'Days Worked', 
      width: 130,
      sort: 'desc',
      cellRenderer: params => (
        <Chip
          icon={<WorkIcon />}
          label={params.value}
          color="primary"
          size="small"
          sx={{ '& .MuiChip-icon': { fontSize: 16 } }}
        />
      )
    },
    { 
      field: 'cafeName',
      headerName: 'Cafe', 
      flex: 1,
      cellRenderer: params => (
        <Chip
          icon={<HomeIcon />}
          label={params.value || 'Unassigned'}
          variant={params.value ? 'filled' : 'outlined'}
          color={params.value ? 'default' : 'warning'}
          size="small"
          sx={{ '& .MuiChip-icon': { fontSize: 16 } }}
        />
      )
    },
    { 
      field: 'gender',
      headerName: 'Gender',
      width: 120,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {getGenderLabel(params.value)}
        </Box>
      )
    },
    {
      headerName: 'Actions',
      width: 120,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Tooltip title="Edit Employee">
          <IconButton
              size="small"
              onClick={() => {
                const id = params.data.id;
                navigate(`/employee/edit/${id}`);
              }}
            >
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Delete Employee">
            <IconButton
              size="small"
              color="error"
              onClick={() => setDeleteId(params.data.id)}
            >
              <DeleteIcon />
            </IconButton>
          </Tooltip>
        </Box>
      )
    }
  ];

  return (
    <Box>
      <Fade in={showSuccessAlert}>
        <Alert 
          severity="success" 
          sx={{ 
            position: 'fixed', 
            top: 20, 
            right: 20, 
            zIndex: 1000
          }}
        >
          Employee deleted successfully
        </Alert>
      </Fade>

      <Paper sx={{ p: 3, mb: 3 }}>
        <Breadcrumbs sx={{ mb: 2 }}>
          <MuiLink
            component={RouterLink}
            to="/"
            sx={{ 
              display: 'flex', 
              alignItems: 'center',
              gap: 0.5,
              textDecoration: 'none'
            }}
          >
            <HomeIcon sx={{ fontSize: 20 }} />
            Cafes
          </MuiLink>
          <Typography color="text.primary" sx={{ display: 'flex', alignItems: 'center' }}>
            <PersonIcon sx={{ mr: 0.5 }} />
            {selectedCafe ? `${selectedCafeName} Employees` : 'All Employees'}
          </Typography>
        </Breadcrumbs>

        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: 3
        }}>
          <Typography variant="h4">
            {selectedCafe ? `${selectedCafeName} Employees` : 'All Employees'}
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleAddNew}
          >
            Add New Employee
          </Button>
        </Box>

        <TextField
          fullWidth
          label="Filter by Name"
          variant="outlined"
          value={nameFilter}
          onChange={(e) => setNameFilter(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
            endAdornment: nameFilter && (
              <InputAdornment position="end">
                <IconButton size="small" onClick={handleClearFilter}>
                  <ClearIcon />
                </IconButton>
              </InputAdornment>
            )
          }}
          sx={{ mb: 2 }}
        />

        {isLoading ? (
          <Typography>Loading...</Typography>
        ) : filteredEmployees.length === 0 ? (
          <Alert severity="info">
            {nameFilter 
              ? `No employees found with name "${nameFilter}"`
              : selectedCafe 
                ? `No employees found in ${selectedCafeName}`
                : 'No employees available'}
          </Alert>
        ) : (
          <div className="ag-theme-material" style={{ height: 600, width: '100%' }}>
            <AgGridReact
              rowData={filteredEmployees}
              columnDefs={columnDefs}
              defaultColDef={{
                sortable: true,
                filter: true,
                resizable: true
              }}
              pagination={true}
              paginationPageSize={10}
            />
          </div>
        )}
      </Paper>

      <Dialog 
        open={!!deleteId} 
        onClose={() => setDeleteId(null)}
        maxWidth="xs"
        fullWidth
      >
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          Are you sure you want to delete this employee?
        </DialogContent>
        <DialogActions>
          <Button 
            onClick={() => setDeleteId(null)} 
            disabled={isDeleting}
          >
            Cancel
          </Button>
          <Button 
            onClick={handleDelete} 
            color="error" 
            variant="contained"
            disabled={isDeleting}
          >
            {isDeleting ? 'Deleting...' : 'Delete'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}