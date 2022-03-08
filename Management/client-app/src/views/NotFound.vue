<template>
  <v-container class="fill-height justify-center" fluid>
    <v-row class="mt-12" align="center" justify="center">
      <v-col cols="11" sm="9" md="7">
        <v-sheet class="neu-glow with-radius fill-height" height="100%">
          <!-- <v-img
            src="/static/illustator/not_found.png"
            lazy-src="/static/illustator/not_found.png"
            height="250"
            contain
          ></v-img> -->
          <v-row justify="end" class="fill-height">
            <v-col cols="12">
              <h1 class="text-center display-4 font-weight-black neu-text-glow">
                404
              </h1>
            </v-col>
            <v-col class="text-center px-10" cols="12">
              <div class="body-1 mb-5">
                الصفحة التي تحاول الوصول لها غير موجودة
              </div>
              <v-btn color="secondary" to="/" class="mb-5">
                الرجوع للصفحة الرئيسية
              </v-btn>
            </v-col>
          </v-row>
        </v-sheet>
      </v-col>
    </v-row>
  </v-container>
</template>
<style scoped>
/* .container-fluid, .container-lg, .container-md, .container-sm, .container-xl {
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    margin-left: auto;
    margin-right: auto;
} */
</style>