import { useEffect, useState } from 'react';
import {
  Box,
  Grid,
  Paper,
  Typography,
  Card as MuiCard,
  CardContent,
  List,
  ListItem,
  ListItemText,
  Divider,
  Chip,
} from '@mui/material';
import EventIcon from '@mui/icons-material/Event';
import DescriptionIcon from '@mui/icons-material/Description';
import PeopleIcon from '@mui/icons-material/People';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import { useAuth } from '../context/AuthContext';
import { useNotification } from '../context/NotificationContext';
import appuntamentoService from '../services/appuntamentoService';
import Spinner from '../components/common/Spinner';
import Button from '../components/common/Button';
import { useNavigate } from 'react-router-dom';
import { formatDate, formatTime, getRelativeDateString } from '../utils/dateUtils';

const DashboardPage = () => {
  const { user } = useAuth();
  const { showError } = useNotification();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [upcomingAppointments, setUpcomingAppointments] = useState([]);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      // Load upcoming appointments
      const appointments = await appuntamentoService.getUpcoming();
      setUpcomingAppointments(appointments);
    } catch (error) {
      console.error('Error loading dashboard:', error);
      showError('Errore nel caricamento dei dati');
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (stato) => {
    switch (stato?.toLowerCase()) {
      case 'confermato':
        return 'success';
      case 'in attesa':
        return 'warning';
      case 'completato':
        return 'info';
      case 'annullato':
        return 'error';
      default:
        return 'default';
    }
  };

  if (loading) {
    return <Spinner />;
  }

  return (
    <Box>
      <Typography variant="h4" fontWeight={600} gutterBottom>
        Benvenuto, {user?.username}!
      </Typography>
      <Typography variant="body1" color="text.secondary" gutterBottom>
        Ecco una panoramica delle tue attività
      </Typography>

      {/* Quick Stats */}
      <Grid container spacing={3} sx={{ mt: 2 }}>
        <Grid item xs={12} sm={6} md={3}>
          <MuiCard elevation={2}>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="text.secondary" variant="body2">
                    Appuntamenti
                  </Typography>
                  <Typography variant="h4" fontWeight={600}>
                    {upcomingAppointments.length}
                  </Typography>
                </Box>
                <EventIcon sx={{ fontSize: 48, color: 'primary.main', opacity: 0.6 }} />
              </Box>
            </CardContent>
          </MuiCard>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <MuiCard elevation={2}>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="text.secondary" variant="body2">
                    Referti
                  </Typography>
                  <Typography variant="h4" fontWeight={600}>
                    -
                  </Typography>
                </Box>
                <DescriptionIcon sx={{ fontSize: 48, color: 'info.main', opacity: 0.6 }} />
              </Box>
            </CardContent>
          </MuiCard>
        </Grid>

        {(user?.ruolo === 'Admin' || user?.ruolo === 'Receptionist') && (
          <>
            <Grid item xs={12} sm={6} md={3}>
              <MuiCard elevation={2}>
                <CardContent>
                  <Box display="flex" alignItems="center" justifyContent="space-between">
                    <Box>
                      <Typography color="text.secondary" variant="body2">
                        Pazienti
                      </Typography>
                      <Typography variant="h4" fontWeight={600}>
                        -
                      </Typography>
                    </Box>
                    <PeopleIcon sx={{ fontSize: 48, color: 'success.main', opacity: 0.6 }} />
                  </Box>
                </CardContent>
              </MuiCard>
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
              <MuiCard elevation={2}>
                <CardContent>
                  <Box display="flex" alignItems="center" justifyContent="space-between">
                    <Box>
                      <Typography color="text.secondary" variant="body2">
                        Medici
                      </Typography>
                      <Typography variant="h4" fontWeight={600}>
                        -
                      </Typography>
                    </Box>
                    <LocalHospitalIcon sx={{ fontSize: 48, color: 'warning.main', opacity: 0.6 }} />
                  </Box>
                </CardContent>
              </MuiCard>
            </Grid>
          </>
        )}
      </Grid>

      {/* Main Content */}
      <Grid container spacing={3} sx={{ mt: 1 }}>
        {/* Upcoming Appointments */}
        <Grid item xs={12} md={8}>
          <Paper elevation={2} sx={{ p: 3 }}>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
              <Typography variant="h6" fontWeight={600}>
                Prossimi Appuntamenti
              </Typography>
              <Button
                variant="contained"
                size="small"
                onClick={() => navigate('/appuntamenti/nuovo')}
              >
                Prenota
              </Button>
            </Box>

            {upcomingAppointments.length === 0 ? (
              <Box textAlign="center" py={4}>
                <EventIcon sx={{ fontSize: 64, color: 'text.disabled', mb: 2 }} />
                <Typography variant="body1" color="text.secondary">
                  Nessun appuntamento in programma
                </Typography>
                <Button
                  variant="outlined"
                  sx={{ mt: 2 }}
                  onClick={() => navigate('/appuntamenti/nuovo')}
                >
                  Prenota un appuntamento
                </Button>
              </Box>
            ) : (
              <List>
                {upcomingAppointments.slice(0, 5).map((appointment, index) => (
                  <Box key={appointment.id || index}>
                    <ListItem
                      sx={{
                        cursor: 'pointer',
                        '&:hover': { bgcolor: 'action.hover' },
                        borderRadius: 1,
                      }}
                      onClick={() => navigate(`/appuntamenti/${appointment.id}`)}
                    >
                      <ListItemText
                        primary={
                          <Box display="flex" alignItems="center" gap={1}>
                            <Typography variant="subtitle1" fontWeight={500}>
                              {appointment.medicoNome || 'Medico'}
                            </Typography>
                            <Chip
                              label={appointment.stato || 'In attesa'}
                              size="small"
                              color={getStatusColor(appointment.stato)}
                            />
                          </Box>
                        }
                        secondary={
                          <Box mt={0.5}>
                            <Typography variant="body2" color="text.secondary">
                              {getRelativeDateString(appointment.data)} - {formatTime(appointment.ora)}
                            </Typography>
                            {appointment.note && (
                              <Typography variant="body2" color="text.secondary">
                                {appointment.note}
                              </Typography>
                            )}
                          </Box>
                        }
                      />
                    </ListItem>
                    {index < upcomingAppointments.length - 1 && <Divider />}
                  </Box>
                ))}
              </List>
            )}

            {upcomingAppointments.length > 5 && (
              <Box textAlign="center" mt={2}>
                <Button variant="text" onClick={() => navigate('/appuntamenti')}>
                  Vedi tutti gli appuntamenti
                </Button>
              </Box>
            )}
          </Paper>
        </Grid>

        {/* Quick Actions */}
        <Grid item xs={12} md={4}>
          <Paper elevation={2} sx={{ p: 3 }}>
            <Typography variant="h6" fontWeight={600} mb={2}>
              Azioni Rapide
            </Typography>
            <Box display="flex" flexDirection="column" gap={2}>
              <Button
                variant="outlined"
                startIcon={<EventIcon />}
                fullWidth
                onClick={() => navigate('/appuntamenti/nuovo')}
              >
                Prenota Appuntamento
              </Button>
              <Button
                variant="outlined"
                startIcon={<LocalHospitalIcon />}
                fullWidth
                onClick={() => navigate('/medici')}
              >
                Trova Medico
              </Button>
              <Button
                variant="outlined"
                startIcon={<DescriptionIcon />}
                fullWidth
                onClick={() => navigate('/referti')}
              >
                I Miei Referti
              </Button>
            </Box>
          </Paper>

          {/* User Info */}
          <Paper elevation={2} sx={{ p: 3, mt: 3 }}>
            <Typography variant="h6" fontWeight={600} mb={2}>
              Informazioni Account
            </Typography>
            <Box>
              <Typography variant="body2" color="text.secondary">
                Username
              </Typography>
              <Typography variant="body1" mb={1}>
                {user?.username}
              </Typography>

              <Typography variant="body2" color="text.secondary">
                Email
              </Typography>
              <Typography variant="body1" mb={1}>
                {user?.email || 'Non disponibile'}
              </Typography>

              <Typography variant="body2" color="text.secondary">
                Ruolo
              </Typography>
              <Chip label={user?.ruolo || 'Paziente'} size="small" color="primary" />
            </Box>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};

export default DashboardPage;
