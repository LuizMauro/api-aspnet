using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Hubs;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class TodoController:ControllerBase
    {
        private readonly IHubContext<PhoneHub> _hubContext;

        public TodoController(IHubContext<PhoneHub> hubContext)
        {
            _hubContext = hubContext;
        }

            [HttpGet]
            [Route("todos")]
            public async Task<IActionResult> Get([FromServices] AppDbContext context) {

                var todos = await context.Todos.AsNoTracking().ToListAsync();

                return Ok(todos);
            }

            [HttpGet]
            [Route("todos/{id}")]
            public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
            {

                 var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                return todo == null ? NotFound() : Ok(todo);
            }


            [HttpPost("sendEventSocket")]
            public async Task<IActionResult> PostAsync(SendEventModel model )
            {
                await _hubContext.Clients.All.SendAsync(model.eventName, model.message);
                return Ok();
            }

            [HttpGet("sendNotificationById")]
            [Route("sendNotificationById/{id}")]
            public async Task<IActionResult> SendNotificationId([FromRoute] int id)
            {
                await _hubContext.Clients.All.SendAsync("notification-by-id-" + id, "teste");
                return Ok();
             }


        [HttpPost("todos")]
            public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model)
                {

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var todo = new Todo
                    {
                        Date = DateTime.Now,
                        Done = false,
                        Title = model.Title
                    };


                    try
                    {

                        await context.Todos.AddAsync(todo);
                        await context.SaveChangesAsync();

                        return Created("v1/todos/{todo.Id}", todo);
                    }
                        
                        catch(Exception ex) {  return BadRequest(ex);
                
                    }
                 }   

    }
}
