import {SyntheticEvent, useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from "react-router-dom";
import "../index.css";

function LoginForm() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
  
    const [errorMsg, setErrorMsg] = useState<string>("");
  
    const navigate = useNavigate();

    const handleSubmit = (event: SyntheticEvent) => {
        event.preventDefault();
        // TODO - perform login operation
        //console.log(`Username: ${username}, Password: ${password}`);
        if(username === "Radek" && password === "radek123")
          navigate("/home");
        else
        {
          //alert("Wrong credentials!");
          setErrorMsg("Wrong credentials!");
          setTimeout(() => setErrorMsg(""), 4000);
          setUsername("");
          setPassword("");
        }
      };

    return (
        <form onSubmit={handleSubmit} className="text-start">
            <div className="d-flex flex-column gap-3">
            <div className="d-flex align-items-center">
                <label className="form-label m-1 fw-bold me-2">Username:</label>
                <input type="text" className="form-control flex-fill" value={username} onChange={e => setUsername(e.target.value)} required />
            </div>
            <div className="d-flex align-items-center">
                <label className="form-label m-1 fw-bold me-2">Password:</label>
                <input type="password" className="form-control flex-fill" value={password} onChange={e => setPassword(e.target.value)} required />
            </div>
            {errorMsg && <div className="alert alert-danger mt-3">{errorMsg}</div>}
            <button type="submit" className="btn btn-primary mt-2 w-100">Login</button>
            </div>
        </form>
    );
}

export default LoginForm;