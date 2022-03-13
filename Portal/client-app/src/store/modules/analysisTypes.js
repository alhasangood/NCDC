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
    analysisTypes: [],
    totalItems: 0,
    analysisType: {},
};

const getters = {
    index: state => id => state.analysisTypes.findIndex(i => i.userId === id)
};

const mutations = {
    setAnalysisTypes: (state, analysisTypes) => (state.analysisTypes = analysisTypes),
    setTotalItems: (state, totalItems) => (state.totalItems = totalItems),
    setAnalysisType: (state, analysisType) => (state.analysisType = analysisType),
    add: (state, user) => state.analysisTypes.unshift(user),
    edit: (state, { index, analysisType }) => state.analysisTypes.splice(index, 1, analysisType),
    lock: (state, index) => (state.analysisTypes[index].status = status.locked),
    unlock: (state, index) => (state.analysisTypes[index].status = status.active),
    delete: (state, index) => state.analysisTypes.splice(index, 1)
};

const actions = {
    async getAll({ commit }, params) {
        try {
            const paramsString = Object.entries(params)
                .filter(([, val]) => val !== null)
                .map(([key, val]) => `${key}=${val}`)
                .join("&");

            startLoading()
            const response = await dataService.analysisTypes.getAll(paramsString);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setAnalysisTypes", response.data.analysisTypes);
            commit("setTotalItems", response.data.totalItems);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getDetails({ commit }, id) {
        try {
            commit("setAnalysisType", {});

            startLoading()
            const response = await dataService.analysisTypes.getDetails(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setAnalysisType", response.data.analysisType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getForEdit(_, id) {
        try {
            startLoading()
            const response = await dataService.analysisTypes.getForEdit(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            return Promise.resolve(response.data.analysisType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async add({ commit }, payload) {
        try {
            startLoading()
            const response = await dataService.analysisTypes.add(payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            commit("add", response.data.analysisType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async edit({ commit, getters}, { id, payload }) {
        try {
            startLoading()
            const response = await dataService.analysisTypes.edit(id, payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(response.data.analysisType.analysisTypeId);
            commit("edit", { index, user: response.data.analysisType });
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    }, 
     async result(_, { id, payload }) {
        try {
            startLoading()
            const response = await dataService.analysisTypes.result(id, payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async lock({ commit, getters }, id) {
        try {
            startLoading()
            const response = await dataService.analysisTypes.lock(id);
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
            const response = await dataService.analysisTypes.unlock(id);
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
            const response = await dataService.analysisTypes.delete(id);
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
            const response = await dataService.analysisTypes.resetPassword(id);
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
            const response = await dataService.analysisTypes.getFeatures();
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