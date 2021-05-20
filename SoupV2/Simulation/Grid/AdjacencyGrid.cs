using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SoupV2.Simulation.Grid
{
    public class AdjacencyGrid {


        public int WorldHeight { get; }
        public int WorldWidth { get; }

        public int WUPerCell { get; }

        // [X][Y]
        private Cell[,] _cells;

        private int _gridWidth;
        private int _gridHeight;

        public AdjacencyGrid(int worldWidth, int worldHeight, int wuPerCell) {

            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            WUPerCell = wuPerCell;

            if (WorldWidth <= 0 || WorldHeight <= 0) {
                throw new ArgumentException("width and height must be greater than 0");
            }



            // Calculate cells in the world
            _gridWidth = (int)Math.Ceiling((float)WorldWidth / WUPerCell);
            _gridHeight = (int)Math.Ceiling((float)WorldHeight / WUPerCell);

            // Initialize the grid
            _cells = new Cell[_gridWidth, _gridHeight];
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    _cells[x, y] = new Cell() { Entities = new List<Entity>(), X = x, Y = y };

                }
            }
        }

        /// <summary>
        /// Converts a word coordinate vector into a tuple containing the x and y of the cell that covers that point.
        /// </summary>
        /// <param name="world">The position to fetch the cell coordinates for.</param>
        /// <returns>x and y pair corresponding to cell coordinates.</returns>
        public (int, int) WorldCoordsToCellCoords(Vector2 world) {

            int x = (int)Math.Floor(world.X / WUPerCell) + _gridWidth / 2;
            int y = (int)Math.Floor(world.Y / WUPerCell) + _gridHeight / 2;

            var clamped = ClampToGridCoords((x, y));

            return clamped;
        }


        /// <summary>
        /// Converts a word coordinate tuple into a tuple containing the x and y of the cell that covers that point.
        /// </summary>
        /// <param name="world">The position to fetch the cell coordinates for.</param>
        /// <returns>x and y pair corresponding to cell coordinates.</returns>
        public (int, int) WorldCoordsToCellCoords((float, float) world)
        {

            int x = (int)Math.Floor(world.Item1 / WUPerCell) + _gridWidth / 2;
            int y = (int)Math.Floor(world.Item2 / WUPerCell) + _gridHeight / 2;

            var clamped = ClampToGridCoords((x, y));

            return clamped;
        }

        public void PlaceIntoGrid(Entity entity) {
            // Get the grid this entity should be in
            TransformComponent transform = entity.GetComponent<TransformComponent>();
            (int, int) coordTuple = ClampToGridCoords(WorldCoordsToCellCoords(transform.WorldPosition));

            (int x, int y) = coordTuple;

            // If the entity has just been placed in the grid it will have no _cell property in its transform.
            // If not we need to remove the entity from its old cell
            // If the entity has moved outside the bounds of the cell it should be in we need to remove it from the grid
            if (!entity.Cell.HasValue)
            {
                // This entity is new, we must place into grid
                entity.Cell = coordTuple;
                _cells[x, y].Entities.Add(entity);

            }
            else if (entity.Cell != coordTuple) {

                RemoveFromGrid(entity);
                // Once any removing has been done set the entities cell etc
                entity.Cell = _cells[x, y].Pos;
                _cells[x, y].Entities.Add(entity);
            }
        }
        public void RemoveFromGrid(Entity entity)
        {
            if (!entity.Cell.HasValue)
            {
                return;
            }
            (int x, int y) = ((int, int))entity.Cell;
            Cell current = _cells[x, y];
            int index = current.Entities.IndexOf(entity);

            // Avoids having to shunt list (We don't care about the order of entities within the list)
            SwapRemove.SwapRemoveList(current.Entities, index);
            entity.Cell = null;
        }

        public static Func<Entity, bool> AlwaysTrue = t => true;

        /// <summary>
        /// Returns a pair of distance squared and entities within the given range
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="WURadius"></param>
        /// <returns>(distance from point squared, entity)</returns>
        public IEnumerable<(float, Entity)> GetNearbyEntities(Vector2 pos, float WURadius, Func<Entity, bool> predicate = null)
        {

            //find top left of range to check
            float tlx = pos.X - WURadius;
            float tly = pos.Y - WURadius;
            var tl = ClampToWorldCoords((tlx, tly));
             
            // find bottom right of range to check
            float brx = pos.X + WURadius;
            float bry = pos.Y + WURadius;
            var br = ClampToWorldCoords((brx, bry));

            var (ctlx, ctly) = WorldCoordsToCellCoords(tl);
            var (cbrx, cbry) = WorldCoordsToCellCoords(br);

            float rs = WURadius * WURadius;

            // Future optimization: use circle arithmetic to check if cell is in circle
            // If top left of circle in radius + 1 wu cell?

            for (int x = ctlx; x <= cbrx; x++)
            {
                for (int y = ctly; y <= cbry; y++)
                {
                    // Yield entities in this cell
                    if (predicate is null)
                    {
                        predicate = AlwaysTrue;
                    }
                    var inRange =_cells[x, y].Entities
                        .Where(e => e.IsActive())
                        .Where(predicate)
                        .Select((entity) => ((pos - entity.GetComponent<TransformComponent>().WorldPosition)
                        .LengthSquared(), entity))
                        .Where((pair) => pair.Item1 < rs);

                    foreach(var p in inRange)
                    {
                        yield return p;
                    }

                }

            }

        }



        /// <summary>
        /// Returns a pair of distance squared and entities within the given range
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="WURadius"></param>
        /// <returns>(distance from point squared, entity)</returns>
        public IEnumerable<(float, Entity)> GetEntitiesInFov(Vector2 pos, float WURadius, float angle, float fov, Func<Entity, bool> predicate = null)
        {

            //find top left of range to check
            float tlx = pos.X - WURadius;
            float tly = pos.Y - WURadius;
            var tl = ClampToWorldCoords((tlx, tly));

            // find bottom right of range to check
            float brx = pos.X + WURadius;
            float bry = pos.Y + WURadius;
            var br = ClampToWorldCoords((brx, bry));

            var (ctlx, ctly) = WorldCoordsToCellCoords(tl);
            var (cbrx, cbry) = WorldCoordsToCellCoords(br);

            float rs = WURadius * WURadius;

            // Future optimization: use circle arithmetic to check if cell is in circle
            // If top left of circle in radius + 1 wu cell?

            for (int x = ctlx; x <= cbrx; x++)
            {
                for (int y = ctly; y <= cbry; y++)
                {
                    // Yield entities in this cell
                    if (predicate is null)
                    {
                        predicate = AlwaysTrue;
                    }
                    var inRange = _cells[x, y].Entities
                        .Where(e => e.IsActive())
                        .Where(predicate)
                        .Select((entity) => ((pos - entity.GetComponent<TransformComponent>().WorldPosition)
                        .LengthSquared(), entity))
                        .Where((pair) => pair.Item1 < rs);

                    foreach (var p in inRange)
                    {
                        yield return p;
                    }

                }

            }

        }

        /// <summary>
        /// Clamps int coordinates to valid grid coords
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public (int, int) ClampToGridCoords((int, int) pos)
        {
            var (x, y) = pos;
            x = Math.Clamp(x, 0, _gridWidth - 1);
            y = Math.Clamp(y, 0, _gridHeight - 1);

            return (x, y);
        }

        /// <summary>
        /// Clamps world coordinates so they are in the range that the grid covers.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public (float, float) ClampToWorldCoords((float, float) pos)
        {
            var (x, y) = pos;
            x = Math.Clamp(x, 0 - WorldWidth / 2, WorldWidth / 2);
            y = Math.Clamp(y, 0 - WorldHeight / 2, WorldHeight / 2);

            return (x, y);
        }

        private static float Sigmoid(double value)
        {
            return (float)(1.0 / (1.0 + Math.Pow(Math.E, -value)));
        }

        public void DrawGrid(SpriteBatch spriteBatch, Matrix camera)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera);
            Color[] colours = { Color.Gray, Color.DarkSlateGray };
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var entityCount = _cells[x, y].Entities.Count;
                    var mult =  Sigmoid(entityCount) + 1;

                    //Flip between colours to create grid pattern
                    var col = colours[(x + y) % 2];
                    spriteBatch.FillRectangle(new Rectangle((x - _gridWidth / 2) * WUPerCell, (y - _gridHeight / 2) * WUPerCell, WUPerCell, WUPerCell), col * mult);
                }
            }
            spriteBatch.End();
        }
    }

}
