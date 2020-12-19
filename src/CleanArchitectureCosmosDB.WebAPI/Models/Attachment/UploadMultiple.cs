using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces.Storage;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Storage.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.Attachment
{
    /// <summary>
    ///     UploadMultiple related commands, validators and handlers
    /// </summary>
    public class UploadMultiple
    {
        /// <summary>
        ///     Model to create a new Attachment
        /// </summary>
        public class UploadMultipleAttachmentCommand : IRequest<CommandResponse>
        {

            /// <summary>
            ///     The binary file to upload
            /// </summary>
            public IEnumerable<IFormFile> Files { get; set; }

        }

        /// <summary>
        ///     Command Response
        /// </summary>
        public class CommandResponse
        {
            /// <summary>
            ///     Attachments uploaded
            /// </summary>
            public List<AttachmentModel> UploadedAttachments { get; set; }   
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class UploadMultipleAttachmentCommandValidator : AbstractValidator<UploadMultipleAttachmentCommand>
        {
            private readonly IStorageService _storageService;

            /// <summary>
            ///     Validator ctor
            /// </summary>
            public UploadMultipleAttachmentCommandValidator(IStorageService storageService)
            {
                this._storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

                // Add Validation rules here

            }


        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<UploadMultipleAttachmentCommand, CommandResponse>
        {
            private readonly IStorageService _storageService;
            private readonly IMapper _mapper;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="storageService"></param>
            /// <param name="mapper"></param>
            public CommandHandler(IStorageService storageService,
                                  IMapper mapper)
            {
                this._storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            ///     Handle
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<CommandResponse> Handle(UploadMultipleAttachmentCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse()
                {
                    UploadedAttachments = new List<AttachmentModel>()
                };

                foreach(var commandFile in command.Files)
                {
                    AttachmentModel attachment = new
                    AttachmentModel();

                    // prepare the upload
                    string originalName = System.IO.Path.GetFileName(commandFile.FileName);
                    string extension = System.IO.Path.GetExtension(commandFile.FileName).ToLower();
                    string storedAsFileName = $"{attachment.Id}{extension}";
                    string fileFullPath = StoragePath.Combine("attachments",
                                                          storedAsFileName);

                    // upload
                    string fullPath = await _storageService.UploadFile(commandFile, fileFullPath);
                    attachment.FilePath = fullPath;

                    // prepare response
                    attachment.FileName = storedAsFileName;
                    attachment.FileType = extension;
                    attachment.ContentType = commandFile.ContentType;
                    attachment.OriginalFileName = originalName;
                    attachment.Name = originalName;
                    attachment.Description = $"Attachment {originalName}";

                    response.UploadedAttachments.Add(attachment);
                }
                
                return response;
            }
        }
    }
}
