import Vue from "vue";
import { ValidationObserver, ValidationProvider } from "vee-validate";
//import * as featherIcons from "vue-feather-icons";
// Common components import
import AppDialog from "@/components/common/AppDialog";
import AppAlert from "@/components/common/AppAlert";
import AppFormDialog from "@/components/common/AppFormDialog";
import AppIconTooltip from "@/components/common/AppIconTooltip";

// Form components import
import AppDatePicker from "@/components/form/AppDatePicker";
import AppFormField from "@/components/form/AppFormField";
import AppFormSelect from "@/components/form/AppFormSelect";
import AppYearSelect from "@/components/form/AppYearSelect";
import AppFileInput from "@/components/form/AppFileInput";
import AppReadonlyField from "@/components/form/AppReadonlyField";
import AppTextArea from "@/components/form/AppTextArea";
import AppPageContainer from "@/components/layout/AppPageContainer";

Vue.component("ValidationObserver", ValidationObserver);
Vue.component("ValidationProvider", ValidationProvider);

//for (const [key, icon] of Object.entries(featherIcons)) {
//  icon.props.size.default = "24";
//  Vue.component(key, icon);
//}

// Common components 
Vue.component("AppDialog", AppDialog);
Vue.component("AppAlert", AppAlert);
Vue.component("AppFormDialog", AppFormDialog);
Vue.component("AppIconTooltip", AppIconTooltip);
Vue.component("AppPageContainer", AppPageContainer);

// Form components
Vue.component("AppDatePicker", AppDatePicker);
Vue.component("AppFormField", AppFormField);
Vue.component("AppFormSelect", AppFormSelect);
Vue.component("AppYearSelect", AppYearSelect);
Vue.component("AppFileInput", AppFileInput);
Vue.component("AppReadonlyField", AppReadonlyField);
Vue.component("AppTextArea", AppTextArea);
