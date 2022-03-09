import Vue from "vue";
import Vuex from "vuex";
// UI Modules
import alert from "./modules/ui/alert";
import loading from "./modules/ui/loading";
import notification from "./modules/ui/notification";
import sidebar from "./modules/ui/sidebar";

// Features Modules
// import lookup from "./modules/lookup";
// import security from "./modules/security";
// import users from "./modules/users";



Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        // UI Modules
        alert,
        loading,
        notification,
        sidebar,
        // Features Modules
     
    },
});
