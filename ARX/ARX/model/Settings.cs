using System;
using System.IO;
using System.Xml.Serialization;

namespace ARX.model
{
    [Serializable]
    public class Settings
    {
        public string Pseudo { get; set; } = "Pseudo1";
        public string ProfileImagePath { get; set; } = "view/Images/Character.png";

        private static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ARXSettings.xml");

        public static Settings Load()
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (Settings)serializer.Deserialize(fs);
                }
            }
            return new Settings();
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }

        public static Uri ToAbsoluteUri(string relativeOrAbsolutePath)
        {
            if (string.IsNullOrEmpty(relativeOrAbsolutePath))
            {
                throw new ArgumentNullException(nameof(relativeOrAbsolutePath));
            }

            if (Uri.IsWellFormedUriString(relativeOrAbsolutePath, UriKind.Absolute))
            {
                return new Uri(relativeOrAbsolutePath);
            }
            else
            {
                string absolutePath = Path.GetFullPath(relativeOrAbsolutePath);
                return new Uri(absolutePath);
            }
        }
    }
}
