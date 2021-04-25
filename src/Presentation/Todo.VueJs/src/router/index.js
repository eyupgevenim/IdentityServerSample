import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import About from "../views/About.vue";
import Todos from "../views/Todo.vue";
import Callback from '../views/Callback';
import store from "../store";

Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        name: "home",
        component: Home
    },
    {
        path: "/home",
        name: "home",
        component: Home
    },
    {
        path: "/about",
        name: "about",
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        //component: () => import(/* webpackChunkName: "about" */ "../views/About.vue")
        component: About
    },
    {
        path: "/todos",
        name: "todos",
        component: Todos,
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/callback.html',
        name: 'callback',
        component: Callback
    }
];

const beforeEach = async (to, from, next) => {
    let status = store.state.auth.status || { isAuthenticated: false }
    if (status.isAuthenticated) {

        //already signed in, we can navigate anywhere
        next();

    } else if (to.matched.some(record => record.meta.requiresAuth)) {

        //authentication is required. Trigger the sign in process, including the return URI
        store.dispatch('auth/authenticate', to.path).then(() => {
            console.log('authenticating a protected url:' + to.path);
            //next();
        });

    } else {

        //No auth required. We can navigate
        next();

    }
};

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes: routes
});

router.beforeEach(beforeEach);

export default router;