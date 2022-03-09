import dataService from "../../api";
import { startLoading, stopLoading, serverErrorMessage, showAlert, showNotification, catchHandler } from "../../utils/helpers";

const state = {
    isLoggedIn: null,
    permissions: [],
    user:null,
};

const getters = {
    hasPermission: (state) => (code) => {
        return state.permissions.includes(code)
    },
}

const mutations = {
    setLoginState: (state, isLoggedIn) => (state.isLoggedIn = isLoggedIn),
    setPermissions: (state, permissions) => (state.permissions = permissions),
    setUserInfo: (state, user) => (state.user = user),
};

const actions = {
    async login({ commit }, payload) {
        try {
            startLoading()
            const response = await dataService.security.login(payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setLoginState", true);
            var user ={
                centerName: response.data.authenticatedUser.centerName,
                userName: response.data.authenticatedUser.fullName
            }
            commit("setUserInfo", user);
            commit("setPermissions", response.data.authenticatedUser.permissions);

            return Promise.resolve();

        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async loginStatus({ commit }) {
        try {
            const response = await dataService.security.loginStatus();

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }
            var user ={
                centerName: response.data.authenticatedUser.centerName,
                userName: response.data.authenticatedUser.fullName
            }
            commit("setLoginState", true);
            commit("setUserInfo", user);    
            commit("setPermissions", response.data.authenticatedUser.permissions);
        } catch (error) {
            commit("setLoginState", false);
        }
    },
	async logout({ commit }) {
        try {
            startLoading()
            const response = await dataService.security.logout();
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message)

			commit("setLoginState", false);
        } catch (error) {
            stopLoading()
            commit("setLoginState", false);
        }
    },
   
}

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};