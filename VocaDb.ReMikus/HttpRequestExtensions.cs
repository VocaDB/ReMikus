using System;
using Microsoft.AspNetCore.Http;

namespace VocaDb.ReMikus
{
	internal static class HttpRequestExtensions
	{
		public static bool IsXInertia(this HttpRequest request) => string.Equals(request.Headers[InertiaHeaderNames.XInertia], "true", StringComparison.OrdinalIgnoreCase);

		public static bool IsDelete(this HttpRequest request) => string.Equals(request.Method, "DELETE", StringComparison.OrdinalIgnoreCase);

		public static bool IsGet(this HttpRequest request) => string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase);

		public static bool IsPatch(this HttpRequest request) => string.Equals(request.Method, "PATCH", StringComparison.OrdinalIgnoreCase);

		public static bool IsPut(this HttpRequest request) => string.Equals(request.Method, "PUT", StringComparison.OrdinalIgnoreCase);

		public static string GetXInertiaVersion(this HttpRequest request) => request.Headers[InertiaHeaderNames.XInertiaVersion].ToString()/* required */;
	}
}
