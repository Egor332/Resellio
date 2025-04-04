import {SyntheticEvent, useState} from "react";
import formStyles from '../../styles/FormStyles.module.css'

const RegisterCustomerForm = () => {
    const [customerData, setCustomerData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: ''
    });
    const [confirmPassword, setConfirmPassword] = useState<string>('');
    const [errorMsg, setErrorMsg] = useState<string>('');
    
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === 'confirmPassword') {
            setConfirmPassword(value);
        } else {
            setCustomerData({ ...customerData, [name]: value });
        }
    };

    const handleSubmit = async (event: SyntheticEvent) => {
        event.preventDefault();
        if (customerData.password !== confirmPassword) {
            setErrorMsg('Passwords do not match');
            return;
        }
        
        try {
            const response = await fetch('/api/customers/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(customerData)
            });

            console.log('Response status:', response.status);
            
            if (!response.ok){
                throw new Error('Registration failed');
            } 
            
            console.log('Registration successful');
        }
        catch (error) {
            if (error instanceof Error) {
                setErrorMsg(error.message);
            } else {
                setErrorMsg('An unknown error occurred');
            }
        }
    };
    
    return (
        <form onSubmit={handleSubmit} className={`text-start ${formStyles['form']}`}>
            <div className={formStyles['form-row']}>
                <label className="form-label fw-bold">First Name:</label>
                <input
                    type="text"
                    name="firstName"
                    className={`form-control ${formStyles['form-input']}`}
                    value={customerData.firstName}
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
                    value={customerData.lastName}
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
                    value={customerData.email}
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
                    value={customerData.password}
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

export default RegisterCustomerForm;