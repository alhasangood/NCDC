 <template>
    <v-col :cols="cols" :sm="sm" :md="md" class="py-1" >
        <validation-provider :name="label" :rules="rules" v-slot="{ errors }">
            <label class="form-label">
                {{ label }} <span v-if="requiredClass">*</span>
            </label>
            <v-autocomplete dense
                            outlined
                            filled
                            :background-color="disabled ? 'blue-grey lighten-5' : 'white'"
                            :placeholder="label"
                            :error-messages="errors[0]"
                            :items="items"
                            :item-text="itemText"
                            :item-value="itemValue"
                            :disabled="disabled"
                            :value="value"
                            :multiple="multiple"
                            @input="$emit('input', $event)"
                            @change="$emit('change')" 
                            @update:search-input="$emit('update:search-input', $event)" />
        </validation-provider>
    </v-col>
</template>
<script>
export default {
  props: {
    value: {
      type: [String, Number, Array],
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
    items: {
      type: Array,
      required: true,
    },
    itemText: {
      type: [String, Number],
      default: "label",
    },
    itemValue: {
      type: [String, Number],
      default: "value",
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    multiple: {
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
  },
  computed: {
    requiredClass() {
      return this.rules.includes("required");
    },
  },
};
</script>