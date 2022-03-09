<template>
    <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
        <validation-provider :rules="rules" :name="label" v-slot="{ errors }">
            <label class="form-label">
                {{ label }} <span v-if="requiredClass">*</span>
            </label>
            <v-textarea 
                        dense
                        outlined
                        class="rounded-lg"
                        background-color="white"
                        :placeholder="label"
                        :error-messages="errors[0]"
                        :type="type"
                        :value="value"
                        @input="$emit('input', $event)" />
        </validation-provider>
    </v-col>
</template>
<script>
export default {
  props: {
    value: {
      type: [String, Number],
      default: ""
    },
    label: {
      type: String,
      required: true
    },
    rules: {
      type: [String, Object],
      default: "",
    },
    type: {
      type: String,
      default: "text"
    },
    cols: {
      type: Number,
      default: 12
    },
    sm: {
      type: Number,
      default: 6
    },
    md: {
      type: Number,
      default: 4
    }
  },
  computed: {
    requiredClass() {
      return this.rules.includes("required");
    }
  }
};
</script>