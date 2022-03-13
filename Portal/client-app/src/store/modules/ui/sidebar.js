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
      title: "المواطنين",
      link: "/citizens",
      code: "05000",

    },
        {
      icon: "mdi-account",
      title: "تحليل المواطنين",
      link: "/citzensAnalysis",
      code: "05000",

    },
        {
      icon: "mdi-account",
      title: "المختبرات",
      link: "/laboratories",
      code: "05000",

    },
    
        {
      icon: "mdi-account",
      title: "انواع النتائج",
      link: "/resultTypes",
      code: "05000",

    },
        {
      icon: "mdi-account",
      title: "انواع التحليل",
      link: "/analysisTypes",
      code: "05000",

    },
    {
      icon: "mdi-account",
      title: "الموظفين",
      link: "/employees",
      code: "05000",

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
