import Vue from "vue";
import App from "@/App.vue";
import router from "@/router";
import vuetify from "./plugins/vuetify";
import store from "@/store";
// import "vuetify/dist/vuetify.min.css";

import "@/components/global";
import "@/utils/validators";
import "@/utils/filters";
import { status } from "./utils/common";

Vue.config.productionTip = false;

Vue.prototype.$status = status;

export const app = new Vue({
    router,
    store,
    vuetify,
    render: (h) => h(App),
}).$mount("#app");
