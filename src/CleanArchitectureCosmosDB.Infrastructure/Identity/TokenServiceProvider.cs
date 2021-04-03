using System;
using System.Collections.Generic;
using System.Text;


namespace CleanArchitectureCosmosDB.Infrastructure.Identity
{
    /// <summary>
    ///     Config settings for token service provider. 
    ///     E.g., application using ASP.NET Core Identity, Identity Server, etc..
    /// </summary>
    public class TokenServiceProvider
    {
        public string Authority { get; set; }
        public string SetPasswordPath { get; set; }
        public string ResetPasswordPath { get; set; }
    }
}
