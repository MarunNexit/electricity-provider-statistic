import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Stack from '@mui/material/Stack';
import {Link, useLocation} from "react-router-dom";
import {mainListItems, secondaryListItems} from "./menuItems.ts";
import {useAppSelector} from "../../store";

export default function MenuContent() {
    const location = useLocation();
    const collapsed = useAppSelector((state) => state.ui.sidebarCollapsed);

    return (
        <Stack sx={{flexGrow: 1, p: 1, justifyContent: "space-between", height: '100%', overflow: 'clip'}}>
            <List dense>
                {mainListItems.map((item, index) => (
                    <ListItem key={index} disablePadding sx={{display: "block"}}>
                        <ListItemButton
                            component={Link}
                            to={item.link}
                            selected={item.link === "/" ? location.pathname === "/" : location.pathname.startsWith(item.link)}
                            sx={{minHeight: "40px"}}
                        >
                            <ListItemIcon sx={{marginLeft: '3px'}}>
                                <item.icon/>
                            </ListItemIcon>
                            {!collapsed && <ListItemText primary={item.text}/>}
                        </ListItemButton>
                    </ListItem>
                ))}
            </List>
            <List dense>
                {secondaryListItems.map((item, index) => (
                    <ListItem key={index} disablePadding sx={{display: "block"}}>
                        <ListItemButton
                            component={Link}
                            to={item.link}
                            selected={item.link === "/" ? location.pathname === "/" : location.pathname.startsWith(item.link)}
                            sx={{minHeight: "40px"}}
                        >
                            <ListItemIcon sx={{marginLeft: '3px'}}>
                                <item.icon/>
                            </ListItemIcon>
                            {!collapsed && <ListItemText primary={item.text}/>}
                        </ListItemButton>
                    </ListItem>
                ))}
            </List>
        </Stack>
    );
}
