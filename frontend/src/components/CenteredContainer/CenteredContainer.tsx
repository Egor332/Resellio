import React, { ReactNode } from 'react'
import './CenteredContainer.module.css'

const CenteredContainer: React.FC<{ children: ReactNode}> = ({ children }) => {
  return <div className="centered-container">{children}</div>
}

export default CenteredContainer
