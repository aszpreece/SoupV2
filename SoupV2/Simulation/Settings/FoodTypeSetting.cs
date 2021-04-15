namespace SoupV2.Simulation.Settings { 
    public enum FoodSpawnStyle
    {
        Singular,
        Cluster
    }

    public class FoodTypeSetting : AbstractEntityTypeSetting
    {
        public FoodSpawnStyle foodSpawnStyle  { get; set; }

        // TODO
        /// <summary>
        /// If food spawn style set to cluster, this controls the size of the cluster that is spawned
        /// </summary>
        public int ClusterSize { get; set; }
    }
}