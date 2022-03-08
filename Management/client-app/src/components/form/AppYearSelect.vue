<template>
    <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
        <validation-provider :name="label" :rules="rules" v-slot="{ errors }">
            <label class="text-caption">
                {{ label }} <span v-if="requiredClass">*</span>
            </label>
            <v-autocomplete solo
                            dense
                            class="mt-2 neu-input"
                            :placeholder="label"
                            :error-messages="errors[0]"
                            :items="years"
                            :value="value"
                            @input="$emit('input', $event)"
                            @change="$emit('change')" />
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
    rules: {
      type: [String, Object],
      default: "",
    },
    firstYear: {
      type: Number,
      required: true,
    },
    lastYear: {
      type: Number,
      required: true,
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
  },
  computed: {
    years() {
      const yearListSize = this.lastYear - this.firstYear + 1;
      return Array(yearListSize)
        .fill()
        .map((_, idx) => this.lastYear - idx);
    },
    requiredClass() {
      return this.rules.includes("required");
    },
  },
};
</script>