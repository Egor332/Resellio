import { Outlet } from 'react-router-dom'
import Banner from '../../components/Banner/Banner'
import { Box, Container, CssBaseline } from '@mui/material'
import styles from './BaseWrapperPage.module.css'

interface BaseWrapperPageProps {
  Header: React.ComponentType
  Footer: React.ComponentType
}

const BaseWrapperPage = ({ Header, Footer }: BaseWrapperPageProps) => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh',
      }}
    >
      <CssBaseline />
      <Header />
      <Banner />
      <Container
        component="main"
        className={styles['content']}
        sx={{ display: 'flex', justifyContent: 'center' }}
      >
        <Outlet />
      </Container>
      <Footer />
    </Box>
  )
}

export default BaseWrapperPage
