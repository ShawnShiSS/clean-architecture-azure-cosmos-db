using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
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
                //RuleFor(x => x.Id)
                //    .NotEmpty();
            }

        }

        /// <summary>
        ///     Handler
        /// </summary>
        public class QueryHandler : IRequestHandler<GetAllQuery, QueryResponse>
        {
            private readonly IToDoItemRepository _repo;
            private readonly IMapper _mapper;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="repo"></param>
            /// <param name="mapper"></param>
            public QueryHandler(IToDoItemRepository repo,
                                  IMapper mapper)
            {
                this._repo = repo ?? throw new ArgumentNullException(nameof(repo));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

                var entities = await _repo.GetItemsAsync($"SELECT * FROM c");

                response.Resource = entities.Select(x => _mapper.Map<ToDoItemModel>(x));

                return response;
            }
        }
    }
}
