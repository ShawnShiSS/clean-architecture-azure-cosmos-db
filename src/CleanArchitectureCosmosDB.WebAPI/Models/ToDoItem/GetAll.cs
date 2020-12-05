using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Specifications;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     GetAll related commands, validators, and handlers
    /// </summary>
    public class GetAll
    {
        /// <summary>
        ///     Model to GetAll entities
        /// </summary>
        public class GetAllQuery : IRequest<QueryResponse>
        {

        }

        /// <summary>
        ///     Query Response
        /// </summary>
        public class QueryResponse
        {
            /// <summary>
            ///     Resource
            /// </summary>
            public IEnumerable<ToDoItemModel> Resource { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class GetAllToDoItemQueryValidator : AbstractValidator<GetAllQuery>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public GetAllToDoItemQueryValidator()
            {

            }

        }

        /// <summary>
        ///     Handler
        /// </summary>
        public class QueryHandler : IRequestHandler<GetAllQuery, QueryResponse>
        {
            private readonly IToDoItemRepository _repo;
            private readonly IMapper _mapper;
            private readonly ICachedToDoItemsService _cachedToDoItemsService;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="repo"></param>
            /// <param name="mapper"></param>
            /// <param name="cachedToDoItemsService"></param>
            public QueryHandler(IToDoItemRepository repo,
                                IMapper mapper,
                                ICachedToDoItemsService cachedToDoItemsService)
            {
                this._repo = repo ?? throw new ArgumentNullException(nameof(repo));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                this._cachedToDoItemsService = cachedToDoItemsService ?? throw new ArgumentNullException(nameof(cachedToDoItemsService));
            }

            /// <summary>
            ///     Handle
            /// </summary>
            /// <param name="query"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<QueryResponse> Handle(GetAllQuery query, CancellationToken cancellationToken)
            {
                QueryResponse response = new QueryResponse();

                // If needed, this is where to implement cache reading and setting logic 
                //var cachedEntities = await _cachedToDoItemsService.GetCachedToDoItemsAsync();

                //var entities = await _repo.GetItemsAsync($"SELECT * FROM c");
                // Get all the incompleted todo items
                ToDoItemGetAllSpecification specification = new ToDoItemGetAllSpecification(false);
                IEnumerable<Core.Entities.ToDoItem> entities = await _repo.GetItemsAsync(specification);
                response.Resource = entities.Select(x => _mapper.Map<ToDoItemModel>(x));

                return response;
            }
        }
    }
}
