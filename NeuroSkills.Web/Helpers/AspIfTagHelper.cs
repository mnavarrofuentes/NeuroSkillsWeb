using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NeuroSkills.Web.Helpers;

[HtmlTargetElement(Attributes = "asp-if")]
public class AspIfTagHelper : TagHelper
{
    [HtmlAttributeName("asp-if")]
    public bool Flag { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Flag)
            output.SuppressOutput();
    }
}
