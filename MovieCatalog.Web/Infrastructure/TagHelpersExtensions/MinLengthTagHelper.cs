namespace MovieCatalog.Web.Infrastructure.TagHelpersExtensions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class MinLengthTagHelper : TagHelper
    {
        public override int Order { get; } = int.MaxValue;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            // Process only if 'minlength' attribute is not present already
            if (context.AllAttributes["minlength"] == null)
            {
                // Attempt to check for a MinLength annotation
                int minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);

                if (minLength >= 0)
                {
                    output.Attributes.Add("minlength", minLength);
                }
            }
        }

        private static int GetMinLength(IReadOnlyList<object> validatorMetadata)
        {
            for (var i = 0; i < validatorMetadata.Count; i++)
            {
                if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute &&
                    stringLengthAttribute.MinimumLength > 0)
                {
                    return stringLengthAttribute.MinimumLength;
                }

                if (validatorMetadata[i] is MinLengthAttribute minLengthAttribute &&
                    minLengthAttribute.Length > 0)
                {
                    return minLengthAttribute.Length;
                }
            }

            return 0;
        }
    }
}
