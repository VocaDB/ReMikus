using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Net.Http.Headers;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResult : IActionResult
	{
		private sealed record Page(string? Component, IReadOnlyDictionary<string, object> Props, string Url, string Version);

		private const string ActionNameKey = "action";
		private const string ControllerKey = "controller";

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

		// Code from: https://github.com/dotnet/aspnetcore/blob/b795ac3546eb3e2f47a01a64feb3020794ca33bb/src/Mvc/Mvc.ViewFeatures/src/ViewResultExecutor.cs#L182
		private static string? GetActionName(ActionContext context)
		{
			if (!context.RouteData.Values.TryGetValue(ActionNameKey, out var routeValue))
				return null;

			var actionDescriptor = context.ActionDescriptor;
			string? normalizedValue = null;
			if (actionDescriptor.RouteValues.TryGetValue(ActionNameKey, out var value) && !string.IsNullOrEmpty(value))
				normalizedValue = value;

			var stringRouteValue = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
			if (string.Equals(normalizedValue, stringRouteValue, StringComparison.OrdinalIgnoreCase))
				return normalizedValue;

			return stringRouteValue;
		}

		public Task ExecuteResultAsync(ActionContext context)
		{
			var (request, response) = (context.HttpContext.Request, context.HttpContext.Response);

			var component = _component ?? $"{RazorViewEngine.GetNormalizedRouteValue(context, ControllerKey)}/{GetActionName(context)}";

			var only = request.GetXInertiaPartialData().Split(',', StringSplitOptions.RemoveEmptyEntries);
			var props = (only.Any() && request.GetXInertiaPartialComponent() == component
				? _props.Where(p => only.Contains(p.Key))
				: _props).ToDictionary(kv => kv.Key, kv => kv.Value);

			var page = new Page(component, props, Url: request.GetEncodedPathAndQuery(), _version);

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
