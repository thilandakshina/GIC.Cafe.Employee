import React, { useState, useMemo } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { AgGridReact } from 'ag-grid-react';
import { 
  Button, 
  TextField, 
  Box, 
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Paper,
  InputAdornment,
  IconButton,
  Tooltip,
  Alert,
  Fade
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import PeopleIcon from '@mui/icons-material/People';
import { fetchCafes, deleteCafe, getAllCafes } from '../store/cafeSlice';

export function CafesPage() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [locationFilter, setLocationFilter] = useState('');
  const [deleteId, setDeleteId] = useState(null);
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);
  const { items: cafes, loading: isLoading, error } = useSelector(state => state.cafes);

  React.useEffect(() => {
    dispatch(fetchCafes(locationFilter));
  }, [dispatch, locationFilter]);

  const columnDefs = useMemo(() => [
    {
      field: 'logo',
      headerName: 'Logo',
      width: 100,
      cellRenderer: params => (
        <img 
          src={`data:image/png;base64,${params.value}`}
          alt="cafe logo" 
          style={{ 
            width: 50, 
            height: 50, 
            borderRadius: '50%',
            objectFit: 'cover' 
          }} 
        />
      )
    },
    { field: 'name', headerName: 'Name', flex: 1 },
    { 
      field: 'description', 
      headerName: 'Description', 
      flex: 2,
      tooltipField: 'description'
    },
    { 
      field: 'employeeCount', 
      headerName: 'Employees', 
      width: 130,
      cellRenderer: params => (
        <Button
          size="small"
          startIcon={<PeopleIcon />}
          onClick={() => navigate(`/employees?cafe=${params.data.id}&cafename=${params.data.name}`)}
          sx={{ minWidth: '90px' }}
        >
          {params.value}
        </Button>
      )
    },
    { field: 'location', headerName: 'Location', flex: 1 },
    {
      headerName: 'Actions',
      width: 200,
      cellRenderer: params => (
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Tooltip title="Edit Cafe">
            <IconButton
              size="small"
              onClick={() => navigate(`/cafe/edit/${params.data.id}`)}
            >
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Delete Cafe">
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
  ], [navigate]);

  const handleDelete = async () => {
    if (deleteId) {
      try {
        await dispatch(deleteCafe(deleteId)).unwrap();
        setShowSuccessAlert(true);
        setTimeout(() => setShowSuccessAlert(false), 3000);
        setDeleteId(null);
      } catch (error) {
        console.error('Error deleting cafe:', error);
      }
    }
  };

  const handleClearFilter = () => {
    setLocationFilter('');
  };

  const handleAddNew = () => {
    navigate('/cafe/add');
  };

  if (error) {
    return <Alert severity="error">Error loading cafes: {error}</Alert>;
  }

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
          Cafe deleted successfully
        </Alert>
      </Fade>

      <Paper sx={{ p: 3, mb: 3 }}>
        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: 3
        }}>
          <Typography variant="h4">Cafes</Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleAddNew}
          >
            Add New Cafe
          </Button>
        </Box>

        <TextField
          fullWidth
          label="Filter by Location"
          variant="outlined"
          value={locationFilter}
          onChange={(e) => setLocationFilter(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
            endAdornment: locationFilter && (
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
        ) : cafes.length === 0 ? (
          <Alert severity="info">
            {locationFilter 
              ? `No Cafes found in location "${locationFilter}"`
              : 'No Cafes available'}
          </Alert>
        ) : (
          <div className="ag-theme-material" style={{ height: 600, width: '100%' }}>
            <AgGridReact
              rowData={cafes}
              columnDefs={columnDefs}
              defaultColDef={{
                sortable: true,
                filter: true,
                resizable: true
              }}
              pagination={true}
              paginationPageSize={10}
              tooltipShowDelay={0}
              tooltipHideDelay={2000}
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
          Are you sure you want to delete this cafe? This will also remove all employee associations.
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteId(null)}>Cancel</Button>
          <Button 
            onClick={handleDelete} 
            color="error" 
            variant="contained"
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}