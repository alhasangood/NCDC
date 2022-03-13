<template src="./Add.html"></template>

<script>
export default {
  created() {},
  computed: {},
  data() {
    return {
      resultType: {
        resultTypeName: null,
        // description: null,
      },
    };
  },
  methods: {
    confirm() {
      this.$store
        .dispatch("resultTypes/add", this.resultType)
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
