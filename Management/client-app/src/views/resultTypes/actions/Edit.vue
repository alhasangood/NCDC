<template src="./Edit.html"></template>

<script>
export default {
  props:{
    itemId: Number
  },
  created() {
        this.$store
      .dispatch("resultTypes/GetForEdit", this.itemId)
      .then((res) => {
        this.resultType = res.resultType;
      })
      .catch(() => {});
    },
  computed: {
    
 },
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
        .dispatch("resultTypes/edit", {
          id: this.itemId,
          payload: this.resultType,
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
