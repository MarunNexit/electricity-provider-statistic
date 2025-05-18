import axiosInstance from './axiosInstance';
import {LoginDto} from "../../types/LoginDto.ts";

const API_URL = 'api/Auth';

export const register = async (userData: LoginDto) => {
    const response = await axiosInstance.post(`${API_URL}/register`, userData, {
        headers: {
            'Content-Type': 'application/json',
        },
    });

    console.log(response);
    return response.data;
};

export const loginUser = async (loginData: LoginDto) => {
    const response = await axiosInstance.post(`${API_URL}/login`, loginData, {
        headers: {
            'Content-Type': 'application/json',
        },
    });

    return response.data;
};

export const logoutUser = async (): Promise<void> => {
    await axiosInstance.post(`${API_URL}/logout`);
};

export const checkAuthStatus = async (): Promise<{ isAuthenticated: boolean }> => {
    try {
        const response = await axiosInstance.get(`${API_URL}/check`);
        return response.data;
    } catch {
        return { isAuthenticated: false };
    }
};


