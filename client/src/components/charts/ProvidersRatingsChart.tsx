import React, { useEffect, useState } from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { BarChart } from '@mui/x-charts/BarChart';
import { useTheme } from '@mui/material/styles';
import { fetchProvidersWithRatings, ProviderWithRating } from "../../utils/services/providerService.ts";
import {Box} from "@mui/material";


type ProviderDataset = {
    provider: string;
    rating: number;
    color: string;
};

export default function ProvidersRatingsChart() {
    const theme = useTheme();
    const [data, setData] = useState<ProviderWithRating[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchProvidersWithRatings()
            .then(fetchedData => {
                const sortedData = [...fetchedData].sort((a, b) => b.averageRating - a.averageRating);
                setData(sortedData);
            })
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <Typography>Loading...</Typography>;
    if (!data.length) return <Typography>No data available</Typography>;

    const colors = [
        '#8e44ad',
        '#2980b9',
        '#27ae60',
        '#f39c12',
        '#c0392b',
    ];

    const dataset: ProviderDataset[] = data.map((p, idx) => ({
        provider: p.name,
        rating: Number(p.averageRating.toFixed(2)),
        color: colors[idx % colors.length],
    }));

    return (
        <Card
            variant="outlined"
            sx={{
                width: '100%',
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                maxHeight: {
                    xs: 'none',
                    md: '368px',
                },
            }}
        >
            <CardContent
                sx={{
                    width: '100%',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                }}
            >
                <Typography component="h2" variant="h6" gutterBottom sx={{ mb: 2 }}>
                    Providers Average Ratings
                </Typography>
                <Box sx={{ flexGrow: 1, overflowY: 'auto', marginBottom: '20px' }}>
                    <BarChart
                        colors={colors}
                        dataset={dataset}
                        height={dataset.length * 30}
                        margin={{ left: 150, right: 20, top: 20, bottom: 40 }}
                        yAxis={[{ scaleType: 'band', dataKey: 'provider', position: 'left' }]}
                        xAxis={[{
                            scaleType: 'linear',
                            min: 0,
                            max: 5,
                            position: 'bottom',
                            label: 'Average Rating',
                        }]}
                        series={[
                            {
                                dataKey: 'rating',
                            },
                        ]}
                        layout="horizontal"
                    />
                </Box>


            </CardContent>
        </Card>
    );
}
