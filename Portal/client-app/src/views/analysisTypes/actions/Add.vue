<template src="./Add.html"></template>

<script>
export default {
  created() {},
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
        .dispatch("analysisTypes/add", this.analysisType)
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
