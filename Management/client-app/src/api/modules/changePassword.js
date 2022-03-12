
import apiClient from "../apiClient";

const resource = "changePassword";

export default {
  
    changePassword: (payload) => apiClient.put(`${resource}`,payload),

};
