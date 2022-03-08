import router from "./"
import store from "../store"

router.beforeEach((to, _, next) => {

    if(store.state.security.isLoggedIn === null) {
        store.dispatch("security/loginStatus").then(() => {
            routeguard(to, next)
        })
    }
    else {
        routeguard(to, next)
    }

})

router.afterEach(() => {
    window.scrollTo(0, 0);
});

function routeguard(to, next) {
    if(store.state.security.isLoggedIn) {
        if(to.name === "Login") {
            next({ name: "Layout" })
        }
        next()
    }
    else {
        if(to.name !== "Login") {
            next({ name: "Login" })
        }
    }
    next()
}