/**
 * Format date to Italian locale
 * @param {string|Date} date - Date to format
 * @param {boolean} includeTime - Include time in formatting
 * @returns {string} Formatted date
 */
export const formatDate = (date, includeTime = false) => {
  if (!date) return '';
  
  const d = new Date(date);
  const options = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  };

  if (includeTime) {
    options.hour = '2-digit';
    options.minute = '2-digit';
  }

  return d.toLocaleDateString('it-IT', options);
};

/**
 * Format time from Date or time string
 * @param {string|Date} time - Time to format
 * @returns {string} Formatted time (HH:MM)
 */
export const formatTime = (time) => {
  if (!time) return '';
  
  const d = new Date(time);
  return d.toLocaleTimeString('it-IT', { hour: '2-digit', minute: '2-digit' });
};

/**
 * Get date in YYYY-MM-DD format
 * @param {Date} date - Date object
 * @returns {string} Date in YYYY-MM-DD format
 */
export const getDateString = (date = new Date()) => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
};

/**
 * Parse ISO date string to Date object
 * @param {string} dateStr - ISO date string
 * @returns {Date} Date object
 */
export const parseDate = (dateStr) => {
  if (!dateStr) return null;
  return new Date(dateStr);
};

/**
 * Check if date is today
 * @param {string|Date} date - Date to check
 * @returns {boolean}
 */
export const isToday = (date) => {
  const today = new Date();
  const d = new Date(date);
  return (
    d.getDate() === today.getDate() &&
    d.getMonth() === today.getMonth() &&
    d.getFullYear() === today.getFullYear()
  );
};

/**
 * Check if date is in the future
 * @param {string|Date} date - Date to check
 * @returns {boolean}
 */
export const isFuture = (date) => {
  return new Date(date) > new Date();
};

/**
 * Get relative date string (e.g., "oggi", "domani", "ieri")
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const getRelativeDateString = (date) => {
  const d = new Date(date);
  const today = new Date();
  
  const diffTime = d.setHours(0, 0, 0, 0) - today.setHours(0, 0, 0, 0);
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  
  if (diffDays === 0) return 'Oggi';
  if (diffDays === 1) return 'Domani';
  if (diffDays === -1) return 'Ieri';
  
  return formatDate(date);
};
