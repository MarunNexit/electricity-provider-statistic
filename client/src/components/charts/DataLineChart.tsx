import React from 'react';
import ReactApexChart from 'react-apexcharts';
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";

interface DataLineProps {
    data: { label: string; value: number }[];
    maxValue: number;
}

const DataLineChart: React.FC<DataLineProps> = ({ data, maxValue }) => {
    const series = data.map((item) => ({
        name: item.label,
        data: [item.value],
    }));

    const totalValue = data.reduce((acc, item) => acc + item.value, 0);

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
            toolbar: {
                show: false,
            },
        },
        tooltip: {
            theme: 'dark',
            x: {
                show: false
            },
        },
        yaxis:{
            show: false
        },
        plotOptions: {
            bar: {
                horizontal: true,
            },
        },
        dataLabels: {
            enabled: false,
            color: ['#fff'],
        },
        grid: {
            show: false,
        },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent'],
        },
        title: {
            show: false,
        },
        fill: {
            opacity: 1,
        },
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
        <Box id="chart">
            <Typography>Provider Popularity Share</Typography>
            <ReactApexChart options={options} series={series} type="bar" height={85} style={{padding: '0', margin: '0'}} />
            <Box display="flex" flexDirection="column" gap={1}>
                {data.map((item, index) => {
                    const percentage = ((item.value / totalValue) * 100).toFixed(2);
                    return (
                        <Box key={item.label} display="flex" alignItems="center">
                            <Box display="flex" alignItems="center" flex={1}>
                                <Box
                                    sx={{
                                        width: 12,
                                        height: 12,
                                        backgroundColor: colors[index % colors.length],
                                        borderRadius: "50%",
                                        marginRight: 1,
                                    }}
                                />
                                <Typography variant="body2">{item.label}</Typography>
                            </Box>

                            <Box display="flex" justifyContent="flex-end" flex={1}>
                                <Typography variant="body2">{item.value}</Typography>
                            </Box>

                            <Box display="flex" justifyContent="flex-end" flex={1}>
                                <Typography variant="body2">{percentage}%</Typography>
                            </Box>
                        </Box>
                    );
                })}
            </Box>
        </Box>
    );
};

export default DataLineChart;
