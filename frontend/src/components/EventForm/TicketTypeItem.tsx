import { memo } from 'react';
import { TicketType } from '../../dtos/EventDto';

import {
    TextField,
    Grid,
    InputAdornment,
    Paper,
    IconButton,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
  } from '@mui/material'
  import DeleteIcon from '@mui/icons-material/Delete'

interface TicketTypeItemProps {
  ticketType: TicketType;
  index: number;
  onChange: (index: number, field: keyof TicketType, value: any) => void;
  onRemove: (index: number) => void;
  canRemove: boolean;
  errors: Record<string, string>;
}

const TicketTypeItem = memo(({
  ticketType,
  index,
  onChange,
  onRemove,
  canRemove,
  errors
}: TicketTypeItemProps) => {
  return (
    <Grid container rowSpacing={1}>
      <Grid size={12}>
        <IconButton
          size="small"
          onClick={() => onRemove(index)}
          disabled={!canRemove}
        >
          <DeleteIcon />
        </IconButton>
      </Grid>

      <Grid size={12}>
        <Paper elevation={1} sx={{ p: 2, mb: 2, position: 'relative' }}>
          <Grid container spacing={2}>
            <Grid size={12}>
              <TextField
                fullWidth
                label="Ticket Description"
                value={ticketType.description}
                onChange={(e) => onChange(index, 'description', e.target.value)}
                error={!!errors[`ticketTypes[${index}].description`]}
                helperText={errors[`ticketTypes[${index}].description`]}
                variant="outlined"
              />
            </Grid>
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Available Tickets"
                type="number"
                value={ticketType.maxCount}
                onChange={(e) => onChange(index, 'maxCount', e.target.value)}
                error={!!errors[`ticketTypes[${index}].maxCount`]}
                helperText={errors[`ticketTypes[${index}].maxCount`]}
                slotProps={{
                  input: {
                    inputProps: { min: 1 },
                  },
                }}
                variant="outlined"
              />
            </Grid>
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Price"
                type="number"
                value={ticketType.price}
                onChange={(e) => onChange(index, 'price', e.target.value)}
                error={!!errors[`ticketTypes[${index}].price`]}
                helperText={errors[`ticketTypes[${index}].price`]}
                slotProps={{
                  input: {
                    startAdornment: (
                      <InputAdornment position="start">$</InputAdornment>
                    ),
                    inputProps: { min: 0, step: 0.01 },
                  },
                }}
                variant="outlined"
              />
            </Grid>
            <Grid size={{ xs: 12, sm: 6 }}>
              <FormControl fullWidth variant="outlined">
                <InputLabel id={`currency-label-${index}`}>Currency</InputLabel>
                <Select
                  labelId={`currency-label-${index}`}
                  value={ticketType.currency}
                  onChange={(e) => onChange(index, 'currency', e.target.value)}
                  label="Currency"
                >
                  <MenuItem value="PLN">PLN</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Available From"
                type="datetime-local"
                value={
                  ticketType.availableFrom instanceof Date
                    ? ticketType.availableFrom.toISOString().slice(0, 16)
                    : ticketType.availableFrom
                }
                onChange={(e) => onChange(index, 'availableFrom', e.target.value)}
                slotProps={{ inputLabel: { shrink: true } }}
                variant="outlined"
              />
            </Grid>
          </Grid>
        </Paper>
      </Grid>
    </Grid>
  );
});

export default TicketTypeItem;