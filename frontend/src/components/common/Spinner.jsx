import { Box, CircularProgress } from '@mui/material';

const Spinner = ({ size = 40, fullPage = false }) => {
  if (fullPage) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="100vh"
      >
        <CircularProgress size={size} />
      </Box>
    );
  }

  return (
    <Box display="flex" justifyContent="center" alignItems="center" p={3}>
      <CircularProgress size={size} />
    </Box>
  );
};

export default Spinner;
