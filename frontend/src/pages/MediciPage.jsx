import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Grid,
  Card as MuiCard,
  CardContent,
  CardActions,
  Avatar,
  Chip,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import { useNotification } from '../context/NotificationContext';
import medicoService from '../services/medicoService';
import Button from '../components/common/Button';
import Spinner from '../components/common/Spinner';
import SearchBar from '../components/common/SearchBar';
import { useDebounce } from '../hooks/useDebounce';
import { getInitials } from '../utils/validators';

const MediciPage = () => {
  const navigate = useNavigate();
  const { showError } = useNotification();
  const [loading, setLoading] = useState(true);
  const [medici, setMedici] = useState([]);
  const [specializzazioni, setSpecializzazioni] = useState([]);
  const [filteredMedici, setFilteredMedici] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSpec, setSelectedSpec] = useState('');
  const debouncedSearch = useDebounce(searchTerm, 300);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    filterMedici();
  }, [debouncedSearch, selectedSpec, medici]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [mediciData, specData] = await Promise.all([
        medicoService.getAll(),
        medicoService.getSpecializzazioni().catch(() => []),
      ]);
      
      setMedici(mediciData);
      setSpecializzazioni(specData);
      setFilteredMedici(mediciData);
    } catch (error) {
      console.error('Error loading medici:', error);
      showError('Errore nel caricamento dei medici');
    } finally {
      setLoading(false);
    }
  };

  const filterMedici = () => {
    let filtered = [...medici];

    // Filter by search term
    if (debouncedSearch) {
      const search = debouncedSearch.toLowerCase();
      filtered = filtered.filter((medico) =>
        medico.nome?.toLowerCase().includes(search) ||
        medico.cognome?.toLowerCase().includes(search) ||
        medico.specializzazione?.toLowerCase().includes(search)
      );
    }

    // Filter by specialization
    if (selectedSpec) {
      filtered = filtered.filter((medico) => 
        medico.specializzazioneId === parseInt(selectedSpec)
      );
    }

    setFilteredMedici(filtered);
  };

  if (loading) {
    return <Spinner />;
  }

  return (
    <Box>
      <Typography variant="h4" fontWeight={600} mb={3}>
        I Nostri Medici
      </Typography>

      <Grid container spacing={2} mb={3}>
        <Grid item xs={12} md={8}>
          <SearchBar
            value={searchTerm}
            onChange={setSearchTerm}
            placeholder="Cerca medico per nome o specializzazione..."
          />
        </Grid>
        <Grid item xs={12} md={4}>
          <FormControl fullWidth size="small">
            <InputLabel>Specializzazione</InputLabel>
            <Select
              value={selectedSpec}
              label="Specializzazione"
              onChange={(e) => setSelectedSpec(e.target.value)}
            >
              <MenuItem value="">Tutte</MenuItem>
              {specializzazioni.map((spec) => (
                <MenuItem key={spec.id} value={spec.id}>
                  {spec.nome}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
      </Grid>

      {filteredMedici.length === 0 ? (
        <Box textAlign="center" py={8}>
          <LocalHospitalIcon sx={{ fontSize: 64, color: 'text.disabled', mb: 2 }} />
          <Typography variant="h6" color="text.secondary">
            Nessun medico trovato
          </Typography>
        </Box>
      ) : (
        <Grid container spacing={3}>
          {filteredMedici.map((medico) => (
            <Grid item xs={12} sm={6} md={4} key={medico.id}>
              <MuiCard
                elevation={2}
                sx={{
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  transition: 'transform 0.2s',
                  '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: 4,
                  },
                }}
              >
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box display="flex" flexDirection="column" alignItems="center" mb={2}>
                    <Avatar
                      sx={{
                        width: 80,
                        height: 80,
                        bgcolor: 'primary.main',
                        fontSize: 32,
                        mb: 2,
                      }}
                    >
                      {getInitials(`${medico.nome} ${medico.cognome}`)}
                    </Avatar>
                    <Typography variant="h6" fontWeight={600} textAlign="center">
                      Dr. {medico.nome} {medico.cognome}
                    </Typography>
                    <Chip
                      label={medico.specializzazione || 'Medicina Generale'}
                      size="small"
                      color="primary"
                      sx={{ mt: 1 }}
                    />
                  </Box>

                  {medico.email && (
                    <Typography variant="body2" color="text.secondary" textAlign="center">
                      {medico.email}
                    </Typography>
                  )}
                  
                  {medico.telefono && (
                    <Typography variant="body2" color="text.secondary" textAlign="center">
                      {medico.telefono}
                    </Typography>
                  )}

                  {medico.descrizione && (
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      mt={2}
                      sx={{
                        display: '-webkit-box',
                        WebkitLineClamp: 3,
                        WebkitBoxOrient: 'vertical',
                        overflow: 'hidden',
                      }}
                    >
                      {medico.descrizione}
                    </Typography>
                  )}
                </CardContent>

                <CardActions sx={{ p: 2, pt: 0 }}>
                  <Button
                    fullWidth
                    variant="outlined"
                    onClick={() => navigate(`/medici/${medico.id}`)}
                  >
                    Vedi Dettagli
                  </Button>
                  <Button
                    fullWidth
                    variant="contained"
                    onClick={() => navigate(`/appuntamenti/nuovo?medicoId=${medico.id}`)}
                  >
                    Prenota
                  </Button>
                </CardActions>
              </MuiCard>
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
};

export default MediciPage;
