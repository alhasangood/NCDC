import dataService from "../../api";
import {
    startLoading,
    stopLoading,
    serverErrorMessage,
    showNotification,
    showAlert,
    catchHandler,
} from "../../utils/helpers";
import { status } from "../../utils/common";
const state = {
    users: [],
    totalItems: 0,
    user: {},
    features: {}
};

const getters = {
    index: state => id => state.users.findIndex(i => i.userId === id)
};

const mutations = {
    setUsers: (state, users) => (state.users = users),
    setTotalItems: (state, totalItems) => (state.totalItems = totalItems),
    setUser: (state, user) => (state.user = user),
    setFeatures: (state, features) => (state.features = features),
    add: (state, user) => state.users.unshift(user),
    edit: (state, { index, user }) => state.users.splice(index, 1, user),
    lock: (state, index) => (state.users[index].status = status.locked),
    unlock: (state, index) => (state.users[index].status = status.active),
    delete: (state, index) => state.users.splice(index, 1)
};

const actions = {
    async getAll({ commit }, params) {
        try {
            const paramsString = Object.entries(params)
                .filter(([, val]) => val !== null)
                .map(([key, val]) => `${key}=${val}`)
                .join("&");

            startLoading()
            const response = await dataService.users.getAll(paramsString);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setUsers", response.data.users);
            commit("setTotalItems", response.data.totalItems);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getDetails({ commit }, id) {
        try {
            commit("setUser", {});

            startLoading()
            const response = await dataService.users.getDetails(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setUser", response.data.user);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getForEdit(_, id) {
        try {
            startLoading()
            const response = await dataService.users.getForEdit(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            return Promise.resolve(response.data.user);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async add({ commit }, payload) {
        try {
            startLoading()
            const response = await dataService.users.add(payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            commit("add", response.data.user);
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async edit({ commit, getters}, { id, payload }) {
        try {
            startLoading()
            const response = await dataService.users.edit(id, payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(response.data.user.userId);
            commit("edit", { index, user: response.data.user });
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async lock({ commit, getters }, id) {
        try {
            startLoading()
            const response = await dataService.users.lock(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(id);
            commit("lock", index);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async unlock({ commit, getters }, id) {
        try {
            startLoading()
            const response = await dataService.users.unlock(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(id);
            commit("unlock", index);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async delete({ commit, getters }, id) {
        try {
            startLoading()
            const response = await dataService.users.delete(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(id);
            commit("delete", index);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async resetPassword(_,  id) {
        try {
            startLoading()
            const response = await dataService.users.resetPassword(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }
            showAlert(response.data.message);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getFeatures({ commit }) {
        try {
            commit("setFeatures", {});

            startLoading()
            const response = await dataService.users.getFeatures();
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setFeatures", response.data.features);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};