using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;
using UnityEngine;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private const int ClosedValue = 0;
        private const int DistanceBetweenNodes = 1;
        private readonly PathFinderOptions _options;
        private readonly WorldGrid _world;
        private readonly ICalculateHeuristic _heuristic;

        public PathFinder(WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
            _heuristic = HeuristicFactory.Create(_options.HeuristicFormula);
        }
        
        ///<inheritdoc/>
        
        ///<inheritdoc/>
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
        {
            var nodesVisited = 0;
            IModelAGraph<PathFinderNode> graph = new PathFinderGraph(_world.Height, _world.Width, _options.UseDiagonals);

            var startNode = new PathFinderNode(position: start, g: 0, h: 2, parentNodePosition: start);
            graph.OpenNode(startNode);

            while (graph.HasOpenNodes)
            {
                var q = graph.GetOpenNodeWithSmallestF();
                
                if (q.Position == end)
                {
                    return OrderClosedNodesAsArray(graph, q);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return new Vector2Int[0];
                }

                foreach (var successor in graph.GetSuccessors(q))
                {
                    
                    if (_world[successor.Position] == ClosedValue)
                    {
                        Debug.Log(successor.Position.x + ":" + successor.Position.y + "=" + _world[successor.Position]);
                        continue;
                    }

                    var newG = q.G + DistanceBetweenNodes;

                    if (_options.PunishChangeDirection)
                    {
                        newG += CalculateModifierToG(q, successor, end);
                    }

                    var updatedSuccessor = new PathFinderNode(
                        position: successor.Position,
                        g: newG,
                        h:_heuristic.Calculate(successor.Position, end),
                        parentNodePosition: q.Position);
                    
                    if (BetterPathToSuccessorFound(updatedSuccessor, successor))
                    {
                        graph.OpenNode(updatedSuccessor);
                    }
                }

                nodesVisited++;
            }

            return new Vector2Int[0];
        }

        private int CalculateModifierToG(PathFinderNode q, PathFinderNode successor, Vector2Int end)
        {
            if (q.Position == q.ParentNodePosition)
            {
                return 0;
            }
            
            var gPunishment = Math.Abs(successor.Position.x - end.x) + Math.Abs(successor.Position.y - end.y);
            
            var successorIsVerticallyAdjacentToQ = successor.Position.x - q.Position.x != 0;

            if (successorIsVerticallyAdjacentToQ)
            {
                var qIsVerticallyAdjacentToParent = q.Position.x - q.ParentNodePosition.x == 0;
                if (qIsVerticallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            var successorIsHorizontallyAdjacentToQ = successor.Position.y - q.Position.y != 0;

            if (successorIsHorizontallyAdjacentToQ)
            {
                var qIsHorizontallyAdjacentToParent = q.Position.y - q.ParentNodePosition.y == 0;
                if (qIsHorizontallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            if (_options.UseDiagonals)
            {
                var successorIsDiagonallyAdjacentToQ = (successor.Position.x - successor.Position.x) == (q.Position.y - q.Position.y);
                if (successorIsDiagonallyAdjacentToQ)
                {
                    var qIsDiagonallyAdjacentToParent = (q.Position.y - q.Position.x) == (q.ParentNodePosition.y - q.ParentNodePosition.x)
                                                        && IsStraightLine(q.ParentNodePosition, q.Position, successor.Position);
                    if (qIsDiagonallyAdjacentToParent)
                    {
                        return gPunishment;
                    }
                }
            }

            return 0;
        }

        private bool IsStraightLine(Vector2Int a, Vector2Int b, Vector2Int c)
        {
            // area of triangle == 0
            return (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / 2 == 0;
        }

        private bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
        {
            return !currentSuccessor.HasBeenVisited ||
                (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);
        }

        private static Vector2Int[] OrderClosedNodesAsArray(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
        {
            var path = new Stack<Vector2Int>();

            var currentNode = endNode;

            while (currentNode.Position != currentNode.ParentNodePosition)
            {
                path.Push(currentNode.Position);
                currentNode = graph.GetParent(currentNode);
            }

            path.Push(currentNode.Position);

            return path.ToArray();
        }
    }
}