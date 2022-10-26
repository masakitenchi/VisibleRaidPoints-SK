using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Verse;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(StorytellerUtility))]
    [HarmonyPatch("DefaultThreatPointsNow")]
    public static class Patch_StorytellerUtility_DefaultThreatPointsNow
    {
        private enum Step
        {
            PlayerWealthForStoryteller,
            PointsFromWealth,
            PlayerPawnsForStoryteller,
            PointsPerPawnName,
            PointsPerPawnPoints,
            PointsFromPawns,
            RandomFactor,
            AdaptationFactor,
            GraceFactor,
            PreClamp,
            Done
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder pawn = il.DeclareLocal(typeof(Pawn));

            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_Clear);

            Step step = Step.PlayerWealthForStoryteller;

            foreach (CodeInstruction instruction in instructions)
            {
                switch (step)
                {

                    case Step.PlayerWealthForStoryteller:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IIncidentTarget_get_PlayerWealthForStoryteller)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_PlayerWealthForStoryteller);
                            step = Step.PointsFromWealth;
                        } 
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsFromWealth:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_SimpleCurve_Evaluate)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_PointsFromWealth);
                            step = Step.PlayerPawnsForStoryteller;
                        } 
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PlayerPawnsForStoryteller:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo) instruction.operand == VisibleRaidPointsRefs.m_IIncidentTarget_get_PlayerPawnsForStoryteller)
                        {
                            yield return instruction;
                            step = Step.PointsPerPawnName;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsPerPawnName:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_IEnumeratorPawn_get_Current)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stloc_S, pawn);
                        }
                        else if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.ToString() == "0")
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, pawn);
                            yield return new CodeInstruction(OpCodes.Callvirt, VisibleRaidPointsRefs.m_Pawn_get_LabelShort);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsName);
                            yield return instruction;
                            step = Step.PointsPerPawnPoints;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsPerPawnPoints:
                        if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.ToString() == "0")
                        {
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsPoints);
                            yield return instruction;
                        }
                        else if (instruction.opcode == OpCodes.Call && (MethodInfo) instruction.operand == VisibleRaidPointsRefs.m_Mathf_Lerp_float_float_float)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Call, VisibleRaidPointsRefs.m_ThreatPointsBreakdown_SetPawnPointsPoints);
                            step = Step.PointsFromPawns;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PointsFromPawns:
                        if (instruction.opcode == OpCodes.Add)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_PointsFromPawns);
                            step = Step.RandomFactor;
                        } 
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.RandomFactor:
                        if (instruction.opcode == OpCodes.Call && (MethodInfo) instruction.operand == VisibleRaidPointsRefs.m_FloatRange_get_RandomInRange)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_TargetRandomFactor);
                            step = Step.AdaptationFactor;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.AdaptationFactor:
                        if (instruction.opcode == OpCodes.Call && (MethodInfo) instruction.operand == VisibleRaidPointsRefs.m_Mathf_Lerp_float_float_float)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_AdaptationFactor);
                            step = Step.GraceFactor;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.GraceFactor:
                        if (instruction.opcode == OpCodes.Callvirt && (MethodInfo) instruction.operand == VisibleRaidPointsRefs.m_SimpleCurve_Evaluate)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_GraceFactor);
                            step = Step.PreClamp;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.PreClamp:
                        if (instruction.opcode == OpCodes.Mul)
                        {
                            yield return instruction;
                            yield return new CodeInstruction(OpCodes.Dup);
                            yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_PreClamp);
                            step = Step.Done;
                        }
                        else
                        {
                            yield return instruction;
                        }
                        break;

                    case Step.Done:
                        yield return instruction;
                        break;
                }
            }
        }
    }
}
