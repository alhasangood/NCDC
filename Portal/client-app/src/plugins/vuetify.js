import Vue from 'vue';
import Vuetify from 'vuetify/lib/framework';
import ar from 'vuetify/es5/locale/ar'
import "@mdi/font/css/materialdesignicons.css";

Vue.use(Vuetify);

export default new Vuetify({
    rtl: true,
	lang: {
		locales: { ar },
		current: 'ar',
	},
    theme: {
        default: "light",
        disable: false,
        themes: {
            light: {
                primary: "#cb0c9f",
                secondary: "#344767",
                accent: "#3295a8",
                error: "#d32929",
                info: "#323b4c",
                success: "#323b4c",
                warning: "#323b4c",
                background: "#F2F3F7",
                head: "#7fbbc7"
            },
        },
    },
});