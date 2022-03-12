import Vue from "vue";
import VueRouter from "vue-router";
import Layout from "../layouts/Layout.vue";
import Dashboard from "../views/dashboard/Index.vue";
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
      }, 
      {
        path: "analysisTypes",
        name: "analysisTypes",
        component: () => import('../views/analysisTypes/Index.vue')
      }, 
      {
        path: "analysisResults",
        name: "analysisResults",
        component: () => import('../views/analysisResults/Index.vue')
      },
      {
        path: "medicalAnalysis",
        name: "medicalAnalysis",
        component: () => import('../views/medicalAnalysis/Index.vue')
      },
      {
        path: "resultTypes",
        name: "resultTypes",
        component: () => import('../views/resultTypes/Index.vue')
      },
      {
        path: "citizens",
        name: "citizens",
        component: () => import('../views/citizens/Index.vue')
      },
     
      {
        path: "citzensAnalysis",
        name: "citzensAnalysis",
        component: () => import('../views/citzensAnalysis/Index.vue')
      },
      {
        path: "employees",
        name: "employees",
        component: () => import('../views/employees/Index.vue')
      },
       {
        path: "laboratories",
        name: "laboratories",
        component: () => import('../views/laboratories/Index.vue')
      },
      {
        path: "users",
        name: "users",
        component: () => import('../views/users/Index.vue')
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
