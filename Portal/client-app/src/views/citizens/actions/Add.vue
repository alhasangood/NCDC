<template src="./Add.html">
</template>

<script>
    export default {
        created() {
         
        },
        computed: {
            identificationTypes() {
      return this.$store.state.lookup.identificationTypes;
    },
    cities() {
      return this.$store.getters["lookup/getCities"];
    },
    municipalities() {
      return this.$store.getters["lookup/getMunicipalities"];
    },
    birthMunicipalities() {
      return this.$store.state.lookup.birthMunicipalities;
    }, 
    countries() {
      return this.$store.state.lookup.countries;
    },
    localities() {
      return this.$store.getters["lookup/getLocalities"];
    },
    nationalities() {
      return this.$store.getters["lookup/getNationalities"];
    },
    genders() {
      return this.$store.getters["lookup/getGender"];
    },
    nationalNumberRules() {
      return this.patient.personalInformation.nationalityId === 1
        ? "required|nationalNumber|length:12"
        : "";
    },
        },
        data() {
            return {
                features: [],
                user: {
                    regionCenterId: null,
                    healthCenterId: null,
                    loginName: null,
                    fullName: null,
                    jobDescription: null,
                    userType: 1,
                    phoneNo: null,
                    email: null,
                    permissions: [],
                },
            };
        },
        methods: {
            back() {
                this.$emit("back");
            },
 allowedDates(val) {
      const today = new Date();
      const date = new Date(val);
      return date < today;
    },
    confirm() {
      let   personalInformation= {
          patientId: this.patient.personalInformation.patientId,
          firstName:  this.patient.personalInformation.firstName,
          fatherName:  this.patient.personalInformation.fatherName,
          grandfatherName:  this.patient.personalInformation.grandfatherName,
          surName:  this.patient.personalInformation.surName,
          motherName:  this.patient.personalInformation.motherName,
          birthDate:  this.patient.personalInformation.birthDate,
          nationalityId:  this.patient.personalInformation.nationalityId,
          identificationType:  this.patient.personalInformation.identificationType,
          gender:  this.patient.personalInformation.gender,
          nationalNo:  this.patient.personalInformation.nationalNo,
          passport:  this.patient.personalInformation.passport,
          mobileNo:  this.patient.personalInformation.mobileNo,

          municipalId:  this.patient.personalInformation.municipalId,
          countryId:  this.patient.personalInformation.countryId,
          birthMunicipality:  this.patient.personalInformation.birthMunicipality,
          birthPlace:  this.patient.personalInformation.birthPlace,
          localityId:  this.patient.personalInformation.localityId,
        };

      if (this.patient.personalInformation.patientId == null) {
        this.$store
          .dispatch("patients/add", personalInformation)
          .then((res) => {
            this.patient.personalInformation.patientId = res.patientId;
            this.patient.personalInformation.birthDate = res.birthDate;
            this.$emit("nextStep");
          })
          .catch(() => {});
      } else {
        this.$emit("nextStep");
      }
    },
        },
    };
</script>
