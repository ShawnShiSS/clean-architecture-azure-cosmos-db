using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Specifications;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     Create related commands, validators and handlers
    /// </summary>
    public class Create
    {
        /// <summary>
        ///     Model to create an entity
        /// </summary>
        public class CreateToDoItemCommand : IRequest<CommandResponse>
        {
            /// <summary>
            ///     Category
            /// </summary>
            public string Category { get; set; }

            /// <summary>
            ///     Title
            /// </summary>
            public string Title { get; set; }

            
        }

        /// <summary>
        ///     Command Response
        /// </summary>
        public class CommandResponse
        {
            /// <summary>
            ///     Item Id
            /// </summary>
            public string Id { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class CreateToDoItemCommandValidator : AbstractValidator<CreateToDoItemCommand>
        {
            private readonly IToDoItemRepository _repo;

            /// <summary>
            ///     Validator ctor
            /// </summary>
            public CreateToDoItemCommandValidator(IToDoItemRepository repo)
            {
                this._repo = repo ?? throw new ArgumentNullException(nameof(repo));

                RuleFor(x => x.Category)
                    .NotEmpty();
                RuleFor(x => x.Title)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .MustAsync(HasUniqueTitle).WithMessage("Title must be unique");

            }

            /// <summary>
            ///     Check uniqueness
            /// </summary>
            /// <param name="title"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> HasUniqueTitle(string title, CancellationToken cancellationToken)
            {
                var specification = new ToDoItemSearchSpecification(title,
                                                                      exactSearch: true);

                var entities = await _repo.GetItemsAsync(specification);

                return entities == null || entities.Count() == 0;

            }
        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<CreateToDoItemCommand, CommandResponse>
        {
            private readonly IToDoItemRepository _repo;
            private readonly IMapper _mapper;

            /// <summary>
            ///     Ctor
            /// </summary>
            /// <param name="repo"></param>
            /// <param name="mapper"></param>
            public CommandHandler(IToDoItemRepository repo,
                                  IMapper mapper)
            {
                this._repo = repo ?? throw new ArgumentNullException(nameof(repo));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            ///     Handle
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<CommandResponse> Handle(CreateToDoItemCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse();
                Core.Entities.ToDoItem entity = _mapper.Map<Core.Entities.ToDoItem>(command);
                await _repo.AddItemAsync(entity);

                response.Id = entity.Id;
                return response;
            }
        }
    }
}
