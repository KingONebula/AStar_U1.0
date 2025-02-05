using System;
using UnityEngine;
namespace AStar.Heuristics
{
    public class DiagonalShortcut : ICalculateHeuristic
    {
        public int Calculate(Vector2Int source, Vector2Int destination)
        {
            var hDiagonal = Math.Min(Math.Abs(source.x - destination.x),
                Math.Abs(source.y - destination.y));
            var hStraight = (Math.Abs(source.x - destination.x) + Math.Abs(source.y - destination.y));
            var heuristicEstimate = 2;
            var h = (heuristicEstimate * 2) * hDiagonal + heuristicEstimate * (hStraight - 2 * hDiagonal);
            return h;
        }
    }
}