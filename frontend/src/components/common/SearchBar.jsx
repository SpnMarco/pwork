import { TextField } from '@mui/material';

const SearchBar = ({ 
  value, 
  onChange, 
  placeholder = 'Cerca...', 
  fullWidth = true,
  ...props 
}) => {
  return (
    <TextField
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder={placeholder}
      fullWidth={fullWidth}
      variant="outlined"
      size="small"
      {...props}
    />
  );
};

export default SearchBar;
