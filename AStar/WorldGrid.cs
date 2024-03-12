using AStar.Collections.MultiDimensional;

namespace AStar
{
    /// <summary>
    /// A world grid consisting of integers where a closed cell is represented by 0
    /// </summary>
    public class WorldGrid : Grid<short>
    {
        /// <summary>
        /// Creates a new world with the given dimensions initialised to closed
        /// </summary>
        /// <param name="height">height of the world (Position.X / Point.Y)</param>
        /// <param name="width">width of the world (Position.Y / Point.X)</param>
        public WorldGrid(int height, int width) : base(height, width)
        {
        }

        /// <summary>
        /// Creates a new world with values set from the provided 2d array.
        /// Height will be first dimension, and Width will be the second,
        /// e.g [4,2] will have a height of 4 and a width of 2.
        /// </summary>
        /// <param name="worldArray">A 2 dimensional array of short where 0 indicates a closed node</param>
        public WorldGrid(short[,] worldArray) : base(worldArray)
        {
        }
    }
}