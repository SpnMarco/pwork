/**
 * Validate Italian fiscal code (Codice Fiscale)
 * @param {string} cf - Fiscal code
 * @returns {boolean}
 */
export const validateCodiceFiscale = (cf) => {
  if (!cf) return false;
  
  // Basic validation: 16 alphanumeric characters
  const cfRegex = /^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$/i;
  return cfRegex.test(cf);
};

/**
 * Validate email address
 * @param {string} email - Email address
 * @returns {boolean}
 */
export const validateEmail = (email) => {
  if (!email) return false;
  
  const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
  return emailRegex.test(email);
};

/**
 * Validate phone number (Italian format)
 * @param {string} phone - Phone number
 * @returns {boolean}
 */
export const validatePhone = (phone) => {
  if (!phone) return false;
  
  // Remove spaces and special characters
  const cleaned = phone.replace(/[\s\-\(\)]/g, '');
  
  // Check if it's a valid Italian phone number
  const phoneRegex = /^(\+39)?[0-9]{9,10}$/;
  return phoneRegex.test(cleaned);
};

/**
 * Validate password strength
 * @param {string} password - Password
 * @returns {Object} - {valid: boolean, message: string}
 */
export const validatePassword = (password) => {
  if (!password) {
    return { valid: false, message: 'Password richiesta' };
  }
  
  if (password.length < 8) {
    return { valid: false, message: 'Password deve essere di almeno 8 caratteri' };
  }
  
  if (!/[A-Z]/.test(password)) {
    return { valid: false, message: 'Password deve contenere almeno una lettera maiuscola' };
  }
  
  if (!/[a-z]/.test(password)) {
    return { valid: false, message: 'Password deve contenere almeno una lettera minuscola' };
  }
  
  if (!/[0-9]/.test(password)) {
    return { valid: false, message: 'Password deve contenere almeno un numero' };
  }
  
  return { valid: true, message: 'Password valida' };
};

/**
 * Format phone number to Italian format
 * @param {string} phone - Phone number
 * @returns {string} Formatted phone number
 */
export const formatPhone = (phone) => {
  if (!phone) return '';
  
  // Remove non-numeric characters
  const cleaned = phone.replace(/\D/g, '');
  
  // Format based on length
  if (cleaned.length === 10) {
    return cleaned.replace(/(\d{3})(\d{3})(\d{4})/, '$1 $2 $3');
  }
  
  return phone;
};

/**
 * Format currency to EUR
 * @param {number} amount - Amount
 * @returns {string} Formatted currency
 */
export const formatCurrency = (amount) => {
  if (amount === null || amount === undefined) return '€ 0,00';
  
  return new Intl.NumberFormat('it-IT', {
    style: 'currency',
    currency: 'EUR',
  }).format(amount);
};

/**
 * Capitalize first letter
 * @param {string} str - String to capitalize
 * @returns {string}
 */
export const capitalize = (str) => {
  if (!str) return '';
  return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
};

/**
 * Get initials from name
 * @param {string} name - Full name
 * @returns {string} Initials
 */
export const getInitials = (name) => {
  if (!name) return '';
  
  const parts = name.trim().split(' ');
  if (parts.length === 1) {
    return parts[0].charAt(0).toUpperCase();
  }
  
  return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
};
