using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
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
        public class CreateCommand : IRequest<CommandResponse>
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
        public class CreateToDoItemCommandValidator : AbstractValidator<CreateCommand>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public CreateToDoItemCommandValidator()
            {
                RuleFor(x => x.Category)
                    .NotEmpty();
                RuleFor(x => x.Title)
                    .NotEmpty();

            }

        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<CreateCommand, CommandResponse>
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
            public async Task<CommandResponse> Handle(CreateCommand command, CancellationToken cancellationToken)
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
