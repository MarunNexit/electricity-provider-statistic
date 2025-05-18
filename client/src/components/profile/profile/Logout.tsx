import React from 'react';
import { useAppDispatch, useAppSelector } from '../../../store';
import {logoutAsync} from "../../../store/authSlice.ts";
import LogoutButton from "../../button/LogoutButton.tsx";
import {useNavigate} from "react-router-dom";

interface LogoutProps {
    version?: string;
}

const Logout: React.FC<LogoutProps> = ({version}: LogoutProps) => {
    const { isAuthenticated } = useAppSelector((state) => state.auth);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        dispatch(logoutAsync())
        navigate('/');
    };

    if (!isAuthenticated) return null;

    return (
        <form onSubmit={handleSubmit} style={{width:'100%', padding: '0', margin: 'auto'}}>
            <LogoutButton version={version}/>
        </form>
    );
};

export default Logout;
