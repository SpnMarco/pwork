import api from './api';

export const medicoService = {
  /**
   * Get all doctors
   * @returns {Promise} List of doctors
   */
  getAll: async () => {
    const response = await api.get('/medici');
    return response.data;
  },

  /**
   * Get doctor by ID
   * @param {number} id - Doctor ID
   * @returns {Promise} Doctor data
   */
  getById: async (id) => {
    const response = await api.get(`/medici/${id}`);
    return response.data;
  },

  /**
   * Get doctors by specialization
   * @param {number} specializzazioneId - Specialization ID
   * @returns {Promise} List of doctors
   */
  getBySpecializzazione: async (specializzazioneId) => {
    const response = await api.get(`/medici/specializzazione/${specializzazioneId}`);
    return response.data;
  },

  /**
   * Get all specializations
   * @returns {Promise} List of specializations
   */
  getSpecializzazioni: async () => {
    const response = await api.get('/specializzazioni');
    return response.data;
  },

  /**
   * Get doctor's availability
   * @param {number} id - Doctor ID
   * @param {string} data - Date (YYYY-MM-DD format)
   * @returns {Promise} Available slots
   */
  getDisponibilita: async (id, data) => {
    const response = await api.get(`/medici/${id}/disponibilita`, {
      params: { data }
    });
    return response.data;
  },
};

export default medicoService;
