import { Button as MuiButton, CircularProgress } from '@mui/material';

const Button = ({ 
  children, 
  loading = false, 
  disabled = false, 
  variant = 'contained', 
  color = 'primary',
  fullWidth = false,
  ...props 
}) => {
  return (
    <MuiButton
      variant={variant}
      color={color}
      disabled={disabled || loading}
      fullWidth={fullWidth}
      {...props}
    >
      {loading ? <CircularProgress size={24} color="inherit" /> : children}
    </MuiButton>
  );
};

export default Button;
