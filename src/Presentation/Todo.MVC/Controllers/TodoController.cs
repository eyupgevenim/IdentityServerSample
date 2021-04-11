using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Todo.MVC.Services;

namespace Todo.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoSvc;

        public TodoController(ITodoService todoSvc)
        {
            _todoSvc = todoSvc;
        }

        // GET: TodoController
        public async Task<ActionResult> Index()
        {
            var response = await _todoSvc.GetTodos();
            return View(response);
        }

        //TODO:...

    }
}
