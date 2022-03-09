const state = {
    message: null,
    label: 'تحذير'
};

const mutations = {
    setMessage: (state, message) => state.message = message,
    setLabel: (state, label) => state.label = label,
};

const actions = {
    setMessage: ({ commit }, message) => commit("setMessage", message),
    setLabel: ({ commit }, label) => commit("setLabel", label),
};

export default {
    namespaced: true,
    state,
    mutations,
    actions
};
