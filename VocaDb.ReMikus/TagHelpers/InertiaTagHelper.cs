using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace VocaDb.ReMikus.TagHelpers
{
	[HtmlTargetElement("inertia", TagStructure = TagStructure.WithoutEndTag)]
	public sealed class InertiaTagHelper : TagHelper
	{
		public object? Model { get; set; }

		[ViewContext]
		public ViewContext ViewContext { get; set; } = default!;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			output.Attributes.SetAttribute("id", "app");
			output.Attributes.SetAttribute("data-page", ViewContext.HttpContext.RequestServices.GetRequiredService<IJsonHelper>().Serialize(Model).ToString()/* required */);
			output.TagMode = TagMode.StartTagAndEndTag;
		}
	}
}
