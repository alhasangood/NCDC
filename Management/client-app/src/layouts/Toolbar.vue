<template>
  <v-navigation-drawer
    app
    width="250"
    enable-resize-watcher
    flat
    right
    class="hidden-xs-and-down bg-gray-100"
  >
    <v-list-item>
      <v-list-item-content>
        <v-list-item-title class="text-h6"> Application </v-list-item-title>
      </v-list-item-content>
    </v-list-item>

    <hr class="horizontal px-2" />
    <v-list dense nav expand class="ma-2">
      <template v-for="(item, j) in navItems">
        <!-- Header with sub-features -->
        <v-list-group
          :key="item.title"
          :group="item.group"
          :prepend-icon="item.icon"
          active-class="white secondary--text"
          v-if="item.subNavs"
        >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item-content>
          </template>
          <template v-for="(subitem, i) in item.subNavs">
            <v-list-item
              :key="i"
              :to="subitem.link"
              active-class="secondary--text"
              ripple
            >
              <v-list-item-title v-text="subitem.title"></v-list-item-title>
            </v-list-item>
          </template>
        </v-list-group>

        <!-- Features -->
        <v-list-item :key="j" :to="item.link" active-class="white secondary--text" ripple>
          <v-list-item-icon class="primary--text">
            <v-icon v-text="item.icon"></v-icon>
          </v-list-item-icon>
          <v-list-item-title v-text="item.title" class="ml-5"></v-list-item-title>
        </v-list-item>
      </template>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  data() {
    return {
      appTitle: "Awesome App",
      sidebar: false,
    };
  },
  computed: {
    navItems() {
      return this.$store.state.sidebar.navItems;
    },
  },
};
</script>
<style scoped>
.toolbar {
  max-width: 80% !important;

  margin: 0px auto;
}
.topbar-nav {
  background: transparent;
  width: 100%;
  background: linear-gradient(
    270deg,
    rgba(50, 149, 168, 1) 55%,
    rgba(163, 186, 70, 1) 100%
  );
  min-height: 51px;
  margin: 0px auto;
}

.v-list-item--dense .v-list-item__title,
.v-list-item--dense .v-list-item__subtitle,
.v-list--dense .v-list-item .v-list-item__title,
.v-list--dense .v-list-item .v-list-item__subtitle {
  font-size: 15px;
  font-weight: 500;
  line-height: 2rem;
}

.v-list-item .v-list-item__title,
.v-list-item .v-list-item__subtitle {
  line-height: 1.5;
}

.v-application a {
  margin: 5px;
}

.v-list-item--active {
  color: #000000;
}
.v-list {
  border-radius: 10px;
}
.v-list-group__header .v-list-item .v-list-item--active {
  color: #000000;
}
.v-list-item__title {
  margin-left: 5px;
}
.v-application--is-rtl .v-list-item__action:first-child,
.v-application--is-rtl .v-list-item__icon:first-child {
  margin-left: 15px;
}
.btn-subMenu {
  text-align: end;
  color: white;
  background: transparent !important;
  padding: 10px 30px 10px 10px !important;
}
.v-menu__content {
  margin: none;
  box-shadow: none;
  border-radius: 10px;
  box-shadow: 1px 1px 5px 1px darkgrey;
}
.menu-icon {
  padding-right: 20px;
}
</style>
