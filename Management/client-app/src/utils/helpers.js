import store from "@/store"

export const serverErrorMessage = "تعذر الاتصال بالخادم، الرجاء المحاولة لاحقا";
export function hasPermission(code) {
    return this.$store.getters["security/hasPermission"](code);
}

export function startLoading() {
    store.dispatch("loading/start")
}

export function stopLoading() {
    store.dispatch("loading/stop")
}

export function showNotification(message) {
    store.dispatch("notification/setMessage", message)
}

export function showAlert(message) {
    store.dispatch("alert/setMessage", message)
}

export function catchHandler(error) {
    if (!error.response.data.statusCode) {
        showAlert(serverErrorMessage);
        return;
    }

    const err = error.response.data;
    showAlert(`${err.message} [${err.statusCode}]`);
}