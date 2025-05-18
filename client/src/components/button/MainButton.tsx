import React from 'react';
import Button, { ButtonPropsVariantOverrides } from '@mui/material/Button';
import { OverridableStringUnion } from '@mui/types';

interface MainButtonProps {
    children: React.ReactNode;
    func?: () => void;
    styles?: React.CSSProperties;
    full?: boolean;
    variant?: OverridableStringUnion<"contained" | "text" | "outlined", ButtonPropsVariantOverrides>;
}

const MainButton: React.FC<MainButtonProps> = ({
                                                   children,
                                                   func,
                                                   styles,
                                                   full = true,
                                                   variant = 'contained',
                                               }) => {
    return (
        <>
            {func ?
                (
                    <Button
                        fullWidth={full}
                        variant={variant}
                        onClick={func}
                        style={styles}
                    >
                        {children}
                    </Button>
                ) : (
                    <Button
                        type={"submit"}
                        fullWidth={full}
                        variant={variant}
                        style={styles}
                    >
                        {children}
                    </Button>
                )
            }
        </>
    );
};

export default MainButton;
