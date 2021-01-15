using System.Collections.Generic;
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
		private sealed record Page(string? Component, IReadOnlyDictionary<string, object> Props, string Url, string Version);

		public static readonly string DefaultRootView = "Views/App.cshtml";

		private readonly string? _component;
		private readonly IReadOnlyDictionary<string, object> _props;
		private readonly string _rootView;
		private readonly string _version;

		public InertiaResult(string? component, IReadOnlyDictionary<string, object> props, string? rootView, string version)
		{
			_component = component;
			_props = props;
			_rootView = rootView ?? DefaultRootView;
			_version = version ?? string.Empty;
		}

		public InertiaResult(string? component, IReadOnlyDictionary<string, object> props, string? rootView) : this(component, props, rootView, version: string.Empty) { }

		public InertiaResult(string? component, IReadOnlyDictionary<string, object> props) : this(component, props, rootView: null) { }

		public Task ExecuteResultAsync(ActionContext context)
		{
			var (request, response) = (context.HttpContext.Request, context.HttpContext.Response);
			var page = new Page(_component, _props, Url: request.GetEncodedPathAndQuery(), _version);

			if (request.IsXInertia())
			{
				response.Headers[HeaderNames.Vary] = "Accept";
				response.Headers[InertiaHeaderNames.XInertia] = "true";
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
