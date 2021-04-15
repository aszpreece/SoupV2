using SoupV2.Simulation.Events.DeathCause;

namespace SoupV2.Simulation.Systems
{
    internal class OldAgeDeathCause : AbstractDeathCause
    {
        public string Cause { get; } = "Old Age";

        public override string ToString()
        {
            return Cause;
        }
    }
}