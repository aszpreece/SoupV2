using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EntityComponentSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoupV2.Simulation.Components;
using SoupV2.util;

namespace Test
{
    [TestClass]
    public class PointerTest
    {
        delegate float GetInput();
        delegate void SetInput(float value);

        [TestMethod]
        public void TestSetter()
        {
            string foobar = typeof(MovementControlComponent).Name;
            AbstractComponent comp = new MovementControlComponent(new Entity());

            var setter = GetterSetterPointers.GetPropSetter<AbstractComponent, float>("WishForceForward", comp.GetType());
            var foo = comp.GetType().GetProperty("WishForceForward").GetSetMethod().GetCustomAttributes(typeof(ControlAttribute));
            
            if (foo.Count() <= 0)
            {
                Assert.Fail("Does not have control attribute");
            }
            

            setter(comp, 10);

            Assert.AreEqual(10, ((MovementControlComponent)comp).WishForceForward);
        }

        //[TestMethod]
        //public void TestGetter()
        //{
        //    VelocityComponent comp = new MovementControlComponent();

        //    var setter = GetterSetterPointers.GetPropSetter<MovementControlComponent, float>("WishForceForward");
        //    var foo = comp.GetType().GetProperty("WishForceForward").GetSetMethod().GetCustomAttributes(typeof(ControlAttribute));

        //    setter(comp, 10);

        //    Assert.AreEqual(10, comp.WishForceForward);
        //}

    }

}
