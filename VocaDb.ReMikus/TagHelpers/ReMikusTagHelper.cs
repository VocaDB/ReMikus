using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VocaDb.ReMikus.TagHelpers
{
	[HtmlTargetElement("remikus", TagStructure = TagStructure.WithoutEndTag)]
	public sealed class ReMikusTagHelper : TagHelper
	{
		private readonly LaravelMix _laravelMix;

		public string? Path { get; set; }

		public ReMikusTagHelper(LaravelMix laravelMix)
		{
			_laravelMix = laravelMix;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (string.IsNullOrEmpty(Path))
				return;

			if (Path.EndsWith(".css"))
			{
				output.TagName = "link";
				output.Attributes.SetAttribute("rel", "stylesheet");
				output.Attributes.SetAttribute("href", _laravelMix.GetVersionedPath(Path));
				output.TagMode = TagMode.SelfClosing;
				return;
			}

			if (Path.EndsWith(".js"))
			{
				output.TagName = "script";
				output.Attributes.SetAttribute("src", _laravelMix.GetVersionedPath(Path));
				output.TagMode = TagMode.StartTagAndEndTag;
				return;
			}
		}
	}
}
