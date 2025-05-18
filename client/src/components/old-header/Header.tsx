import React from "react";
import { AppBar, Toolbar, Typography, IconButton } from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import './Header.css';

const Header: React.FC = () => {
    return (
        <AppBar position="relative" className="appbar">
            <Toolbar>
                <IconButton edge="start" color="inherit" aria-label="menu" sx={{ mr: 2 }}>
                    <MenuIcon />
                </IconButton>
                <Typography variant="h6" sx={{ flexGrow: 1 }}>
                    MyApp
                </Typography>
{/*
                <Navigation />
*/}
            </Toolbar>
        </AppBar>
    );
};

export default Header;