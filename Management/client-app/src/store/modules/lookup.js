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
  birthMunicipalities: [],
  localities: [],
  categories: [],
  regionsCenters: [],
  healthCenters: [],
  departments: [],
  dataSources: [],
  morphologies: [],
  Stage: [],
  T: [],
  N: [],
  M: [],
  subSites: [],
  diagnosisBases: [],
  lateralities: [],
  primarySites: [],
  behaviours: [],
  grades: [],
  stages: [],
  identificationTypes: [
    { label: "رقم وطني", value: 1 },
    { label: "رقم اداري", value: 2 },
    { label: "غير معروف ", value: 9 },
  ],
  classifications: [
    { label: "الكل", value: 0 },
    { label: " عام ", value: 1 },
    { label: "خاص", value: 2 },
  ],
  userTypes: [
    { value: 0, label: "الكل" },
    { value: 1, label: "إداري" },
    { value: 2, label: "مستخدمي سجلات مناطق" },
    { value: 3, label: "مستخدمي المرافق صحية" },
  ],
};

const getters = {
  getClassifications: (state) => state.classifications.filter(x => x.value !== 0),
  getNationalities: (state) => state.nationalities.filter(x => x.value !== 0),
  getCities: (state) => state.cities.filter(x => x.value !== 0),
  getMunicipalities: (state) => state.municipalities.filter(x => x.value !== 0),
  getLocalities: (state) => state.localities.filter(x => x.value !== 0),
  getRegionsCenters: (state) => state.regionsCenters.filter(x => x.value !== 0),
  getHealthCenters: (state) => state.healthCenters.filter(x => x.value !== 0),
  getHealthCentersByUserType: (state) => state.healthCenters.filter(x => x.value !== 0),
  getGender: (state) => state.genderList.filter(x => x.value !== 0),
};


