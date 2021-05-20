using Microsoft.Xna.Framework;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation.EntityDefinitions;
using System.Collections.Generic;

namespace SoupV2.Simulation
{
    public class EntityDefinitionDatabase
    {

        public static List<string> DefaultEntities = new List<string>
        {
            "DefaultEye", 
            "DefaultFood",
            "DefaultMouth",
            "DefaultNose",
            "DefaultWeapon",
            "Soupling",
        };

        private Dictionary<string, EntityDefinition> _definitionDict = new Dictionary<string, EntityDefinition>();

        /// <summary>
        /// Creates a new database and adds in default entities
        /// </summary>
        public EntityDefinitionDatabase()
        {
            AddDefinition("DefaultEye", Eye.GetEye(Color.White).ToDefinition());
            AddDefinition("DefaultFood", FoodPellets.GetFoodPellet(Color.White).ToDefinition());
            AddDefinition("DefaultMouth", Mouth.GetMouth(Color.White).ToDefinition());
            AddDefinition("DefaultNose", Nose.GetNose(Color.White).ToDefinition());
            AddDefinition("DefaultWeapon", Weapon.GetWeapon(Color.White).ToDefinition());
            AddDefinition("Soupling", Soupling.GetCritter(Color.White).ToDefinition());
        }


        /// <summary>
        /// Adds a definition to the database if there is not already one with that id in it.
        /// Throws an exception on duplicate key.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="definition"></param>
        public void AddDefinition(string id, EntityDefinition definition)
        {
            if (_definitionDict.ContainsKey(id))
            {
                throw new System.Exception("Database already contains a definition with this key.");
            }
            _definitionDict[id] = definition;
        }

        /// <summary>
        /// Removed definmition from database.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveDefinition(string id)
        {
            _definitionDict.Remove(id);
        }

        /// <summary>
        /// Returns true if there is an id with this key.
        /// </summary>
        /// <param name="id"></param>
        public bool ContainsId(string id)
        {
            return _definitionDict.ContainsKey(id);
        }

        public EntityDefinition GetEntityDefinition(string id)
        {
            return _definitionDict[id];
        }

    }
}