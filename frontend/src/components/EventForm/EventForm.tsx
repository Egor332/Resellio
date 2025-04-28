import React, { useState, useEffect } from 'react';
import { 
  Box, 
  TextField, 
  Button, 
  Typography, 
  Grid,
  InputAdornment,
  Paper,
  Divider
} from '@mui/material';
import { EventFormDto } from '../../dtos/EventFormDto';

interface EventFormProps {
  initialData?: EventFormDto;
  onSubmit: (eventData: EventFormDto) => void;
  isSubmitting: boolean;
  submitButtonText?: string;
}

const defaultFormData: EventFormDto = {
  name: '',
  description: '',
  start: '',
  end: '',
  ticketsTotal: 0
};

const EventForm: React.FC<EventFormProps> = ({ 
  initialData, 
  onSubmit, 
  isSubmitting,
  submitButtonText = 'Submit'
}) => {
  const [formData, setFormData] = useState<EventFormDto>(initialData || defaultFormData);
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (initialData) {
      setFormData(initialData);
    }
  }, [initialData]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'ticketsTotal' ? parseInt(value) || 0 : value
    });
    
    // Clear error when field is edited
    if (errors[name]) {
      setErrors({ ...errors, [name]: '' });
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};
    
    if (!formData.name.trim()) newErrors.name = 'Event name is required';
    if (!formData.description.trim()) newErrors.description = 'Description is required';
    if (!formData.start) newErrors.start = 'Start date/time is required';
    if (!formData.end) newErrors.end = 'End date/time is required';
    if (formData.ticketsTotal <= 0) newErrors.ticketsTotal = 'Number of tickets must be greater than 0';
    
    // Check if end date is after start date
    if (formData.start && formData.end && new Date(formData.end) <= new Date(formData.start)) {
      newErrors.end = 'End date/time must be after start date/time';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (validateForm()) {
      onSubmit(formData);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Event Details
        </Typography>
        <Divider sx={{ mb: 3 }} />
        
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Event Name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              error={!!errors.name}
              helperText={errors.name}
              variant="outlined"
              size="medium"
            />
          </Grid>
          
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              required
              multiline
              rows={4}
              error={!!errors.description}
              helperText={errors.description}
              variant="outlined"
            />
          </Grid>
        </Grid>
      </Paper>
      
      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Schedule
        </Typography>
        <Divider sx={{ mb: 3 }} />
        
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              label="Start Date/Time"
              name="start"
              type="datetime-local"
              value={formData.start}
              onChange={handleChange}
              required
              InputLabelProps={{ shrink: true }}
              error={!!errors.start}
              helperText={errors.start}
              variant="outlined"
            />
          </Grid>
          
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              label="End Date/Time"
              name="end"
              type="datetime-local"
              value={formData.end}
              onChange={handleChange}
              required
              InputLabelProps={{ shrink: true }}
              error={!!errors.end}
              helperText={errors.end}
              variant="outlined"
            />
          </Grid>
        </Grid>
      </Paper>
      
      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Ticket Information
        </Typography>
        <Divider sx={{ mb: 3 }} />
        
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              label="Total Tickets Available"
              name="ticketsTotal"
              type="number"
              value={formData.ticketsTotal}
              onChange={handleChange}
              required
              inputProps={{ min: 1 }}
              error={!!errors.ticketsTotal}
              helperText={errors.ticketsTotal}
              InputProps={{
                startAdornment: <InputAdornment position="start">#</InputAdornment>
              }}
              variant="outlined"
            />
          </Grid>
        </Grid>
      </Paper>
      
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'flex-end' }}>
        <Button 
          type="submit" 
          variant="contained" 
          color="primary"
          disabled={isSubmitting}
          size="large"
          sx={{ px: 4, py: 1 }}
        >
          {isSubmitting ? 'Submitting...' : submitButtonText}
        </Button>
      </Box>
    </Box>
  );
};

export default EventForm;
