<template src="./Result.html"></template>

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
      results: [],
    };
  },
  methods: {
    confirm() {
      this.$store
        .dispatch("analysisTypes/result", {
          id: this.itemId,
          payload: this.results,
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
