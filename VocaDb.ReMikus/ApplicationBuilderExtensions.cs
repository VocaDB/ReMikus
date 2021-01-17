using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace VocaDb.ReMikus
{
	public static class ApplicationBuilderExtensions
	{
		private static bool ShouldCheckVersion(HttpRequest request) => request.IsXInertia() && request.IsGet() && request.GetXInertiaVersion() != Inertia.VersionSelector();

		private static bool ShouldCheckVersion(HttpContext context) => ShouldCheckVersion(context.Request);

		private static void CheckVersion(IApplicationBuilder app) => app.Run(context =>
		{
			var tempData = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>().GetTempData(context);
			if (tempData.Any())
				tempData.Keep();

			var (request, response) = (context.Request, context.Response);
			response.StatusCode = (int)HttpStatusCode.Conflict;
			response.Headers[InertiaHeaderNames.XInertiaLocation] = Uri.UnescapeDataString(request.GetEncodedPathAndQuery());

			return response.CompleteAsync();
		});

		private static bool ShouldChangeRedirectCode(HttpRequest request) => request.IsXInertia() && (request.IsPut() || request.IsPatch() || request.IsDelete());

		private static bool ShouldChangeRedirectCode(HttpContext context) => ShouldChangeRedirectCode(context.Request);

		private static void ChangeRedirectCode(IApplicationBuilder app) => app.Use(async (context, next) =>
		{
			var (request, response) = (context.Request, context.Response);
			response.OnStarting(() =>
			{
				if ((HttpStatusCode)response.StatusCode == HttpStatusCode.Found)
					response.StatusCode = (int)HttpStatusCode.SeeOther;

				return Task.CompletedTask;
			});

			await next();
		});

		public static IApplicationBuilder UseInertia(this IApplicationBuilder app) => app
			.UseWhen(predicate: ShouldCheckVersion, configuration: CheckVersion)
			.UseWhen(predicate: ShouldChangeRedirectCode, configuration: ChangeRedirectCode);
	}
}
