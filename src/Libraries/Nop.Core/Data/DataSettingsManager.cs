using System;
using System.Collections.Generic;
using System.IO;

namespace Nop.Core.Data
{
    /// <summary>
    /// 数据管理器（连接字符串）
    /// </summary>
    public partial class DataSettingsManager
    {
        protected const char separator = ':';
        protected const string filename = "Settings.txt";

        /// <summary>
        ///解析设置
        /// </summary>
        /// <param name="text">设置文件的文本</param>
        /// <returns>解析数据设置</returns>
        protected virtual DataSettings ParseSettings(string text)
        {
            var shellSettings = new DataSettings();
            if (string.IsNullOrEmpty(text))
                return shellSettings;

            //Old way of file reading. This leads to unexpected behavior when a user's FTP program transfers these files as ASCII (\r\n becomes \n).
            //var settings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var settings = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                    settings.Add(str);
            }

            foreach (var setting in settings)
            {
                var separatorIndex = setting.IndexOf(separator);
                if (separatorIndex == -1)
                {
                    continue;
                }
                string key = setting.Substring(0, separatorIndex).Trim();
                string value = setting.Substring(separatorIndex + 1).Trim();

                switch (key)
                {
                    case "DataProvider":
                        shellSettings.DataProvider = value;
                        break;
                    case "DataConnectionString":
                        shellSettings.DataConnectionString = value;
                        break;
                    default:
                        shellSettings.RawDataSettings.Add(key,value);
                        break;
                }
            }

            return shellSettings;
        }

        /// <summary>
        /// 将数据设置转换为字符串表示形式
        /// </summary>
        /// <param name="settings">设置信息</param>
        /// <returns></returns>
        protected virtual string ComposeSettings(DataSettings settings)
        {
            if (settings == null)
                return "";

            return string.Format("DataProvider: {0}{2}DataConnectionString: {1}{2}",
                                 settings.DataProvider,
                                 settings.DataConnectionString,
                                 Environment.NewLine
                );
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="filePath">文件路径; 传递null以使用默认设置文件路径</param>
        /// <returns></returns>
        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), filename);
            }
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                return ParseSettings(text);
            }
            
            return new DataSettings();
        }

        /// <summary>
        ///将设置保存到文件
        /// </summary>
        /// <param name="settings">设置信息</param>
        public virtual void SaveSettings(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            
            string filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), filename);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }
            }
            
            var text = ComposeSettings(settings);
            File.WriteAllText(filePath, text);
        }
    }
}
