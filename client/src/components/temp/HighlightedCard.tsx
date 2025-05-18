import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import ChevronRightRoundedIcon from '@mui/icons-material/ChevronRightRounded';
import InsightsRoundedIcon from '@mui/icons-material/InsightsRounded';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
import LoginButton from "../button/LoginButton.tsx";
import {useNavigate} from "react-router-dom";

type HighlightedCardProps = {
    isAuthenticated: boolean;
};

export default function HighlightedCard({ isAuthenticated}: HighlightedCardProps) {
    const theme = useTheme();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'));
    const navigate = useNavigate();

    const onProfileClick = () => navigate('/profile');

    return (
        <Card sx={{ height: '100%' }}>
            <CardContent>
                <InsightsRoundedIcon sx={{ fontSize: 40, color: 'primary.main' }} />
                <Typography
                    component="h2"
                    variant="subtitle1"
                    gutterBottom
                    sx={{ fontWeight: '600' }}
                >
                    Get Local Insights
                </Typography>
                <Typography sx={{ color: 'text.secondary', mb: 1 }}>
                    Access local statistics to improve your decisions.
                </Typography>
                {isAuthenticated ? (
                    <Button
                        variant="contained"
                        size="medium"
                        color="primary"
                        endIcon={<ChevronRightRoundedIcon />}
                        fullWidth
                        onClick={onProfileClick}
                    >
                        Profile
                    </Button>
                ) : (
                    <LoginButton />
                )}
            </CardContent>
        </Card>
    );
}