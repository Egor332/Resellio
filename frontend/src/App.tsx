import { Router } from "./routing/Router";
import {AuthProvider} from "./context/AuthContext.tsx";

function App() {
    return (
        <AuthProvider>
            <Router />
        </AuthProvider>
        
    );
};

export default App;