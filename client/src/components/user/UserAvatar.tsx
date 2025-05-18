import React from 'react';
import Avatar from '@mui/material/Avatar';

interface UserAvatarProps {
    alt: string;
    src: string;
    size?: number;
}

const UserAvatar: React.FC<UserAvatarProps> = ({ alt, src, size = 36 }) => {
    return (
        <Avatar
            alt={alt}
            src={src}
            sx={{ width: size, height: size }}
        />
    );
};

export default UserAvatar;