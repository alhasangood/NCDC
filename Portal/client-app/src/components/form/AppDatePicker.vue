<template>
    <v-col :cols="cols" :sm="sm" :md="md" class="py-1">
        <label class="form-label">
            {{ label }} <span v-if="requiredClass">*</span>
        </label>
        <v-menu v-model="menu"
                :close-on-content-click="false"
                color="primary"
                transition="scale-transition"
                offset-y
                min-width="auto">
            <template v-slot:activator="{ on, attrs }">
                <validation-provider :name="label" :rules="rules" v-slot="{ errors }">
                    <v-text-field dense
                                  outlined
                                  readonly
                                  class="rounded-lg"
                                  :background-color="disabled ? 'blue-grey lighten-5' : 'white'"
                                  :value="value"
                                  :disabled="disabled"
                                  :error-messages="errors[0]"
                                  :placeholder="label"
                                  v-bind="attrs"
                                  v-on="on" />
                </validation-provider>
            </template>
            <v-date-picker show-adjacent-months
                           :disabled="disabled"
                           :allowed-dates="allowedDates"
                           :type="type"
                           :value="value"
                           @input="inputHandler"
                           @change="$emit('change')" />
        </v-menu>
    </v-col>
</template>
<script>
export default {
  props: {
    value: {
      type: [String, Number],
      default: "",
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    allowedDates: {
      type: Function,
      default: null,
    },
    label: {
      type: String,
      required: true,
    },
    type: {
      type: String,
      default: "date",
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
  },
  computed: {
    requiredClass() {
      return this.rules.includes("required");
    },
  },
  data() {
    return {
      menu: false,
    };
  },
  methods: {
    inputHandler($event) {
      this.$emit("input", $event);
      this.menu = false;
    },
  },
};
</script>