import { useState } from 'react'
import { Typography, Container, Paper } from '@mui/material'
import EventForm from '../../components/EventForm/EventForm'
import useBanner from '../../hooks/useBanner'
import { useNavigate } from 'react-router-dom'
import { apiRequest } from '../../services/httpClient'
import { API_ENDPOINTS } from '../../assets/constants/api'
import { EventDto } from '../../dtos/EventDto'
import { Navigation } from '../../assets/constants/navigation'

function OrganisersAddEvent() {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const banner = useBanner()
  const navigate = useNavigate()

  const handleSubmit = async (eventData: EventDto) => {
    setIsSubmitting(true)
    try {
      // multipart/form-data request
      const formData = new FormData()

      formData.append('Name', eventData.name)
      formData.append('Description', eventData.description)
      formData.append('Start', new Date(eventData.start).toISOString())
      formData.append('End', new Date(eventData.end).toISOString())

      const ticketTypesArray = eventData.ticketTypes.map(ticketType => ({
        description: ticketType.description,
        maxCount: ticketType.maxCount,
        price: ticketType.price,
        currency: ticketType.currency,
        availableFrom: typeof ticketType.availableFrom === 'string'
          ? new Date(ticketType.availableFrom).toISOString()
          : ticketType.availableFrom.toISOString(),
      }));
      
      formData.append('TicketTypeDtos', JSON.stringify(ticketTypesArray));

      if (eventData.image && eventData.image instanceof File) {
        formData.append('EventImage', eventData.image)
      }

      await apiRequest(
        API_ENDPOINTS.CREATE_EVENT,
        formData,
        true // isFormData
      )

      banner.showSuccess('Event created successfully!')
      navigate(Navigation.ORGANISERS)
    } catch (error) {
      console.error('Error creating event:', error)
      banner.showError('Failed to create event. Please try again.')
    } finally {
      setIsSubmitting(false)
    }
  }

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
  )
}

export default OrganisersAddEvent
