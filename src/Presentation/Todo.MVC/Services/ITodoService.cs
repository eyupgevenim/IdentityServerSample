namespace Todo.MVC.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Todo.MVC.Models;

    public interface ITodoService
    {
        Task<List<TodoViewModel>> GetTodos();
        Task<TodoViewModel> GetTodo(string todoId);
    }
}
