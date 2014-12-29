using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Applications
{
	public class AssemblyLoader
	{
		const string baseDirectoryPath = "Pe/bin/dir";
		const string libDirectoryName = "lib";

		public AssemblyLoader()
		{
			AppDomain = AppDomain.CurrentDomain;
			Assembly = Assembly.GetExecutingAssembly();
			BaseDirectoryPath = baseDirectoryPath;
		}

		public AssemblyLoader(AppDomain appDomain, Assembly assembly)
		{
			AppDomain = appDomain;
			Assembly = assembly;
			BaseDirectoryPath = baseDirectoryPath;
		}

		public AssemblyLoader(AppDomain appDomain, Assembly assembly, string baseDirectoryPath)
		{
			AppDomain = appDomain;
			Assembly = assembly;
			BaseDirectoryPath = baseDirectoryPath;
		}

		#region Property

		public AppDomain AppDomain { get; private set; }
		public Assembly Assembly { get; private set; }
		public string BaseDirectoryPath { get; private set; }
		public string LibDirectoryPath { get; private set; }

		#endregion ////////////////////////////////////

		public void AttachmentEvent()
		{
			AttachmentEvent(libDirectoryName);
		}

		public void AttachmentEvent(string libDirName)
		{
			var dirPath = Assembly.Location;
			foreach(var n in Enumerable.Range(0, BaseDirectoryPath.Split('/').Count())) {
				dirPath = Path.GetDirectoryName(dirPath);
			}
			LibDirectoryPath = Path.Combine(dirPath, libDirName);

			AppDomain.AssemblyResolve += EventAssemblyResolve;
		}

		Assembly EventAssemblyResolve(object sender, ResolveEventArgs args)
		{
			var assemblyName = args.Name.Split(',').First();
			var assemblyPath = Path.Combine(LibDirectoryPath, assemblyName + ".dll");
			if(File.Exists(assemblyPath)) {
				var loadAssembly = Assembly.LoadFrom(assemblyPath);
				return loadAssembly;
			}

			return null;
		}
	}
}

