<template src="./Edit.html"></template>

<script>
export default {
  props: {
    itemId: Number,
  },
  created() {
    this.$store.dispatch("lookup/resultsTypes");
    this.$store
      .dispatch("medicalAnalysis/GetForEdit", this.itemId)
      .then((res) => {
        this.medicalAnalysis = res.medicalAnalysis;
      })
      .catch(() => {});
  },
  computed: {
    resultsTypes() {
      return this.$store.state.lookup.resultsTypes;
    },
  },
  data() {
    return {
      analysisType: {
        analysisTypeName: null,
        analysisTypeCode: null,
        description: null,
      },
    };
  },
  methods: {
    confirm() {
      this.$store
        .dispatch("medicalAnalysis/edit", {
          id: this.itemId,
          payload: this.medicalAnalysis,
        })
        .then(() => {
          this.back();
        })
        .catch(() => {});
    },
    back() {
      this.$emit("back");
    },
  },
};
</script>
