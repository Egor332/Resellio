import { Box, Container, Typography } from '@mui/material'

interface FooterProps {
  backgroundColor: string
}

function Footer({ backgroundColor }: FooterProps) {
  return (
    <Box
      component="footer"
      sx={{ py: 2, textAlign: 'center', bgcolor: backgroundColor }}
    >
      <Container>
        <Typography variant="body2" color="white">
          © 2025 Resellio. All rights reserved.
        </Typography>
      </Container>
    </Box>
  )
}

export default Footer
