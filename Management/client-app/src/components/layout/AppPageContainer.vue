<template>
  <v-container fluid class="ma-0">
    <div class="container-title">
      <div class="page-title">{{ title }}</div>
      <span class="sub-header"> {{ subtitle }}</span>
    </div>
    <v-row>
      <v-col cols="12" class="pa-0">
        <v-fade-transition mode="out-in">
          <slot />
        </v-fade-transition>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
    },
    subtitle: {
      type: String,
    },
  },
};
</script>

<style lang="scss" scoped>
.container-title {
  margin: 20px 0px 40px 0px;
}
.page-title {
  color: #3b3f5c !important;
  letter-spacing: 0;
  display: inline;
  font-size: 2em;
  margin-block-start: 0.67em;
  margin-block-end: 0.67em;
  margin-inline-start: 0px;
  margin-inline-end: 0px;
  font-weight: bold;
  border-left: 3px solid #1c9aafeb;
  padding-left: 5px;
}
.sub-header {
  font-size: 1.2em;
  margin-right: 10px;
}
</style>