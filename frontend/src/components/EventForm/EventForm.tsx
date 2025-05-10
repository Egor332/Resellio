import React, { useState, useEffect } from 'react'
import {
  Box,
  TextField,
  Button,
  Typography,
  Grid,
  InputAdornment,
  Paper,
  Divider,
  IconButton,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material'
import DeleteIcon from '@mui/icons-material/Delete'
import AddIcon from '@mui/icons-material/Add'
import { EventDto, TicketType } from '../../dtos/EventDto'

const defaultEventData: EventDto = {
  name: '',
  description: '',
  start: '',
  end: '',
  image: null,
  ticketTypes: [
    {
      description: '',
      maxCount: 1,
      price: 0,
      currency: 'PLN',
      availableFrom: new Date(),
    },
  ],
}

interface EventFormProps {
  onSubmit: (eventData: EventDto) => void
  isSubmitting: boolean
  submitButtonText?: string
}

const EventForm: React.FC<EventFormProps> = ({
  onSubmit,
  isSubmitting,
  submitButtonText = 'Submit',
}) => {
  const [formData, setFormData] = useState<EventDto>({...defaultEventData})
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [imagePreview, setImagePreview] = useState<string | null>(
    typeof formData.image === 'string' ? formData.image : null
  )

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setFormData(prev => ({...prev, [name]: value}))
    if (errors[name]) setErrors({...errors, [name]: ''})
  }

  const handleTicketTypeChange = (index: number, field: keyof TicketType, value: any) => {
    const updatedTicketTypes = [...formData.ticketTypes]
    updatedTicketTypes[index] = {
      ...updatedTicketTypes[index],
      [field]: field === 'maxCount' || field === 'price' ? Number(value) : value,
    }
    setFormData({...formData, ticketTypes: updatedTicketTypes})

    const errorKey = `ticketTypes[${index}].${field}`
    if (errors[errorKey]) {
      const newErrors = {...errors}
      delete newErrors[errorKey]
      setErrors(newErrors)
    }
  }

  const addTicketType = () => {
    setFormData({
      ...formData,
      ticketTypes: [
        ...formData.ticketTypes,
        {
          description: '',
          maxCount: 1,
          price: 0,
          currency: 'PLN',
          availableFrom: new Date(),
        },
      ],
    })
  }

  const removeTicketType = (index: number) => {
    if (formData.ticketTypes.length > 1) {
      const updatedTicketTypes = [...formData.ticketTypes]
      updatedTicketTypes.splice(index, 1)
      setFormData({...formData, ticketTypes: updatedTicketTypes})
    }
  }

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0]
      setFormData({...formData, image: file})
      
      const reader = new FileReader()
      reader.onload = () => setImagePreview(reader.result as string)
      reader.readAsDataURL(file)
      
      if (errors.image) setErrors({...errors, image: ''})
    }
  }

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {}

    if (!formData.name.trim()) newErrors.name = 'Event name is required'
    if (!formData.description.trim()) newErrors.description = 'Description is required'
    if (!formData.start) newErrors.start = 'Start date/time is required'
    if (!formData.end) newErrors.end = 'End date/time is required'

    if (formData.start && formData.end && 
        new Date(formData.end) <= new Date(formData.start)) {
      newErrors.end = 'End date/time must be after start date/time'
    }

    formData.ticketTypes.forEach((ticketType, index) => {
      if (!ticketType.description.trim()) {
        newErrors[`ticketTypes[${index}].description`] = 'Description is required'
      }
      if (ticketType.maxCount <= 0) {
        newErrors[`ticketTypes[${index}].maxCount`] = 'Ticket count must be greater than 0'
      }
      if (ticketType.price < 0) {
        newErrors[`ticketTypes[${index}].price`] = 'Price cannot be negative'
      }
    })

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (validateForm()) onSubmit(formData)
  }

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>Event Details</Typography>
        <Divider sx={{ mb: 3 }} />

        <Grid container spacing={3}>
          <Grid size={12}>
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
            />
          </Grid>

          <Grid size={12}>
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

          <Grid size={12}>
            <Typography variant="subtitle1" gutterBottom>Event Image</Typography>
            <input
              accept="image/*"
              style={{ display: 'none' }}
              id="event-image-upload"
              type="file"
              onChange={handleImageChange}
            />
            <label htmlFor="event-image-upload">
              <Button variant="outlined" component="span">Upload Image</Button>
            </label>
            {errors.image && (
              <Typography color="error" variant="caption" display="block">{errors.image}</Typography>
            )}
            {imagePreview && (
              <Box mt={2} textAlign="center">
                <img
                  src={imagePreview}
                  alt="Event preview"
                  style={{ maxWidth: '100%', maxHeight: '200px' }}
                />
              </Box>
            )}
          </Grid>
        </Grid>
      </Paper>

      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>Schedule</Typography>
        <Divider sx={{ mb: 3 }} />

        <Grid container spacing={3}>
          <Grid size={{ xs: 12, md: 6}}>
            <TextField
              fullWidth
              label="Start Date/Time"
              name="start"
              type="datetime-local"
              value={formData.start}
              onChange={handleChange}
              required
              error={!!errors.start}
              helperText={errors.start}
              slotProps={{ inputLabel: { shrink: true } }}
              variant="outlined"
            />
          </Grid>

          <Grid size={{ xs: 12, md: 6}}>
            <TextField
              fullWidth
              label="End Date/Time"
              name="end"
              type="datetime-local"
              value={formData.end}
              onChange={handleChange}
              required
              error={!!errors.end}
              helperText={errors.end}
              slotProps={{ inputLabel: { shrink: true } }}
              variant="outlined"
            />
          </Grid>
        </Grid>
      </Paper>

      <Paper elevation={0} sx={{ p: 2, mb: 3 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="h6">Ticket Types</Typography>
          <Button 
            startIcon={<AddIcon />} 
            onClick={addTicketType}
            variant="outlined"
            size="small"
          >
            Add Ticket Type
          </Button>
        </Box>
        <Divider sx={{ mb: 3 }} />

        {formData.ticketTypes.map((ticketType, index) => (
          <Grid container rowSpacing={1} key={index}>
            <Grid size={12}>
              <IconButton
                size="small"
                onClick={() => removeTicketType(index)}
                disabled={formData.ticketTypes.length <= 1}
              >
                <DeleteIcon />
              </IconButton>
            </Grid>

            <Grid size={12}>
              <Paper 
              elevation={1} 
              sx={{ p: 2, mb: 2, position: 'relative' }}
              >
                <Grid container spacing={2}>
                  <Grid size={12}>
                    <TextField
                      fullWidth
                      label="Ticket Description"
                      value={ticketType.description}
                      onChange={(e) => handleTicketTypeChange(index, 'description', e.target.value)}
                      error={!!errors[`ticketTypes[${index}].description`]}
                      helperText={errors[`ticketTypes[${index}].description`]}
                      variant="outlined"
                    />
                  </Grid>
                  
                  <Grid size={{ xs: 12, sm: 6}}>
                    <TextField
                      fullWidth
                      label="Available Tickets"
                      type="number"
                      value={ticketType.maxCount}
                      onChange={(e) => handleTicketTypeChange(index, 'maxCount', e.target.value)}
                      error={!!errors[`ticketTypes[${index}].maxCount`]}
                      helperText={errors[`ticketTypes[${index}].maxCount`]}
                      slotProps={{
                        input:{
                          inputProps: { min: 1 }
                        }
                      }}
                      variant="outlined"
                    />
                  </Grid>
                  
                  <Grid size={{ xs: 12, sm: 6}}>
                    <TextField
                      fullWidth
                      label="Price"
                      type="number"
                      value={ticketType.price}
                      onChange={(e) => handleTicketTypeChange(index, 'price', e.target.value)}
                      error={!!errors[`ticketTypes[${index}].price`]}
                      helperText={errors[`ticketTypes[${index}].price`]}
                      slotProps={{
                        input: {
                          startAdornment: (
                            <InputAdornment position="start">$</InputAdornment>
                          ),
                          inputProps: { min: 0, step: 0.01 }
                        }
                      }}
                      variant="outlined"
                    />
                  </Grid>
                  
                  <Grid size={{ xs: 12, sm: 6}}>
                    <FormControl fullWidth variant="outlined">
                      <InputLabel id={`currency-label-${index}`}>Currency</InputLabel>
                      <Select
                        labelId={`currency-label-${index}`}
                        value={ticketType.currency}
                        onChange={(e) => handleTicketTypeChange(index, 'currency', e.target.value)}
                        label="Currency"
                      >
                        <MenuItem value="PLN">PLN</MenuItem>
                      </Select>
                    </FormControl>
                  </Grid>
                  
                  <Grid size={{ xs: 12, sm: 6}}>
                    <TextField
                      fullWidth
                      label="Available From"
                      type="datetime-local"
                      value={ticketType.availableFrom instanceof Date 
                        ? ticketType.availableFrom.toISOString().slice(0, 16) 
                        : ticketType.availableFrom}
                      onChange={(e) => handleTicketTypeChange(index, 'availableFrom', e.target.value)}
                      slotProps={{ inputLabel: { shrink: true } }}
                      variant="outlined"
                    />
                  </Grid>
                </Grid>
              </Paper>
            </Grid>
          </Grid>
        ))}
      </Paper>

      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          type="submit"
          variant="contained"
          color="primary"
          size="large"
          disabled={isSubmitting}
        >
          {isSubmitting ? 'Submitting...' : submitButtonText}
        </Button>
      </Box>
    </Box>
  )
}

export default EventForm