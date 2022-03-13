<template>
  <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
    <label class="form-label">{{ label }}</label>
    <v-text-field
      dense
      readonly
      outlined
      filled
      :background-color="bgColor"
      :value="value"
    />
  </v-col>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true,
    },
    value: {
      type: [String, Number],
      // required: true
    },
    cols: {
      type: Number,
      default: 6,
    },
    sm: {
      type: Number,
      default: 6,
    },
    md: {
      type: Number,
      default: 3,
    },
    bgColor: {
      type: String,
      default: "white",
    },
  },
};
</script>

<style lang="scss">
.v-text-field--outlined fieldset {
  border-color: #e2e8f0;
  --tw-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  box-shadow: var(--tw-ring-offset-shadow, 0 0 transparent),
    var(--tw-ring-shadow, 0 0 transparent), var(--tw-shadow);
}
</style>