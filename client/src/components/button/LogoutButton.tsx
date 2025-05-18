import React from 'react';
import MainButton from "./MainButton.tsx";
import LogoutRoundedIcon from "@mui/icons-material/LogoutRounded";
import Button from "@mui/material/Button";
import ListItemText from "@mui/material/ListItemText";
import ListItemIcon, {listItemIconClasses} from "@mui/material/ListItemIcon";
import Typography from "@mui/material/Typography";

const logoutVariants: Record<string, React.ReactNode> = {
    outlined: (
        <Button type={"submit"} variant="outlined" fullWidth startIcon={<LogoutRoundedIcon />}>
            Logout
        </Button>
    ),
    menu: (
        <Button
            type="submit"
            sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                width: '100%',
                textTransform: 'none',
                paddingLeft: '20px',
                paddingRight: '20px',
            }}
        >
            <Typography color={'textPrimary'}>Logout</Typography>
            <LogoutRoundedIcon fontSize="small" />
        </Button>
    ),
    default: (
        <MainButton styles={{ marginTop: '20px' }} full={true}>
            Logout
        </MainButton>
    ),
};


interface LogoutButtonProps {
    version?: string;
}

const LogoutButton: React.FC<LogoutButtonProps> = ({version = 'full'}: LogoutButtonProps) => {
    return (
        <>
            {logoutVariants[version] ?? logoutVariants.default}
        </>

    );
};

export default LogoutButton;
