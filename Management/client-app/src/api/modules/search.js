import apiClient from "../apiClient";

const resource = "search";

export default {
    getPatient: (registryNo) => apiClient.get(`${resource}/${registryNo}`),
    getDetails: (id) => apiClient.get(`${resource}/${id}/details`),  };
