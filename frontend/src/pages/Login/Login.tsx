import LoginForm from '../../components/LoginForm/LoginForm'
import CenteredContainer from '../../components/CenteredContainer/CenteredContainer'
import RegistrationPopup from "../../components/RegistrationPopup/RegistrationPopup.tsx";
import {useState} from "react";

function Login() {
    // TODO poniższy useState zastąpić jakimś Reduxem
    const [showPopup, setShowPopup] = useState(false);
    
  return (
    <CenteredContainer>
      <LoginForm onRegisterClick={() => setShowPopup(true)} />
        <RegistrationPopup show={showPopup} onHide={() => setShowPopup(false)} />
    </CenteredContainer>
  )
};

export default Login;     
