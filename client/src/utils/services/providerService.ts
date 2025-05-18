import axiosInstance from "./axiosInstance.ts";

export interface ProviderWithRating {
    id: number;
    averageRating: number;
    name: string;
    website: string;
    logo: string;
}

const API_URL = 'api/Providers';

export const fetchProvidersWithRatings = async (): Promise<ProviderWithRating[]> => {
    const response = await axiosInstance.get(`${API_URL}/with-ratings`);
    return response.data.$values;
};