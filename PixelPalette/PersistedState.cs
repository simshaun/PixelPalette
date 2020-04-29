using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace PixelPalette
{
    public class PersistedData
    {
        [XmlElement("SelectedColorModelTabIndex")]
        public int SelectedColorModelTabIndex { get; set; } = 0;
    }

    public static class PersistedState
    {
        public static PersistedData Data;
        private static readonly XmlSerializer Serializer;

        static PersistedState()
        {
            Data = new PersistedData();
            Serializer = new System.Xml.Serialization.XmlSerializer(Data.GetType());
            Read();
        }

        public static void Read()
        {
            if (!File.Exists(SettingFilePath()))
            {
                return;
            }

            var sr = new StringReader(File.ReadAllText(SettingFilePath()));
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
            File.WriteAllText(SettingFilePath(), sw.ToString());
        }

        private static string SettingFilePath()
        {
            return Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");
        }
    }
}