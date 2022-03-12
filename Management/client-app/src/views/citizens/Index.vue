<template src="./Index.html"></template>

<script>
import AddView from "./actions/Add";
import EditView from "./actions/Edit";
import DetailsView from "./details/Details";

export default {
  components: {
    AddView,
    EditView,
    DetailsView,
  },
    created() {
      this.getAll();
    },
  computed: {  
   
  },
  data() {
    return {
      users:[],
      subtitle: "قائمة المستخدمين",
      dialog: false,
      dialogType: null,
      dialogMessage: "",
      itemId: 0,
      view: 0,
      filters: {
        page: 1,
        pageSize: 10,
        search: null,
      },
      headers: [
        {
          text: "اسم المستخدم",
          value: "loginName",
          sortable: false,
          align: "start",
        },
        { text: "نوع المستخدم   ", value: "userType", sortable: false },
        { text: "الوصف الوظيفي ", value: "jobDescription", sortable: false },
        { text: "السجل التابع له  ", value: "parentCenter", sortable: false },
        { text: "الحالة ", value: "status", sortable: false },
        { text: "مسجل بواسطة", value: "createdBy", sortable: false },
        { text: "تاريخ التسجيل", value: "createdOn", sortable: false },
        {
          text: "الإجراءات",
          value: "actions",
          sortable: false,
          align: "center",
        },
      ],
    };
  },
  methods: {
    getAll(page) {
      this.filters.page = page || 1;
      // this.$store.dispatch("users/getAll", this.filters);
    },
    addItem() {
      this.view = 1;
      this.subtitle="إضافة مستخدم"
    },
    editItem(item) {
      this.itemId = item.userId;
      this.view = 2;
      this.subtitle="تعديل مستخدم"
    },
    showDetails(item) {
      this.itemId = item.userId;
      this.view = 3;
      this.subtitle="عرض التفاصيل"
    },
    showDialog(item, type) {
      switch (type) {
        case "delete":
          this.dialogMessage = "هل انت متأكد من رغبتك حذف المستخدم";
          break;
        case "lock":
          this.dialogMessage = "هل انت متأكد من رغبتك تجميد المستخدم";
          break; 
        case "unlock":
          this.dialogMessage = "هل انت متأكد من رغبتك تفعيل المستخدم";
          break; 
        case "resetPassword":
          this.dialogMessage = "هل انت متأكد من رغبتك اعادة تعيين كلمة المرور ";
          break;
      }
      this.dialogType = type;
      this.itemId = item.userId;
      this.dialog = true;
    },
    confirmDialog() {
      this.$store.dispatch(
        `users/${this.dialogType}`,
        this.itemId
      );
      this.cancelDialog();
    },
    cancelDialog() {
      this.dialog = false;
      this.itemId = 0;
      this.dialogMessage = "";
      this.dialogType = null;
    },
    back() {
      this.view = 0;
      this.subtitle="قائمة المستخدمين"
    },
    reload() {
      this.filters.search = null;
      this.getAll(1);
      this.back();
    },
  },
};
</script>