const mutations = {
  setCities: (state, t) => (state.cities = t),
  setNationalities: (state, t) => (state.nationalities = t),
  setMunicipalities: (state, t) => (state.municipalities = t),
  setBirthMunicipalities: (state, t) => (state.birthMunicipalities = t),
  setLocalities: (state, t) => (state.localities = t),
  setCategories: (state, t) => (state.categories = t),
  setRegionsCenters: (state, t) => (state.regionsCenters = t),
  setHealthCenters: (state, t) => (state.healthCenters = t),
  setDepartments: (state, t) => (state.departments = t),
  setDataSources: (state, t) => (state.dataSources = t),
  setMorphologies: (state, t) => (state.morphologies = t),
  setStages: (state, t) => (state.stages = t),
  setGrades: (state, t) => (state.grades = t),
  setBehaviours: (state, t) => (state.behaviours = t),
  setPrimarySites: (state, t) => (state.primarySites = t),
  setSubSites: (state, t) => (state.subSites = t),
  setLateralities: (state, t) => (state.lateralities = t),
  setDiagnosisBases: (state, t) => (state.diagnosisBases = t),
  setT: (state, t) => (state.T = t),
  setM: (state, t) => (state.M = t),
  setN: (state, t) => (state.N = t),
  setCountries: (state, countries) => (state.countries = countries),
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
  async getCategories({ commit, state }) {
    try {
      if (state.categories.length === 0) {
        const response = await dataService.lookup.getCategories();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setCategories", response.data.categories);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getRegionsCenters({ commit, state }) {
    try {
      if (state.regionsCenters.length === 0) {
        const response = await dataService.lookup.getRegionsCenters();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }
        const regionsCenters = response.data.regionsCenters;
        regionsCenters.unshift({ value: 0, label: "الكل" })
        commit("setRegionsCenters", regionsCenters);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

  async getHealthCenters({ commit }) {
    try {
      const response = await dataService.lookup.getHealthCenters();

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }
      const healthCenters = response.data.healthCenters;
      healthCenters.unshift({ value: 0, label: "الكل" })
      commit("setHealthCenters", healthCenters);
    } catch (error) {
      catchHandler(error);
    }
  },
  async getRegionsCentersByUserType({ commit }) {
    try {
      const response = await dataService.lookup.getRegionsCentersByUserType();

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }
      const regionsCenters = response.data.regionsCenters;
      regionsCenters.unshift({ value: 0, label: "الكل" })
      commit("setRegionsCenters", regionsCenters);

    } catch (error) {
      catchHandler(error);
    }
  },
  async getHealthCentersByUserType({ commit }, id) {
    try {
      const response = await dataService.lookup.getHealthCentersByUserType(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }
      const healthCenters = response.data.healthCenters;
      healthCenters.unshift({ value: 0, label: "الكل" })
      commit("setHealthCenters", healthCenters);

    } catch (error) {
      catchHandler(error);
    }
  },
  async getHealthCentersByRegion({ commit }, id) {
    try {
      const response = await dataService.lookup.getHealthCentersByRegion(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }

      commit("setHealthCenters", response.data.healthCenters);
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
        nationalities.unshift({ value: 0, label: "الكل" })
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
        cities.unshift({ value: 0, label: "الكل" })
        commit("setCities", cities);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getBirthMunicipalities({ commit }, id) {
    try {
      const response = await dataService.lookup.getMunicipalities(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }
      commit("setBirthMunicipalities", response.data.municipalities);

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
      municipalities.unshift({ value: 0, label: "الكل" })
      commit("setMunicipalities", municipalities);

    } catch (error) {
      catchHandler(error);
    }
  },
  async getDataSources({ commit, state }) {
    try {
      if (state.dataSources.length === 0) {
        const response = await dataService.lookup.getDataSources();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setDataSources", response.data.dataSources);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getDataSourcesByHealthCenter({ commit, state }, id) {
    try {
      if (state.dataSources.length === 0) {
        const response = await dataService.lookup.getDataSourcesByHealthCenter(id);

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setDataSources", response.data.dataSources);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

  async getLocalities({ commit, state }) {
    try {
      if (state.localities.length === 0) {
        const response = await dataService.lookup.getLocalities();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setLocalities", response.data.localities);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

  async getBehaviours({ commit, state }) {
    try {
      if (state.behaviours.length === 0) {
        const response = await dataService.lookup.getBehaviours();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setBehaviours", response.data.behaviours);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getDepartments({ commit }, id) {
    try {
      const response = await dataService.lookup.getDepartments(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }

      commit("setDepartments", response.data.departments);
    } catch (error) {
      catchHandler(error);
    }
  },

  async getDiagnosisBases({ commit, state }) {
    try {
      if (state.diagnosisBases.length === 0) {
        const response = await dataService.lookup.getDiagnosisBases();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setDiagnosisBases", response.data.diagnosisBases);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getLateralities({ commit, state }) {
    try {
      if (state.lateralities.length === 0) {
        const response = await dataService.lookup.getLateralities();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setLateralities", response.data.lateralities);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getPrimarySites({ commit, state }) {
    try {
      if (state.primarySites.length === 0) {
        const response = await dataService.lookup.getPrimarySites();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setPrimarySites", response.data.primarySites);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

  async getSubSites({ commit }, id) {
    try {
      const response = await dataService.lookup.getSubSites(id);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }

      commit("setSubSites", response.data.subSites);

    } catch (error) {
      catchHandler(error);
    }
  },

  async getMorphologies({ commit },id) {
    try {
        const response = await dataService.lookup.getMorphologies(id);

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setMorphologies", response.data.morphologies);
      
    } catch (error) {
      catchHandler(error);
    }
  },

  async getGrades({ commit, state }) {
    try {
      if (state.grades.length === 0) {
        const response = await dataService.lookup.getGrades();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setGrades", response.data.grades);
      }
    } catch (error) {
      catchHandler(error);
    }
  },

  async getStages({ commit },params) {
    try {
      const paramsString = Object.entries(params)
        .filter(([, val]) => val !== null)
        .map(([key, val]) => `${key}=${val}`)

        .join("&"); const response = await dataService.lookup.getStages(paramsString);

      if (!response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
      }

        commit("setStages", response.data.stages);
      
    } catch (error) {
      catchHandler(error);
    }
  },
  async getT({ commit, state }) {
    try {
      if (state.T.length === 0) {
        const response = await dataService.lookup.getT();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }
        commit("setT", response.data.t);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getN({ commit, state }) {
    try {
      if (state.N.length === 0) {
        const response = await dataService.lookup.getN();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setN", response.data.n);
      }
    } catch (error) {
      catchHandler(error);
    }
  },
  async getM({ commit, state }) {
    try {
      if (state.M.length === 0) {
        const response = await dataService.lookup.getM();

        if (!response.data.statusCode) {
          showAlert(serverErrorMessage);
          return;
        }

        commit("setM", response.data.m);
      }
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
