namespace MovieCatalog.Web.Infrastructure.TagHelpersExtensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class RequiredTagHelper : TagHelper
    {
        public override int Order
        {
            get { return int.MaxValue; }
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (context.AllAttributes["required"] == null)
            {
                bool isRequired = For.ModelExplorer
                    .Metadata.ValidatorMetadata.Any(a => a is RequiredAttribute);

                if (isRequired)
                {
                    TagHelperAttribute requiredAttribute = new TagHelperAttribute("required");
                    output.Attributes.Add(requiredAttribute);
                }
            }
        }
    }
}
