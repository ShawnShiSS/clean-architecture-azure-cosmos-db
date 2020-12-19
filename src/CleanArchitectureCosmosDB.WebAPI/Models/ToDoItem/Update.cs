using AutoMapper;
using CleanArchitectureCosmosDB.Core.Exceptions;
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
            private readonly IToDoItemRepository _repo;

            /// <summary>
            ///     Validator ctor
            /// </summary>
            public UpdateToDoItemCommandValidator(IToDoItemRepository repo)
            {
                this._repo = repo ?? throw new ArgumentNullException(nameof(repo));

                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Category)
                    .NotEmpty();

                RuleFor(x => x.Title)
                    .NotEmpty();
            }

            /// <summary>
            ///     Check uniqueness
            /// </summary>
            /// <param name="command"></param>
            /// <param name="title"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> HasUniqueName(UpdateCommand command, string title, CancellationToken cancellationToken)
            {
                ToDoItemSearchSpecification specification = new ToDoItemSearchSpecification(title,
                                                                      exactSearch: true);

                IEnumerable<Core.Entities.ToDoItem> entities = await _repo.GetItemsAsync(specification);

                return entities == null ||
                       entities.Count() == 0 ||
                       // self
                       entities.All(x => x.Id == command.Id);

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
