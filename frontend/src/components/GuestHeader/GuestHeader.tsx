import 'bootstrap/dist/css/bootstrap.min.css'
import { Navbar } from 'react-bootstrap'
import { Link } from 'react-router-dom'

function GuestHeader() {
  return (
    <Navbar bg="dark" variant="dark" expand="lg" fixed="top">
      <Navbar.Brand as={Link} to="/" className="m-2 p-1">
        Resellio Guest
      </Navbar.Brand>
    </Navbar>
  )
}

export default GuestHeader;
