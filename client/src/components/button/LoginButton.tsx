import React from 'react';

import MainButton from "./MainButton.tsx";

const LoginButton: React.FC = () => {
    return (
        <MainButton styles={{ marginTop: '20px' }} full={true}>
            Login
        </MainButton>
    );
};

export default LoginButton;
