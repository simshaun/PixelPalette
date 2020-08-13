using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace PixelPalette
{
    public class PersistedData
    {
        [XmlElement("ActiveColorModelTab")] public string ActiveColorModelTab { get; set; } = null;

        [XmlElement("ActiveColorValue")] public string ActiveColorValue { get; set; } = null;
    }

    public static class PersistedState
    {
        public static PersistedData Data;
        private static readonly XmlSerializer Serializer;

        static PersistedState()
        {
            Data = new PersistedData();
            Serializer = new XmlSerializer(Data.GetType());
            Read();
        }

        public static void Read()
        {
            var settingFilePath = SettingFilePath();

            AppDebug.WriteLine($"Setting file: {settingFilePath}");
            if (!File.Exists(settingFilePath))
            {
                AppDebug.WriteLine($"File does not exist");
                return;
            }
            AppDebug.WriteLine($"File exists");
            AppDebug.WriteLine($"--------");
            AppDebug.WriteLine(File.ReadAllText(settingFilePath));
            AppDebug.WriteLine($"--------");

            var sr = new StringReader(File.ReadAllText(settingFilePath));
            Data = (PersistedData) Serializer.Deserialize(sr);
        }

        public static void Save()
        {
            var sw = new StringWriter();
            var xw = new XmlTextWriter(sw)
            {
                Formatting = Formatting.Indented
            };
            Serializer.Serialize(xw, Data);
            try
            {
                var path = SettingFilePath();
                new FileInfo(path).Directory?.Create();
                File.WriteAllText(SettingFilePath(), sw.ToString());
            }
            catch (Exception)
            {
                // Oh well
            }
        }

        private static string SettingFilePath()
        {
            string path;
            if (CanWriteToExeFolder())
            {
                path = Path.Join(ExeFolder(), "settings.xml");
            }
            else
            {
                var folderPath = Path.Join(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "PixelPalette"
                );
                path = Path.Join(folderPath, "settings.xml");
            }

            return path;
        }

        // TODO ADD DEBUG FLAG TO STARTUP

        private static bool CanWriteToExeFolder()
        {
            return !File.Exists(Path.Join(ExeFolder(), "IS_INSTALLATION.do-not-delete"));
        }

        private static string ExeFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}