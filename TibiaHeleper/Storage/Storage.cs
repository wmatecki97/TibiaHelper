using Microsoft.Win32;
using System;
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
        public const string THVersion = "1.0.0";



        private static void Save(object toSave, string extension = defaultExtension)
        {
            VersionedObject obj = new VersionedObject(toSave);

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
                formatter.Serialize(stream, obj);
                stream.Close();
            }
        }
        private static object Load(string extension = defaultExtension)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*." + extension;
            ofd.DefaultExt = extension;
            ofd.Filter = "tibia helper files (*." + extension + ")|*." + extension;
            VersionedObject obj = null;

            if (ofd.ShowDialog() == true)
            {
                string filename = Path.GetFullPath(ofd.FileName);

                // Restore from file
                var formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(filename);
                obj = formatter.Deserialize(stream) as VersionedObject;
                stream.Close();
                checkVersion(obj);
            }

            return obj.obj;
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
            return (List<WalkerStatement>)Load(procedureExtension);
        }

        public static void checkVersion(VersionedObject obj)
        {
            if (obj.version == "0.0.0")
            {
                try
                {
                    DataCollecter data = (DataCollecter)obj.obj;
                }
                catch (Exception)
                {

                }
            }
        }

        public static object Copy(object obj)
        {
            object result;
            string filename = "TemporaryTibiaHelperFile";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            FileStream stream = File.Create(filename);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Close();
            stream = File.OpenRead(filename);
            result = formatter.Deserialize(stream);
            stream.Close();

            File.Delete(filename);

            return result;
        }

    }
}
