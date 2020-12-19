using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces.Storage;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Storage.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.Attachment
{
    /// <summary>
    ///     Upload related commands, validators and handlers
    /// </summary>
    public class Upload
    {
        /// <summary>
        ///     Model to create a new Attachment
        /// </summary>
        public class UploadAttachmentCommand : IRequest<CommandResponse>
        {

            /// <summary>
            ///     The binary file to upload
            /// </summary>
            public IFormFile File { get; set; }

        }

        /// <summary>
        ///     Command Response
        /// </summary>
        public class CommandResponse
        {
            /// <summary>
            ///     Attachment that is uploaded
            /// </summary>
            public AttachmentModel Resource { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
        {
            private readonly IStorageService _storageService;

            /// <summary>
            ///     Validator ctor
            /// </summary>
            public UploadAttachmentCommandValidator(IStorageService storageService)
            {
                this._storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

                // Add Validation rules here

            }


        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<UploadAttachmentCommand, CommandResponse>
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
            public async Task<CommandResponse> Handle(UploadAttachmentCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse();
                AttachmentModel uploadedAttachment = new AttachmentModel();
                // prepare the upload
                string originalName = System.IO.Path.GetFileName(command.File.FileName);
                string extension = System.IO.Path.GetExtension(command.File.FileName).ToLower();
                string storedAsFileName = $"{uploadedAttachment.Id}{extension}";
                string fileFullPath = StoragePath.Combine("attachments", 
                                                          storedAsFileName);

                // upload
                string fullPath = await _storageService.UploadFile(command.File, fileFullPath);
                uploadedAttachment.FilePath = fullPath;

                // prepare response
                uploadedAttachment.FileName = storedAsFileName;
                uploadedAttachment.FileType = extension;
                uploadedAttachment.ContentType = command.File.ContentType;
                uploadedAttachment.OriginalFileName = originalName;
                uploadedAttachment.Name = originalName;
                uploadedAttachment.Description = $"Attachment {originalName}";

                response.Resource = uploadedAttachment;
               
                return response;
            }
        }
    }
}
