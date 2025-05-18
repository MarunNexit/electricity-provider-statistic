import axiosInstance from './axiosInstance';

const API_URL = 'api/UserProviderChoice';

export const GetUserCountsForAllProviders = async () => {
    const response = await axiosInstance.get(`${API_URL}/providers/user-counts`);
    return response.data.$values;
};

