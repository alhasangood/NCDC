import Vue from "vue";
import VueRouter from "vue-router";
import Layout from "../layouts/Layout.vue";
import Dashboard from "../views/dashboard/Index.vue";
import users from "../views/users/Index.vue";
import Login from "../views/Login.vue";
Vue.use(VueRouter);

const routes = [
  {
    path: "/login",
    name: "Login",
    component: Login,
  },
  {
    path: "/",
    name: "Layout",
    component: Layout,
    redirect: { name: "dashboard" },
    children: [
      {
        path: "dashboard",
        name: "dashboard",
        component: Dashboard,
      }, {
        path: "users",
        name: "users",
        component: users,
      },
    ],
  },
  {
    path: "*",
    name: "NotFound",
    component: Layout,
    children: [
      {
        path: "*",
        component: () => import("@/views/NotFound.vue")
      }
    ]
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
