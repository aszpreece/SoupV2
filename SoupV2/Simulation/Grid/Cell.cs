using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Grid
{
    public class Cell
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }

        public List<Entity> Entities { get; set; }

        public (int, int) Pos { get => (X, Y); }
    }
}
