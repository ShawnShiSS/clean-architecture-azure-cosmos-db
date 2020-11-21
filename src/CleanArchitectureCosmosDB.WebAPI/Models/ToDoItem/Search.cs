using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Specifications;
using CleanArchitectureCosmosDB.Core.Specifications.Interfaces;
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
    ///     Search related commands, validators, and handlers
    /// </summary>
    public class Search
    {
        /// <summary>
        ///     Model to Search
        /// </summary>
        public class SearchToDoItemQuery : IRequest<QueryResponse>, ISearchQuery
        {
            // Pagination and Sort
            /// <summary>
            ///     Starting point (translates to OFFSET)
            /// </summary>
            public int Start { get; set; }
            /// <summary>
            ///     Page Size (translates to LIMIT)
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            ///     Sort by Column
            /// </summary>
            public string SortColumn { get; set; }
            /// <summary>
            ///     Sort direction
            /// </summary>
            public SortDirection? SortDirection { get; set; }

            // Search
            /// <summary>
            ///     Title
            /// </summary>
            public string TitleFilter { get; set; }
        }

        /// <summary>
        ///     Query Response
        /// </summary>
        public class QueryResponse
        {
            /// <summary>
            ///     Current Page, 0-indexed
            /// </summary>
            public int CurrentPage { get; set; }

            /// <summary>
            ///     Total Records Matched. For Pagination purpose.
            /// </summary>
            public int TotalRecordsMatched { get; set; }

            /// <summary>
            ///     Resource
            /// </summary>
            public IEnumerable<ToDoItemModel> Resource { get; set; }
        }



        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class SearchToDoItemQueryValidator : AbstractValidator<SearchToDoItemQuery>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public SearchToDoItemQueryValidator()
            {
                RuleFor(x => x.PageSize)
                    .GreaterThan(0);

            }

        }

        /// <summary>
        ///     Handler
        /// </summary>
        public class QueryHandler : IRequestHandler<SearchToDoItemQuery, QueryResponse>
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
            public async Task<QueryResponse> Handle(SearchToDoItemQuery query, CancellationToken cancellationToken)
            {
                QueryResponse response = new QueryResponse();

                // records
                var specification = new ToDoItemSearchSpecification(query.TitleFilter,
                                                                      query.Start,
                                                                      query.PageSize,
                                                                      query.SortColumn,
                                                                      query.SortDirection ?? query.SortDirection.Value);

                var entities = await _repo.GetItemsAsync(specification);
                response.Resource = entities.Select(x => _mapper.Map<ToDoItemModel>(x));

                // count
                var countSpecification = new ToDoItemSearchAggregationSpecification(query.TitleFilter);
                response.TotalRecordsMatched = await _repo.GetItemsCountAsync(countSpecification);

                response.CurrentPage = (query.PageSize != 0) ? query.Start / query.PageSize : 0;

                return response;
            }
        }
    }
}
