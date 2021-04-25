<template>
    <div id="app">

        <div id="nav">
            <top-header></top-header>
        </div>

        <b-container>
            <router-view />
        </b-container>
        
    </div>
</template>
<script>
    import TopHeader from "@/components/top-header.vue";
    export default {
        components: { "top-header": TopHeader }
    };
</script>

<style scoped>
</style>
