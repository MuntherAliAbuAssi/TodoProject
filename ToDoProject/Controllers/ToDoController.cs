using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using TODOProject.Controllers;
using TODOProject.Core.Managers.interfaces;
using TODOProject.ViewModel.ViewModels;

namespace ToDoProject.Controllers
{
    public class ToDoController : ApiBaseController
    {
        private readonly IToDoManager _toDoManager;
        private readonly ILogger<ToDoController> _looger;
        public ToDoController(IToDoManager toDoManager, ILogger<ToDoController> looger)
        {
            _toDoManager = toDoManager;
            _looger = looger;
        }
        [Route("api/toDo")]
        [HttpGet]
        public IActionResult GetToDo(int page = 1, int pageSize = 5, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var result = _toDoManager.GetToDo(page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(result);
        }
         
        [Route("api/toDo/{id}")]
        [HttpGet]
        public IActionResult GetToDo(int id) 
        {
            var result = _toDoManager.GetToDo(id);
            return Ok(result);
        }
        [Route("api/toDo/IsRead/{id}")]
        [HttpGet]
        public IActionResult IsRead(int id)
        {
            _toDoManager.IsRead(id);
            return Ok();
        }


        [Route("api/toDo/{id}")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        public IActionResult ArchiveToDo(int id)
        {
            _toDoManager.ArchivedToDo(LoggedInUser, id);
            return Ok();
        }

        [Route("api/toDo")] 
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult PutToDo(ToDoRequest blogRequest)
        {
            var result = _toDoManager.PutToDo(LoggedInUser, blogRequest);
            return Ok(result);
        }
    }
}
