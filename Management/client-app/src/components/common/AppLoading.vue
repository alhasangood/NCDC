<template>
    <v-overlay :value="isLoading" absolute :opacity=".2">
        <v-progress-circular indeterminate size="64" color="primary"></v-progress-circular>
    </v-overlay>
</template>

<script>
export default {
  computed: {
    isLoading() {
      return this.$store.state.loading.isLoading
    }
  }
}
</script>