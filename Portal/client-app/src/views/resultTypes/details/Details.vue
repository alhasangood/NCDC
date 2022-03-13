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
    this.$store.dispatch("resultTypes/getDetails", this.itemId);
  },
  data() {
    return {};
  },
  computed: {
    resultType() {
      return this.$store.state.resultTypes.resultType;
    },
  },
};
</script>
