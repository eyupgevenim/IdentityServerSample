<template>
    <div class="callback">
        <h1>Sign-in in progress ... </h1>
    </div>
</template>

<script>
    export default {
        async created() {
            try {

                var user = await this.$store.state.auth.userManager.signinRedirectCallback();
                if (!user) {
                    this.$router.push({ name: 'home' });
                }

                this.$store.commit('auth/loginSuccess', user);

                var returnToUrl = user.state || '/';
                this.$router.push({ path: returnToUrl });

            } catch (e) {
                console.log("err:", e);
                this.$router.push({ name: 'home' });
            }
        }
    }
</script>