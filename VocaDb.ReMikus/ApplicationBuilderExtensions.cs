using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace VocaDb.ReMikus
{
	public static class ApplicationBuilderExtensions
	{
		private static bool ShouldCheckVersion(HttpRequest request) => request.IsXInertia() && request.IsGet() && request.GetXInertiaVersion() != Inertia.VersionSelector();

		private static bool ShouldCheckVersion(HttpContext context) => ShouldCheckVersion(context.Request);

		private static void CheckVersion(IApplicationBuilder app) => app.Run(context =>
		{
			var (request, response) = (context.Request, context.Response);
			response.StatusCode = (int)HttpStatusCode.Conflict;
			response.Headers[InertiaHeaderNames.XInertiaLocation] = Uri.UnescapeDataString(request.GetEncodedPathAndQuery());

			return response.CompleteAsync();
		});

		public static IApplicationBuilder UseInertia(this IApplicationBuilder app) => app.UseWhen(predicate: ShouldCheckVersion, configuration: CheckVersion);
	}
}
