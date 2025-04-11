import {SyntheticEvent, useState} from "react";
import formStyles from '../../styles/FormStyles.module.css'
import { apiEndpoints, getApiEndpoint } from "../../assets/constants/api";
import { apiRequest } from "../../utils/httpClient";

const RegisterForm = () => {
    const [userData, setUserData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        organiserName: '',
    });
    const [confirmPassword, setConfirmPassword] = useState<string>('');
    const [errorMsg, setErrorMsg] = useState<string>('');
    const [userType, setUserType] = useState<'customer' | 'organizer'>('customer');

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

        const endpoint = userType === 'organizer'
            ? apiEndpoints.ORGANIZERS_REGISTER
            : apiEndpoints.CUSTOMERS_REGISTER;

        const payload = { ...userData };

        if (userType !== 'organizer') {
            delete payload.organiserName;
        }

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
            <label className="form-label fw-bold me-3">Register as:</label>
            <div className="form-check form-check-inline">
                <input
                    className="form-check-input"
                    type="radio"
                    name="userType"
                    id="customer"
                    value="customer"
                    checked={userType === 'customer'}
                    onChange={() => setUserType('customer')}
                />
                <label className="form-check-label" htmlFor="customer">Customer</label>
            </div>
            <div className="form-check form-check-inline">
                <input
                    className="form-check-input"
                    type="radio"
                    name="userType"
                    id="organizer"
                    value="organizer"
                    checked={userType === 'organizer'}
                    onChange={() => setUserType('organizer')}
                />
                <label className="form-check-label" htmlFor="organizer">Organizer</label>
            </div>
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
            {userType === 'organizer' && (
                <div className={formStyles['form-row']}>
                    <label className="form-label fw-bold">Organiser Name:</label>
                    <input
                        type="text"
                        name="organiserName"
                        className={`form-control ${formStyles['form-input']}`}
                        value={userData.organiserName}
                        onChange={handleChange}
                        required={userType === 'organizer'}
                    />
                </div>
            )}
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

export default RegisterForm;