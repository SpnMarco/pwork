import { useEffect, useState } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Avatar,
  Divider,
  Chip,
  List,
  ListItem,
  ListItemText,
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import EmailIcon from '@mui/icons-material/Email';
import BadgeIcon from '@mui/icons-material/Badge';
import { useAuth } from '../context/AuthContext';
import { useNotification } from '../context/NotificationContext';
import pazienteService from '../services/pazienteService';
import Spinner from '../components/common/Spinner';
import { formatDate } from '../utils/dateUtils';

const ProfiloPage = () => {
  const { user } = useAuth();
  const { showError } = useNotification();
  const [loading, setLoading] = useState(true);
  const [pazienteData, setPazienteData] = useState(null);

  useEffect(() => {
    if (user?.pazienteId) {
      loadPazienteData();
    } else {
      setLoading(false);
    }
  }, [user]);

  const loadPazienteData = async () => {
    try {
      const data = await pazienteService.getById(user.pazienteId);
      setPazienteData(data);
    } catch (error) {
      console.error('Error loading paziente data:', error);
      showError('Errore nel caricamento dei dati del profilo');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <Spinner />;
  }

  return (
    <Box>
      <Typography variant="h4" fontWeight={600} gutterBottom>
        Il Mio Profilo
      </Typography>
      <Typography variant="body1" color="text.secondary" gutterBottom>
        Visualizza e gestisci le tue informazioni personali
      </Typography>

      <Grid container spacing={3} sx={{ mt: 2 }}>
        {/* Account Information */}
        <Grid item xs={12} md={4}>
          <Paper elevation={2} sx={{ p: 3, textAlign: 'center' }}>
            <Avatar
              sx={{
                width: 120,
                height: 120,
                mx: 'auto',
                mb: 2,
                bgcolor: 'primary.main',
                fontSize: '3rem',
              }}
            >
              {user?.username?.charAt(0).toUpperCase()}
            </Avatar>
            <Typography variant="h5" fontWeight={600} gutterBottom>
              {user?.username}
            </Typography>
            <Chip label={user?.ruolo || 'Paziente'} color="primary" sx={{ mb: 2 }} />
            <Divider sx={{ my: 2 }} />
            <Box textAlign="left">
              <Typography variant="body2" color="text.secondary" gutterBottom>
                Email
              </Typography>
              <Typography variant="body1" gutterBottom>
                {user?.email || 'Non disponibile'}
              </Typography>
            </Box>
          </Paper>
        </Grid>

        {/* Patient Details (if patient) */}
        {user?.ruolo === 'Paziente' && pazienteData && (
          <Grid item xs={12} md={8}>
            <Paper elevation={2} sx={{ p: 3 }}>
              <Typography variant="h6" fontWeight={600} gutterBottom>
                Dati Anagrafici
              </Typography>
              <Divider sx={{ mb: 3 }} />

              <Grid container spacing={3}>
                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Nome
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {pazienteData.nome}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Cognome
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {pazienteData.cognome}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Codice Fiscale
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {pazienteData.codiceFiscale}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Data di Nascita
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {formatDate(pazienteData.dataNascita)}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Email
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {pazienteData.email}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Telefono
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {pazienteData.telefono}
                  </Typography>
                </Grid>

                {pazienteData.indirizzo && (
                  <>
                    <Grid item xs={12}>
                      <Typography variant="body2" color="text.secondary">
                        Indirizzo
                      </Typography>
                      <Typography variant="body1" fontWeight={500}>
                        {pazienteData.indirizzo}
                      </Typography>
                    </Grid>

                    <Grid item xs={12} sm={6}>
                      <Typography variant="body2" color="text.secondary">
                        Città
                      </Typography>
                      <Typography variant="body1" fontWeight={500}>
                        {pazienteData.citta}
                      </Typography>
                    </Grid>

                    <Grid item xs={12} sm={6}>
                      <Typography variant="body2" color="text.secondary">
                        CAP
                      </Typography>
                      <Typography variant="body1" fontWeight={500}>
                        {pazienteData.cap}
                      </Typography>
                    </Grid>
                  </>
                )}

                <Grid item xs={12}>
                  <Typography variant="body2" color="text.secondary">
                    Data Registrazione
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {formatDate(pazienteData.dataRegistrazione)}
                  </Typography>
                </Grid>
              </Grid>
            </Paper>
          </Grid>
        )}

        {/* For other roles (Medico, Admin, Receptionist) */}
        {user?.ruolo !== 'Paziente' && (
          <Grid item xs={12} md={8}>
            <Paper elevation={2} sx={{ p: 3 }}>
              <Typography variant="h6" fontWeight={600} gutterBottom>
                Informazioni Account
              </Typography>
              <Divider sx={{ mb: 3 }} />

              <List>
                <ListItem>
                  <PersonIcon sx={{ mr: 2, color: 'text.secondary' }} />
                  <ListItemText
                    primary="Username"
                    secondary={user?.username}
                  />
                </ListItem>
                <ListItem>
                  <EmailIcon sx={{ mr: 2, color: 'text.secondary' }} />
                  <ListItemText
                    primary="Email"
                    secondary={user?.email || 'Non disponibile'}
                  />
                </ListItem>
                <ListItem>
                  <BadgeIcon sx={{ mr: 2, color: 'text.secondary' }} />
                  <ListItemText
                    primary="Ruolo"
                    secondary={user?.ruolo}
                  />
                </ListItem>
              </List>

              {user?.medicoId && (
                <Box sx={{ mt: 3, p: 2, bgcolor: 'info.light', borderRadius: 1 }}>
                  <Typography variant="body2" color="info.dark">
                    Sei registrato come medico. Le tue informazioni professionali
                    sono gestite dall'amministrazione.
                  </Typography>
                </Box>
              )}
            </Paper>
          </Grid>
        )}
      </Grid>
    </Box>
  );
};

export default ProfiloPage;
