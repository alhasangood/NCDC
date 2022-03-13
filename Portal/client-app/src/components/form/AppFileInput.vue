<template>
  <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
    <validation-provider :name="label" :rules="rules" v-slot="{ errors }">
      <label class="text-caption">
        {{ label }} <span v-if="requiredClass">*</span>
      </label>
      <v-file-input
        solo
        dense
        show-size
        :placeholder="placeholder"
        :error-messages="errors[0]"
        class="mt-2 neu-input"
        :value="value"
        type="file"
        @input="$emit('input', $event)"
        @change="$emit('change', $event)"
        :accept="accept"
      />
    </validation-provider>
  </v-col>
</template>
<script>
export default {
  props: {
    value: {
      type: File,
    },
    label: {
      type: String,
      required: true,
    },
    placeholder: {
      type: String,
      required: false,
    },
    rules: {
      type: [String, Object],
      default: "",
    },
    cols: {
      type: Number,
      default: 12,
    },
    sm: {
      type: Number,
      default: 6,
    },
    md: {
      type: Number,
      default: 3,
    },
    accept: {
      type: String,
      default: ".pdf",
    },
  },
  computed: {
    requiredClass() {
      return this.rules.includes("required");
    },
  },
};
</script>