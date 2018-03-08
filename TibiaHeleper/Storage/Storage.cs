using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TibiaHeleper.Storage
{
    class Storage
    {
        private static DataCollecter collecter;

        public static void Save()
        {
            collecter = new DataCollecter();

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "*.th";
            sfd.DefaultExt = "th";
            sfd.Filter = "tibia helper files (*.th)|*.th";

            if (sfd.ShowDialog() == true)
            {
                string filename = Path.GetFullPath(sfd.FileName);

                // Delete old file, if it exists
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                // Persist to file
                FileStream stream = File.Create(filename);
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, collecter);
                stream.Close();
            }
        }

        public static void Load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*.th";
            ofd.DefaultExt = "th";
            ofd.Filter = "tibia helper files (*.th)|*.th";

            if (ofd.ShowDialog() == true)
            {
                string filename = Path.GetFullPath(ofd.FileName);

                // Restore from file
                var formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(filename);
                collecter = (DataCollecter)formatter.Deserialize(stream);
                stream.Close();
                collecter.activateLoadedSettings();
            }
        }
    }
}
