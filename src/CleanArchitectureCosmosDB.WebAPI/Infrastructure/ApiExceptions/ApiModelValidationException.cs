using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitectureCosmosDB.WebAPI.Infrastructure.ApiExceptions
{
    /// <summary>
    ///     Api validation exception
    /// </summary>
    public class ApiModelValidationException : Exception
    {
        /// <summary>
        ///     Validation errors
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }

        /// <summary>
        ///     ctor
        /// </summary>
        public ApiModelValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        ///     ctor 
        /// </summary>
        /// <param name="failures"></param>
        public ApiModelValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

    }
}
