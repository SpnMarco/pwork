import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  Box,
  Typography,
  Paper,
  Stepper,
  Step,
  StepLabel,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  Card as MuiCard,
  CardContent,
  CardActionArea,
  Chip,
  Alert,
} from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useAuth } from '../context/AuthContext';
import { useNotification } from '../context/NotificationContext';
import medicoService from '../services/medicoService';
import appuntamentoService from '../services/appuntamentoService';
import Button from '../components/common/Button';
import Spinner from '../components/common/Spinner';
import { getDateString } from '../utils/dateUtils';

const steps = ['Scegli Specializzazione', 'Scegli Medico', 'Scegli Data e Ora', 'Conferma'];

const NuovoAppuntamentoPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const { user } = useAuth();
  const { showSuccess, showError } = useNotification();
  
  const [activeStep, setActiveStep] = useState(0);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  // Data
  const [specializzazioni, setSpecializzazioni] = useState([]);
  const [medici, setMedici] = useState([]);
  const [slotsDisponibili, setSlotsDisponibili] = useState([]);

  // Selected values
  const [selectedSpec, setSelectedSpec] = useState('');
  const [selectedMedico, setSelectedMedico] = useState(null);
  const [selectedData, setSelectedData] = useState('');
  const [selectedSlot, setSelectedSlot] = useState('');
  const [note, setNote] = useState('');

  useEffect(() => {
    loadSpecializzazioni();
    
    // Check if medicoId is passed in URL
    const medicoId = searchParams.get('medicoId');
    if (medicoId) {
      loadMedicoDirectly(medicoId);
    }
  }, []);

  const loadSpecializzazioni = async () => {
    try {
      const data = await medicoService.getSpecializzazioni();
      setSpecializzazioni(data);
    } catch (error) {
      console.error('Error loading specializzazioni:', error);
      showError('Errore nel caricamento delle specializzazioni');
    }
  };

  const loadMedicoDirectly = async (medicoId) => {
    try {
      const medico = await medicoService.getById(medicoId);
      setSelectedMedico(medico);
      setActiveStep(2); // Skip to date selection
    } catch (error) {
      console.error('Error loading medico:', error);
    }
  };

  const handleSpecChange = async (specId) => {
    setSelectedSpec(specId);
    setLoading(true);
    
    try {
      const data = await medicoService.getBySpecializzazione(specId);
      setMedici(data);
    } catch (error) {
      console.error('Error loading medici:', error);
      showError('Errore nel caricamento dei medici');
    } finally {
      setLoading(false);
    }
  };

  const handleMedicoSelect = (medico) => {
    setSelectedMedico(medico);
  };

  const handleDataChange = async (data) => {
    setSelectedData(data);
    setSelectedSlot('');
    setLoading(true);

    try {
      const slots = await appuntamentoService.getSlotsDisponibili(selectedMedico.id, data);
      setSlotsDisponibili(slots);
    } catch (error) {
      console.error('Error loading slots:', error);
      showError('Errore nel caricamento degli slot disponibili');
      setSlotsDisponibili([]);
    } finally {
      setLoading(false);
    }
  };

  const handleNext = () => {
    setActiveStep((prev) => prev + 1);
  };

  const handleBack = () => {
    setActiveStep((prev) => prev - 1);
  };

  const handleSubmit = async () => {
    setSubmitting(true);

    try {
      // Get pazienteId from user context
      const pazienteId = user?.pazienteId;
      
      if (!pazienteId) {
        showError('Devi essere un paziente per prenotare un appuntamento');
        return;
      }

      // Combina data e ora in un DateTime
      const dataOraCompleta = new Date(`${selectedData}T${selectedSlot}:00`);
      
      const appuntamentoData = {
        pazienteId: pazienteId,
        medicoId: selectedMedico.id,
        dataOra: dataOraCompleta.toISOString(),
        durataMinuti: 30, // Default 30 minuti
        note: note || null,
        motivoVisita: null,
      };

      await appuntamentoService.create(appuntamentoData);
      showSuccess('Appuntamento prenotato con successo!');
      navigate('/appuntamenti');
    } catch (error) {
      console.error('Error creating appuntamento:', error);
      showError(error.response?.data?.message || 'Errore nella prenotazione dell\'appuntamento');
    } finally {
      setSubmitting(false);
    }
  };

  const canProceed = () => {
    switch (activeStep) {
      case 0:
        return selectedSpec !== '';
      case 1:
        return selectedMedico !== null;
      case 2:
        return selectedData !== '' && selectedSlot !== '';
      default:
        return true;
    }
  };

  const renderStepContent = () => {
    switch (activeStep) {
      case 0:
        return (
          <Box>
            <Typography variant="h6" mb={3}>
              Seleziona una specializzazione medica
            </Typography>
            <FormControl fullWidth>
              <InputLabel>Specializzazione</InputLabel>
              <Select
                value={selectedSpec}
                label="Specializzazione"
                onChange={(e) => handleSpecChange(e.target.value)}
              >
                {specializzazioni.map((spec) => (
                  <MenuItem key={spec.id} value={spec.id}>
                    {spec.nome}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Box>
        );

      case 1:
        return (
          <Box>
            <Typography variant="h6" mb={3}>
              Seleziona un medico
            </Typography>
            {loading ? (
              <Spinner />
            ) : medici.length === 0 ? (
              <Alert severity="info">
                Nessun medico disponibile per questa specializzazione
              </Alert>
            ) : (
              <Grid container spacing={2}>
                {medici.map((medico) => (
                  <Grid item xs={12} sm={6} key={medico.id}>
                    <MuiCard
                      variant={selectedMedico?.id === medico.id ? 'elevation' : 'outlined'}
                      elevation={selectedMedico?.id === medico.id ? 4 : 0}
                      sx={{
                        border: selectedMedico?.id === medico.id ? 2 : 1,
                        borderColor: selectedMedico?.id === medico.id ? 'primary.main' : 'divider',
                      }}
                    >
                      <CardActionArea onClick={() => handleMedicoSelect(medico)}>
                        <CardContent>
                          <Typography variant="h6">
                            Dr. {medico.nome} {medico.cognome}
                          </Typography>
                          <Chip
                            label={medico.specializzazione}
                            size="small"
                            color="primary"
                            sx={{ mt: 1 }}
                          />
                        </CardContent>
                      </CardActionArea>
                    </MuiCard>
                  </Grid>
                ))}
              </Grid>
            )}
          </Box>
        );

      case 2:
        return (
          <Box>
            <Typography variant="h6" mb={3}>
              Seleziona data e ora
            </Typography>
            <TextField
              fullWidth
              type="date"
              label="Data"
              value={selectedData}
              onChange={(e) => handleDataChange(e.target.value)}
              InputLabelProps={{ shrink: true }}
              inputProps={{ min: getDateString() }}
              sx={{ mb: 3 }}
            />

            {selectedData && (
              <>
                <Typography variant="subtitle1" mb={2}>
                  Slot disponibili:
                </Typography>
                {loading ? (
                  <Spinner />
                ) : slotsDisponibili.length === 0 ? (
                  <Alert severity="warning">
                    Nessuno slot disponibile per questa data
                  </Alert>
                ) : (
                  <Grid container spacing={1}>
                    {slotsDisponibili.map((slot, index) => (
                      <Grid item xs={6} sm={4} md={3} key={index}>
                        <Button
                          fullWidth
                          variant={selectedSlot === slot ? 'contained' : 'outlined'}
                          onClick={() => setSelectedSlot(slot)}
                        >
                          {slot}
                        </Button>
                      </Grid>
                    ))}
                  </Grid>
                )}
              </>
            )}
          </Box>
        );

      case 3:
        return (
          <Box>
            <Typography variant="h6" mb={3}>
              Riepilogo prenotazione
            </Typography>
            <Paper variant="outlined" sx={{ p: 3, mb: 3 }}>
              <Grid container spacing={2}>
                <Grid item xs={12}>
                  <Typography variant="body2" color="text.secondary">
                    Medico
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    Dr. {selectedMedico.nome} {selectedMedico.cognome}
                  </Typography>
                  <Chip
                    label={selectedMedico.specializzazione}
                    size="small"
                    color="primary"
                    sx={{ mt: 0.5 }}
                  />
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Data
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {new Date(selectedData).toLocaleDateString('it-IT')}
                  </Typography>
                </Grid>

                <Grid item xs={12} sm={6}>
                  <Typography variant="body2" color="text.secondary">
                    Ora
                  </Typography>
                  <Typography variant="body1" fontWeight={500}>
                    {selectedSlot}
                  </Typography>
                </Grid>
              </Grid>
            </Paper>

            <TextField
              fullWidth
              multiline
              rows={3}
              label="Note (opzionale)"
              value={note}
              onChange={(e) => setNote(e.target.value)}
              placeholder="Inserisci eventuali note per il medico..."
            />
          </Box>
        );

      default:
        return null;
    }
  };

  return (
    <Box>
      <Box display="flex" alignItems="center" mb={3}>
        <Button
          variant="text"
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate('/appuntamenti')}
          sx={{ mr: 2 }}
        >
          Indietro
        </Button>
        <Typography variant="h4" fontWeight={600}>
          Prenota un Appuntamento
        </Typography>
      </Box>

      <Paper elevation={2} sx={{ p: 4 }}>
        <Stepper activeStep={activeStep} sx={{ mb: 4 }}>
          {steps.map((label) => (
            <Step key={label}>
              <StepLabel>{label}</StepLabel>
            </Step>
          ))}
        </Stepper>

        {renderStepContent()}

        <Box display="flex" justifyContent="space-between" mt={4}>
          <Button
            disabled={activeStep === 0}
            onClick={handleBack}
            variant="outlined"
          >
            Indietro
          </Button>

          {activeStep === steps.length - 1 ? (
            <Button
              variant="contained"
              onClick={handleSubmit}
              loading={submitting}
              disabled={!canProceed()}
            >
              Conferma Prenotazione
            </Button>
          ) : (
            <Button
              variant="contained"
              onClick={handleNext}
              disabled={!canProceed()}
            >
              Avanti
            </Button>
          )}
        </Box>
      </Paper>
    </Box>
  );
};

export default NuovoAppuntamentoPage;
