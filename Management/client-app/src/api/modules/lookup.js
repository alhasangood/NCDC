import apiClient from "../apiClient";

const resource = "lookup";

export default {
    getCategories: () => apiClient.get(`${resource}/Categories`),
    getRegionsCenters: () => apiClient.get(`${resource}/RegionsCenters`),
    getHealthCenters: () => apiClient.get(`${resource}/HealthCenters`),
    getHealthCentersByRegion: id => apiClient.get(`${resource}/HealthCentersByRegion/${id}`),
    getHealthCentersByUserType: id => apiClient.get(`${resource}/HealthCentersByUserType?id=${id}`),
    getRegionsCentersByUserType: () => apiClient.get(`${resource}/RegionsCentersByUserType`),
    
    getNationalities: () => apiClient.get(`${resource}/Nationalities`),
    getCities: () => apiClient.get(`${resource}/cities`),
    getMunicipalities: id => apiClient.get(`${resource}/Municipalities/${id}`),
    getLocalities: () => apiClient.get(`${resource}/Localities`),
    
    getDataSourcesByHealthCenter: id => apiClient.get(`${resource}/DataSourcesByHealthCenter/${id}`),
    getDepartments: id => apiClient.get(`${resource}/Departments/${id}`),
    getBehaviours: () => apiClient.get(`${resource}/Behaviours`),
    getDataSources: () => apiClient.get(`${resource}/DataSources`),
    getDiagnosisBases: () => apiClient.get(`${resource}/DiagnosisBases`),
    getLateralities: () => apiClient.get(`${resource}/Lateralities`),
    getPrimarySites: () => apiClient.get(`${resource}/PrimarySites`),
    getSubSites: id => apiClient.get(`${resource}/SubSites/${id}`),
    getMorphologies: id => apiClient.get(`${resource}/Morphologies/${id}`),
    getGrades: () => apiClient.get(`${resource}/Grades`),
    getStages: (params) => apiClient.get(`${resource}/Stages?${params}`),
    getM: () => apiClient.get(`${resource}/M`),
    getN: () => apiClient.get(`${resource}/N`),
    getT: () => apiClient.get(`${resource}/T`),
    getCountries: () => apiClient.get(`${resource}/Countries`),
  };
