using SharedEntities.Constants;
using SharedEntities.Entities.DataEntities;
using SharedEntities.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedEntities.Handlers
{
    public class PluginsHandler
    {
        private static PluginsHandler instance;

        public static PluginsHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PluginsHandler();
                }
                return instance;
            }
        }

        private List<Assembly> PluginAssemblies = new List<Assembly>();
        private Dictionary<Plugin, List<MethodInfo>> PluginWithCommands = new Dictionary<Plugin, List<MethodInfo>>();

        private PluginsHandler()
        {
            InitializeHandler();
        }

        public IEnumerable<string> GetCommandNamesByPluginName(string pluginName)
        {
            return PluginWithCommands.FirstOrDefault(p => p.Key.PluginName == pluginName).Value.Select(p => p.Name);
        }

        public Plugin GetPluginByPluginName(string pluginName)
        {
            return PluginWithCommands.FirstOrDefault(p => p.Key.PluginName == pluginName).Key;
        }

        public MethodInfo GetCurrentCommand(string pluginName, string commandName)
        {
            return PluginWithCommands.FirstOrDefault(p => p.Key.PluginName == pluginName).Value.First(p => p.Name == commandName);
        }

        public void InitializeHandler()
        {
            var pluginDirectory = ConfigUtils.GetAppConfigValue(ConfigFiles.IS_SERVER_KEY) =="True"?
                ConfigUtils.GetAppConfigValue(ConfigFiles.EXECUTABLE_PLUGINS_PATH):
                ConfigUtils.GetAppConfigValue(ConfigFiles.EXECUTABLE_PLUGINS_PATH, true);
            if (String.IsNullOrEmpty(pluginDirectory))
            {
                pluginDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigFiles.PLUGIN_FOLDER);
            }
            if (!Directory.Exists(pluginDirectory))
            {
                Directory.CreateDirectory(pluginDirectory);
            }
            else
            {
                var assemblyFiles = new DirectoryInfo(pluginDirectory).GetFiles().Where(p => p.Extension.ToLower() == ".dll");
                foreach (var file in assemblyFiles)
                {
                    var assembly = Assembly.Load(AssemblyName.GetAssemblyName(file.FullName));
                    PluginAssemblies.Add(assembly);
                    var libraryType = assembly.GetTypes().First(p => p.Name == PluginConstants.PLUGIN_LIBRARY);
                    var plugin = new Plugin
                    {
                        PluginGuid = (Guid)libraryType.GetProperty(PluginConstants.PLUGIN_GUID).GetValue(null, null),
                        PluginName = (string)libraryType.GetProperty(PluginConstants.PLUGIN_NAME).GetValue(null, null),
                        PluginReadableName = (string)libraryType.GetProperty(PluginConstants.PLUGIN_READABLE_NAME).GetValue(null, null),
                        AssemblyName = file.Name,
                        AssemblyBytes = File.ReadAllBytes(Path.Combine(file.DirectoryName, file.FullName))
                    };
                    var pluginCommands = libraryType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => !m.IsSpecialName).ToList();
                    PluginWithCommands.Add(plugin, pluginCommands);
                }
            }
        }
    }
}
