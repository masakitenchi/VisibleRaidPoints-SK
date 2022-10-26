using System.Collections.Generic;

namespace VisibleRaidPoints
{
    public static class ThreatPointsBreakdown
    {
        public class PawnPoints
        {
            public string Name;
            public float Points;
        }

        public static float PlayerWealthForStoryteller;
        public static float PointsFromWealth;
        public static List<PawnPoints> PointsPerPawn;
        public static float PointsFromPawns;
        public static float TargetRandomFactor;
        public static float AdaptationFactor;
        public static float GraceFactor;
        public static float PreClamp;
        public static float StorytellerRandomFactor;
        public static float RaidArrivalModeFactor;
        public static string RaidArrivalModeDesc;
        public static float RaidStrategyFactor;
        public static string RaidStrategyDesc;

        public static void Clear()
        {
            PlayerWealthForStoryteller = 0f;
            PointsFromWealth = 0f;
            PointsPerPawn = new List<PawnPoints>();
            PointsFromPawns = 0f;
            TargetRandomFactor = 0f;
            AdaptationFactor = 0f;
            GraceFactor = 0f;
            PreClamp = 0f;
            StorytellerRandomFactor = 0f;
            RaidArrivalModeFactor = 0f;
            RaidArrivalModeDesc = null;
            RaidStrategyFactor = 0f;
            RaidStrategyDesc = null;
        }

        public static void SetPawnPointsName(string name)
        {
            PawnPoints last;
            if (PointsPerPawn.Count == 0 || PointsPerPawn[PointsPerPawn.Count - 1].Name != null)
            {
                PointsPerPawn.Add(new PawnPoints());
            }
            last = PointsPerPawn[PointsPerPawn.Count - 1];
            last.Name = name;
        }

        public static void SetPawnPointsPoints(float points)
        {
            PawnPoints last;
            if (PointsPerPawn.Count == 0)
            {
                PointsPerPawn.Add(new PawnPoints());
            }
            last = PointsPerPawn[PointsPerPawn.Count - 1];
            last.Points = points;
        }
    }
}