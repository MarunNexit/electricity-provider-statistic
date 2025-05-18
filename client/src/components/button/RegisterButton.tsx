import React from 'react';
import MainButton from "./MainButton.tsx";



const RegisterButton: React.FC = () => {
    return (
        <MainButton styles={{marginTop: '20px', minHeight: '45px'}} full={true}>
            Register
        </MainButton>
    );
};

export default RegisterButton;
