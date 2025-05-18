import { useEffect, useState } from 'react';
import { LineChart } from '@mui/x-charts/LineChart';
import { Card, CardContent, Typography } from '@mui/material';
import {getAveragePricesForward} from "../../utils/services/dataAnalyticsService.ts";


type DataPricePoint = {
    $id: number;
    $values: PricePoint[];
};

type PricePoint = {
    dateTime: string;
    price: number;
};

export default function AveragePricesForwardChart() {
    const [data, setData] = useState<DataPricePoint>();
    const [xLabels, setXLabels] = useState<string[]>([]);
    const [prices, setPrices] = useState<number[]>([]);

    useEffect(() => {
        getAveragePricesForward()
            .then(setData)
            .catch(console.error);
    }, []);

    useEffect(() => {
        console.log(data)
        if((data && data.$values && data.$values.length > 0) && (xLabels.length < 1 || prices.length < 1 )){
            const labels = data.$values.map((d) =>
                new Date(d.dateTime).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' })
            );
            const values = data.$values.map((d) => Number(d.price));

            setXLabels(labels);
            setPrices(values);
        }
    }, [data]);


    return (
        <>
            {
                (data && data.$values && data.$values.length > 0) && (
                    <Card variant="outlined" sx={{ width: '100%' }}>
                        <CardContent>
                            <Typography component="h2" variant="subtitle2" gutterBottom>
                                Forward Electricity Prices
                            </Typography>
                            <LineChart
                                xAxis={[{ scaleType: 'point', data: xLabels }]}
                                series={[{ data: prices, label: 'Price (£/MWh)', area: true, showMark: false }]}
                                height={300}
                                margin={{ left: 60, right: 30, top: 20, bottom: 30 }}
                                grid={{ horizontal: true }}
                            />
                        </CardContent>
                    </Card>
                )
            }
        </>

    );
}
