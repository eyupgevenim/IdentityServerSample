import axios from 'axios';

const TODO_API_BASE_URL = 'http://localhost:5102';

const TodoService = {

    getAllTodos() {
        return axios.get(`${TODO_API_BASE_URL}/api/v1/todo`);
    }

}

export default TodoService;