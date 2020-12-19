using CleanArchitectureCosmosDB.WebAPI.Models.Attachment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Controllers
{
    /// <summary>
    ///     Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        /// <param name="mediator"></param>
        public AttachmentController(IMediator mediator)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/Attachment/Download?filePath=abc.JPG
        /// <summary>
        ///     Download an item by path
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="originalFileName"></param>
        /// <returns></returns>
        [HttpGet("Download", Name = "DownloadAttachment")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<FileStreamResult> Download([FromQuery] string filePath,
                                                     [FromQuery] string originalFileName = "")
        {
            var response = await _mediator.Send(
                new Download.DownloadQuery()
                {
                    FilePath = filePath,
                    OriginalFileName = originalFileName
                });

            return File(response.Stream, response.ContentType, response.FileName);
        }

        // POST: api/Attachment
        /// <summary>
        ///     Upload an item
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<Models.Attachment.Upload.CommandResponse>> Upload([FromForm] Models.Attachment.Upload.UploadAttachmentCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtRoute("DownloadAttachment",
                                  new
                                  {
                                      filePath = response.Resource.FilePath,
                                      originalFileName = response.Resource.OriginalFileName
                                  },
                                  response);
        }

        // POST: api/Attachment/Multiple
        /// <summary>
        ///     Upload multiple files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Multiple", Name = "UploadMultipleAttachment")]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<Models.Attachment.Upload.CommandResponse>> UploadMultiple([FromForm] Models.Attachment.UploadMultiple.UploadMultipleAttachmentCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response.UploadedAttachments);
        }
    }
}
