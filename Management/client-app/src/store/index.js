import Vue from "vue";
import Vuex from "vuex";
// UI Modules
import alert from "./modules/ui/alert";
import loading from "./modules/ui/loading";
import notification from "./modules/ui/notification";
import sidebar from "./modules/ui/sidebar";

// Features Modules
import lookup from "./modules/lookup";
import security from "./modules/security";
import users from "./modules/users";
import analysisResults from "./modules/analysisResults";
import analysisTypes from "./modules/analysisTypes";
import citizens from "./modules/citizens";
import citzensAnalysis from "./modules/citzensAnalysis";
import dashboard from "./modules/dashboard";
import employees from "./modules/employees";
import laboratories from "./modules/laboratories";
import medicalAnalysis from "./modules/medicalAnalysis";
import resultTypes from "./modules/resultTypes";



Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        // UI Modules
        alert,
        loading,
        notification,
        sidebar,
        // Features Modules
        lookup,
        security,
        dashboard,
        users,
        analysisResults,
        analysisTypes,
        employees,
        citizens,
        citzensAnalysis,
         laboratories, 
         medicalAnalysis,
        resultTypes,
    },
});
