using System;
using System.Runtime.CompilerServices;
using Cuemon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Codebelt.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not a valid JSON representation as specified in RFC 8259.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The JSON string to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a JSON representation that complies with RFC 8259.
        /// </exception>
        public static void InvalidJsonDocument(this Validator _, string argument, string message = "Value must be a JSON representation that complies with RFC 8259.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            try
            {
                JToken.Parse(argument);
            }
            catch (Exception e)
            {
                throw new ArgumentException(message, paramName, e);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not a valid JSON representation as specified in RFC 8259.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The <see cref="JsonReader"/> to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a JSON representation that complies with RFC 8259.
        /// </exception>
        public static void InvalidJsonDocument(this Validator _, ref JsonReader argument, string message = "Value must be a JSON representation that complies with RFC 8259.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (argument == null) { return; }
            var reader = argument;
            try
            {
                var o = JToken.Load(reader);
                reader = o.CreateReader();
            }
            catch (Exception e)
            {
                throw new ArgumentException(message, paramName, e);
            }
            argument = reader;
        }
    }
}
