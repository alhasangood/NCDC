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
    resultTypes: [],
    totalItems: 0,
    resultType: {},
};

const getters = {
    index: state => id => state.resultTypes.findIndex(i => i.resultTypesId === id)
};

const mutations = {
    setResultTypess: (state, resultTypes) => (state.resultTypes = resultTypes),
    setTotalItems: (state, totalItems) => (state.totalItems = totalItems),
    setResultType: (state, resultType) => (state.resultType = resultType),
    add: (state, resultType) => state.resultTypes.unshift(resultType),
    edit: (state, { index, resultType }) => state.resultTypess.splice(index, 1, resultType),
    lock: (state, index) => (state.resultTypes[index].status = status.locked),
    unlock: (state, index) => (state.resultTypes[index].status = status.active),
    delete: (state, index) => state.resultTypes.splice(index, 1)
};

const actions = {
    async getAll({ commit }, params) {
        try {
            const paramsString = Object.entries(params)
                .filter(([, val]) => val !== null)
                .map(([key, val]) => `${key}=${val}`)
                .join("&");

            startLoading()
            const response = await dataService.resultTypes.getAll(paramsString);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setResultTypes", response.data.resultTypes);
            commit("setTotalItems", response.data.totalItems);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getDetails({ commit }, id) {
        try {
            commit("setResultType", {});

            startLoading()
            const response = await dataService.resultTypes.getDetails(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            commit("setResultType", response.data.resultType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async getForEdit(_, id) {
        try {
            startLoading()
            const response = await dataService.resultTypes.getForEdit(id);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            return Promise.resolve(response.data.resultType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
        }
    },
    async add({ commit }, payload) {
        try {
            startLoading()
            const response = await dataService.resultTypes.add(payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            commit("add", response.data.resultType);
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async edit({ commit, getters}, { id, payload }) {
        try {
            startLoading()
            const response = await dataService.resultTypes.edit(id, payload);
            stopLoading()

            if (!response.data.statusCode) {
                showAlert(serverErrorMessage);
                return;
            }

            showNotification(response.data.message);

            const index = getters.index(response.data.resultType.resultTypeId);
            commit("edit", { index, resultType: response.data.resultType });
        } catch (error) {
            stopLoading()
            catchHandler(error);
            return Promise.reject(error);
        }
    },
    async lock({ commit, getters }, id) {
        try {
            startLoading()
            const response = await dataService.resultTypes.lock(id);
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
            const response = await dataService.resultTypes.unlock(id);
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
            const response = await dataService.resultTypes.delete(id);
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
 
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};