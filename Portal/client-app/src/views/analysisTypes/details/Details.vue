<template src="./Details.html"></template>

<script>
export default {
  props: {
    itemId: {
      type: Number,
      required: true,
    },
  },
  created() {
      this.$store
      .dispatch("analysisTypes/getDetails", this.itemId);
  },
  data() {
    return {
    };
  },
  computed: {
    analysisType() {
      return this.$store.state.analysisTypes.analysisType;
    },
  },
};
</script>
