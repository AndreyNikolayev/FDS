using SharedEntities.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedEntities.Utils
{
    /// <summary>
    /// Utility methods for working with Application Config
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// Gets value of app config key.
        /// </summary>
        /// <param name="key">appConfig key</param>
        /// <param name="isClientSide"></param>
        /// <returns>appConfig value</returns>
        public static string GetAppConfigValue(string key, bool isClientSide = false)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (!String.IsNullOrEmpty(appSetting))
            {
                return appSetting;
            }

            var configFilePath = FindConfigFile(isClientSide ?  ConfigFiles.CONFIG_CLIENT_FILE_NAME : ConfigFiles.CONFIG_FILE_NAME);
            var config = GetConfigurationFromFile(configFilePath);
            var appSettingsSection = config.AppSettings.Settings[key];
            if (appSettingsSection != null) appSetting = appSettingsSection.Value;

            return appSetting;
        }

        /// <summary>
        /// Gets value of app config key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">appConfig key</param>
        /// <param name="defaultValue">default value</param>
        /// <param name="isClientSide">is clientside config</param>
        /// <returns></returns>
        public static T GetAppConfigValue<T>(string key, T defaultValue, bool isClientSide = false) where T : IConvertible
        {
            var returnValue = defaultValue;

            var appSetting = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(appSetting))
            {
                return (T)Convert.ChangeType(appSetting, typeof(T));
            }

            //Try to get app config value from lsmbc config
            var externalConfigValue = GetAppConfigValue(key, isClientSide);

            if (!string.IsNullOrEmpty(externalConfigValue))
            {
                return (T)Convert.ChangeType(externalConfigValue, typeof(T));
            }

            return returnValue;
        }

        /// <summary>
        /// Gets connection string by key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>connection string</returns>
        public static string GetConnectionString(string key)
        {
            var connectionString = String.Empty;

            var connectionStringSection = ConfigurationManager.ConnectionStrings[key];
            if (connectionStringSection != null)
            {
                connectionString = connectionStringSection.ConnectionString;
            }
            else
            {
                var configFilePath = FindConfigFile(ConfigFiles.CONFIG_FILE_NAME);
                var config = GetConfigurationFromFile(configFilePath);
                connectionStringSection = config.ConnectionStrings.ConnectionStrings[key];
                if (connectionStringSection != null) connectionString = connectionStringSection.ConnectionString;
            }
            return connectionString;
        }

        /// <summary>
        /// Get configuration object from specified config file.
        /// </summary>
        /// <param name="filePath">Config file path.</param>
        /// <returns>Configuration object</returns>
        private static Configuration GetConfigurationFromFile(string filePath)
        {
            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            return ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Finds the configuration file (from application folder up to root).
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>full path to configuration file</returns>
        public static string FindConfigFile(string fileName)
        {
            var applicationCodeBase = Assembly.GetExecutingAssembly().CodeBase;

            var applicationUri = new UriBuilder(applicationCodeBase);

            var path = Path.GetDirectoryName(Uri.UnescapeDataString(applicationUri.Path));

            var pathToFile = String.Empty;
            if (path == null)
            {
                return pathToFile;
            }

            pathToFile = Path.Combine(path, fileName);

            var root = Directory.GetDirectoryRoot(path);

            while (!File.Exists(pathToFile) && path != root)
            {
                path = Directory.GetParent(path).FullName;
                pathToFile = Path.Combine(path, fileName);
            }
            return path == root && !File.Exists(pathToFile) ? String.Empty : pathToFile;
        }

        /// <summary>
        /// Return path to folder where current assembly is running
        /// (Ignores "bin" folder)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentApplicationPath()
        {
            var returnResult = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").TrimEnd(new char[] { '/', '\\' })); ;
            if (returnResult.EndsWith("bin", StringComparison.CurrentCultureIgnoreCase))
            {
                returnResult = returnResult.Substring(0, returnResult.Length - 3);
            }
            return returnResult;
        }
    }
}
