import React from 'react';
import { useTheme } from '@mui/material/styles';
import { Box } from '@mui/material';
import logoSrc from '../../assets/logo.png';
import Typography from "@mui/material/Typography";

interface LogoProps {
    full?: boolean;
    bigIcon?: boolean;
    bigText?: boolean;
    center?: boolean;
    row?: boolean;
}

const Logo: React.FC<LogoProps> = ({full = false, bigIcon = false, bigText = false, center = false, row = false} : LogoProps) => {
    const theme = useTheme();

    return (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: center ? 'center' : 'start',
                height: '100%',
                width: '100%',
            }}
        >
            <img
                src={logoSrc}
                alt="Logo"
                style={{
                    height: bigIcon ? '60px' : '35px',
                    padding: '2px',
                    width: 'auto',
                    filter: theme.palette.mode === 'dark' ? 'invert(1)' : 'none',
                }}
            />
            {full &&
                <Box sx={{
                    display: 'flex',
                    flexDirection: row ? 'row' : 'column',
                    justifyContent: 'center',
                    alignItems: 'center',
                    marginTop: row ? '6px' : '',
                }}>
                    <Typography sx={{ fontSize: bigText ? '2rem' : '1rem', fontWeight: 'bold', marginLeft: row ? '6px' : '',}}>
                        UK</Typography>
                    <Typography sx={{ fontSize: bigText ? '1.5rem' : '1rem', lineHeight: 1, marginLeft: row ? '6px' : '', }}>
                        Electricity Providers</Typography>
                </Box>
            }
        </Box>
    );
};

export default Logo;