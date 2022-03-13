import dataService from "../../api";
import {
  serverErrorMessage,
  showAlert,
  catchHandler,
} from "../../utils/helpers";

const state = {
  countries:[],
  statuses: [
    {
      label: "الكل",
      value: 0,
    },
    {
      label: "مفعل",
      value: 1,
    },
    {
      label: "مجمد",
      value: 2,
    },
  ],
  genderList: [
    {
      label: "الكل",
      value: 0,
    },
    {
      label: "ذكر",
      value: 1,
    },
    {
      label: "انثي",
      value: 2,
    },
    {
      label: "غير معروف",
      value: 9,
    },

  ],
  nationalities: [],
  cities: [],
  municipalities: [],
  resultsTypes: [],
  analysisTypes: [],

  identificationTypes: [
    { label: "رقم وطني", value: 1 },
    { label: "رقم اداري", value: 2 },
    { label: "غير معروف ", value: 9 },
  ],
 
  userTypes: [
    { value: 0, label: "الكل" },
    { value: 1, label: "إداري" },
    { value: 2, label: "مستخدمي سجلات مناطق" },
    { value: 3, label: "مستخدمي المرافق صحية" },
  ],
};

const getters = {

};


const mutations = {
  setCities: (state, t) => (state.cities = t),
  setNationalities: (state, t) => (state.nationalities = t),
  setMunicipalities: (state, t) => (state.municipalities = t),
  setCountries: (state, countries) => (state.countries = countries),
  setResultsTypes: (state, resultsTypes) => (state.resultsTypes = resultsTypes),
  setanalysisTypes: (state, analysisTypes) => (state.analysisTypes = analysisTypes),
};

const actions = {
  async getCountries({ commit, state }) {
    try {
      if (state.countries.length === 0) {
        const response = await dataService.lookup.getCountries();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setCountries", response.data.countries);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getResultsTypes({ commit, state }) {
    try {
      if (state.resultsTypes.length === 0) {
        const response = await dataService.lookup.getResultsTypes();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setResultsTypes", response.data.resultsTypes);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getnalysisTypes({ commit, state }) {
    try {
      if (state.analysisTypes.length === 0) {
        const response = await dataService.lookup.getAnalysisTypes();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setAnalysisTypes", response.data.analysisTypes);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getNationalities({ commit, state }) {
    try {
      if (state.nationalities.length === 0) {
        const response = await dataService.lookup.getNationalities();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }
        const nationalities = response.data.nationalities;

        commit("setNationalities", nationalities);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getCities({ commit, state }) {
    try {
      if (state.cities.length === 0) {
        const response = await dataService.lookup.getCities();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }
        const cities = response.data.cities;
        commit("setCities", cities);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

   async getMunicipalities({ commit }, id) {
    try {
      const response = await dataService.lookup.getMunicipalities(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }
      const municipalities = response.data.municipalities;
      commit("setMunicipalities", municipalities);

    } catch (error) {
      catchHandler(error);
    }
  },
 
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};
