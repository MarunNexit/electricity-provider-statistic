import { Box, Stack } from "@mui/material";
import Header from "../header/Header";
import { Routes, Route } from "react-router-dom";
import DashboardPage from "../../pages/DashboardPage.tsx";
import ProfilePage from "../../pages/ProfilePage.tsx";
import HomePage from "../../pages/home/HomePage.tsx";
import LoginPage from "../../pages/LoginPage.tsx";
import ProtectedRoute from "../route/ProtectedRoute.tsx";
import RegisterPage from "../../pages/RegisterPage.tsx";
import RedirectToUserProfile from "../route/RedirectToUserProfile.tsx";

export default function MainContent() {
    return (
        <Box
            component="main"
            sx={(theme) => ({
                flexGrow: 1,
                transition: "margin-left 0.3s ease-in-out",
                overflow: "auto",
            })}
        >
            <Stack spacing={2} sx={{ alignItems: "center", mx: 3, pb: 5, mt: { xs: 8, md: 0 } }}>
                <Header />
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/dashboard" element={<DashboardPage />} />
                    {/*<Route path="/profile" element={<RedirectToUserProfile />} />*/}
                    <Route path="/profile" element={<ProtectedRoute><ProfilePage /></ProtectedRoute>} />                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                </Routes>
            </Stack>
        </Box>
    );
}
