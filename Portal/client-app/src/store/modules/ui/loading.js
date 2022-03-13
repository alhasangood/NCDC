const state = {
    isLoading: false
};

const mutations = {
    start: (state) => state.isLoading = true,
    stop: (state) => state.isLoading = false
};

const actions = {
    start: ({ commit }) => commit("start"),
    stop: ({ commit }) => commit("stop")
};

export default {
    namespaced: true,
    state,
    mutations,
    actions
};
