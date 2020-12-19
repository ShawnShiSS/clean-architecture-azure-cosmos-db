using AutoMapper;
using CleanArchitectureCosmosDB.Core.Exceptions;
using CleanArchitectureCosmosDB.Core.Interfaces.Storage;
using FluentValidation;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.Attachment
{

    /// <summary>
    ///     Download related query, validators, and handlers
    /// </summary>
    public class Download
    {
        /// <summary>
        ///     Model to Download 
        /// </summary>
        public class DownloadQuery : IRequest<QueryResponse>
        {
            /// <summary>
            ///     Full Path
            /// </summary>
            public string FilePath { get; set; }

            /// <summary>
            ///     Original name of the file
            /// </summary>
            public string OriginalFileName { get; set; }
        }

        /// <summary>
        ///     Query Response
        /// </summary>
        public class QueryResponse
        {
            /// <summary>
            ///     File Name
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            ///     Content Type
            /// </summary>
            public string ContentType { get; set; }

            /// <summary>
            ///     Stream
            /// </summary>
            public Stream Stream { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class DownloadAttachmentQueryValidator : AbstractValidator<DownloadQuery>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public DownloadAttachmentQueryValidator()
            {
                RuleFor(x => x.FilePath)
                    .NotEmpty();
            }

        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class QueryHandler : IRequestHandler<DownloadQuery, QueryResponse>
        {
            private readonly IStorageService _storageService;
            private readonly IMapper _mapper;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="storageService"></param>
            /// <param name="mapper"></param>
            public QueryHandler(IStorageService storageService,
                                IMapper mapper)
            {
                this._storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            ///     Handle
            /// </summary>
            /// <param name="query"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<QueryResponse> Handle(DownloadQuery query, CancellationToken cancellationToken)
            {
                QueryResponse response = new QueryResponse();
                Stream ms = new MemoryStream();

                try
                {
                    ms = await _storageService.GetFileStream(query.FilePath);
                    ms.Position = 0; // Have to reset the current position
                }
                catch(Exception ex)
                {
                    // Exception and logging are handled in a centralized place, see ApiExceptionFilter.
                    throw new EntityNotFoundException(nameof(AttachmentModel), query.FilePath);
                }

                // Content Type mapping
                string[] fileNameParts = query.FilePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                string fileName = fileNameParts[fileNameParts.Length - 1];

                var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                string contentType = "application/octet-stream";
                if (!contentTypeProvider.TryGetContentType(fileName, out contentType))
                {
                    contentType = "application/octet-stream"; // fallback
                }

                response.FileName = String.IsNullOrEmpty(query.OriginalFileName) ? fileName : query.OriginalFileName;
                response.Stream = ms;
                response.ContentType = contentType;

                return response;
            }

        }

        
    }
}
