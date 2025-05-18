import React from 'react';
import SpotlightCard from "./SpotlightCard.tsx";

interface CardComponentProps {
    children?: React.ReactNode;
}

export const CardComponent: React.FC<CardComponentProps> = ({ children  }) => {
    return (
        <SpotlightCard className="custom-spotlight-card" spotlightColor="rgba(0, 229, 255, 0.2)">
            <div>
                {children}
            </div>
        </SpotlightCard>
    );
};
