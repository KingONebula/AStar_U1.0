using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
namespace AStar.Collections.MultiDimensional
{
    public class Grid<T> : IModelAGrid<T>
    {
        private readonly T[,] _grid;
        public Grid(int height, int width)
        {
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
            
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            
            Height = height;
            Width = width;

            _grid = new T[height,width];
        }
        public Grid(T[,] grid)
        {
            
            Height = grid.GetLength(0);
            Width = grid.GetLength(1);

            _grid = grid;
        }
        public int Height { get; }

        public int Width { get; }
        
        public IEnumerable<Vector2Int> GetSuccessorPositions(Vector2Int node, bool optionsUseDiagonals = false)
        {
            var offsets = GridOffsets.GetOffsets(optionsUseDiagonals);
            foreach (var neighbourOffset in offsets)
            {
                var successorRow = node.x + neighbourOffset.row;
                
                if (successorRow < 0 || successorRow >= Height)
                {
                    continue;
                    
                }
                
                var successorColumn = node.y + neighbourOffset.column;

                if (successorColumn < 0 || successorColumn >= Width)
                {
                    continue;
                }
                
                yield return new Vector2Int(successorRow, successorColumn);
            }
        }
        public T this[Vector2Int position]
        {
            get
            {
                return _grid[position.y, position.x];
            }
            set
            {
                _grid[position.y, position.x] = value;
            }
        }
        public T this[int row, int column]
        {
            get
            {
                return _grid[row, column];
            }
            set
            {
                _grid[row, column] = value;
            }
        }
    }
}