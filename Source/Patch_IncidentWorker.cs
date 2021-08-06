using Verse;
using RimWorld;
using HarmonyLib;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker))]
    [HarmonyPatch("SendStandardLetter")]
    [HarmonyPatch(new[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(IncidentParms), typeof(LookTargets), typeof(NamedArgument[]) })]
    public static class Patch_IncidentWorker_SendStandardLetter
    {
        public static void Prefix(IncidentWorker __instance, ref TaggedString baseLetterLabel, ref TaggedString baseLetterText, ref IncidentParms parms)
        {
            if (__instance is IncidentWorker_RaidEnemy || 
                __instance is IncidentWorker_Infestation || 
                __instance is IncidentWorker_CrashedShipPart || 
                __instance is IncidentWorker_Ambush || 
                __instance is IncidentWorker_AnimalInsanityMass || 
                __instance is IncidentWorker_ManhunterPack || 
                __instance is IncidentWorker_MechCluster)
            {
                if (VisibleRaidPointsSettings.ShowInLabel)
                {
                    baseLetterLabel = $"({(int) parms.points}) {baseLetterLabel}";
                }

                if (VisibleRaidPointsSettings.ShowInText)
                {
                    baseLetterText += $"\n\n{"VisibleRaidPoints_RaidPointsUsed".Translate()}: {(int) parms.points}";
                }
            }
        }
    }
}
