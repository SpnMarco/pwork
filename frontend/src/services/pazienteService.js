import api from './api';

export const pazienteService = {
  /**
   * Get all patients
   * @returns {Promise} List of patients
   */
  getAll: async () => {
    const response = await api.get('/pazienti');
    return response.data;
  },

  /**
   * Get patient by ID
   * @param {number} id - Patient ID
   * @returns {Promise} Patient data
   */
  getById: async (id) => {
    const response = await api.get(`/pazienti/${id}`);
    return response.data;
  },

  /**
   * Create new patient
   * @param {Object} data - Patient data
   * @returns {Promise} Created patient
   */
  create: async (data) => {
    const response = await api.post('/pazienti', data);
    return response.data;
  },

  /**
   * Update patient
   * @param {number} id - Patient ID
   * @param {Object} data - Updated patient data
   * @returns {Promise} Updated patient
   */
  update: async (id, data) => {
    const response = await api.put(`/pazienti/${id}`, data);
    return response.data;
  },

  /**
   * Delete patient
   * @param {number} id - Patient ID
   * @returns {Promise}
   */
  delete: async (id) => {
    const response = await api.delete(`/pazienti/${id}`);
    return response.data;
  },

  /**
   * Get patient's appointments
   * @param {number} id - Patient ID
   * @returns {Promise} List of appointments
   */
  getAppuntamenti: async (id) => {
    const response = await api.get(`/pazienti/${id}/appuntamenti`);
    return response.data;
  },

  /**
   * Get patient's medical reports
   * @param {number} id - Patient ID
   * @returns {Promise} List of referti
   */
  getReferti: async (id) => {
    const response = await api.get(`/pazienti/${id}/referti`);
    return response.data;
  },
};

export default pazienteService;
