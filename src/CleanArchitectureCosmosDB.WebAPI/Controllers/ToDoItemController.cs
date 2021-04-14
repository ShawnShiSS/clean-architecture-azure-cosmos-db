using CleanArchitectureCosmosDB.Core.Exceptions;
using CleanArchitectureCosmosDB.WebAPI.Models.Shared;
using CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Controllers
{
    /// <summary>
    ///     ToDoItem Controller
    /// </summary>
    //[Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        /// <param name="mediator"></param>
        public ToDoItemController(IMediator mediator)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/ToDoItem
        // OData: https://localhost:5001/api/ToDoItem?$select=title
        /// <summary>
        ///     Get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IEnumerable<ToDoItemModel>> GetAll()
        {
            GetAll.QueryResponse response = await _mediator.Send(new GetAll.GetAllQuery());
            return response.Resource;
        }

        // GET: api/ToDoItem/5
        /// <summary>
        ///     Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetToDoItem")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<ToDoItemModel>> Get(string id)
        {
            Get.QueryResponse response = await _mediator.Send(new Get.GetQuery() { Id = id });

            return response.Resource;
        }

        // POST: api/ToDoItem
        /// <summary>
        ///     Create 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<IActionResult> Create([FromBody] Create.CreateToDoItemCommand command)
        {
            Create.CommandResponse response = await _mediator.Send(command);
            return CreatedAtRoute("GetToDoItem", new { id = response.Id }, null);
        }

        // PUT: api/ToDoItem/5
        /// <summary>
        ///     Update 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> Update(string id, [FromBody] Update.UpdateCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            Update.CommandResponse response = await _mediator.Send(command);

            return NoContent();
        }

        // DELETE: api/ToDoItem/5
        /// <summary>
        ///     Delete 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new Delete.DeleteToDoItemCommand() { Id = id });

            return NoContent();
        }

        // GET: api/ToDoItem/5/AuditHistory
        /// <summary>
        ///     Get audit history of an item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/AuditHistory", Name = "GetToDoItemAuditHistory")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IEnumerable<WebAPI.Models.ToDoItem.ToDoItemAuditModel>> GetAuditHistory(string id)
        {
            GetAuditHistory.QueryResponse response = await _mediator.Send(new GetAuditHistory.GetQuery() { Id = id });

            return response.Resource;
        }

        // Search: api/ToDoItem/Search
        /// <summary>
        ///     Search 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("Search", Name = "SearchDefinition")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        public async Task<DataTablesResponse> Search(Search.SearchToDoItemQuery query)
        {
            Search.QueryResponse response = await _mediator.Send(query);
            DataTablesResponse result = new DataTablesResponse()
            {
                Data = response.Resource,
                TotalRecords = response.TotalRecordsMatched,
                Page = response.CurrentPage
            };

            return result;
        }

    }
}
