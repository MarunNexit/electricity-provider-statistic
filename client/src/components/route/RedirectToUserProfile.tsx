import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../store"; // або звідки ти дістаєш user
import { CircularProgress } from "@mui/material";

const RedirectToUserProfile: React.FC = () => {
    const navigate = useNavigate();
    const { user: user, loading } = useAppSelector((state) => ({
        user: state.user.profile,
        loading: state.user.loading,
    }));

    useEffect(() => {
        if (!loading && user?.id) {
            navigate(`/profile/${user.id}`, { replace: true });
        }
    }, [user, loading, navigate]);

    return <CircularProgress />; // Поки йде завантаження
};

export default RedirectToUserProfile;
