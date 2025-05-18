import './App.css'
import { BrowserRouter as Router} from "react-router-dom";
import SideMenu from "./components/sidebar/SideMenu.tsx";
import AppNavbar from "./components/menu/AppNavbar.tsx";
import { ThemeProvider, CssBaseline, Box } from '@mui/material';
import MainContent from "./components/main/MainContent.tsx";
import theme from "./theme/theme.ts";
import Aurora from "./components/bg/Aurora.tsx";
import {useAppDispatch} from "./store";
import {useEffect} from "react";
import {fetchAuthStatus} from "./store/authSlice.ts";


function App() {
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(fetchAuthStatus());
    }, [dispatch]);

    return (
    <div>
        <ThemeProvider theme={theme}>
            <Router>
                <CssBaseline enableColorScheme/>
                <div style={{position: "relative", width: "100%"}}>
                    <div style={{position: "absolute", top: 0, left: 0, width: "100%", height: "100vh",  zIndex: 1, overflow: "hidden"}}>
                        <Aurora colorStops={["#ff007f", "#ff007f", "#ff007f"]} amplitude={0.3} blend={5.5}/>
                    </div>

           {/*         <div style={{position: "absolute", top: 0, left: 0, width: "100%", height: "2000px", backgroundColor: theme.palette.background.default, zIndex: 0}}>
                    </div>*/}
                    <div style={{
                        position: "relative",
                        top: "0",
                        left: "0",
                        zIndex: 1,
                        color: "white",
                        fontSize: "2rem"
                    }}>
                        <Box sx={{display: 'flex', position: 'relative'}}>
                            <SideMenu/>
                            <AppNavbar/>
                            <MainContent/>
                        </Box>
                    </div>
                </div>
            </Router>
        </ThemeProvider>
    </div>
  )
}

export default App
