import React, { useEffect, useState } from 'react';
import ReactApexChart from 'react-apexcharts';
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import {getTop6MarketShares} from "../../utils/services/dataAnalyticsService.ts";


interface TopSupplierShareDto {
    supplier: string;
    share: number;
}

const ChartTopMarketShares: React.FC = () => {
    const [data, setData] = useState<TopSupplierShareDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getTop6MarketShares()
            .then((result) => {
                setData(result.$values);
                setLoading(false);
            })
            .catch(() => setLoading(false));
    }, []);

    if (loading) return <Typography>Loading...</Typography>;

    const totalValue = data.reduce((acc, item) => acc + item.share, 0);

    const series = data.map((item) => ({
        name: item.supplier,
        data: [item.share],
    }));

    const colors = [
        '#00796b', '#c2185b', '#1976d2', '#ffa000', '#8e24aa',
        '#0288d1', '#d32f2f', '#388e3c', '#0288d1', '#f57c00',
    ];

    const options = {
        chart: {
            type: 'bar',
            stacked: true,
            stackType: '100%',
            background: 'transparent',
            border: 'none',
            toolbar: { show: false },
        },
        tooltip: {
            theme: 'dark',
            x: { show: false },
        },
        yaxis: { show: false },
        plotOptions: {
            bar: { horizontal: true },
        },
        dataLabels: {
            enabled: false,
            color: ['#fff'],
        },
        grid: { show: false },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent'],
        },
        title: { show: false },
        fill: { opacity: 1 },
        legend: {
            show: false,
            position: 'bottom',
            itemMargin: {
                horizontal: 10,
                vertical: 5,
            },
            labels: {
                useSeriesColors: true,
            },
        },
        colors: colors,
    };

    return (
        <Box width="100%">
            <Typography>Provider Popularity Share</Typography>
            <ReactApexChart
                options={options}
                series={series}
                type="bar"
                height={85}
                style={{ padding: '0', margin: '0' }}
            />
            <Box display="flex" flexDirection="column" gap={1} mt={2}>
                <Box display="flex" alignItems="center" sx={{ fontWeight: 'bold', pb: 1 }}>
                    <Box>
                        <Typography variant="body2">Provider</Typography>
                    </Box>
                    <Box flex={2} textAlign="right">
                        <Typography variant="body2">Value</Typography>
                    </Box>
                    <Box flex={1} textAlign="right">
                        <Typography variant="body2">%</Typography>
                    </Box>
                </Box>

                {data.map((item, index) => {
                    const percentage = ((item.share / totalValue) * 100).toFixed(2);
                    return (
                        <Box
                            key={item.supplier}
                            display="flex"
                            alignItems="center"
                            sx={{
                                borderBottom: '1px solid #e0e0e0',
                                py: 0.5,
                            }}
                        >
                            <Box display="flex" alignItems="center" flex={2} minWidth={0}>
                                <Box
                                    sx={{
                                        width: 12,
                                        height: 12,
                                        backgroundColor: colors[index % colors.length],
                                        borderRadius: '50%',
                                        marginRight: 1,
                                        flexShrink: 0,
                                    }}
                                />
                                <Typography
                                    variant="body2"
                                    noWrap
                                    sx={{ overflow: 'hidden', textOverflow: 'ellipsis' }}
                                >
                                    {item.supplier}
                                </Typography>
                            </Box>

                            <Box flex={1} textAlign="right">
                                <Typography variant="body2">{item.share}</Typography>
                            </Box>

                            <Box flex={1} textAlign="right">
                                <Typography variant="body2">{percentage}%</Typography>
                            </Box>
                        </Box>
                    );
                })}
            </Box>
        </Box>
    );
};

export default ChartTopMarketShares;
