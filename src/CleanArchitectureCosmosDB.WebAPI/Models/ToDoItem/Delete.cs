using AutoMapper;
using CleanArchitectureCosmosDB.Core.Exceptions;
using CleanArchitectureCosmosDB.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     Delete related commands, validators, and handlers
    /// </summary>
    public class Delete
    {
        /// <summary>
        ///     Model to Delete an entity
        /// </summary>
        public class DeleteToDoItemCommand : IRequest<CommandResponse>
        {
            /// <summary>
            ///     Id
            /// </summary>
            public string Id { get; set; }

        }

        /// <summary>
        ///     Command Response
        /// </summary>
        public class CommandResponse
        {
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class DeleteToDoItemCommandValidator : AbstractValidator<DeleteToDoItemCommand>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public DeleteToDoItemCommandValidator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            }

        }


        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<DeleteToDoItemCommand, CommandResponse>
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
            public async Task<CommandResponse> Handle(DeleteToDoItemCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse();

                Core.Entities.ToDoItem entity = await _repo.GetItemAsync(command.Id);
                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(ToDoItem), command.Id);
                }

                await _repo.DeleteItemAsync(command.Id);

                return response;
            }
        }
    }
}
