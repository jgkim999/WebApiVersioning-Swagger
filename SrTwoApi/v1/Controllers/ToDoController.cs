using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SrTwoApi.v1.Models;
using System.Collections.Generic;
using System.Linq;

namespace SrTwoApi.v1.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all to-do items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse("400", typeof(BadRequestResult), Description = "Bad Request")]
        [SwaggerResponse("404", typeof(NotFoundResult), Description = "Not Found")]
        [SwaggerResponse("200", typeof(List<TodoItem>), Description = "200 Success")]
        public ActionResult<List<TodoItem>> GetAll()
        {
            List<TodoItem> items = _context.TodoItems.ToList();
            if (items != null)
                return items;
            else
                return NotFound();
        }

        /// <summary>
        /// Gets a single todo.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTodo")]
        [SwaggerResponse("400", typeof(BadRequestResult), Description = "Bad Request")]
        [SwaggerResponse("404", typeof(NotFoundResult), Description = "Not Found")]
        [SwaggerResponse("200", typeof(List<TodoItem>), Description = "200 Success")]
        public ActionResult<TodoItem> GetById(long id)
        {
            TodoItem item = _context.TodoItems.Find(id);
            if (item != null)
                return item;
            else
                return NotFound();
        }

        /// <summary>
        /// Add new item.
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [SwaggerResponse("201", typeof(TodoItem), Description = "201 Create Success")]
        [SwaggerResponse("400", typeof(BadRequestResult), Description = "Bad Request")]
        public IActionResult Create(TodoItem item)
        {
            try
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
                return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPut("{id}")]
        [SwaggerResponse("204", typeof(NoContentResult), Description = "Success")]
        [SwaggerResponse("400", typeof(BadRequestResult), Description = "Bad Request")]
        [SwaggerResponse("404", typeof(NotFoundResult), Description = "Not Found")]
        public IActionResult Update(long id, TodoItem item)
        {
            TodoItem todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [SwaggerResponse("204", typeof(NoContentResult), Description = "Success")]
        [SwaggerResponse("400", typeof(BadRequestResult), Description = "Bad Request")]
        [SwaggerResponse("404", typeof(NotFoundResult), Description = "Not Found")]
        public IActionResult Delete(long id)
        {
            TodoItem todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return Ok();
        }
    }
}