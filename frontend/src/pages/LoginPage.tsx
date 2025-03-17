import GuestNavbar from "../components/GuestNavbar";
import LoginForm from "../components/LoginForm";
import CenteredContainer from "../components/CenteredContainer";

function LoginPage() {
    return (
        <>
            <GuestNavbar />
            <CenteredContainer>
                <LoginForm />
            </CenteredContainer>
        </>
    );
}

export default LoginPage;