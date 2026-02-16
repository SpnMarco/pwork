import { useState } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import {
  Container,
  Box,
  Paper,
  Typography,
  TextField,
  Link,
  Alert,
  Grid,
} from '@mui/material';
import { useForm } from 'react-hook-form';
import { useAuth } from '../context/AuthContext';
import { useNotification } from '../context/NotificationContext';
import Button from '../components/common/Button';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import { validateEmail, validateCodiceFiscale } from '../utils/validators';

const RegisterPage = () => {
  const navigate = useNavigate();
  const { register: registerUser } = useAuth();
  const { showSuccess, showError } = useNotification();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm();

  const password = watch('password');

  const onSubmit = async (data) => {
    setLoading(true);
    setError('');

    try {
      // Prepare registration data
      const registrationData = {
        username: data.username,
        password: data.password,
        email: data.email,
        nome: data.nome,
        cognome: data.cognome,
        codiceFiscale: data.codiceFiscale,
        dataNascita: data.dataNascita,
        telefono: data.telefono,
        indirizzo: data.indirizzo,
      };

      await registerUser(registrationData);
      showSuccess('Registrazione completata con successo!');
      navigate('/', { replace: true });
    } catch (err) {
      const errorMessage = err.response?.data?.message || 'Errore durante la registrazione';
      setError(errorMessage);
      showError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="md">
      <Box
        sx={{
          marginTop: 4,
          marginBottom: 4,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Paper elevation={3} sx={{ p: 4, width: '100%' }}>
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          >
            <LocalHospitalIcon sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography component="h1" variant="h5" fontWeight={600}>
              Registrati su MedicalApp
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
              Crea un account per prenotare appuntamenti e gestire le tue visite
            </Typography>
          </Box>

          <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 3 }}>
            {error && (
              <Alert severity="error" sx={{ mb: 2 }}>
                {error}
              </Alert>
            )}

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Nome"
                  autoComplete="given-name"
                  error={!!errors.nome}
                  helperText={errors.nome?.message}
                  {...register('nome', {
                    required: 'Nome richiesto',
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Cognome"
                  autoComplete="family-name"
                  error={!!errors.cognome}
                  helperText={errors.cognome?.message}
                  {...register('cognome', {
                    required: 'Cognome richiesto',
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Codice Fiscale"
                  error={!!errors.codiceFiscale}
                  helperText={errors.codiceFiscale?.message}
                  {...register('codiceFiscale', {
                    required: 'Codice Fiscale richiesto',
                    validate: (value) =>
                      validateCodiceFiscale(value) || 'Codice Fiscale non valido',
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Data di Nascita"
                  type="date"
                  InputLabelProps={{ shrink: true }}
                  error={!!errors.dataNascita}
                  helperText={errors.dataNascita?.message}
                  {...register('dataNascita', {
                    required: 'Data di nascita richiesta',
                  })}
                />
              </Grid>

              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Email"
                  autoComplete="email"
                  error={!!errors.email}
                  helperText={errors.email?.message}
                  {...register('email', {
                    required: 'Email richiesta',
                    validate: (value) =>
                      validateEmail(value) || 'Email non valida',
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Telefono"
                  autoComplete="tel"
                  error={!!errors.telefono}
                  helperText={errors.telefono?.message}
                  {...register('telefono', {
                    required: 'Telefono richiesto',
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Indirizzo"
                  autoComplete="street-address"
                  error={!!errors.indirizzo}
                  helperText={errors.indirizzo?.message}
                  {...register('indirizzo')}
                />
              </Grid>

              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Username"
                  autoComplete="username"
                  error={!!errors.username}
                  helperText={errors.username?.message}
                  {...register('username', {
                    required: 'Username richiesto',
                    minLength: {
                      value: 4,
                      message: 'Username deve essere di almeno 4 caratteri',
                    },
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Password"
                  type="password"
                  autoComplete="new-password"
                  error={!!errors.password}
                  helperText={errors.password?.message}
                  {...register('password', {
                    required: 'Password richiesta',
                    minLength: {
                      value: 8,
                      message: 'Password deve essere di almeno 8 caratteri',
                    },
                  })}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Conferma Password"
                  type="password"
                  autoComplete="new-password"
                  error={!!errors.confirmPassword}
                  helperText={errors.confirmPassword?.message}
                  {...register('confirmPassword', {
                    required: 'Conferma password richiesta',
                    validate: (value) =>
                      value === password || 'Le password non corrispondono',
                  })}
                />
              </Grid>
            </Grid>

            <Button
              type="submit"
              fullWidth
              variant="contained"
              loading={loading}
              sx={{ mt: 3, mb: 2 }}
            >
              Registrati
            </Button>

            <Box textAlign="center">
              <Link component={RouterLink} to="/login" variant="body2">
                Hai già un account? Accedi
              </Link>
            </Box>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default RegisterPage;
