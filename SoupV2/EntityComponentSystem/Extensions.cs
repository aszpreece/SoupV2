using System;
using System.Linq;
using System.Collections.Generic;

namespace EntityComponentSystem 
{
    public static class Extensions
    {
        public static bool IsComponent(this Type classType) => typeof(AbstractComponent).IsAssignableFrom(classType);
    }
}