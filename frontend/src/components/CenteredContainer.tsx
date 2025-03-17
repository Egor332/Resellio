import React, { ReactNode } from 'react';
import './CenteredContainer.css';

interface CenteredContainerProps {
    children: ReactNode;
}

const CenteredContainer: React.FC<CenteredContainerProps> = ({ children }) => {
  return <div className="centered-container">
    {children}
    </div>;
};

export default CenteredContainer;