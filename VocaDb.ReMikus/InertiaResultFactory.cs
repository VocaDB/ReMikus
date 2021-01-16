using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VocaDb.ReMikus
{
	public sealed class InertiaResultFactory
	{
		private static readonly IReadOnlyDictionary<string, object> s_emptyProps = ImmutableDictionary<string, object>.Empty;

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

		public InertiaResult Render(string? component) => Render(component, s_emptyProps);

		public InertiaResult Render(IReadOnlyDictionary<string, object> props) => Render(component: null, props);

		public InertiaResult Render() => Render(s_emptyProps);
	}
}
