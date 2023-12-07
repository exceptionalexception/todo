using Dtos;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoManager _todoManager;

        public TodoController(
            ILogger<TodoController> logger,
            ITodoManager todoManager)
        {
            _logger = logger;
            _todoManager = todoManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TodoDto>), 200)]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var todos = await _todoManager.GetTodos();
                return Ok(todos);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured getting todos.");
                throw;
            }
        }

        [HttpGet]
        [Route("{todoId}")]
        [ProducesResponseType(typeof(TodoDto), 200)]
        public async Task<IActionResult> GetTodoById(Guid todoId)
        {
            try
            {
                var todo = await _todoManager.GetTodo(todoId);
                return Ok(todo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured getting todo by id.");
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(TodoDto), 200)]
        public async Task<IActionResult> AddTodo([FromBody] TodoDto todo)
        {
            try
            {
                var todoAdded = await _todoManager.AddTodo(todo);
                return Ok(todoAdded);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured adding todo.");
                throw;
            }
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(TodoDto), 200)]
        public async Task<IActionResult> UpdateTodo([FromBody] TodoDto todo)
        {
            try
            {
                var todoUpdated = await _todoManager.UpdateTodo(todo);
                return Ok(todoUpdated);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured updating todo.");
                throw;
            }
        }


        [HttpPut]
        [Route("{todoUId}/complete")]
        public async Task<IActionResult> CompleteTodoById(Guid todoUId)
        {
            try
            {
                await _todoManager.CompleteTodo(todoUId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured completing todo by id.");
                throw;
            }
        }

        [HttpDelete]
        [Route("{todoUId}")]
        public async Task<IActionResult> DeleteTodoById(Guid todoUId)
        {
            try
            {
                await _todoManager.DeleteTodo(todoUId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured deleting todo by id.");
                throw;
            }
        }
    }
}
