import axiosInstance from './axiosInstance';

const API_URL = 'api/user';

export const getMyUserData = async () => {
    const response = await axiosInstance.get(`${API_URL}/me`);
    return response.data;
};


export const getUserData = async (id: string) => {
    const response = await axiosInstance.get(`${API_URL}/user/${id}`);
    return response.data;
};