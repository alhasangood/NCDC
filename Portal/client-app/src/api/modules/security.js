import apiClient from "../apiClient";

const resource = "security";

export default {
  login: payload => apiClient.post(`${resource}/login`, payload),
  loginStatus: () => apiClient.get(`${resource}/loginStatus`),
  logout: () => apiClient.post(`${resource}/logout`),
};
