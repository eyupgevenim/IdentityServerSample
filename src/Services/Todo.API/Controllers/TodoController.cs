using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Todo.API.Data;
using Todo.API.Infrastructure.Exceptions;
using Todo.API.Models;

namespace Todo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ILogger<TodoController> logger, TodoDbContext todoDbContext)
        {
            _logger = logger;
            _todoDbContext = todoDbContext;
        }

        [HttpGet]
        public IActionResult Get() => RunSafely(() =>
        {
            var todos = _todoDbContext.Todos.Select(x => new TodoDTO { Id = x.Id, Name = x.Name, Content = x.Content }).ToList();
            return Ok(todos);
        });

        [HttpGet("{id}")]
        public IActionResult Get(int id) => RunSafely(() =>
        {
            var todo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return NotFound();

            return Ok(new TodoDTO { Id = todo.Id, Name = todo.Name, Content = todo.Content });
        });

        [HttpPost]
        public IActionResult Post([FromBody] TodoDTO model) => RunSafely(() =>
        {
            var entity = new Todo.API.Data.Todo
            {
                Name = model.Name,
                Content = model.Name
            };
            _todoDbContext.Todos.Add(entity);
            _todoDbContext.SaveChanges();

            return Ok();
        });

        [HttpPut]
        public IActionResult Put(int id, [FromBody] TodoDTO model) => RunSafely(() =>
        {
            var entity = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return NotFound();

            entity.Name = model.Name;
            entity.Content = model.Name;

            _todoDbContext.Todos.Add(entity);
            _todoDbContext.SaveChanges();

            return Ok();
        });

        [HttpDelete]
        public IActionResult Delete(int id) => RunSafely(() =>
        {
            var entity = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return NotFound();

            _todoDbContext.Todos.Remove(entity);
            _todoDbContext.SaveChanges();

            return Ok();
        });

        #region Helpers

        private IActionResult RunSafely(Func<IActionResult> func)
        {
            try
            {
                return func();
            }
            catch (TodoException todoException)
            {
                _logger.LogError(todoException, $"todoException - Message: {todoException.Message}");
                //TODO:...
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Message: {exception.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

        #endregion

    }
}
