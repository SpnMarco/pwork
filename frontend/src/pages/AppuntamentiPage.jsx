import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  IconButton,
  Chip,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Grid,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import AddIcon from '@mui/icons-material/Add';
import EventIcon from '@mui/icons-material/Event';
import { useNotification } from '../context/NotificationContext';
import appuntamentoService from '../services/appuntamentoService';
import Table from '../components/common/Table';
import Button from '../components/common/Button';
import Spinner from '../components/common/Spinner';
import { formatDate, formatTime } from '../utils/dateUtils';

const AppuntamentiPage = () => {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotification();
  const [loading, setLoading] = useState(true);
  const [appuntamenti, setAppuntamenti] = useState([]);
  const [filteredAppuntamenti, setFilteredAppuntamenti] = useState([]);
  const [statoFilter, setStatoFilter] = useState('');

  useEffect(() => {
    loadAppuntamenti();
  }, []);

  useEffect(() => {
    filterAppuntamenti();
  }, [statoFilter, appuntamenti]);

  const loadAppuntamenti = async () => {
    try {
      setLoading(true);
      const data = await appuntamentoService.getAll();
      setAppuntamenti(data);
      setFilteredAppuntamenti(data);
    } catch (error) {
      console.error('Error loading appuntamenti:', error);
      showError('Errore nel caricamento degli appuntamenti');
    } finally {
      setLoading(false);
    }
  };

  const filterAppuntamenti = () => {
    if (!statoFilter) {
      setFilteredAppuntamenti(appuntamenti);
      return;
    }

    const filtered = appuntamenti.filter((app) => 
      app.stato?.toLowerCase() === statoFilter.toLowerCase()
    );
    setFilteredAppuntamenti(filtered);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Sei sicuro di voler cancellare questo appuntamento?')) {
      return;
    }

    try {
      await appuntamentoService.delete(id);
      showSuccess('Appuntamento cancellato con successo');
      loadAppuntamenti();
    } catch (error) {
      console.error('Error deleting appuntamento:', error);
      showError('Errore nella cancellazione dell\'appuntamento');
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

  const columns = [
    {
      id: 'data',
      label: 'Data',
      render: (row) => formatDate(row.dataOra),
    },
    {
      id: 'ora',
      label: 'Ora',
      render: (row) => formatTime(row.dataOra),
    },
    {
      id: 'medico',
      label: 'Medico',
      render: (row) => `Dr. ${row.medicoNome} ${row.medicoCognome}`,
    },
    {
      id: 'paziente',
      label: 'Paziente',
      render: (row) => `${row.pazienteNome} ${row.pazienteCognome}`,
    },
    {
      id: 'stato',
      label: 'Stato',
      render: (row) => (
        <Chip
          label={row.stato || 'In attesa'}
          size="small"
          color={getStatusColor(row.stato)}
        />
      ),
    },
    {
      id: 'actions',
      label: 'Azioni',
      align: 'right',
      render: (row) => (
        <Box>
          <IconButton
            size="small"
            color="primary"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/appuntamenti/${row.id}`);
            }}
          >
            <VisibilityIcon />
          </IconButton>
          <IconButton
            size="small"
            color="warning"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/appuntamenti/${row.id}/modifica`);
            }}
          >
            <EditIcon />
          </IconButton>
          <IconButton
            size="small"
            color="error"
            onClick={(e) => {
              e.stopPropagation();
              handleDelete(row.id);
            }}
          >
            <DeleteIcon />
          </IconButton>
        </Box>
      ),
    },
  ];

  if (loading) {
    return <Spinner />;
  }

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4" fontWeight={600}>
          I Miei Appuntamenti
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => navigate('/appuntamenti/nuovo')}
        >
          Nuovo Appuntamento
        </Button>
      </Box>

      <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={4}>
            <FormControl fullWidth size="small">
              <InputLabel>Filtra per Stato</InputLabel>
              <Select
                value={statoFilter}
                label="Filtra per Stato"
                onChange={(e) => setStatoFilter(e.target.value)}
              >
                <MenuItem value="">Tutti</MenuItem>
                <MenuItem value="In attesa">In attesa</MenuItem>
                <MenuItem value="Confermato">Confermato</MenuItem>
                <MenuItem value="Completato">Completato</MenuItem>
                <MenuItem value="Annullato">Annullato</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {filteredAppuntamenti.length === 0 ? (
        <Paper elevation={2} sx={{ p: 6, textAlign: 'center' }}>
          <EventIcon sx={{ fontSize: 64, color: 'text.disabled', mb: 2 }} />
          <Typography variant="h6" color="text.secondary" mb={2}>
            Nessun appuntamento trovato
          </Typography>
          <Button
            variant="contained"
            onClick={() => navigate('/appuntamenti/nuovo')}
          >
            Prenota il tuo primo appuntamento
          </Button>
        </Paper>
      ) : (
        <Table
          columns={columns}
          data={filteredAppuntamenti}
          onRowClick={(row) => navigate(`/appuntamenti/${row.id}`)}
          emptyMessage="Nessun appuntamento trovato"
        />
      )}
    </Box>
  );
};

export default AppuntamentiPage;
