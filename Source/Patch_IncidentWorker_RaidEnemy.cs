using Verse;
using RimWorld;
using HarmonyLib;

namespace RealTimeAutoSave
{
    [HarmonyPatch(typeof(IncidentWorker_RaidEnemy))]
    [HarmonyPatch("GetLetterLabel")]
    public static class Patch_IncidentWorker_RaidEnemy_GetLetterLabel
    {
        public static void Postfix(ref IncidentParms parms, ref string __result)
        {
            if (VisibleRaidPointsSettings.ShowInLabel)
            {
                __result = $"({(int)parms.points}) {__result}";
            }
        }
    }

    [HarmonyPatch(typeof(IncidentWorker_RaidEnemy))]
    [HarmonyPatch("GetLetterText")]
    public static class Patch_IncidentWorker_RaidEnemy_GetLetterText
    {
        public static void Postfix(ref IncidentParms parms, ref string __result)
        {
            if (VisibleRaidPointsSettings.ShowInText)
            {
                __result += $"\n\n{"VisibleRaidPoints_RaidPointsUsed".Translate()}: {(int)parms.points}";
            }
        }
    }
}
