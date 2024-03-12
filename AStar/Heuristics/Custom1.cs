using System;
using UnityEngine;
namespace AStar.Heuristics
{
    public class Custom1 : ICalculateHeuristic
    {
        public int Calculate(Vector2Int source, Vector2Int destination)
        {
            var heuristicEstimate = 2;
            var dxy = new Vector2Int(Math.Abs(destination.x - source.x), Math.Abs(destination.y - source.y));
            var Orthogonal = Math.Abs(dxy.x - dxy.y);
            var Diagonal = Math.Abs(((dxy.x + dxy.y) - Orthogonal) / 2);
            var h = heuristicEstimate * (Diagonal + Orthogonal + dxy.x + dxy.y);
            return h;
        }
    }
}