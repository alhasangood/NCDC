import apiClient from "../apiClient";

const resource = "resultTypes";

export default {
    getAll: params => apiClient.get(`${resource}?${params}`),
    getDetails: id => apiClient.get(`${resource}/${id}/details`),
    getForEdit: id => apiClient.get(`${resource}/${id}`),
    add: payload => apiClient.post(`${resource}`, payload),
    edit: (id, payload) => apiClient.put(`${resource}/${id}`, payload),
    lock: id => apiClient.put(`${resource}/${id}/lock`),
    unlock: id => apiClient.put(`${resource}/${id}/unlock`),
    delete: id => apiClient.delete(`${resource}/${id}`),
};
