import { Snackbar, Alert as MuiAlert } from '@mui/material';
import { useNotification } from '../../context/NotificationContext';

const Notification = () => {
  const { notification, hideNotification } = useNotification();

  if (!notification) return null;

  return (
    <Snackbar
      open={notification.open}
      autoHideDuration={5000}
      onClose={hideNotification}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
    >
      <MuiAlert
        onClose={hideNotification}
        severity={notification.severity}
        variant="filled"
        sx={{ width: '100%' }}
      >
        {notification.message}
      </MuiAlert>
    </Snackbar>
  );
};

export default Notification;
