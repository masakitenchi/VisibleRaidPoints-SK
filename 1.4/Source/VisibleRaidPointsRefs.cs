using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using static VisibleRaidPoints.ThreatPointsBreakdown;

namespace VisibleRaidPoints
{
    public static class VisibleRaidPointsRefs
    {
        public static readonly MethodInfo m_ThreatPointsBreakdown_Clear = AccessTools.Method(typeof(ThreatPointsBreakdown), "Clear");
        public static readonly MethodInfo m_IIncidentTarget_get_PlayerWealthForStoryteller = AccessTools.Method(typeof(IIncidentTarget), "get_PlayerWealthForStoryteller");
        public static readonly MethodInfo m_SimpleCurve_Evaluate = AccessTools.Method(typeof(SimpleCurve), "Evaluate");
        public static readonly MethodInfo m_IIncidentTarget_get_PlayerPawnsForStoryteller = AccessTools.Method(typeof(IIncidentTarget), "get_PlayerPawnsForStoryteller");
        public static readonly MethodInfo m_IEnumeratorPawn_get_Current = AccessTools.Method(typeof(IEnumerator<Pawn>), "get_Current");
        public static readonly MethodInfo m_Pawn_get_LabelShort = AccessTools.Method(typeof(Pawn), "get_LabelShort");
        public static readonly MethodInfo m_ThreatPointsBreakdown_SetPawnPointsName = AccessTools.Method(typeof(ThreatPointsBreakdown), "SetPawnPointsName");
        public static readonly MethodInfo m_ThreatPointsBreakdown_SetPawnPointsPoints = AccessTools.Method(typeof(ThreatPointsBreakdown), "SetPawnPointsPoints");
        public static readonly MethodInfo m_Mathf_Lerp_float_float_float = AccessTools.Method(typeof(Mathf), "Lerp", new[] { typeof(float), typeof(float), typeof(float) });
        public static readonly MethodInfo m_FloatRange_get_RandomInRange = AccessTools.Method(typeof(FloatRange), "get_RandomInRange");

        public static readonly FieldInfo f_ThreatPointsBreakdown_PlayerWealthForStoryteller = AccessTools.Field(typeof(ThreatPointsBreakdown), "PlayerWealthForStoryteller");
        public static readonly FieldInfo f_ThreatPointsBreakdown_PointsFromWealth = AccessTools.Field(typeof(ThreatPointsBreakdown), "PointsFromWealth");
        public static readonly FieldInfo f_ThreatPointsBreakdown_PointsFromPawns = AccessTools.Field(typeof(ThreatPointsBreakdown), "PointsFromPawns");
        public static readonly FieldInfo f_ThreatPointsBreakdown_TargetRandomFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "TargetRandomFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_AdaptationFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "AdaptationFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_GraceFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "GraceFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_PreClamp = AccessTools.Field(typeof(ThreatPointsBreakdown), "PreClamp");
        public static readonly FieldInfo f_ThreatPointsBreakdown_StorytellerRandomFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "StorytellerRandomFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_RaidArrivalModeFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "RaidArrivalModeFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_RaidArrivalModeDesc = AccessTools.Field(typeof(ThreatPointsBreakdown), "RaidArrivalModeDesc");
        public static readonly FieldInfo f_ThreatPointsBreakdown_RaidStrategyFactor = AccessTools.Field(typeof(ThreatPointsBreakdown), "RaidStrategyFactor");
        public static readonly FieldInfo f_ThreatPointsBreakdown_RaidStrategyDesc = AccessTools.Field(typeof(ThreatPointsBreakdown), "RaidStrategyDesc");
        public static readonly FieldInfo f_Def_defName = AccessTools.Field(typeof(Def), "defName");
    }
}
