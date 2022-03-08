<template>
  <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
    <validation-provider :name="label" :rules="rules" v-slot="{ errors }">
      <label class="form-label">
        {{ label }} <span v-if="requiredClass">*</span>
      </label>
      <v-text-field
        dense
        outlined
        filled
        class="rounded-lg"
        :background-color="disabled ? 'blue-grey lighten-5' : 'white'"
        :counter="counter"
        :placeholder="label"
        :append-icon="icon"
        :error-messages="errors[0]"
        :type="type"
        :suffix="suffix"
        :prefix="prefix"
        :value="value"
        :disabled="disabled"
        @input="$emit('input', $event)"
        @click:append="$emit('click:append', $event)"
      />
    </validation-provider>
  </v-col>
</template>
<script>
export default {
  props: {
    value: {
      type: [String, Number],
      default: "",
    },
    label: {
      type: String,
      required: true,
    },
    counter: {
      type: Number,
      default: null,
    },
    rules: {
      type: [String, Object],
      default: "",
    },
    type: {
      type: String,
      default: "text",
    },
    disabled: {
      type: Boolean,
      default: false,
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
    maxlength: {
      type: Number,
      default: 50,
    },
    icon: {
      type: String,
      default: "",
    },
    suffix: {
      type: String,
      default: null,
    },
    prefix: {
      type: String,
      default: null,
    },
    
  },
  computed: {
    requiredClass() {
      return this.rules.includes("required");
    },
  },
};
</script>