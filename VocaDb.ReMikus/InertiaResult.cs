using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Net.Http.Headers;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResult : IActionResult
	{
		private sealed record Page(string? Component, object? Props, string Url, string? Version);

		public static readonly string DefaultRootView = "Views/App.cshtml";

		private readonly string? _component;
		private readonly object? _props;
		private readonly string _rootView;
		private readonly string? _version;

		public InertiaResult(string? component, object? props, string? rootView = null, string? version = null)
		{
			_component = component;
			_props = props;
			_rootView = rootView ?? DefaultRootView;
			_version = version;
		}

		public Task ExecuteResultAsync(ActionContext context)
		{
			var request = context.HttpContext.Request;
			var page = new Page(_component, _props, Url: request.GetEncodedPathAndQuery(), _version);

			if (request.Headers[InertiaHeaderNames.XInertia] == "true")
			{
				context.HttpContext.Response.Headers[HeaderNames.Vary] = "Accept";
				context.HttpContext.Response.Headers[InertiaHeaderNames.XInertia] = "true";
				return new JsonResult(page).ExecuteResultAsync(context);
			}

			var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
			{
				Model = page,
			};

			return new ViewResult
			{
				ViewName = _rootView,
				ViewData = viewData,
			}.ExecuteResultAsync(context);
		}
	}
}
