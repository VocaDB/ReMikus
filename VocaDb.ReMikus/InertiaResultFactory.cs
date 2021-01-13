using System;
using Microsoft.AspNetCore.Mvc;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResultFactory
	{
		public string? RootView { get; set; }
		public Func<string?>? Version { get; set; }

		public void Share<T>(string? key, T? value = default) => throw new NotImplementedException();

		public T? GetShared<T>(string? key = null) => throw new NotImplementedException();

		public LazyProp? Lazy(Action? callback) => throw new NotImplementedException();

		public InertiaResult Render(string? component, object? props = null) => new(component, props, RootView, Version?.Invoke());

		public IActionResult? Location(string? url) => throw new NotImplementedException();
	}
}
