using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VocaDb.ReMikus.TagHelpers
{
	[HtmlTargetElement("inertia", TagStructure = TagStructure.WithoutEndTag)]
	public sealed class InertiaTagHelper : TagHelper
	{
		private static readonly JsonSerializerSettings s_settings = new()
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		};

		public object? Model { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			output.Attributes.SetAttribute("id", "app");
			output.Attributes.SetAttribute("data-page", JsonConvert.SerializeObject(Model, s_settings));
			output.TagMode = TagMode.StartTagAndEndTag;
		}
	}
}
