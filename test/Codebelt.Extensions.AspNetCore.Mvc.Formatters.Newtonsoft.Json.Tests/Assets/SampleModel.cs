using System.ComponentModel.DataAnnotations;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets
{
    public class SampleModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
