<template>
  <div class="bg-gray-100"> 
    <AppHeader />
    <AppToolbar />
      <v-scroll-y-transition mode="out-in">
   
         <v-main>

    <v-container fluid>

      <router-view></router-view>
    </v-container>
  </v-main>

      </v-scroll-y-transition>
    <AppFooter />
  </div>
</template>

<script>
export default {
  components: {
    AppFooter: () => import("./Footer.vue"),
    AppHeader: () => import("./Header.vue"),
    AppToolbar: () => import("./Toolbar.vue"),
  },
};
</script>
<style lang="scss">
.layout {
    max-width: 80% !important;
  margin: 0 auto;
  
}
.v-application--wrap {
  height: 100%;
  font-size: 0.875rem;
  background: #f1f2f3;
  overflow-x: hidden;
  overflow-y: auto;
  letter-spacing: 0.0312rem;
  max-width: 100%;
  margin: 0 auto;
}


</style>