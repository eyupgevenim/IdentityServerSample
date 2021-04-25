<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="#">Todo VueJs</b-navbar-brand>
            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item to="/">Home</b-nav-item>
                    <b-nav-item to="/about">About</b-nav-item>
                </b-navbar-nav>

                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <template v-if="isAuthenticated">
                        <b-nav-item to="/todos">Todos</b-nav-item>
                        <b-nav-item @click="logout">Logout</b-nav-item>
                    </template>
                    <template v-else>
                        <b-nav-item v-on:click="login">Login</b-nav-item>
                    </template>

                    <!--<div class="d-none"> {{isAuthenticated}} </div>-->

                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    import store from "../store";
    export default {
        data: function () {
            return {
                isLogin: false
            };
        },
        methods: {
            login() {
                this.$store.dispatch('auth/login', '/');
            },
            logout() {
                this.$store.dispatch('auth/logout');
            }
        },
        async created() {
            var user = await this.$store.dispatch('auth/getUser');
            if (user) {
                this.$store.commit('auth/loginSuccess', user);
            }
        },
        computed: {
            isAuthenticated: () => store.state.auth.status.isAuthenticated
        }
    };
</script>

<style scoped>
    #nav a.router-link-exact-active {
        color: white;
    }
</style>
