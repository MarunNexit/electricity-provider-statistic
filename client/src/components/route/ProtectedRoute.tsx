import { Navigate } from 'react-router-dom';
import {useAppSelector} from "../../store";
import {JSX} from "react";

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
    const { isAuthenticated, loading } = useAppSelector((state) => state.auth);

    if (loading) return <div>Loading...</div>;
    if (!isAuthenticated) return <Navigate to="/login" replace />;
    return children;
};

export default ProtectedRoute;