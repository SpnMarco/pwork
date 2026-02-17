import {
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Box,
  Divider,
} from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import EventIcon from '@mui/icons-material/Event';
import DescriptionIcon from '@mui/icons-material/Description';
import PersonIcon from '@mui/icons-material/Person';
import { useAuth } from '../../context/AuthContext';

const drawerWidth = 240;

const Sidebar = ({ mobileOpen, onClose }) => {
  const location = useLocation();
  const { user } = useAuth();

  // Define menu items with role restrictions
  const menuItems = [
    { 
      text: 'Dashboard', 
      icon: <DashboardIcon />, 
      path: '/' 
    },
    { 
      text: 'Pazienti', 
      icon: <PeopleIcon />, 
      path: '/pazienti', 
      roles: ['Admin', 'Medico', 'Receptionist'] 
    },
    { 
      text: 'Medici', 
      icon: <LocalHospitalIcon />, 
      path: '/medici',
      roles: ['Paziente', 'Admin', 'Receptionist'] // Medici non vedono altri medici
    },
    { 
      text: 'Appuntamenti', 
      icon: <EventIcon />, 
      path: '/appuntamenti' 
    },
    { 
      text: 'Referti', 
      icon: <DescriptionIcon />, 
      path: '/referti' 
    },
    { 
      text: 'Profilo', 
      icon: <PersonIcon />, 
      path: '/profilo' 
    },
  ];

  // Filter menu items based on user role
  const filteredMenuItems = menuItems.filter(item => {
    if (!item.roles) return true; // Show if no role restriction
    return item.roles.includes(user?.ruolo);
  });

  const drawer = (
    <Box>
      <Toolbar />
      <Divider />
      <List>
        {filteredMenuItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton
              component={Link}
              to={item.path}
              selected={location.pathname === item.path}
              onClick={onClose}
            >
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Box>
  );

  return (
    <Box
      component="nav"
      sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
    >
      {/* Mobile drawer */}
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={onClose}
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: 'block', sm: 'none' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
        }}
      >
        {drawer}
      </Drawer>
      {/* Desktop drawer */}
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: 'none', sm: 'block' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
        }}
        open
      >
        {drawer}
      </Drawer>
    </Box>
  );
};

export default Sidebar;
