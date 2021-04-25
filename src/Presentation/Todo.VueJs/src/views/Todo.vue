<template>
    <div class="todos">
        <h1 v-if="isLoading">Load Todos ...</h1>
        <template v-if="todos.length>0">
            <div>
                <b-table striped hover :items="todos" :fields="tableFields"></b-table>
            </div>
        </template>
        <template v-else>
            <div>
                <h4>No things to do</h4>
            </div>
        </template>
    </div>
</template>

<script>
    import TodoService from '../services/todo.service';
    export default {
        name: 'Todo',
        data() {
            return {
                isLoading: true,
                todos: [],
                tableFields: [
                    {
                        key: 'id',
                        label: 'Id',
                        sortable: true,
                        variant: 'danger'
                    },
                    {
                        key: 'name',
                        label: 'Name',
                        sortable: true
                    },
                    {
                        key: 'content',
                        label: 'Content',
                        sortable: false
                    }, 
                ]
            };
        },
        async created() {
            try {

                TodoService.getAllTodos().then(
                    response => {
                        this.todos = response.data;
                        this.isLoading = false;
                    },
                    error => {
                        console.log("error: ", error);
                        this.isLoading = false;
                    }
                );

            } catch (e) {
                console.log("err:", e);
            }
        }
    }
</script>

<style scoped>
</style>