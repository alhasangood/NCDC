<template>
    <v-tooltip top color="secondary">
        <template v-slot:activator="{ on, attrs }">
            <v-icon :color="color"
                    :size="size"
                    v-bind="attrs"
                    v-on="on"
                    class="mr-1"
                    @click="$emit('click')">
                {{ icon }}
            </v-icon>
        </template>
        <span>{{ tooltip }}</span>
    </v-tooltip>
</template>

<script>
export default {
  props: {
    icon: {
      type: String,
      required: true,
    },
    color: {
      type: String,
      default: null,
    },
    size:{
      type: Number,
      default: 17,
    },
    tooltip: {
      type: String,
      required: true,
    },
  },
};
</script>

<style></style>