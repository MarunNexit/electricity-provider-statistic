import React, { useState } from "react";
import {
    TextField,
    Container,
    Alert,
    Box,
    Typography
} from "@mui/material";
import {useNavigate} from "react-router-dom";
import RegisterButton from "../components/button/RegisterButton.tsx";
import {register} from "../utils/services/authService.ts";


const RegisterPage: React.FC = () => {
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
    const [error, setError] = useState<string>("");
    const [success, setSuccess] = useState<string>("");
    const navigate = useNavigate();

    const handleRegister = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        try {
            await register({ email, password });

            setError("");
            setSuccess("Registration successful! You can now log in.");
            setEmail("");
            setPassword("");
            setConfirmPassword("");
            navigate('/login');
        } catch (error: any) {
            const message =
                error.response?.data?.message || "Registration failed.";
            setError(message);
        }
    };

    return (
        <Container maxWidth="sm">
            <Box sx={{ mt: 3 }}>
                <Typography component="h2" variant="h4" sx={{ mb: 2 }}>
                    Register
                </Typography>

                {error && <Alert severity="error">{error}</Alert>}
                {success && <Alert severity="success">{success}</Alert>}

                <form onSubmit={handleRegister}>
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
                    <TextField
                        fullWidth
                        margin="normal"
                        label="Confirm Password"
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        required
                    />
                    <RegisterButton></RegisterButton>
                </form>
            </Box>
        </Container>
    );
};

export default RegisterPage;
