import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import {Box, Link, Rating} from '@mui/material';
import {getProviderStatsTable} from "../../utils/services/dataAnalyticsService.ts";
import Typography from "@mui/material/Typography";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";

interface ProviderStats {
    id: number;
    name: string;
    website: string;
    logo: string;
    lastShare: number | null;
    averageYear: number | null;
    peak: number | null;
    min: number | null;
    since: number | null;
    averageRating: number;
}

export default function ProviderStatsTable() {
    const [rows, setRows] = useState<ProviderStats[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [year, setYear] = useState<number>(2023);
    const availableYears = [2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024]

    useEffect(() => {
        async function fetchData() {
            try {
                const data = await getProviderStatsTable(year);
                console.log(data.$values);
                setRows(data.$values);
            } catch (error) {
                console.error(error);
            } finally {
                setLoading(false);
            }
        }
        fetchData();
    }, [year]);

    const columns: GridColDef[] = [
        {
            field: 'logo',
            headerName: '',
            width: 50,
            sortable: false,
            filterable: false,
            renderCell: (params) => (
                <img
                    src={params.value}
                    alt={params.row.name}
                    style={{ width: 40, height: 40, objectFit: 'contain' }}
                />
            ),
        },
        {
            field: 'name',
            headerName: 'Provider',
            width: 140,
            renderCell: (params) => (
                <Link href={params.row.website} target="_blank" rel="noopener noreferrer" underline="hover"
                      style={{color: 'white'}}>
                    {params.value}
                </Link>
            ),
        },
        { field: 'lastShare', headerName: 'Last Share (%)', width: 130, type: 'number' },
        {
            field: 'averageYear',
            headerName: `Average ${year} (%)`,
            width: 150,
            type: 'number',
            valueFormatter: (value) => {
                return value !== null && value !== undefined ? value.toString() : 'N/A';
            }
        },
        { field: 'peak', headerName: 'Peak (%)', width: 100, type: 'number' },
        { field: 'min', headerName: 'Min (%)', width: 100, type: 'number' },
        { field: 'since', headerName: 'Since Year', width: 110, type: 'number' },
        {
            field: 'averageRating',
            headerName: 'Avg Rating',
            width: 160,
            type: 'number',
            sortable: true,
            renderCell: (params) => {
                const value = Number(params.value);
                return (
                    <Rating
                        value={value}
                        precision={0.1}
                        readOnly
                        size="small"
                    />
                );
            },
        }
    ];
    return (
        <Box sx={{ height: 480, width: '100%', marginBottom: '120px' }}>
            <Box display="flex" alignItems="center" mb={2}>
                <Typography variant="h6" mr={2}>
                    Provider Statistics
                </Typography>
                <Select
                    value={year}
                    onChange={(e) => setYear(Number(e.target.value))}
                    size="small"
                >
                    {availableYears.map((y) => (
                        <MenuItem key={y} value={y}>
                            {y}
                        </MenuItem>
                    ))}
                </Select>
            </Box>

            <DataGrid
                loading={loading}
                rows={rows}
                columns={columns}
                pageSizeOptions={[10, 15, 20]}
                initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
                disableColumnResize
                density="compact"
                getRowClassName={(params) => (params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd')}
            />
        </Box>
    );
}
