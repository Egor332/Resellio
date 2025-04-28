import { useState } from 'react';
import { Box, Typography, Container, Paper } from '@mui/material';
import EventForm from '../../components/EventForm/EventForm';
import { EventFormDto } from '../../dtos/EventFormDto';
import useBanner from '../../hooks/useBanner';
import { useNavigate } from 'react-router-dom';

function OrganisersAddEvent() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const banner = useBanner();
  const navigate = useNavigate();

  const handleSubmit = async (eventData: EventFormDto) => {
    setIsSubmitting(true);
    try {
      // This would be replaced with actual API call
      console.log('Submitting event data:', eventData);
      
      // Simulate API call delay
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      banner.showSuccess('Event created successfully!');
      navigate('/organiser/events'); // Navigate to events list
    } catch (error) {
      console.error('Error creating event:', error);
      banner.showError('Failed to create event. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Container maxWidth="md">
      <Paper elevation={3} sx={{ p: 4, mt: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Create New Event
        </Typography>
        <Typography variant="body1" color="text.secondary" paragraph>
          Fill in the details below to create a new event.
        </Typography>
        
        <EventForm 
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
          submitButtonText="Create Event"
        />
      </Paper>
    </Container>
  );
}

export default OrganisersAddEvent;