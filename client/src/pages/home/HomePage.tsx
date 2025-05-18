import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import React from "react";
import {CardComponent} from "../../components/cards/CardComponent.tsx";
import Grid from "@mui/material/Grid2";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";
import SplitText from "../../components/text/SplitText.tsx";
import "../home/HomePage.css";
import ChartTopMarketShares from "../../components/charts/ChartTopMarketShares.tsx";
import {useNavigate} from "react-router-dom";

const HomePage: React.FC = () => {
    const navigate = useNavigate();

    const handleAnimationComplete = () => {
        console.log('All letters have animated!');
    };

    const handleNavigate = () => {
        navigate("/dashboard");
    };

    return (
        <Box className="home-page">
            <Box className="background-image" />
            <Box className="left-panel">
                <Box className="stats-block">
                    <Typography variant="h4">UK Electricity Providers</Typography>
                    <Typography>Total UK electricity customers</Typography>
                    <Typography variant="h3">
                        <SplitText
                            text="27,500,000"
                            className="text-2xl font-semibold text-center"
                            delay={150}
                            animationFrom={{ opacity: 0, transform: 'translate3d(0,50px,0)' }}
                            animationTo={{ opacity: 1, transform: 'translate3d(0,0,0)' }}
                            easing="easeOutCubic"
                            threshold={0.2}
                            rootMargin="-50px"
                            onLetterAnimationComplete={handleAnimationComplete}
                        />
                    </Typography>
                </Box>

                <Grid container spacing={2} className="main-grid">
                    <Box sx={{ padding: 4 }}>

                        <Grid container spacing={2} className="main-grid" sx={{ mt: 3 }}>
                            <Grid item xs={12} md={7} className="left-chart">
                                <Grid container spacing={2} className="nested-grid">
                                    <Grid xs={12}>
                                        <CardComponent>
                                            <Typography variant="subtitle1" gutterBottom>
                                                Monitor provider market shares, compare statistics, and stay informed with real-time data visualizations.
                                            </Typography>
                                        </CardComponent>
                                    </Grid>
                                    <Grid  xs={12}>
                                        <CardComponent>
                                            <Stack spacing={2} >
                                                <Typography variant="body1">
                                                    Dive deeper into the analytics dashboard to explore detailed insights.
                                                </Typography>
                                                <Button
                                                    variant="contained"
                                                    color="primary"
                                                    onClick={handleNavigate}
                                                >
                                                    Go to Dashboard
                                                </Button>
                                            </Stack>
                                        </CardComponent>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Box>
                </Grid>
            </Box>

            <Box className="right-panel">
                <Grid size={{ xs: 12, md: 12 }} >
                    <CardComponent>
                        <Box className="chart-box">
                            <ChartTopMarketShares></ChartTopMarketShares>
                        </Box>
                    </CardComponent>
                </Grid>
            </Box>
        </Box>
    );
}
export default HomePage;