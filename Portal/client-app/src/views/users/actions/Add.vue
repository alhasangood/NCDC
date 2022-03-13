<template src="./Add.html">
</template>

<script>
    export default {
        created() {
         
        },
        computed: {
         
        },
        data() {
            return {
                features: [],
                user: {
                    regionCenterId: null,
                    healthCenterId: null,
                    loginName: null,
                    fullName: null,
                    jobDescription: null,
                    userType: 1,
                    phoneNo: null,
                    email: null,
                    permissions: [],
                },
            };
        },
        methods: {
            back() {
                this.$emit("back");
            },

        },
    };
</script>
