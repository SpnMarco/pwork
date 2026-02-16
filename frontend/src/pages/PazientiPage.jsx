import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  IconButton,
  Chip,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import AddIcon from '@mui/icons-material/Add';
import { useNotification } from '../context/NotificationContext';
import pazienteService from '../services/pazienteService';
import Table from '../components/common/Table';
import SearchBar from '../components/common/SearchBar';
import Button from '../components/common/Button';
import Spinner from '../components/common/Spinner';
import { useDebounce } from '../hooks/useDebounce';
import { formatDate } from '../utils/dateUtils';

const PazientiPage = () => {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotification();
  const [loading, setLoading] = useState(true);
  const [pazienti, setPazienti] = useState([]);
  const [filteredPazienti, setFilteredPazienti] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearch = useDebounce(searchTerm, 300);

  useEffect(() => {
    loadPazienti();
  }, []);

  useEffect(() => {
    filterPazienti();
  }, [debouncedSearch, pazienti]);

  const loadPazienti = async () => {
    try {
      setLoading(true);
      const data = await pazienteService.getAll();
      setPazienti(data);
      setFilteredPazienti(data);
    } catch (error) {
      console.error('Error loading pazienti:', error);
      showError('Errore nel caricamento dei pazienti');
    } finally {
      setLoading(false);
    }
  };

  const filterPazienti = () => {
    if (!debouncedSearch) {
      setFilteredPazienti(pazienti);
      return;
    }

    const filtered = pazienti.filter((paziente) => {
      const search = debouncedSearch.toLowerCase();
      return (
        paziente.nome?.toLowerCase().includes(search) ||
        paziente.cognome?.toLowerCase().includes(search) ||
        paziente.codiceFiscale?.toLowerCase().includes(search) ||
        paziente.email?.toLowerCase().includes(search)
      );
    });

    setFilteredPazienti(filtered);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Sei sicuro di voler eliminare questo paziente?')) {
      return;
    }

    try {
      await pazienteService.delete(id);
      showSuccess('Paziente eliminato con successo');
      loadPazienti();
    } catch (error) {
      console.error('Error deleting paziente:', error);
      showError('Errore nell\'eliminazione del paziente');
    }
  };

  const columns = [
    {
      id: 'nome',
      label: 'Nome',
      render: (row) => `${row.nome} ${row.cognome}`,
    },
    {
      id: 'codiceFiscale',
      label: 'Codice Fiscale',
    },
    {
      id: 'dataNascita',
      label: 'Data di Nascita',
      render: (row) => formatDate(row.dataNascita),
    },
    {
      id: 'telefono',
      label: 'Telefono',
    },
    {
      id: 'email',
      label: 'Email',
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
              navigate(`/pazienti/${row.id}`);
            }}
          >
            <VisibilityIcon />
          </IconButton>
          <IconButton
            size="small"
            color="warning"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/pazienti/${row.id}/modifica`);
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
          Gestione Pazienti
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => navigate('/pazienti/nuovo')}
        >
          Nuovo Paziente
        </Button>
      </Box>

      <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
        <SearchBar
          value={searchTerm}
          onChange={setSearchTerm}
          placeholder="Cerca per nome, cognome, codice fiscale o email..."
        />
      </Paper>

      <Table
        columns={columns}
        data={filteredPazienti}
        onRowClick={(row) => navigate(`/pazienti/${row.id}`)}
        emptyMessage="Nessun paziente trovato"
      />
    </Box>
  );
};

export default PazientiPage;
