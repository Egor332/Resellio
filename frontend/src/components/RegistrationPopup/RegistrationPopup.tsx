import { useNavigate } from 'react-router-dom';
import { Navigation } from '../../assets/constants/navigation';
import { Modal, Button } from 'react-bootstrap';

interface Props {
    show: boolean;
    onHide: () => void;
}

const RegistrationPopup = ({ show, onHide }: Props) => {
    const navigate = useNavigate();

    const handleChoice = (type: 'customer' | 'organiser') => {
        onHide();
        navigate(
            type === 'customer' ? Navigation.REGISTER_CUSTOMER : Navigation.REGISTER_ORGANISER
        );
    };

    return (
        <Modal show={show} onHide={onHide} centered>
            <Modal.Header closeButton>
                <Modal.Title>Choose account type</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p>
                    Choose a client account to get access to our wide variety of tickets to unique event in entire Europe.
                     <br/>
                    Choose organizer account to ...
                </p>
                <div className="d-flex justify-content-around">
                    <Button variant="primary" onClick={() => handleChoice('customer')}>Klient</Button>
                    <Button variant="secondary" onClick={() => handleChoice('organiser')}>Organizator</Button>
                </div>
            </Modal.Body>
        </Modal>
    );
};

export default RegistrationPopup;
