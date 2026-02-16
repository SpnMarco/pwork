import api from './api';

export const appuntamentoService = {
  /**
   * Get all appointments (with optional filters)
   * @param {Object} filters - Optional filters {medicoId, pazienteId, data, stato}
   * @returns {Promise} List of appointments
   */
  getAll: async (filters = {}) => {
    const response = await api.get('/appuntamenti', { params: filters });
    return response.data;
  },

  /**
   * Get appointment by ID
   * @param {number} id - Appointment ID
   * @returns {Promise} Appointment data
   */
  getById: async (id) => {
    const response = await api.get(`/appuntamenti/${id}`);
    return response.data;
  },

  /**
   * Create new appointment
   * @param {Object} data - Appointment data
   * @returns {Promise} Created appointment
   */
  create: async (data) => {
    const response = await api.post('/appuntamenti', data);
    return response.data;
  },

  /**
   * Update appointment
   * @param {number} id - Appointment ID
   * @param {Object} data - Updated appointment data
   * @returns {Promise} Updated appointment
   */
  update: async (id, data) => {
    const response = await api.put(`/appuntamenti/${id}`, data);
    return response.data;
  },

  /**
   * Delete/Cancel appointment
   * @param {number} id - Appointment ID
   * @returns {Promise}
   */
  delete: async (id) => {
    const response = await api.delete(`/appuntamenti/${id}`);
    return response.data;
  },

  /**
   * Get available time slots for booking
   * @param {number} medicoId - Doctor ID
   * @param {string} data - Date (YYYY-MM-DD format)
   * @returns {Promise} Available slots
   */
  getSlotsDisponibili: async (medicoId, data) => {
    const response = await api.get('/appuntamenti/disponibilita', {
      params: { medicoId, data }
    });
    return response.data;
  },

  /**
   * Get upcoming appointments for current user
   * @returns {Promise} List of upcoming appointments
   */
  getUpcoming: async () => {
    const response = await api.get('/appuntamenti/miei');
    return response.data;
  },
};

export default appuntamentoService;
