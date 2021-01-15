using System;
using System.Collections.Generic;
using System.Linq;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResultFactory
	{
		public IReadOnlyDictionary<string, object> SharedProps { get; set; } = new Dictionary<string, object>();
		public string? RootView { get; set; }
		public Func<string> VersionSelector { get; set; } = () => string.Empty;

		public LazyProp? Lazy(Action? callback) => throw new NotImplementedException();

		public InertiaResult Render(string? component, IReadOnlyDictionary<string, object> props) => new(
			component,
			// Prefer `props` over `SharedProps`.
			props: SharedProps.Where(kv => !props.ContainsKey(kv.Key)).Concat(props).ToDictionary(kv => kv.Key, kv => kv.Value),
			RootView,
			version: VersionSelector());
	}
}
