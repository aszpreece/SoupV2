using Microsoft.Xna.Framework;

namespace SoupV2.Simulation.Settings { 
    public enum FoodSpawnStyle
    {
        Singular,
        Cluster
    }

    public class FoodTypeSetting : AbstractEntityTypeSetting
    {
        // TODO
        /// <summary>
        /// If food spawn style set to cluster, this controls the size of the cluster that is spawned
        /// </summary>
        public int ClusterSize { get; set; } = 1;
        public float ClusterRadius { get; set; } = 50;
    }
}