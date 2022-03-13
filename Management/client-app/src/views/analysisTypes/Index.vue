<template src="./Index.html"></template>

<script>
import AddView from "./actions/Add";
import EditView from "./actions/Edit";
import DetailsView from "./details/Details";
import ResultView from "./actions/Result";

export default {
  components: {
    AddView,
    EditView,
    DetailsView,
    ResultView,
  },
  created() {
    this.getAll();
  },
  computed: {
    analysisTypes() {
      return this.$store.state.analysisTypes.analysisTypes;
    },
  },
  data() {
    return {
      subtitle: "قائمة التحاليل",
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
          text: "اسم التحليل",
          value: "analysisTypeName",
          sortable: false,
          align: "start",
        },
        { text: "رمز التحليل", value: "analysisTypeCode", sortable: false },
        { text: "الوصف  ", value: "description", sortable: false },
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
      this.$store.dispatch("analysisTypes/getAll", this.filters);
    },
    addItem() {
      this.view = 1;
      this.subtitle = "إضافة تحليل";
    },
    editItem(item) {
      this.itemId = item.analysisTypeId;
      this.view = 2;
      this.subtitle = "تعديل بيانات التحليل";
    },
    showDetails(item) {
      this.itemId = item.analysisTypeId;
      this.view = 3;
      this.subtitle = "عرض التفاصيل";
    },
    showResult(item) {
      this.itemId = item.analysisTypeId;
      this.view = 4;
      this.subtitle = "نتائد التحاليل";
    },
    showDialog(item, type) {
      switch (type) {
        case "delete":
          this.dialogMessage = "هل انت متأكد من رغبتك حذف التحليل";
          break;
        case "lock":
          this.dialogMessage = "هل انت متأكد من رغبتك تجميد التحليل";
          break;
        case "unlock":
          this.dialogMessage = "هل انت متأكد من رغبتك تفعيل التحليل";
          break;
      }
      this.dialogType = type;
      this.itemId = item.analysisTypeId;
      this.dialog = true;
    },
    confirmDialog() {
      this.$store.dispatch(`analysisTypes/${this.dialogType}`, this.itemId);
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
      this.subtitle = "قائمة التحاليل";
    },
    reload() {
      this.filters.search = null;
      this.getAll(1);
      this.back();
    },
  },
};
</script>
