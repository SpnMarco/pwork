import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Divider,
  List,
  ListItem,
  ListItemText,
  Chip,
  Tab,
  Tabs,
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import EditIcon from '@mui/icons-material/Edit';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useNotification } from '../context/NotificationContext';
import pazienteService from '../services/pazienteService';
import Button from '../components/common/Button';
import Spinner from '../components/common/Spinner';
import { formatDate } from '../utils/dateUtils';

const PazienteDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { showError } = useNotification();
  const [loading, setLoading] = useState(true);
  const [paziente, setPaziente] = useState(null);
  const [appuntamenti, setAppuntamenti] = useState([]);
  const [referti, setReferti] = useState([]);
  const [currentTab, setCurrentTab] = useState(0);

  useEffect(() => {
    loadPazienteData();
  }, [id]);

  const loadPazienteData = async () => {
    try {
      setLoading(true);
      const [pazienteData, appuntamentiData, refertiData] = await Promise.all([
        pazienteService.getById(id),
        pazienteService.getAppuntamenti(id).catch(() => []),
        pazienteService.getReferti(id).catch(() => []),
      ]);
      
      setPaziente(pazienteData);
      setAppuntamenti(appuntamentiData);
      setReferti(refertiData);
    } catch (error) {
      console.error('Error loading paziente:', error);
      showError('Errore nel caricamento del paziente');
      navigate('/pazienti');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <Spinner />;
  }

  if (!paziente) {
    return (
      <Box textAlign="center" py={4}>
        <Typography variant="h6">Paziente non trovato</Typography>
        <Button onClick={() => navigate('/pazienti')} sx={{ mt: 2 }}>
          Torna alla lista
        </Button>
      </Box>
    );
  }

  return (
    <Box>
      <Box display="flex" alignItems="center" mb={3}>
        <Button
          variant="text"
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate('/pazienti')}
          sx={{ mr: 2 }}
        >
          Indietro
        </Button>
        <Typography variant="h4" fontWeight={600} sx={{ flexGrow: 1 }}>
          Dettaglio Paziente
        </Typography>
        <Button
          variant="contained"
          startIcon={<EditIcon />}
          onClick={() => navigate(`/pazienti/${id}/modifica`)}
        >
          Modifica
        </Button>
      </Box>

      <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
        <Box display="flex" alignItems="center" mb={3}>
          <PersonIcon sx={{ fontSize: 48, color: 'primary.main', mr: 2 }} />
          <Box>
            <Typography variant="h5" fontWeight={600}>
              {paziente.nome} {paziente.cognome}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Codice Fiscale: {paziente.codiceFiscale}
            </Typography>
          </Box>
        </Box>

        <Divider sx={{ my: 2 }} />

        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <Typography variant="body2" color="text.secondary">
              Data di Nascita
            </Typography>
            <Typography variant="body1" mb={2}>
              {formatDate(paziente.dataNascita)}
            </Typography>

            <Typography variant="body2" color="text.secondary">
              Email
            </Typography>
            <Typography variant="body1" mb={2}>
              {paziente.email || 'Non disponibile'}
            </Typography>

            <Typography variant="body2" color="text.secondary">
              Telefono
            </Typography>
            <Typography variant="body1" mb={2}>
              {paziente.telefono || 'Non disponibile'}
            </Typography>
          </Grid>

          <Grid item xs={12} sm={6}>
            <Typography variant="body2" color="text.secondary">
              Indirizzo
            </Typography>
            <Typography variant="body1" mb={2}>
              {paziente.indirizzo || 'Non disponibile'}
            </Typography>

            <Typography variant="body2" color="text.secondary">
              Data Registrazione
            </Typography>
            <Typography variant="body1" mb={2}>
              {paziente.dataRegistrazione ? formatDate(paziente.dataRegistrazione) : 'Non disponibile'}
            </Typography>
          </Grid>
        </Grid>
      </Paper>

      <Paper elevation={2}>
        <Tabs value={currentTab} onChange={(e, val) => setCurrentTab(val)}>
          <Tab label={`Appuntamenti (${appuntamenti.length})`} />
          <Tab label={`Referti (${referti.length})`} />
        </Tabs>

        <Box p={3}>
          {currentTab === 0 && (
            <Box>
              {appuntamenti.length === 0 ? (
                <Typography variant="body2" color="text.secondary" textAlign="center" py={3}>
                  Nessun appuntamento registrato
                </Typography>
              ) : (
                <List>
                  {appuntamenti.map((app) => (
                    <ListItem
                      key={app.id}
                      sx={{
                        cursor: 'pointer',
                        '&:hover': { bgcolor: 'action.hover' },
                        borderRadius: 1,
                      }}
                      onClick={() => navigate(`/appuntamenti/${app.id}`)}
                    >
                      <ListItemText
                        primary={`${formatDate(app.data)} - ${app.medicoNome}`}
                        secondary={app.note || 'Nessuna nota'}
                      />
                      <Chip label={app.stato} size="small" />
                    </ListItem>
                  ))}
                </List>
              )}
            </Box>
          )}

          {currentTab === 1 && (
            <Box>
              {referti.length === 0 ? (
                <Typography variant="body2" color="text.secondary" textAlign="center" py={3}>
                  Nessun referto disponibile
                </Typography>
              ) : (
                <List>
                  {referti.map((ref) => (
                    <ListItem
                      key={ref.id}
                      sx={{
                        cursor: 'pointer',
                        '&:hover': { bgcolor: 'action.hover' },
                        borderRadius: 1,
                      }}
                      onClick={() => navigate(`/referti/${ref.id}`)}
                    >
                      <ListItemText
                        primary={ref.titolo || 'Referto'}
                        secondary={`${formatDate(ref.data)} - Dr. ${ref.medicoNome}`}
                      />
                    </ListItem>
                  ))}
                </List>
              )}
            </Box>
          )}
        </Box>
      </Paper>
    </Box>
  );
};

export default PazienteDetailPage;
