import React from 'react';
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../store";
import AccountIcon from '@mui/icons-material/AccountCircle';
import MainButton from "./MainButton.tsx";

const LoginPageButton: React.FC = () => {
    const collapsed = useAppSelector((state) => state.ui.sidebarCollapsed);
    const navigate = useNavigate();

    const handleLogin = () => {
        navigate('/login');
    };

    return (
        <>
            {collapsed ? (
                <MainButton func={handleLogin} full={true}>
                    <AccountIcon />
                </MainButton>
            ) : (
                <MainButton func={handleLogin} styles={{ margin: '20px' }} full={false}>
                    Login
                </MainButton>
            )}
        </>
    );
};

export default LoginPageButton;
