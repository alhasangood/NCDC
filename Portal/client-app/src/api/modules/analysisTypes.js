import apiClient from "../apiClient";

const resource = "analysisTypes";

export default {
    getAll: params => apiClient.get(`${resource}?${params}`),
    getDetails: id => apiClient.get(`${resource}/${id}/details`),
    getForEdit: id => apiClient.get(`${resource}/${id}`),
    add: payload => apiClient.post(`${resource}`, payload),
    edit: (id, payload) => apiClient.put(`${resource}/${id}`, payload),
    results: (id, payload) => apiClient.put(`${resource}/${id}/result`, payload),
    lock: id => apiClient.put(`${resource}/${id}/lock`),
    unlock: id => apiClient.put(`${resource}/${id}/unlock`),
    delete: id => apiClient.delete(`${resource}/${id}`),
};
