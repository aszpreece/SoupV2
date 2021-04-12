using Newtonsoft.Json;
using System.ComponentModel;

namespace EntityComponentSystem
{
    public class AbstractComponent
    {
        [JsonIgnore]
        private readonly int _hash;

        [JsonIgnore]
        [Browsable(false)]
        public Entity Owner { get; set;  }
        public AbstractComponent(Entity owner)
        {
            Owner = owner;
            _hash = this.GetType().GetHashCode();
        }


        public override int GetHashCode()
        {
            return _hash; 
        }
    }
}