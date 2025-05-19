import React, { useState } from "react";
import { TextField, Container, Alert, Box } from "@mui/material";
import Typography from "@mui/material/Typography";
import RegisterPageButton from "../components/button/RegisterPageButton.tsx";
import LoginButton from "../components/button/LoginButton.tsx";
import {loginAsync} from "../store/authSlice.ts";
import {useAppDispatch} from "../store";
import {useNavigate} from "react-router-dom";


const LoginPage: React.FC = () => {
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [error, setError] = useState<string>("");
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        dispatch(loginAsync({email, password}));
        navigate('/');
    };


    return (
        <Container maxWidth="sm">
            <Box sx={{ mt: 3 }}>
                <Typography component="h2" variant="h4" sx={{ mb: 2 }}>Login</Typography>

                {error && <Alert severity="error">{error}</Alert>}
                <form onSubmit={handleLogin}>
                    <TextField
                        fullWidth
                        margin="normal"
                        label="Email Address"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <TextField
                        fullWidth
                        margin="normal"
                        label="Password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <LoginButton></LoginButton>
                    <RegisterPageButton></RegisterPageButton>
                </form>
            </Box>
        </Container>
    );
};

export default LoginPage;
