<template src="./Edit.html"></template>

<script>
export default {
  props:{
    itemId: Number
  },
  created() {
    },
  computed: {
 },
  data() {
    return {
    };
  },
  methods: {
    back() {
      this.$emit("back");
    },
  },
};
</script>
