using System;

namespace VocaDb.ReMikus
{
	public sealed class LazyProp
	{
		private readonly Action _callback;

		public LazyProp(Action callback)
		{
			_callback = callback;
		}

		public void Invoke() => _callback();
	}
}
