using Verse;
using RimWorld;
using HarmonyLib;
using static VisibleRaidPoints.ThreatPointsBreakdown;
using UnityEngine;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(IncidentWorker))]
    [HarmonyPatch("SendStandardLetter")]
    [HarmonyPatch(new[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(IncidentParms), typeof(LookTargets), typeof(NamedArgument[]) })]
    public static class Patch_IncidentWorker_SendStandardLetter
    {


        public static readonly float clampLow = StorytellerUtility.GlobalPointsMin();
        public static readonly float clampHigh = 30000f;
        public static void Prefix(IncidentWorker __instance, ref TaggedString baseLetterLabel, ref TaggedString baseLetterText, ref IncidentParms parms)
        {
            if (parms == null)
            {
                Debug.Log("No IncidentParms available. Cannot determine threat points. This should never happen.");
                return;
            }

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
                    baseLetterText += $"\n\n{"VisibleRaidPoints_RaidPointsUsed".Translate()}: {((int) parms.points).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
                }

                if (VisibleRaidPointsSettings.ShowBreakdown)
                {
                    if (ThreatPointsBreakdown.PointsPerPawn == null)
                    {
                        Debug.Log("Points per pawn not initialized. Cannot provide threat points breakdown. Harmony transpiler patch probably failed.");
                        return;
                    }

                    if (Find.Storyteller == null || Find.Storyteller.difficulty == null)
                    {
                        Debug.Log("Storyteller or difficulty settings not available. Cannot provide threat points breakdown. This shouldn't happen unless version < 1.3.");
                        return;
                    }

                    baseLetterText += $"\n\n=== {"VisibleRaidPoints_PointsBreakdown".Translate()} ===";
                    
                    baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownPlayerWealthForStorytellerDesc".Translate()}: {$"${(int) ThreatPointsBreakdown.PlayerWealthForStoryteller}".Colorize(ColoredText.CurrencyColor)} {$"({"VisibleRaidPoints_BreakdownPlayerWealthForStorytellerExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor)}";
                    baseLetterText += $"\n{"VisibleRaidPoints_BreakdownPointsFromWealthDesc".Translate()}: {((int)ThreatPointsBreakdown.PointsFromWealth).ToString().Colorize(ColoredText.FactionColor_Hostile)}";

                    baseLetterText += "\n";
                    if (ThreatPointsBreakdown.PointsPerPawn.Count > 0) 
                    {
                        baseLetterText += $"\n{"VisibleRaidPoints_BreakdownPointsPerPawnDesc".Translate()}: {$"({"VisibleRaidPoints_BreakdownPointsPerPawnExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor)}";
                    }
                    foreach (PawnPoints p in ThreatPointsBreakdown.PointsPerPawn)
                    {
                        if (p.Points > 0f)
                        {
                            baseLetterText += $"\n  {p.Name}: {((int)p.Points).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
                        }
                    }
                    baseLetterText += $"\n{"VisibleRaidPoints_BreakdownPointsFromPawnsDesc".Translate()}: {((int)ThreatPointsBreakdown.PointsFromPawns).ToString().Colorize(ColoredText.FactionColor_Hostile)}";

                    if (ThreatPointsBreakdown.TargetRandomFactor != 1f)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownTargetRandomFactorDesc".Translate()}: {ThreatPointsBreakdown.TargetRandomFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)}";
                    }

                    baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownAdaptationFactorDesc".Translate()}: {ThreatPointsBreakdown.AdaptationFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)}";
                    
                    baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownThreatScaleDesc".Translate()}: {Find.Storyteller.difficulty.threatScale.ToString("0.00").Colorize(ColoredText.ImpactColor)}";

                    if (ThreatPointsBreakdown.GraceFactor != 1f)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownGraceFactorDesc".Translate()}: {ThreatPointsBreakdown.GraceFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} {$"({"VisibleRaidPoints_BreakdownGraceFactorExpl".Translate()})".Colorize(ColoredText.SubtleGrayColor)}";
                    }

                    // Running total pre-clamp
                    baseLetterText += $"\n\n{"VisibleRaidPoints_RunningTotalPreClampDesc".Translate()}";
                    baseLetterText += $"\n({((int)ThreatPointsBreakdown.PointsFromWealth).ToString().Colorize(ColoredText.FactionColor_Hostile)} + {((int)ThreatPointsBreakdown.PointsFromPawns).ToString().Colorize(ColoredText.FactionColor_Hostile)}) {(ThreatPointsBreakdown.TargetRandomFactor != 1f? $"x {ThreatPointsBreakdown.TargetRandomFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} ": "")}x {ThreatPointsBreakdown.AdaptationFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} x {Find.Storyteller.difficulty.threatScale.ToString("0.00").Colorize(ColoredText.ImpactColor)} {(ThreatPointsBreakdown.GraceFactor != 1f? $"x {ThreatPointsBreakdown.GraceFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} ": "")}= {((int) ThreatPointsBreakdown.PreClamp).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
                    
                    if (ThreatPointsBreakdown.PreClamp < clampLow)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownClampLowDesc".Translate(clampLow)}";
                    }

                    if (ThreatPointsBreakdown.PreClamp > clampHigh)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_BreakdownClampHighDesc".Translate(clampHigh)}";
                    }

                    if (ThreatPointsBreakdown.StorytellerRandomFactor > 0f)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_StorytellerRandomFactorDesc".Translate()}: {ThreatPointsBreakdown.StorytellerRandomFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)}";
                    }

                    if (ThreatPointsBreakdown.RaidArrivalModeFactor > 0f)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_RaidArrivalModeFactorDesc".Translate()}: {ThreatPointsBreakdown.RaidArrivalModeFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} {$"({ThreatPointsBreakdown.RaidArrivalModeDesc})".Colorize(ColoredText.SubtleGrayColor)}";
                    }

                    if (ThreatPointsBreakdown.RaidStrategyFactor > 0f)
                    {
                        baseLetterText += $"\n\n{"VisibleRaidPoints_RaidStrategyFactorDesc".Translate()}: {ThreatPointsBreakdown.RaidStrategyFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} {$"({ThreatPointsBreakdown.RaidStrategyDesc})".Colorize(ColoredText.SubtleGrayColor)}";
                    }

                    if (ThreatPointsBreakdown.StorytellerRandomFactor > 0f || ThreatPointsBreakdown.RaidArrivalModeFactor > 0f || ThreatPointsBreakdown.RaidStrategyFactor > 0f)
                    {
                        // Running total pre-final
                        baseLetterText += $"\n\n{"VisibleRaidPoints_RunningTotalPreClampDesc".Translate()}";
                        baseLetterText += $"\n{((int)Mathf.Clamp(ThreatPointsBreakdown.PreClamp, clampLow, clampHigh)).ToString().Colorize(ColoredText.FactionColor_Hostile)} {(ThreatPointsBreakdown.StorytellerRandomFactor > 0f ? $"x {ThreatPointsBreakdown.StorytellerRandomFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} " : "")}{(ThreatPointsBreakdown.RaidArrivalModeFactor > 0f ? $"x {ThreatPointsBreakdown.RaidArrivalModeFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} " : "")}{(ThreatPointsBreakdown.RaidStrategyFactor > 0f ? $"x {ThreatPointsBreakdown.RaidStrategyFactor.ToString("0.00").Colorize(ColoredText.ImpactColor)} " : "")}= {((int)parms.points).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
                    }

                    baseLetterText += "\n\n----------------------";
                    baseLetterText += $"\n{"VisibleRaidPoints_BreakdownTotal".Translate()}: {((int) parms.points).ToString().Colorize(ColoredText.FactionColor_Hostile)}";
                }
            }
        }
    }
}
