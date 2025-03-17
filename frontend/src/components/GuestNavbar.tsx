import 'bootstrap/dist/css/bootstrap.min.css';
import '../index.css';
import { Navbar } from 'react-bootstrap';
import { Link } from 'react-router-dom';

function GuestNavbar() {
  return (
    <Navbar bg="darkblue" variant="dark" expand="lg" fixed="top">
        <Navbar.Brand as={Link} to="/" className="navbar-brand-custom">
            Resellio Guest
        </Navbar.Brand>
    </Navbar>
  );
}

export default GuestNavbar;