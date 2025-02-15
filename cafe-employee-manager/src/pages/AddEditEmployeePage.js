import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { 
  Box, 
  Button, 
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
  MenuItem,
  Select,
  InputLabel,
  Alert
} from '@mui/material';
import { ReusableTextBox } from '../components/ReusableTextBox';
import { GenderType } from '../constants/enums';
import { 
  fetchEmployee, 
  createEmployee, 
  updateEmployee,
  clearError,
  fetchEmployees 
} from '../store/employeeSlice';
import { fetchCafes } from '../store/cafeSlice';

export function AddEditEmployeePage() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { id: employeeId } = useParams();
  const isEdit = !!employeeId;

  const [showLeaveDialog, setShowLeaveDialog] = useState(false);
  const [isDirty, setIsDirty] = useState(false);
  const [errors, setErrors] = useState({});

  const { currentEmployee, loading, error: apiError } = useSelector(state => state.employees);
  const { items: cafes, loading: cafesLoading } = useSelector(state => state.cafes);

  const [formData, setFormData] = useState({
    name: '',
    emailAddress: '',
    phoneNumber: '',
    gender: '',
    cafeId: ''
  });

  useEffect(() => {
    dispatch(fetchCafes());
    if (isEdit) {
      dispatch(fetchEmployee(employeeId));
    }
  }, [dispatch, employeeId, isEdit]);

  useEffect(() => {
    if (currentEmployee && isEdit) {
      setFormData({
        name: currentEmployee.name || '',
        emailAddress: currentEmployee.emailAddress || '',
        phoneNumber: currentEmployee.phoneNumber || '',
        gender: currentEmployee.gender?.toString() || '',
        cafeId: currentEmployee.cafeId ? currentEmployee.cafeId.toString() : ''
      });
    }
  }, [currentEmployee, isEdit]);

  const validateField = (name, value) => {
    let newErrors = { ...errors };

    switch (name) {
      case 'name':
        if (!value || value.length < 6 || value.length > 10) {
          newErrors[name] = 'Name must be between 6 and 10 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'emailAddress':
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!value || !emailRegex.test(value)) {
          newErrors[name] = 'Please enter a valid email address';
        } else {
          delete newErrors[name];
        }
        break;
      case 'phoneNumber':
        const phoneRegex = /^[89]\d{7}$/;
        if (!value || !phoneRegex.test(value)) {
          newErrors[name] = 'Phone number must start with 8 or 9 and have 8 digits';
        } else {
          delete newErrors[name];
        }
        break;
      case 'gender':
        if (!value) {
          newErrors[name] = 'Please select a gender';
        } else {
          delete newErrors[name];
        }
        break;
      default:
        break;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    setIsDirty(true);
    dispatch(clearError());
    validateField(name, value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    const fieldsToValidate = ['name', 'emailAddress', 'phoneNumber', 'gender'];
    const isValid = fieldsToValidate.every(field => validateField(field, formData[field]));

    if (isValid) {
      try {
        const submitData = {
          ...formData,
          gender: parseInt(formData.gender),
          cafeId: formData.cafeId || null
        };

        if (isEdit) {
          await dispatch(updateEmployee({ id: employeeId, data: submitData })).unwrap();
        } else {
          await dispatch(createEmployee(submitData)).unwrap();
          await dispatch(fetchEmployees(null));
        }
        navigate('/employees');
      } catch (error) {
        console.error('Form submission error:', error);
      }
    }
  };

  if (loading || cafesLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <Typography>Loading...</Typography>
      </Box>
    );
  }

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 600, mx: 'auto', mt: 4 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        {isEdit ? 'Edit Employee' : 'Add New Employee'}
      </Typography>

      {apiError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {apiError}
        </Alert>
      )}

      <ReusableTextBox
        name="name"
        label="Name"
        value={formData.name}
        onChange={handleChange}
        error={errors.name}
        helperText={errors.name || '6-10 characters'}
        required
      />

      <ReusableTextBox
        name="emailAddress"
        label="Email Address"
        type="email"
        value={formData.emailAddress}
        onChange={handleChange}
        error={errors.emailAddress}
        helperText={errors.emailAddress}
        required
      />

      <ReusableTextBox
        name="phoneNumber"
        label="Phone Number"
        value={formData.phoneNumber}
        onChange={handleChange}
        error={errors.phoneNumber}
        helperText={errors.phoneNumber || 'Must start with 8 or 9 and have 8 digits'}
        required
      />

      <FormControl fullWidth margin="normal" error={!!errors.gender} required>
        <FormLabel id="gender-label">Gender</FormLabel>
        <RadioGroup
          aria-labelledby="gender-label"
          name="gender"
          value={formData.gender}
          onChange={handleChange}
          row
        >
          <FormControlLabel value={GenderType.Male.toString()} control={<Radio />} label="Male" />
          <FormControlLabel value={GenderType.Female.toString()} control={<Radio />} label="Female" />
        </RadioGroup>
        {errors.gender && (
          <Typography color="error" variant="caption">
            {errors.gender}
          </Typography>
        )}
      </FormControl>

      <FormControl fullWidth margin="normal">
        <InputLabel id="cafe-label">Assigned Cafe</InputLabel>
        <Select
          labelId="cafe-label"
          id="cafe-select"
          name="cafeId"
          value={formData.cafeId}
          onChange={handleChange}
          label="Assigned Cafe"
        >
          <MenuItem value="">
            <em>None</em>
          </MenuItem>
          {cafes.map(cafe => (
            <MenuItem key={cafe.id} value={cafe.id.toString()}>
              {cafe.name}
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      <Box sx={{ mt: 4, display: 'flex', gap: 2 }}>
        <Button
          variant="contained"
          type="submit"
          disabled={Object.keys(errors).length > 0 || loading}
        >
          {isEdit ? 'Update' : 'Create'} Employee
        </Button>
        <Button
          variant="outlined"
          onClick={() => isDirty ? setShowLeaveDialog(true) : navigate('/employees')}
        >
          Cancel
        </Button>
      </Box>

      <Dialog open={showLeaveDialog} onClose={() => setShowLeaveDialog(false)}>
        <DialogTitle>Unsaved Changes</DialogTitle>
        <DialogContent>
          Are you sure you want to leave? All unsaved changes will be lost.
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowLeaveDialog(false)}>Stay</Button>
          <Button 
            onClick={() => {
              setShowLeaveDialog(false);
              navigate('/employees');
            }} 
            color="error"
          >
            Leave
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}