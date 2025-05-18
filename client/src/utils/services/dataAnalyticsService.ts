import axiosInstance from './axiosInstance';

const API_URL = 'api/DataAnalytics';

export const getAveragePricesForward = async () => {
    const response = await axiosInstance.get(`${API_URL}/average_prices_forward`);
    return response.data;
};

export const getTop6MarketShares  = async () => {
    const response = await axiosInstance.get(`${API_URL}/top6-market-shares`);
    return response.data;
};

export const getProviderStatsTable = async (year: number = 2023) => {
    const response = await axiosInstance.get(`${API_URL}/providers-stats-table?year=${year}`);
    return response.data;
};

export const getMarketSharePieWithYear = async (year: number = 2024) => {
    const response = await axiosInstance.get(`${API_URL}/market-share-pie?year=${year}`);
    return response.data;
};
