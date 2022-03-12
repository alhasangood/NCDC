<template src="./Details.html"> </template>

<script>
export default {
  props: {
    itemId: {
      type: Number,
      required: true,
    },
  },
  created() {
  },
  data() {
     return {
user:{
  loginName:null
}     };
  },
  computed: {
 
  },
};
</script>
