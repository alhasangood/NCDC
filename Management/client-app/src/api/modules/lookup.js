import apiClient from "../apiClient";

const resource = "lookup";

export default {
    getResultsTypes: () => apiClient.get(`${resource}/ResultsTypes`),
    getAnalysisTypes: () => apiClient.get(`${resource}/AnalysisTypes`),
    
    getNationalities: () => apiClient.get(`${resource}/Nationalities`),
    getCities: () => apiClient.get(`${resource}/cities`),
    getMunicipalities: id => apiClient.get(`${resource}/Municipalities/${id}`),
    getCountries: () => apiClient.get(`${resource}/Countries`),
  };
