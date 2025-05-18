import React from 'react';
import {useAppSelector} from "../../../store";
import ProfileSidebarAuth from "./ProfileSidebarAuth.tsx";
import ProfileSidebarUnAuth from "./ProfileSidebarUnAuth.tsx";


const ProfileSidebar: React.FC = () => {
    const { isAuthenticated } = useAppSelector((state) => state.auth);

    return isAuthenticated ? (
        <ProfileSidebarAuth />
    ) : (
        <ProfileSidebarUnAuth />
    );
};

export default ProfileSidebar;