using AutoMapper;
using CleanArchitectureCosmosDB.Core.Exceptions;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Models.Authentication;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.Token
{
    /// <summary>
    ///     Authenticate
    /// </summary>
    public class Authenticate
    {
        /// <summary>
        ///     command
        /// </summary>
        public class AuthenticateCommand : TokenRequest, IRequest<CommandResponse>
        {
        }

        /// <summary>
        ///     Response
        /// </summary>
        public class CommandResponse
        {
            /// <summary>
            ///     Resource
            /// </summary>
            public TokenResponse Resource { get; set; }
        }

        /// <summary>
        ///     Register Validation 
        /// </summary>
        public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
        {

            /// <summary>
            ///     Validator ctor
            /// </summary>
            public AuthenticateCommandValidator()
            {
                RuleFor(x => x.Username)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty();

                RuleFor(x => x.Password)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty();
            }

        }

        /// <summary>
        ///     Handler
        /// </summary>
        public class CommandHandler : IRequestHandler<AuthenticateCommand, CommandResponse>
        {
            private readonly ITokenService _tokenService;
            private readonly IMapper _mapper;
            private readonly HttpContext _httpContext;

            /// <summary>
            ///     ctor
            /// </summary>
            /// <param name="tokenService"></param>
            /// <param name="mapper"></param>
            /// <param name="httpContextAccessor"></param>
            public CommandHandler(ITokenService tokenService,
                                  IMapper mapper,
                                  IHttpContextAccessor httpContextAccessor)
            {
                this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
                this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

            }

            /// <summary>
            ///     Handle
            /// </summary>
            /// <param name="command"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<CommandResponse> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
            {
                CommandResponse response = new CommandResponse();

                string ipAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                TokenResponse tokenResponse = await _tokenService.Authenticate(command, ipAddress);
                if (tokenResponse == null)
                {
                    throw new InvalidCredentialsException();
                }

                response.Resource = tokenResponse;
                return response;
            }
        }

    }
}
