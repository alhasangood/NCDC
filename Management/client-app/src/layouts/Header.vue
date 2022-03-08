<template>
  <div class="header">
    <v-row class="header">
      <v-col cols="12" sm="3">
        <img src="@/assets/logo.svg" height="70" />
      </v-col>

      <v-spacer />

      <v-col cols="12" sm="12" md="4" class="mt-6">
        <h3 class="secondary--text">
          {{ user.centerName }}
        </h3>
      </v-col>
      <v-col cols="12" sm="6" md="2" class="mt-5">
        <v-text-field
          v-model="searchTerm"
          dense
          flat
          rounded
          solo-inverted
          label="البحث السريع"
        >
          <v-icon @click="Search()" slot="append" color="white"
            >mdi-magnify</v-icon
          >
        </v-text-field>
      </v-col>

      <v-col cols="12" sm="1" class="mt-5 text-center">
        <!-- Profile Menu -->
        <v-menu
          bottom
          offset-y
          transition="slide-y-reverse-transition"
          rounded="lg"
          class="text-left"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn icon v-bind="attrs" v-on="on">
              <v-avatar color="blue" size="40">
                <span class="white--text text-h5">{{ initials }}</span>
              </v-avatar>
            </v-btn>
          </template>

          <v-list class="px-2">        
            <!-- <v-list-item>
              <v-list-item-content>
                <v-list-item-title> {{ user.userName }} </v-list-item-title>
              </v-list-item-content>
            </v-list-item>  -->
            <v-list-item v-for="(item, i) in items" :key="i" :to="item.link">
              <v-list-item-title>{{ item.text }}</v-list-item-title>
              
            </v-list-item>
          <v-divider ></v-divider>
            <v-list-item @click="logout">
              <v-list-item-content>
                <v-list-item-title>تسجيل خروج</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-col>
    </v-row>
  </div>
</template>


<script>
export default {
  computed: {
    user() {
      return this.$store.state.security.user;
    },
  },
  data() {
    return {
      searchTerm: "",
      authUser: {
        avatar: this.GetPhoto(),
      },
        initials: "A",
   
      items: [
        { icon: "person", text: "الملف الشخصي", link: "/profile" },
        {
          icon: "mdi-lock",
          text: "تغيير كلمة المرور",
          link: "/changePassword",
        },
      ],
    };
  },
  methods: {
    Search() {
      if (this.searchTerm != "") {
        this.searchTerm = this.searchTerm.trim();

        if (this.$route.params.searchTerm != this.searchTerm) {
          this.$router.push({
            name: "Search",
            params: { searchTerm: this.searchTerm },
          });
        }
      }
    },
    logout() {
      this.$store.dispatch("security/logout").then(() => {
        this.$router.push({ name: "Login" });
      });
    },

    GetPhoto() {
      return "/api/profile/UserPhoto";
    },
  },
};
</script>
<style scoped>
.header {
  max-width: 90% !important;

  margin: 0px auto;
}
</style>