import { styled } from '@mui/material/styles';
import MuiDrawer, { drawerClasses } from '@mui/material/Drawer';
import Box from '@mui/material/Box';
import Divider from '@mui/material/Divider';
import MenuContent from '../menu/MenuContent.tsx';
import {ChevronLeft, Menu} from "@mui/icons-material";
import './Sidebar.css';
import MenuButton from "../button/MenuButton.tsx";
import ProfileSidebar from "../profile/profile-sidebar/ProfileSidebar.tsx";
import {useAppDispatch, useAppSelector} from "../../store";
import {toggleSidebar} from "../../store/uiSlice.ts";
import Logo from "../logo/Logo.tsx";

const drawerWidth = 210;
const collapsedWidth = 80;

const Drawer = styled(MuiDrawer, {
    shouldForwardProp: (prop) => prop !== 'collapsed',
})<{ collapsed: boolean }>(({ collapsed }) => ({
    width: collapsed ? collapsedWidth : drawerWidth,
    flexShrink: 0,
    boxSizing: 'border-box',
    [`& .${drawerClasses.paper}`]: {
        width: collapsed ? collapsedWidth : drawerWidth,
        overflowX: 'hidden',
    },
}));

export default function SideMenu() {
    const collapsed = useAppSelector((state) => state.ui.sidebarCollapsed);
    const dispatch = useAppDispatch();

    return (
        <Drawer
            variant="permanent"
            collapsed={collapsed}
            sx={{
                display: { xs: 'none', md: 'block' },
                [`& .${drawerClasses.paper}`]: {
                    backgroundColor: 'background.paper',
                },
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    mt: 'calc(var(--template-frame-height, 0px) + 4px)',
                    p: 3.5,
                    alignItems: 'center',
                    justifyContent: 'space-between',
                }}
            >
                <Logo bigIcon={!collapsed} full={!collapsed} center={true}></Logo>
            </Box>
            <Divider/>
            <Box
                sx={{
                    overflow: 'auto',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                }}
            >
                <Box sx={{margin:'20px 22px 0 20px', display: 'flex', justifyContent: 'flex-end'}}>
                    <MenuButton className="toggle-btn" onClick={() => dispatch(toggleSidebar())}>
                        {collapsed ? <Menu /> : <ChevronLeft />}
                    </MenuButton>
                </Box>

                <MenuContent />

                <ProfileSidebar></ProfileSidebar>
            </Box>
        </Drawer>
    );
}
