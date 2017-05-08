using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Chensoft.Plugins
{
    public class PluginUtils
    {
        #region 异步方法
        public static async Task<T> GetAsync<T>(string fileName) where T : new()
        {
            if (File.Exists(fileName))
            {
                using (var sr = File.OpenText(fileName))
                {
                    var content = await sr.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<T>(content);

                }
            }
            return default(T);
        }

        public static async Task SaveAsync<T>(string fileName, T option)
        {
            using (var fs = File.OpenWrite(fileName))
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, option);
                var json = textWriter.ToString();

                var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }

        }

        #endregion

        #region 同步方法
        public static T Get<T>(string fileName) where T : new()
        {
            if (File.Exists(fileName))
            {
                using (var sr = File.OpenText(fileName))
                {
                    var content = sr.ReadToEnd();

                    return JsonConvert.DeserializeObject<T>(content);

                }
            }
            return default(T);
        }

        public static void Save<T>(string fileName, T option)
        {
            using (var fs = File.OpenWrite(fileName))
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, option);
                var json = textWriter.ToString();

                var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                fs.Write(bytes, 0, bytes.Length);
            }

        }

        #endregion

        public static void AppendPrivatePath(string path)
        {
            path = path.Replace("\\", "/");
            AppDomain.CurrentDomain.AppendPrivatePath(path);
        }

        #region Assembly 方法

        internal static Assembly GetAssemblyFormFeature(AssemblyName assemblyName, PluginFeature feature)
        {
            var fileName = Directory.GetFiles(feature.FullPath, assemblyName.Name + ".dll", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (string.IsNullOrEmpty(fileName)) return null;
            var assembly = Assembly.LoadFile(fileName);
            bool matched = string.Equals(assembly.GetName().Name, assemblyName.Name, StringComparison.OrdinalIgnoreCase);
            byte[] token = assemblyName.GetPublicKeyToken();
            if (token != null && token.Length > 0)
                matched &= CompareBytes(token, assembly.GetName().GetPublicKeyToken());

            if (matched) return assembly;
            return null;

        }
        internal static Assembly GetAssemblyFormFeature(string name, PluginFeature feature)
        {
            if (!name.ToLower().EndsWith(".dll")) name += ".dll";
            var file = Directory.GetFiles(feature.FullPath, name, SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (string.IsNullOrEmpty(file)) return null;
            return Assembly.LoadFile(file);
        }
        internal static Assembly GetAssemblyFormPath(string name, string path)
        {
            if (!name.ToLower().EndsWith(".dll")) name += ".dll";
            var file = Directory.GetFiles(path, name, SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (string.IsNullOrEmpty(file)) return null;
            return Assembly.LoadFile(file);
        }
        #endregion

        #region Type 方法
        private static bool CompareBytes(byte[] a, byte[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        internal static Assembly ResolveAssembly(AssemblyName assemblyName, ApplicationContext context)
        {

            if (assemblyName == null) return null;

            byte[] token = assemblyName.GetPublicKeyToken();
            IList<Assembly> assemblies = new List<Assembly>();
            foreach (var feature in context.Features)
            {
                var assembly = GetAssemblyFormFeature(assemblyName, feature);
                if (assembly != null) assemblies.Add(assembly);
            }

            if (assemblies.Count < 1) return null;

            if (assemblies.Count == 1) return assemblies[0];

            Assembly maxAssembly = assemblies[0];

            foreach (Assembly assembly in assemblies)
            {
                if (assembly.GetName().Version == null)
                    continue;

                if (assembly.GetName().Version.CompareTo(maxAssembly.GetName().Version) > 0)
                    maxAssembly = assembly;
            }

            return maxAssembly;
        }
        public static Type GetType(Builtin builtin, ApplicationContext context)
        {
            if (builtin == null) return null;

            Type type = Type.GetType(builtin.Type);
            if (type != null) return type;

            type = builtin.GetBuiltinType();
            if (type != null) return type;

            type = Type.GetType(builtin.Type, assemblyName =>
            {
                Assembly assembly = ResolveAssembly(assemblyName, context);
                return assembly;
            }, (assembly, typeName, ignoreCase) =>
            {
                if (assembly == null)
                    return Type.GetType(typeName, false, ignoreCase);
                else
                    return assembly.GetType(typeName, false, ignoreCase);
            }, false);

            if (type == null)
                throw new PluginException(string.Format("The '{0}' type resolve failed.", builtin.Type));

            return type;
        }

        #endregion
    }
}
