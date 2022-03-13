import Vue from "vue";


Vue.filter("status", (str) => {
	switch (str) {
		case 10:
			return "مدخل"
		case 1:
			return "مفعل"
		case 2:
			return "مجمد"	
		case 11:
			return "مستلم"	
		case 14:
			return "معتمد"
		case 15:
			return "مرسل"
		case 16:
			return "معترف به"
		case 13:
			return "مؤجل"
		case 12:
			return "مراجع"	
		case 17:
			return "غير مكتمل"	
	}
});

Vue.filter("patientStatus", (str) => {
	switch (str) {
		case 10:
			return "مدخل"
		case 8:
			return "غير مستكمل"	
		case 1:
			return "معتمد"
	
	}
});
Vue.filter("lastContactStatus", (str) => {
	switch (str) {
		case 1:
			return "حي"
		case 2:
			return "متوفي "	
		case 9:
			return "غير معروف"
	
	}
});

Vue.filter("countries", (str) => {
	switch (str) {
		case 0:
			return "ليبيا"
		case 1:
			return "تونس"
		case 2:
			return "مصر"
	}
});
Vue.filter("cities", (str) => {
	switch (str) {
		case 0:
			return "طرابلس"
		case 1:
			return "بنغازي"
		case 2:
			return "صفاقس"
	}
});
Vue.filter("userTypes", (str) => {
	switch (str) {
		case 1:
			return "إداري"
		case 2:
			return "مستخدم مناطق"
		case 3:
			return "مستخدم مرفق صحي"
	}
});
Vue.filter("healthCenterType", (str) => {
	switch (str) {
		case true:
			return "عام"
		case false:
			return "خاص "
	
	}
});

Vue.filter("null", (str) => {
	if (str === "" || str === null) return "لا يوجد";
	else return str;
});