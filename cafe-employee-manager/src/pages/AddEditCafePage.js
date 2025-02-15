import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { Box, CircularProgress, Alert } from '@mui/material';
import { CafeForm } from '../components/cafe/CafeForm';
import { UnsavedChangesDialog } from '../components/cafe/UnsavedChangesDialog';
import { 
  fetchCafe, 
  createCafe, 
  updateCafe, 
  clearError 
} from '../store/cafeSlice';

export function AddEditCafePage() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { id: cafeId } = useParams();
  const isEdit = Boolean(cafeId);

  const { currentCafe, loading, error } = useSelector(state => state.cafes);
  
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    location: '',
    logo: null
  });
  
  const [showLeaveDialog, setShowLeaveDialog] = useState(false);
  const [isDirty, setIsDirty] = useState(false);
  const [errors, setErrors] = useState({});
  const [previewLogo, setPreviewLogo] = useState(null);
  const [submitError, setSubmitError] = useState('');

  useEffect(() => {
    if (isEdit) {
      dispatch(fetchCafe(cafeId));
    }
  }, [dispatch, cafeId, isEdit]);

  useEffect(() => {
    if (currentCafe && isEdit) {
      setFormData({
        name: currentCafe.name || '',
        description: currentCafe.description || '',
        location: currentCafe.location || '',
        logo: currentCafe.logo || null 
      });
      
      if (currentCafe.logo) {
        setPreviewLogo(`data:image/png;base64,${currentCafe.logo}`);
      }
    }
  }, [currentCafe, isEdit]);

  const validateField = (name, value) => {
    const newErrors = { ...errors };
  
    switch (name) {
      case 'name':
        if (!value || value.length < 6 || value.length > 10) {
          newErrors[name] = 'Name must be between 6 and 10 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'description':
        if (!value) {
          newErrors[name] = 'Description is required';
        } else if (value.length > 256) {
          newErrors[name] = 'Description must be less than 256 characters';
        } else {
          delete newErrors[name];
        }
        break;
      case 'location':
        if (!value) {
          newErrors[name] = 'Location is required';
        } else {
          delete newErrors[name];
        }
        break;
    }
  
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateForm = () => {
    const requiredFields = ['name', 'description', 'location'];
    const newErrors = {};

    requiredFields.forEach(field => {
      if (!formData[field]) {
        newErrors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} is required`;
      } else {
        validateField(field, formData[field]);
      }
    });

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    setIsDirty(true);
    validateField(name, value);
    setSubmitError('');
    dispatch(clearError());
  };

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file) {
      if (file.size <= 2 * 1024 * 1024) {
        setFormData(prev => ({ ...prev, logo: file }));
        setIsDirty(true);
        
        setErrors(prev => {
          const newErrors = { ...prev };
          delete newErrors.logo;
          return newErrors;
        });
        
        const reader = new FileReader();
        reader.onloadend = () => {
          setPreviewLogo(reader.result);
        };
        reader.readAsDataURL(file);
      } else {
        setErrors(prev => ({ ...prev, logo: 'File size must be less than 2MB' }));
        event.target.value = '';
      }
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setSubmitError('');
    
    if (validateForm()) {
      try {
        if (isEdit) {
          const updateData = {
            name: formData.name,
            description: formData.description,
            location: formData.location,
            logo: formData.logo instanceof File ? formData.logo : 
                  (typeof formData.logo === 'string' ? formData.logo : null)
          };
          await dispatch(updateCafe({ id: cafeId, data: updateData })).unwrap();
        } else {
          await dispatch(createCafe(formData)).unwrap();
        }
        navigate('/');
      } catch (error) {
        setSubmitError(error.message || 'Failed to save cafe');
      }
    }
  };

  const handleNavigateAway = () => {
    if (isDirty) {
      setShowLeaveDialog(true);
    } else {
      navigate('/');
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  if (error && isEdit) {
    return (
      <Alert severity="error" sx={{ mt: 2 }}>
        Error loading cafe: {error}
      </Alert>
    );
  }

  return (
    <>
      <CafeForm
        formData={formData}
        errors={errors}
        submitError={submitError}
        isEdit={isEdit}
        isPending={loading}
        onSubmit={handleSubmit}
        onChange={handleChange}
        onFileChange={handleFileChange}
        onCancel={handleNavigateAway}
        previewLogo={previewLogo}
      />
      
      <UnsavedChangesDialog
        open={showLeaveDialog}
        onStay={() => setShowLeaveDialog(false)}
        onLeave={() => {
          setShowLeaveDialog(false);
          navigate('/');
        }}
      />
    </>
  );
}