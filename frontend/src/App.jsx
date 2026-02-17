import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { AuthProvider } from './context/AuthContext';
import { NotificationProvider } from './context/NotificationContext';
import Layout from './components/layout/Layout';
import ProtectedRoute from './components/auth/ProtectedRoute';
import Notification from './components/common/Notification';

// Pages
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import PazientiPage from './pages/PazientiPage';
import PazienteDetailPage from './pages/PazienteDetailPage';
import MediciPage from './pages/MediciPage';
import AppuntamentiPage from './pages/AppuntamentiPage';
import NuovoAppuntamentoPage from './pages/NuovoAppuntamentoPage';
import RefertiPage from './pages/RefertiPage';
import ProfiloPage from './pages/ProfiloPage';

// Create MUI theme with medical/healthcare colors
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2', // Blue
      light: '#42a5f5',
      dark: '#1565c0',
    },
    secondary: {
      main: '#2e7d32', // Green
      light: '#4caf50',
      dark: '#1b5e20',
    },
    background: {
      default: '#f5f5f5',
      paper: '#ffffff',
    },
  },
  typography: {
    fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
    h4: {
      fontWeight: 600,
    },
    h5: {
      fontWeight: 600,
    },
    h6: {
      fontWeight: 600,
    },
  },
  shape: {
    borderRadius: 8,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          fontWeight: 500,
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
        },
      },
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <AuthProvider>
          <NotificationProvider>
            <Notification />
            <Layout>
              <Routes>
                {/* Public Routes */}
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />

                {/* Protected Routes */}
                <Route
                  path="/"
                  element={
                    <ProtectedRoute>
                      <DashboardPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/pazienti"
                  element={
                    <ProtectedRoute allowedRoles={['Admin', 'Medico', 'Receptionist']}>
                      <PazientiPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/pazienti/:id"
                  element={
                    <ProtectedRoute allowedRoles={['Admin', 'Medico', 'Receptionist']}>
                      <PazienteDetailPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/medici"
                  element={
                    <ProtectedRoute allowedRoles={['Paziente', 'Admin', 'Receptionist']}>
                      <MediciPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/appuntamenti"
                  element={
                    <ProtectedRoute>
                      <AppuntamentiPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/appuntamenti/nuovo"
                  element={
                    <ProtectedRoute allowedRoles={['Paziente']}>
                      <NuovoAppuntamentoPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/referti"
                  element={
                    <ProtectedRoute>
                      <RefertiPage />
                    </ProtectedRoute>
                  }
                />

                <Route
                  path="/profilo"
                  element={
                    <ProtectedRoute>
                      <ProfiloPage />
                    </ProtectedRoute>
                  }
                />

                {/* Catch all - redirect to home */}
                <Route path="*" element={<Navigate to="/" replace />} />
              </Routes>
            </Layout>
          </NotificationProvider>
        </AuthProvider>
      </Router>
    </ThemeProvider>
  );
}

export default App;
