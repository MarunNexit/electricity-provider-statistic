import Grid from '@mui/material/Grid2';
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

import HighlightedCard from '../components/temp/HighlightedCard.tsx';

import StatCard, { StatCardProps } from '../components/temp/StatCard.tsx';
import CustomMap from '../components/map/CustomMap.tsx'
import AveragePricesForwardChart from "../components/charts/AveragePricesForwardChart.tsx";
import ProvidersRatingsChart from "../components/charts/ProvidersRatingsChart.tsx";
import ProviderStatsTable from "../components/table/ProviderStatsTable.tsx";
import PieChartProviderGroups from "../components/charts/PieChart/PieChartProviderGroups.tsx";
import ProvidersUsersChart from "../components/charts/ProvidersUsersChart.tsx";
import {useSelector} from "react-redux";
const data: StatCardProps[] = [
    {
        title: 'Users',
        value: '14k',
        interval: 'Last 30 days',
        trend: 'up',
        data: [
            200, 24, 220, 260, 240, 380, 100, 240, 280, 240, 300, 340, 320, 360, 340, 380,
            360, 400, 380, 420, 400, 640, 340, 460, 440, 480, 460, 600, 880, 920,
        ],
    },
];

export default function DashboardPage() {
    const isUserAuthenticated = useSelector(state => state.auth.isAuthenticated);

    return (
        <Box sx={{ width: '100%', maxWidth: { sm: '100%', md: '1700px' } }}>
            <Typography component="h2" variant="h6" sx={{ mb: 2 }}>
                Overview
            </Typography>


            <Grid
                container
                spacing={2}
                columns={12}
                sx={{ mb: (theme) => theme.spacing(2) }}
            >

                <Grid size={{ xs: 12, sm: 6, lg: 6 }}>
                    <ProvidersUsersChart />
                </Grid>
                {data.map((card, index) => (
                    <Grid key={index} size={{ xs: 12, sm: 6, lg: 3 }}>
                        <StatCard {...card} />
                    </Grid>
                ))}
                <Grid size={{ xs: 12, sm: 6, lg: 3 }}>
                    <HighlightedCard isAuthenticated={isUserAuthenticated}/>
                </Grid>

                <Grid size={{ xs: 12, md: 6 }}>
                    <AveragePricesForwardChart />
                </Grid>
                <Grid size={{ xs: 12, md: 6 }}>
                    <ProvidersRatingsChart/>
                    {/*<PageViewsBarChart />*/}
                </Grid>
            </Grid>
            <Typography component="h2" variant="h6" sx={{ mb: 2 }}>
                Details
            </Typography>
            <Grid container spacing={2} columns={12}>
                <Grid size={{ xs: 12, lg: 9 }}>
                    <CustomMap />
                    <ProviderStatsTable />
                    {/*<CustomizedDataGrid />*/}
                </Grid>
                <Grid size={{ xs: 12, lg: 3 }}>
                    <Stack gap={2} direction={{ xs: 'column', sm: 'row', lg: 'column' }}>
                        {/*<ChartUserByCountry />*/}
                        <PieChartProviderGroups />
                    </Stack>
                </Grid>
            </Grid>
        </Box>
    );
}
