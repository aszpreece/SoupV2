namespace SoupV2.Simulation.Brain
{
    public abstract class AbstractBrain
    {
        public abstract void SetInput(string name, float value);

        public abstract void SetInput(string name, float[] value);

        public abstract float GetInput(string name);
        public abstract string[] GetInputs();


    }
}