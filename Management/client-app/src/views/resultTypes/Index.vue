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
    resultTypes() {
      return this.$store.state.resultTypes.resultTypes;
    },
  },
  data() {
    return {
      subtitle: "قائمة انواع النتائج",
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
          text: "الاسم",
          value: "resultTypeName",
          sortable: false,
          align: "start",
        },
        // { text: "الوصف  ", value: "description", sortable: false },
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
      this.$store.dispatch("resultTypes/getAll", this.filters);
    },
    addItem() {
      this.view = 1;
      this.subtitle = "إضافة ";
    },
    editItem(item) {
      this.itemId = item.resultTypeId;
      this.view = 2;
      this.subtitle = "تعديل البيانات ";
    },
    showDetails(item) {
      this.itemId = item.resultTypeId;
      this.view = 3;
      this.subtitle = "عرض التفاصيل";
    },
  
    showDialog(item, type) {
      switch (type) {
        case "delete":
          this.dialogMessage = "هل انت متأكد من رغبتك حذف النتيجة";
          break;
        case "lock":
          this.dialogMessage = "هل انت متأكد من رغبتك تجميد النتيجة";
          break;
        case "unlock":
          this.dialogMessage = "هل انت متأكد من رغبتك تفعيل النتيجة";
          break;
      }
      this.dialogType = type;
      this.itemId = item.resultTypeId;
      this.dialog = true;
    },
    confirmDialog() {
      this.$store.dispatch(`resultTypes/${this.dialogType}`, this.itemId);
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
      this.subtitle = "قائمة انواع النتائج";
    },
    reload() {
      this.filters.search = null;
      this.getAll(1);
      this.back();
    },
  },
};
</script>
