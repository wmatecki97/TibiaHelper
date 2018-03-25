using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TibiaHeleper.Modules.WalkerModule;

namespace TibiaHeleper.Storage
{
    class Storage
    {
        const string procedureExtension = "thp";
        const string defaultExtension = "th";

        private static void Save(object toSave, string extension = defaultExtension)
        {

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "*." + extension;
            sfd.DefaultExt = extension;
            sfd.Filter = "tibia helper files (*." + extension + ")|*." + extension;

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
                formatter.Serialize(stream, toSave);
                stream.Close();
            }
        }
        private static object Load(string extension = defaultExtension)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*." + extension;
            ofd.DefaultExt = extension;
            ofd.Filter = "tibia helper files (*."+extension+")|*."+extension;
            object result=null;

            if (ofd.ShowDialog() == true)
            {
                string filename = Path.GetFullPath(ofd.FileName);

                // Restore from file
                var formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(filename);
                result = formatter.Deserialize(stream);
                stream.Close();
            }
            return result;
        }

        public static void SaveAllModules()
        {
            DataCollecter collecter = new DataCollecter();
            Save(collecter);
        }
        public static void LoadAllModules()
        {
            DataCollecter collecter;
            collecter = (DataCollecter)Load();
            collecter.activateLoadedSettings();
        }

        public static void SaveProcedure(List<WalkerStatement> list)
        {
            Save(list, procedureExtension);
        }
        public static List<WalkerStatement> LoadProcedure()
        {
            return (List<WalkerStatement>) Load(procedureExtension);
        }

    }
}
