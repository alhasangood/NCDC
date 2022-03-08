<template>
    <v-snackbar v-model="showNotification"
                :timeout="5000"
                bottom
                right
                color="success"
                elevation="24">
        <v-icon> mdi-checkbox-marked-circle </v-icon>
        <span>
            {{ message }}
        </span>
    </v-snackbar>
</template>

<script>
export default {
  computed: {
    showNotification: {
      get: function() {
        return this.message !== null;
      },
      set: function() {
        this.removeNotification();
        return false;
      },
    },
    message() {
      return this.$store.state.notification.message;
    },
  },
  methods: {
    removeNotification() {
      this.$store.dispatch("notification/setMessage", null);
    },
  },
};
</script>