import { styled } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import Breadcrumbs, { breadcrumbsClasses } from '@mui/material/Breadcrumbs';
import NavigateNextRoundedIcon from '@mui/icons-material/NavigateNextRounded';
import {useLocation} from "react-router-dom";
import {allListItems} from "../menu/menuItems.ts";

const StyledBreadcrumbs = styled(Breadcrumbs)(({ theme }) => ({
    margin: theme.spacing(1, 0),
    [`& .${breadcrumbsClasses.separator}`]: {
        color: (theme).palette.action.disabled,
        margin: 1,
    },
    [`& .${breadcrumbsClasses.ol}`]: {
        alignItems: 'center',
    },
}));

export default function NavbarBreadcrumbs() {
    const location = useLocation();
    const currentPath = location.pathname;

    const matchedItem = allListItems.find(item => item.link === "/" ? currentPath === "/" : currentPath.startsWith(item.link));

    return (
        <StyledBreadcrumbs
            aria-label="breadcrumb"
            separator={<NavigateNextRoundedIcon fontSize="small" />}
        >
            <Typography variant="body1">UK Electricity Providers</Typography>
            {matchedItem && (
                <Typography
                    variant="body1"
                    sx={{ color: 'text.primary', fontWeight: 600 }}
                >
                    {matchedItem.text}
                </Typography>
            )}
        </StyledBreadcrumbs>
    );
}
