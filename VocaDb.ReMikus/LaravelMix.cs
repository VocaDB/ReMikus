using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace VocaDb.ReMikus
{
	public sealed class LaravelMix
	{
		private readonly Dictionary<string, string> _manifest;

		public string ManifestPath { get; }

		public LaravelMix(IWebHostEnvironment webHostEnvironment)
		{
			ManifestPath = Path.Combine(webHostEnvironment.WebRootPath, "mix-manifest.json");
			_manifest = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ManifestPath));
		}

		/// <summary>
		/// Gets the path to a <see href="https://laravel.com/docs/8.x/mix">versioned Mix file</see>.
		/// </summary>
		/// <param name="path">The file path.</param>
		/// <returns>The path to a versioned Mix file.</returns>
		public string GetVersionedPath(string path) => _manifest.GetValueOrDefault(path) ?? throw new KeyNotFoundException($"Unable to locate Mix file: {path}");
	}
}
