using SoupV2.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupForm
{
    public static class LocalStorage
    {
        public static readonly string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string EntityFileExtension = "entityd";

        /// <summary>
        /// Loads the entity definitions present at the given path.
        /// </summary>
        /// <returns></returns>
        public static EntityDefinitionDatabase GetDefinitionDatabase(string path)
        {
            EntityDefinitionDatabase database = new EntityDefinitionDatabase();

            if (!Directory.Exists(path))
            {
                throw new Exception("Entity definition path does not exist");
            }
            foreach (string entityPath in Directory.GetFiles(path, $"*.{EntityFileExtension}"))
            {
                // Definition id is declared as the name of the file.
                string definitionId = Path.GetFileName(entityPath);

                using (StreamReader reader = new(entityPath))
                {
                    string json = reader.ReadToEnd();
                    database.AddDefinition(definitionId, new SoupV2.EntityComponentSystem.EntityDefinition(json));
                }
            }

            return database;
        }

        /// <summary>
        /// Check the specified folder, and create if it doesn't exist.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static string CheckDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
