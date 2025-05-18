import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';
import { BarChart } from '@mui/x-charts/BarChart';
import {GetUserCountsForAllProviders} from "../../utils/services/userProviderChoicesService.ts";

type ProviderUsersDto = {
    id: number;
    providerName: string;
    userCount: number;
};

export default function ProvidersUsersChart() {
    const [data, setData] = useState<ProviderUsersDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        GetUserCountsForAllProviders()
            .then(fetchedData => {
                setData(fetchedData);
            })
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <Typography>Loading...</Typography>;
    if (!data.length) return <Typography>No data available</Typography>;

    const colors = [ '#27ae60', '#f39c12', '#c0392b'];

    const dataset = data.map((p, idx) => ({
        provider: p.providerName,
        users: p.userCount,
        color: colors[idx % colors.length],
    }));

    return (
        <Card variant="outlined" sx={{ width: '100%', height: '100%' }}>
            <CardContent sx={{ position: 'relative' }}>
                <Typography
                    variant="h6"
                    sx={{
                        position: 'absolute',
                        top: 20,
                        left: '50%',
                        transform: 'translateX(-50%)',
                        zIndex: 1,
                        px: 1,
                    }}
                >
                    Clients per Provider
                </Typography>
                <Box sx={{ width: '100%', overflowX: 'auto' }}>
                    <BarChart
                        title={'Users Count per Provider'}
                        dataset={dataset}
                        height={180}
                        width={Math.max(dataset.length * 39, 500)}
                        margin={{ left: 40, right: 20 }}
                        colors={colors}
                        xAxis={[
                            {
                                scaleType: 'band',
                                dataKey: 'provider',
                                position: 'bottom',
                                labelStyle: {
                                    angle: -45,
                                    textAnchor: 'end',
                                    fontSize: 10,
                                },
                            },
                        ]}
                        yAxis={[{ scaleType: 'linear', position: 'left' }]}
                        series={[
                            {
                                dataKey: 'users',
                                itemProps: (_, index) => ({
                                    style: {
                                        fill: colors[index % colors.length],
                                    },
                                }),
                            },
                        ]}
                        layout="vertical"
                    />
                </Box>
            </CardContent>
        </Card>
    );
}
