<template src="./Result.html"></template>

<script>
import { showAlert } from "@/utils/helpers";

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
      resultIds: [],
    };
  },
  methods: {
    addProductAndPackage() {
      let findItem = this.results.findIndex((x) => {
        if (this.resultsTypes.resultsTypeId == x.resultsTypeId) {
          return x;
        }
      });
      if (findItem != -1) {
        showAlert(`لا يمكن اضافة نفس النتيجة اكثر من مرة`, "warning");
        return false;
      }

      let resultsType = this.resultsTypes.find((x) => {
        if (this.resultsTypeId == x.value) {
          return x;
        }
      });

      this.results.unshift({
        value: this.resultsTypeId,
        label: resultsType.label,
      });

   //   this.resultIds.push(this.resultsTypeId);

      this.resultsTypeId = null;
    },
    deleteItem(item) {
      let index = this.results.findIndex((x) => {
        if (item.resultsTypeId == x.resultsTypeId) {
          return x;
        }
      });
    //  this.resultIds.splice(index, 1);
      this.results.splice(index, 1);
    },

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
