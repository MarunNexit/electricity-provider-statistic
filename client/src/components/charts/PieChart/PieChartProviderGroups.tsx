import * as React from 'react';
import {useEffect, useMemo, useState} from 'react';
import { PieChart } from '@mui/x-charts/PieChart';
import { useDrawingArea } from '@mui/x-charts/hooks';
import { styled } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import Card from '@mui/material/Card';
import Box from '@mui/material/Box';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import {getMarketSharePieWithYear} from "../../../utils/services/dataAnalyticsService.ts";
import ProviderGroupsList from "./ProviderGroupsList.tsx";


interface GroupProvider {
    name: string;
    share: number;
}

interface Group {
    groupName: string;
    share: number;
    providers: GroupProvider[];
}

interface ApiResponse {
    $values: Group[];
}

interface StyledTextProps {
    variant: 'primary' | 'secondary';
}

const StyledText = styled('text', {
    shouldForwardProp: (prop) => prop !== 'variant',
})<StyledTextProps>(({ theme }) => ({
    textAnchor: 'middle',
    dominantBaseline: 'central',
    fill: (theme).palette.text.secondary,
    variants: [
        {
            props: {
                variant: 'primary',
            },
            style: {
                fontSize: theme.typography.h5.fontSize,
            },
        },
        {
            props: ({ variant }) => variant !== 'primary',
            style: {
                fontSize: theme.typography.body2.fontSize,
            },
        },
        {
            props: {
                variant: 'primary',
            },
            style: {
                fontWeight: theme.typography.h5.fontWeight,
            },
        },
        {
            props: ({ variant }) => variant !== 'primary',
            style: {
                fontWeight: theme.typography.body2.fontWeight,
            },
        },
    ],
}));


interface PieCenterLabelProps {
    primaryText: string;
    secondaryText: string;
}

function PieCenterLabel({ primaryText, secondaryText }: PieCenterLabelProps) {
    const { width, height, left, top } = useDrawingArea();
    const primaryY = top + height / 2 - 10;
    const secondaryY = primaryY + 24;

    return (
        <>
            <StyledText variant="primary" x={left + width / 2} y={primaryY}>
                {primaryText}
            </StyledText>
            <StyledText variant="secondary" x={left + width / 2} y={secondaryY}>
                {secondaryText}
            </StyledText>
        </>
    );
}

export default function PieChartProviderGroups() {
    const [year, setYear] = useState<number>(2024);
    const [groups, setGroups] = useState<Group[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const availableYears = [2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024]
    const [selectedGroup, setSelectedGroup] = useState<string | null>(null);

    useEffect(() => {
        async function fetchData() {
            setLoading(true);
            try {
                const response: ApiResponse = await getMarketSharePieWithYear(year);
                setGroups(response.$values.map(group => ({
                    groupName: group.groupName,
                    share: group.share,
                    providers: group.providers.$values.map((p: any) => ({ name: p.name, share: p.share })),
                })));

            } catch (e) {
                console.error(e);
                setGroups([]);
            } finally {
                setLoading(false);
            }
        }
        fetchData();

    }, [year]);


    useEffect(() => {
        if(groups.length > 0){
            setSelectedGroup(groups[0].groupName);
        }
    }, [groups]);


    const pieData = groups.map(group => ({
        label: group.groupName,
        value: group.share,
    }));

    const colors = ['hsl(220, 25%, 65%)', 'hsl(220, 25%, 45%)', 'hsl(220, 25%, 30%)', 'hsl(220, 25%, 20%)'];

    const topGroup = useMemo(() => {
        if (!selectedGroup || groups.length === 0) return null;
        return groups.find(group => group.groupName === selectedGroup) || null;
    }, [selectedGroup, groups]);


    return (
        <Card variant="outlined" sx={{ p: 2, flexGrow: 1 }}>
            <Typography variant="h6" gutterBottom>
                Provider Groups
            </Typography>

            <Box sx={{ mb: 3 }}>
                <Typography variant="subtitle2">Select Year:</Typography>
                <Select value={year} onChange={(e) => setYear(Number(e.target.value))} size="small" sx={{ width: 120 }}>
                    {availableYears.map((yr) => (
                        <MenuItem key={yr} value={yr}>
                            {yr}
                        </MenuItem>
                    ))}
                </Select>
            </Box>

            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 3 }}>
                <PieChart
                    colors={colors}
                    margin={{ left: 80, right: 80, top: 80, bottom: 80 }}
                    series={[
                        {
                            data: pieData,
                            innerRadius: 75,
                            outerRadius: 100,
                            paddingAngle: 0,
                            highlightScope: { faded: 'global', highlighted: 'item' },
                        },
                    ]}
                    height={260}
                    width={260}
                    slotProps={{ legend: { hidden: true } }}
                    loading={loading}
                >
                    {topGroup && (
                        <PieCenterLabel
                            primaryText={`${topGroup.share.toFixed(2)}%`}
                            secondaryText={topGroup.groupName}
                        />
                    )}
                </PieChart>
            </Box>

            <ProviderGroupsList
                groups={groups}
                selectedGroupName={selectedGroup}
                onSelectGroup={(name) => setSelectedGroup(name)}
            />

        </Card>
    );
}

