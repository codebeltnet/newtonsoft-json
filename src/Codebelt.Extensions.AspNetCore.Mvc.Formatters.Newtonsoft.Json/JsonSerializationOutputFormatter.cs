using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.AspNetCore.Mvc.Formatters;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// This class handles serialization of objects to JSON using <see cref="NewtonsoftJsonFormatter"/>.
    /// </summary>
    public class JsonSerializationOutputFormatter : StreamOutputFormatter<NewtonsoftJsonFormatter, NewtonsoftJsonFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationOutputFormatter"/> class.
        /// </summary>
        /// <param name="options">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        public JsonSerializationOutputFormatter(NewtonsoftJsonFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
            foreach (var mediaType in options.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(mediaType.ToString());
            }
        }
    }
}
