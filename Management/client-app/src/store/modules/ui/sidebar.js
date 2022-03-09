export const state = {
  navItems: [
    {
      icon: "mdi-home",
      title: "الرئيسية",
      link: "/dashboard",
      code: "01000",

    },
     
     {
      icon: "mdi-account",
      title: "المستخدمين",
      link: "/users",
      code: "05000",

    },
      
    
  
  ],
};


export default {
  namespaced: true,
  state,
};
