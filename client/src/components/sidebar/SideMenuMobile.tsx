import Divider from '@mui/material/Divider';
import Drawer, { drawerClasses } from '@mui/material/Drawer';
import Stack from '@mui/material/Stack';
import MenuContent from '../menu/MenuContent.tsx';
import ProfileSidebar from "../profile/profile-sidebar/ProfileSidebar.tsx";
import {useEffect} from "react";
import {useDispatch} from "react-redux";
import { resetSidebarCollapsed } from '../../store/uiSlice';
import {useAppSelector} from "../../store";
import Logout from "../profile/profile/Logout.tsx";

interface SideMenuMobileProps {
    open: boolean | undefined;
    toggleDrawer: (newOpen: boolean) => () => void;
}

export default function SideMenuMobile({ open, toggleDrawer }: SideMenuMobileProps) {
    const dispatch = useDispatch();
    const isAuth = useAppSelector((state) => state.auth.isAuthenticated);

    useEffect(() => {
        if (open) {
            dispatch(resetSidebarCollapsed());
        }
    }, [open]);

    return (
        <Drawer
            anchor="right"
            open={open}
            onClose={toggleDrawer(false)}
            sx={{
                zIndex: (theme) => theme.zIndex.drawer + 1,
                [`& .${drawerClasses.paper}`]: {
                    backgroundImage: 'none',
                    backgroundColor: 'background.paper',
                },
            }}
        >
            <Stack
                sx={{
                    maxWidth: '70dvw',
                    height: '100%',
                }}
            >
                <ProfileSidebar></ProfileSidebar>
                <Divider />
                <Stack sx={{ flexGrow: 1 }}>
                    <MenuContent />
                    <Divider />
                </Stack>

                {isAuth &&
                    <Stack sx={{ p: 2 }}>
                        <Logout version='outlined'></Logout>
                    </Stack>
                }
            </Stack>
        </Drawer>
    );
}
