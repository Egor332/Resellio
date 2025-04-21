import { SyntheticEvent, useState} from "react";
import formStyles from '../../styles/FormStyles.module.css'
import { apiEndpoints } from "../../assets/constants/api";
import { apiRequest } from "../../utils/httpClient";

const RegisterOrganizerForm = () => {
    const [userData, setUserData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        organiserName: '',
    });
    const [confirmPassword, setConfirmPassword] = useState<string>('');
    const [errorMsg, setErrorMsg] = useState<string>('');

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === 'confirmPassword') {
            setConfirmPassword(value);
        } else {
            setUserData({ ...userData, [name]: value });
        }
    };

    const handleSubmit = async (event: SyntheticEvent) => {
        event.preventDefault();
        if (userData.password !== confirmPassword) {
            setErrorMsg('Passwords do not match');
            return;
        }

        const endpoint = apiEndpoints.ORGANIZERS_REGISTER;

        const payload = { ...userData };

        try {
            const response = await apiRequest(endpoint, payload);
            console.log('Registration successful:', response); // TODO proper visual feedback
            setErrorMsg('');
        } catch (error: any) {
            console.error('Registration failed:', error.message);
            setErrorMsg(error.message || 'An error occurred during registration');
        }
    };

    return (
        <form onSubmit={handleSubmit} className={`text-start ${formStyles['form']}`}>
            <label className="form-label fw-bold me-3">Register as an organizer</label>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">First Name:</label>
                <input
                    type="text"
                    name="firstName"
                    className={`form-control ${formStyles['form-input']}`}
                    value={userData.firstName}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">Last Name:</label>
                <input
                    type="text"
                    name="lastName"
                    className={`form-control ${formStyles['form-input']}`}
                    value={userData.lastName}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">Organiser Name:</label>
                <input
                    type="text"
                    name="organiserName"
                    className={`form-control ${formStyles['form-input']}`}
                    value={userData.organiserName}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">Email:</label>
                <input
                    type="email"
                    name="email"
                    className={`form-control ${formStyles['form-input']}`}
                    value={userData.email}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">Password:</label>
                <input
                    type="password"
                    name="password"
                    className={`form-control ${formStyles['form-input']}`}
                    value={userData.password}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">Confirm Password:</label>
                <input
                    type="password"
                    name="confirmPassword"
                    className={`form-control ${formStyles['form-input']}`}
                    value={confirmPassword}
                    onChange={handleChange}
                    required
                />
            </div>
            {errorMsg && <div className="alert alert-danger">{errorMsg}</div>}
            <div className={formStyles['btn-row']}>
                <button type="submit" className={`btn btn-primary ${formStyles['btn-frst']}`}>
                    Register
                </button>
            </div>
        </form>
    );
};

export default RegisterOrganizerForm;