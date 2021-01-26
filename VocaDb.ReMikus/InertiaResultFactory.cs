using System;
using System.Linq;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResultFactory
	{
		public object? SharedProps { get; set; }
		public string? RootView { get; set; }
		public Func<string> VersionSelector { get; set; } = () => string.Empty;

		public InertiaResult Render(string? component, object? props) => new(
			component,
			// Prefer `props` over `SharedProps`.
			props: new PropValueDictionary(SharedProps).Concat(new PropValueDictionary(props))
				.GroupBy(kvp => kvp.Key, (_, kvp) => kvp.Last())
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			RootView,
			version: VersionSelector());

		public InertiaResult Render(string? component) => Render(component, props: null);

		public InertiaResult Render(object? props) => Render(component: null, props);

		public InertiaResult Render() => Render(props: null);
	}
}
