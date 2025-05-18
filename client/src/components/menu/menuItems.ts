import HomeRoundedIcon from '@mui/icons-material/HomeRounded';
import AnalyticsRoundedIcon from '@mui/icons-material/AnalyticsRounded';
import SettingsRoundedIcon from '@mui/icons-material/SettingsRounded';
import InfoRoundedIcon from '@mui/icons-material/InfoRounded';
import HelpRoundedIcon from '@mui/icons-material/HelpRounded';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';
import LoginRoundedIcon from '@mui/icons-material/LoginRounded';

export const mainListItems = [
    { text: 'Home', icon: HomeRoundedIcon, link: '/' },
    { text: 'Dashboard', icon: AnalyticsRoundedIcon, link: '/dashboard' },
];

export const secondaryListItems = [
    { text: 'Settings', icon: SettingsRoundedIcon, link: '/settings' },
    { text: 'About', icon: InfoRoundedIcon, link: '/about' },
    { text: 'Feedback', icon: HelpRoundedIcon, link: '/feedback' },
];

export const additionalItems = [
    { text: 'Profile', icon: AccountCircleRoundedIcon, link: '/profile' },
    { text: 'Login', icon: LoginRoundedIcon, link: '/login' },
];

export const allListItems = [
    ...mainListItems,
    ...secondaryListItems,
    ...additionalItems,
];
