using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HC.Email
{
    /// <summary>
    ///     配置文件辅助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        ///     获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        public static string GetConfigPath<T>()
        {
            string path = Directory.GetCurrentDirectory() + "\\";

            //如果是集合对象
            path = CheckIsList<T>(path);
            if (path == Directory.GetCurrentDirectory() + "\\")
            {
                path = path + typeof (T).Name + ".config";
            }
            return path;
        }

        /// <summary>
        ///     更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <param name="config">配置信息</param>
        public static void UpdateConfig<T>(T config)
        {
            Type configClassType = typeof (T);
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
            try
            {
                var xmlSerializer = new XmlSerializer(configClassType);
                using (var xmlTextWriter = new XmlTextWriter(configFilePath, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    var xmlNamespace = new XmlSerializerNamespaces();
                    xmlNamespace.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlTextWriter, config, xmlNamespace);
                }
            }
            catch (SecurityException ex)
            {
                throw new SecurityException(ex.Message, ex.DenySetInstance, ex.PermitOnlySetInstance, ex.Method,
                                            ex.Demanded, ex.FirstPermissionThatFailed);
            }
        }


        /// <summary>
        ///     获取配置信息
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        public static T GetConfig<T>() where T : class, new()
        {
            var configObject = new object();
            Type configClassType = typeof (T);
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
            if (File.Exists(configFilePath))
            {
                using (var xmlTextReader = new XmlTextReader(configFilePath))
                {
                    var xmlSerializer = new XmlSerializer(configClassType);
                    configObject = xmlSerializer.Deserialize(xmlTextReader);
                }
            }
            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
        }


        private static string CheckIsList<T>(string path)
        {
            if (
                typeof (T).GetInterfaces()
                          .Any(
                              interfaceType =>
                              interfaceType.IsGenericType &&
                              interfaceType.GetGenericTypeDefinition() == typeof (IList<>)))
            {
                Type itemType = typeof (T).GetGenericArguments()[0];
                path = path + itemType.Name + ".config";
            }
            return path;
        }
    }
}