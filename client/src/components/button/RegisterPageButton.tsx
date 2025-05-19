import React from 'react';
import { useNavigate } from "react-router-dom";
import MainButton from "./MainButton.tsx";

const RegisterPageButton: React.FC = () => {
    const navigate = useNavigate();

    const handleNavigate = () => {
        navigate('/register');
    };

    return (
        <MainButton func={handleNavigate} full={true} variant="text">
            Not registered yet?
        </MainButton>
    );
};

export default RegisterPageButton;
