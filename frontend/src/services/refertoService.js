import api from './api';

export const refertoService = {
  /**
   * Get all medical reports (with optional filters)
   * @param {Object} filters - Optional filters {pazienteId, medicoId, data}
   * @returns {Promise} List of referti
   */
  getAll: async (filters = {}) => {
    // Backend doesn't have referti endpoint yet - return empty array
    return Promise.resolve([]);
    // const response = await api.get('/referti', { params: filters });
    // return response.data;
  },

  /**
   * Get referto by ID
   * @param {number} id - Referto ID
   * @returns {Promise} Referto data
   */
  getById: async (id) => {
    // Backend doesn't have referti endpoint yet
    return Promise.reject(new Error('Endpoint non implementato'));
    // const response = await api.get(`/referti/${id}`);
    // return response.data;
  },

  /**
   * Create new referto
   * @param {Object} data - Referto data
   * @returns {Promise} Created referto
   */
  create: async (data) => {
    return Promise.reject(new Error('Endpoint non implementato'));
    // const response = await api.post('/referti', data);
    // return response.data;
  },

  /**
   * Update referto
   * @param {number} id - Referto ID
   * @param {Object} data - Updated referto data
   * @returns {Promise} Updated referto
   */
  update: async (id, data) => {
    return Promise.reject(new Error('Endpoint non implementato'));
    // const response = await api.put(`/referti/${id}`, data);
    // return response.data;
  },

  /**
   * Delete referto
   * @param {number} id - Referto ID
   * @returns {Promise}
   */
  delete: async (id) => {
    return Promise.reject(new Error('Endpoint non implementato'));
    // const response = await api.delete(`/referti/${id}`);
    // return response.data;
  },

  /**
   * Download referto as PDF (if available)
   * @param {number} id - Referto ID
   * @returns {Promise} PDF blob
   */
  downloadPDF: async (id) => {
    return Promise.reject(new Error('Endpoint non implementato'));
    // const response = await api.get(`/referti/${id}/pdf`, {
    //   responseType: 'blob'
    // });
    // return response.data;
  },
};

export default refertoService;
