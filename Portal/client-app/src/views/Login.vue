<template>
  <div>
    <v-sheet height="100vh" width="100%">
      <v-container class="fill-height justify-center" fluid>
        <v-card class="mx-auto my-12 shadow-card" width="400">
          <v-card flat color="transparent" class="mt-12">
            <v-row justify="center">
              <v-col cols="6">
                <v-img
                  src="@/assets/logo.svg"
                  width="200"
                  align="center"
                  class="justify-center"
                />
              </v-col>
            </v-row>
          </v-card>

          <v-card-text>
            <v-sheet max-width="380" class="mx-auto" color="transparent">
              <ValidationObserver v-slot="{ handleSubmit }">
                <v-form @submit.prevent="handleSubmit(submit)" class="my-5">
                  <AppFormField
                    :sm="12"
                    :md="12"
                    v-model="form.loginName"
                    label="اسم المستخدم"
                    class="py-0"
                    rules=""
                  />
                  <AppFormField
                    :sm="12"
                    :md="12"
                    type="password"
                    v-model="form.password"
                    label="كلمة المرور"
                    class="py-0"
                    rules=""
                  />
                                   <v-col>
                    <v-btn block type="submit" color="primary"
                      >تسجيل دخول
                    </v-btn>
                  </v-col>
                </v-form>
              </ValidationObserver>
            </v-sheet>
          </v-card-text>

          <v-divider class="mx-4"></v-divider>
        </v-card>
      </v-container>
    </v-sheet>
  </div>
</template>

<script>
export default {
  data() {
    return {
      form: {
        loginName: "",
        password: "",
        captcha: null,
      },

      showPwd: false,
    };
  },
  methods: {
    submit() {
          this.$router.push({ name: "dashboard" });
      
    },
  },
};
</script>
<style scoped>
.theme--light.v-application {
  max-width: 100vh !important;
  margin: 0 auto;
}

.shadow-card {
  box-shadow: 0 4px 6px 0 rgb(85 85 85 / 9%), 0 1px 20px 0 rgb(0 0 0 / 8%),
    0px 1px 11px 0px rgb(0 0 0 / 6%) !important;
}
</style>