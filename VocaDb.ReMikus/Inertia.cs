using System;
using System.Collections.Generic;

namespace VocaDb.ReMikus
{
	public static class Inertia
	{
		private static readonly InertiaResultFactory s_factory = new();

		public static IReadOnlyDictionary<string, object> SharedProps
		{
			get => s_factory.SharedProps;
			set => s_factory.SharedProps = value;
		}

		public static string? RootView
		{
			get => s_factory.RootView;
			set => s_factory.RootView = value;
		}

		public static Func<string> VersionSelector
		{
			get => s_factory.VersionSelector;
			set => s_factory.VersionSelector = value;
		}

		public static LazyProp? Lazy(Action? callback) => s_factory.Lazy(callback);

		public static InertiaResult Render(string? component, IReadOnlyDictionary<string, object> props) => s_factory.Render(component, props);
	}
}
