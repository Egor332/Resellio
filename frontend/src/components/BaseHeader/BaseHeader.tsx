import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  CssBaseline,
  IconButton,
  Menu,
  MenuItem,
} from '@mui/material'
import { Link } from 'react-router-dom'
import { ReactNode, useState, MouseEvent } from 'react'

// Define the structure for dropdown menu items
interface MenuItemType {
  label: string
  path?: string
  onClick?: () => void
}

// Define the structure for navigation icons
export interface NavItem {
  icon: ReactNode
  path?: string // Optional - if direct link without dropdown
  menuItems?: MenuItemType[] // Optional - for dropdown menus
  tooltip?: string
}

interface BaseHeaderProps {
  title: string
  backgroundColor: string
  linkPath: string
  navItems?: NavItem[] // Optional array of navigation items
}

function BaseHeader({
  title,
  backgroundColor,
  linkPath,
  navItems = [],
}: BaseHeaderProps) {
  // State for handling dropdown menus
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const [activeMenuIndex, setActiveMenuIndex] = useState<number | null>(null)

  const handleMenuOpen = (event: MouseEvent<HTMLElement>, index: number) => {
    setAnchorEl(event.currentTarget)
    setActiveMenuIndex(index)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
    setActiveMenuIndex(null)
  }

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar
        component="nav"
        sx={{
          bgcolor: backgroundColor,
        }}
      >
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
          <Typography
            variant="h6"
            component={Link}
            to={linkPath}
            color="inherit"
            sx={{
              textDecoration: 'none',
              margin: '8px',
              padding: '4px',
            }}
          >
            {title}
          </Typography>

          {/* Navigation Items */}
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            {navItems.map((item, index) => (
              <Box key={index}>
                {item.path ? (
                  // Direct link icon
                  <IconButton
                    component={Link}
                    to={item.path}
                    color="inherit"
                    title={item.tooltip}
                  >
                    {item.icon}
                  </IconButton>
                ) : (
                  // Dropdown menu icon
                  <IconButton
                    color="inherit"
                    onClick={(e) => handleMenuOpen(e, index)}
                    title={item.tooltip}
                  >
                    {item.icon}
                  </IconButton>
                )}

                {/* Menu dropdown if any */}
                {item.menuItems && (
                  <Menu
                    anchorEl={anchorEl}
                    open={activeMenuIndex === index}
                    onClose={handleMenuClose}
                  >
                    {item.menuItems.map((menuItem, menuIndex) => (
                      <MenuItem
                        key={menuIndex}
                        onClick={() => {
                          handleMenuClose()
                          if (menuItem.onClick) {
                            menuItem.onClick()
                          }
                        }}
                        component={menuItem.path ? Link : 'li'}
                        to={menuItem.path}
                      >
                        {menuItem.label}
                      </MenuItem>
                    ))}
                  </Menu>
                )}
              </Box>
            ))}
          </Box>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </Box>
  )
}

export default BaseHeader
