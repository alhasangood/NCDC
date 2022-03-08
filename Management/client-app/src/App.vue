<template>
  <v-app>
    <AppAlert />
    <AppLoading />
    <AppNotification />
    <router-view />
  </v-app>
</template>


<script>
import AppAlert from "@/components/common/AppAlert";
import AppLoading from "@/components/common/AppLoading";
import AppNotification from "@/components/common/AppNotification";

export default {
  components: {
    AppAlert,
    AppLoading,
    AppNotification,
  },
};
</script>
<style lang="scss">
</style>

