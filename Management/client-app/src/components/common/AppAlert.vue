<template>
    <v-dialog v-model="showAlert" max-width="400px" persistent>
        <v-card>
            <v-card-title> {{ label }} </v-card-title>
            <v-card-text class="py-1">
                <p>{{ message }}</p>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn small color="secondary" @click="removeAlert">موافق</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
export default {
	computed: {
		showAlert() {
			return this.message !== null && this.label !== null;
		},
		message() {
			return this.$store.state.alert.message;
		},
		label() {
			return this.$store.state.alert.label;
		}
	},
	methods: {
		removeAlert() {
			this.$store.dispatch("alert/setMessage", null);
		}
	}
};
</script>