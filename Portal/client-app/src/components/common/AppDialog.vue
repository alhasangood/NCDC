<template>
  <v-dialog v-model="isShown" max-width="400px" persistent>
    <v-card class="neu-glow">
      <v-card-title>تحذير</v-card-title>
      <v-card-text class="py-1">
        <p>{{ message }}</p>
      </v-card-text>
      <v-card-actions class="justify-end">
        <v-btn small text @click="cancel">إلغاء</v-btn>
        <v-btn small color="error" @click="confirm">تأكيد</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    isShown: {
      type: Boolean,
      default: false,
    },
    message: {
      type: String,
      required: true,
    },
  },
  methods: {
    cancel() {
      this.$emit("cancel");
    },
    confirm() {
      this.$emit("confirm");
    },
  },
};
</script>
