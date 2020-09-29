using AutoMapper;
using CleanArchitectureCosmosDB.Core.Exceptions;
using CleanArchitectureCosmosDB.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     Update related commands, validators, and handlers
    /// </summary>
    public class Update
    {
        /// <summary>
        ///     Model to Update an entity
        /// </summary>
        public class UpdateCommand : IRequest<CommandResponse>
        {
            /// <summary>
            ///     Id
            /// </summary>
            public string Id { get; set; }

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

        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class UpdateToDoItemCommandValidator : AbstractValidator<UpdateCommand>
        {
            /// <summary>
            ///     Validator ctor
            /// </summary>
            public UpdateToDoItemCommandValidator()
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
        public class CommandHandler : IRequestHandler<UpdateCommand, CommandResponse>
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
            public async Task<CommandResponse> Handle(UpdateCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse();

                Core.Entities.ToDoItem entity = await _repo.GetItemAsync(command.Id);
                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(ToDoItem), command.Id);
                }

                entity.Category = command.Category;
                entity.Title = command.Title;
                await _repo.UpdateItemAsync(command.Id, entity);

                return response;
            }
        }
    }
}
