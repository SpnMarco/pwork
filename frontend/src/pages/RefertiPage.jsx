import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  IconButton,
  Chip,
  Grid,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import DownloadIcon from '@mui/icons-material/Download';
import DescriptionIcon from '@mui/icons-material/Description';
import { useNotification } from '../context/NotificationContext';
import refertoService from '../services/refertoService';
import Table from '../components/common/Table';
import Spinner from '../components/common/Spinner';
import { formatDate } from '../utils/dateUtils';

const RefertiPage = () => {
  const navigate = useNavigate();
  const { showError, showSuccess } = useNotification();
  const [loading, setLoading] = useState(true);
  const [referti, setReferti] = useState([]);

  useEffect(() => {
    loadReferti();
  }, []);

  const loadReferti = async () => {
    try {
      setLoading(true);
      const data = await refertoService.getAll();
      setReferti(data);
    } catch (error) {
      console.error('Error loading referti:', error);
      showError('Errore nel caricamento dei referti');
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = async (id) => {
    try {
      const blob = await refertoService.downloadPDF(id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `referto_${id}.pdf`;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
      showSuccess('Download completato');
    } catch (error) {
      console.error('Error downloading referto:', error);
      showError('Errore nel download del referto');
    }
  };

  const columns = [
    {
      id: 'data',
      label: 'Data',
      render: (row) => formatDate(row.data),
    },
    {
      id: 'titolo',
      label: 'Titolo',
      render: (row) => row.titolo || 'Referto Medico',
    },
    {
      id: 'medico',
      label: 'Medico',
      render: (row) => row.medicoNome || 'N/A',
    },
    {
      id: 'tipo',
      label: 'Tipo',
      render: (row) => (
        <Chip
          label={row.tipo || 'Generale'}
          size="small"
          color="info"
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
              navigate(`/referti/${row.id}`);
            }}
          >
            <VisibilityIcon />
          </IconButton>
          <IconButton
            size="small"
            color="success"
            onClick={(e) => {
              e.stopPropagation();
              handleDownload(row.id);
            }}
          >
            <DownloadIcon />
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
      <Typography variant="h4" fontWeight={600} mb={3}>
        I Miei Referti
      </Typography>

      {referti.length === 0 ? (
        <Paper elevation={2} sx={{ p: 6, textAlign: 'center' }}>
          <DescriptionIcon sx={{ fontSize: 64, color: 'text.disabled', mb: 2 }} />
          <Typography variant="h6" color="text.secondary" mb={1}>
            Nessun referto disponibile
          </Typography>
          <Typography variant="body2" color="text.secondary">
            I tuoi referti medici appariranno qui dopo gli appuntamenti completati
          </Typography>
        </Paper>
      ) : (
        <Table
          columns={columns}
          data={referti}
          onRowClick={(row) => navigate(`/referti/${row.id}`)}
          emptyMessage="Nessun referto trovato"
        />
      )}
    </Box>
  );
};

export default RefertiPage;
