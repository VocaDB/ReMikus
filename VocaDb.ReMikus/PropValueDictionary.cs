using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace VocaDb.ReMikus
{
	// HACK
	internal sealed class PropValueDictionary : IDictionary<string, object?>, IReadOnlyDictionary<string, object?>
	{
		private readonly RouteValueDictionary _routeValueDictionary;

		public PropValueDictionary()
		{
			_routeValueDictionary = new();
		}

		public PropValueDictionary(object? values)
		{
			_routeValueDictionary = new(values);
		}

		public object? this[string key]
		{
			get => _routeValueDictionary[key];
			set => _routeValueDictionary[key] = value;
		}

		public int Count => _routeValueDictionary.Count;

		bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => ((ICollection<KeyValuePair<string, object?>>)_routeValueDictionary).IsReadOnly;

		public ICollection<string> Keys => _routeValueDictionary.Keys;

		IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => ((IReadOnlyDictionary<string, object?>)_routeValueDictionary).Keys;

		public ICollection<object?> Values => _routeValueDictionary.Values;

		IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => ((IReadOnlyDictionary<string, object?>)_routeValueDictionary).Values;

		void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) => ((ICollection<KeyValuePair<string, object?>>)_routeValueDictionary).Add(item);

		public void Add(string key, object? value) => _routeValueDictionary.Add(key, value);

		public void Clear() => _routeValueDictionary.Clear();

		bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) => ((ICollection<KeyValuePair<string, object?>>)_routeValueDictionary).Contains(item);

		public bool ContainsKey(string key) => _routeValueDictionary.ContainsKey(key);

		void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, object?>>)_routeValueDictionary).CopyTo(array, arrayIndex);

		public IEnumerator GetEnumerator() => _routeValueDictionary.GetEnumerator();

		IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator() => ((IEnumerable<KeyValuePair<string, object?>>)_routeValueDictionary).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_routeValueDictionary).GetEnumerator();

		bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) => ((ICollection<KeyValuePair<string, object?>>)_routeValueDictionary).Remove(item);

		public bool Remove(string key) => _routeValueDictionary.Remove(key);

		public bool TryGetValue(string key, out object? value) => _routeValueDictionary.TryGetValue(key, out value);
	}
}
