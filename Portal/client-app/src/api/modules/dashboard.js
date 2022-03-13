import apiClient from "../apiClient";

const resource = "dashboard";

export default {
    getByAge:           () => apiClient.get(`${resource}/byAge`),
    getByRegionsCenter: () => apiClient.get(`${resource}/ByRegionsCenter`),
    getByHealthCenter:  () => apiClient.get(`${resource}/ByHealthCenter`),
    getByPrimarySite: () => apiClient.get(`${resource}/ByPrimarySite`),
};
