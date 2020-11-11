using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces.Persistence;
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
    ///     Get related query, validators, and handlers
    /// </summary>
    public class GetAuditHistory
    {
        /// <summary>
        ///     Get
        /// </summary>
        public class GetQuery : IRequest<QueryResponse>
        {
            /// <summary>
            ///     Entity Id
            /// </summary>
            public string Id { get; set; }

        }

        /// <summary>
        ///     Query Response
        /// </summary>
        public class QueryResponse
        {
            /// <summary>
            ///     Resource
            /// </summary>
            public IEnumerable<WebAPI.Models.ToDoItem.ToDoItemAuditModel> Resource { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class GetDefinitionQueryValidator : AbstractValidator<GetQuery>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public GetDefinitionQueryValidator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            }

        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class QueryHandler : IRequestHandler<GetQuery, QueryResponse>
        {
            private readonly IAuditRepository _repo;
            private readonly IMapper _mapper;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="repo"></param>
            /// <param name="mapper"></param>
            public QueryHandler(IAuditRepository repo,
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
            public async Task<QueryResponse> Handle(GetQuery query, CancellationToken cancellationToken)
            {
                QueryResponse response = new QueryResponse();

                var specification = new AuditFilterSpecification(query.Id);
                var entities = await _repo.GetItemsAsync(specification);

                // Map audit records to entity-specific audit model
                response.Resource = entities.Select(x => _mapper.Map<ToDoItemAuditModel>(x));

                return response;
            }

        }


    }
}
