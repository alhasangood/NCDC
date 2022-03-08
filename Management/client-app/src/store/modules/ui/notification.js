const state = {
    message: null
};

const mutations = {
    setMessage: (state, message) => state.message = message,
};

const actions = {
    setMessage: ({ commit }, message) => commit("setMessage", message),
};

export default {
    namespaced: true,
    state,
    mutations,
    actions
};
