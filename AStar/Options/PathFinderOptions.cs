using AStar.Heuristics;

namespace AStar.Options
{
    public class PathFinderOptions
    {
        public HeuristicFormula HeuristicFormula { get; set; }

        public bool UseDiagonals { get; set; }

        public bool PunishChangeDirection { get; set; }

        public int SearchLimit { get; set; }

        public PathFinderOptions()
        {
            HeuristicFormula = HeuristicFormula.Manhattan;
            UseDiagonals = false;
            PunishChangeDirection = true;
            SearchLimit = 2000;
        }
    }
}
