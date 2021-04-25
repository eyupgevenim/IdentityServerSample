import Vue from 'vue';
import "./plugins/bootstrap-vue";
import App from './App.vue';
import router from "./router";
import store from "./store";
import axios from 'axios';

Vue.config.productionTip = true;

let vue = new Vue({
    router: router,
    store: store,
    render: h => h(App)
}).$mount('#app');


axios.interceptors.request.use((config) => {
    const user = store.state.auth.user;
    if (user) {
        const authToken = user.id_token;//user.access_token;
        if (authToken) {
            config.headers.Authorization = `Bearer ${authToken}`;
        }
    }
    return config;
}, (err) => {
    console.log("main.js - axios.interceptors.request.use -> ", err);
    //What do we do when we get errors?
});

