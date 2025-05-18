import React, { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from "../../../store";
import { fetchUserProfile } from "../../../store/userSlice"; // <- твій AsyncThunk
import UserAvatar from "../../user/UserAvatar";
import OptionsMenu from "../../menu/OptionsMenu";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";
import ProfileSidebarUnAuth from "./ProfileSidebarUnAuth.tsx";
import {useNavigate} from "react-router-dom";

const ProfileSidebarAuth: React.FC = () => {
    const dispatch = useAppDispatch();
    const collapsed = useAppSelector((state) => state.ui.sidebarCollapsed);
    const user = useAppSelector((state) => state.user);
    const [profileTried, setProfileTried] = React.useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        if (!user.profile && !profileTried) {
            setProfileTried(true);
            dispatch(fetchUserProfile());
        }
    }, [dispatch, user, profileTried]);

    const handleProfileClick = () => {
        console.log("Go to profile");
        navigate(`/profile`);
    };

    return (
        <>
            {user && user.profile ? (
                <>
                    <Stack
                        direction={collapsed ? "column" : "row"}
                        sx={{
                            p: 2,
                            gap: 1,
                            alignItems: 'center',
                            borderTop: '1px solid',
                            borderColor: 'divider',
                            cursor: 'pointer',
                        }}
                    >
                        {!collapsed ? (
                            <>
                            <Box onClick={handleProfileClick}>
                                <UserAvatar alt={user.profile.name} src={user.profile.avatarUrl} size={36} />
                            </Box>
                            <Box sx={{ mr: 'auto', display: 'flex', flexDirection: 'column', alignItems: 'start' }} onClick={handleProfileClick}>
                                {user.profile.name ?
                                    (
                                        <Typography variant="body2"
                                                    sx={{
                                                        color: 'text.secondary',
                                                        overflow: 'hidden',
                                                        whiteSpace: 'nowrap',
                                                        textOverflow: 'ellipsis',
                                                        maxWidth: 90,
                                                        display: 'block',
                                                    }}
                                        >
                                            {user.profile.name}
                                        </Typography>
                                    ) : (
                                        <Typography variant="caption"
                                                    sx={{
                                                        color: 'text.secondary',
                                                        overflow: 'hidden',
                                                        whiteSpace: 'nowrap',
                                                        textOverflow: 'ellipsis',
                                                        maxWidth: 90,
                                                        display: 'block',
                                                    }}
                                        >
                                            {user.profile.email}
                                        </Typography>
                                    )
                                }
                            </Box>
                            <OptionsMenu />
                        </>
                        ):(
                        <Box onClick={handleProfileClick}
                             sx={{
                                 display: 'flex',
                                 alignItems: 'center',
                                 justifyContent: 'center',
                                 width:'100%',
                             }}>
                            <UserAvatar alt={user.profile.name} src={user.profile.avatarUrl} size={24} />
                        </Box>
                        )}
                    </Stack>
                </>
            ):(
                <>
                    <ProfileSidebarUnAuth />
                </>
            )
            }
        </>
    );
};

export default ProfileSidebarAuth;
