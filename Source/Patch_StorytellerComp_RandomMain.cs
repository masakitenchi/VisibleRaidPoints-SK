using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace VisibleRaidPoints
{
    [HarmonyPatch(typeof(StorytellerComp_RandomMain))]
    [HarmonyPatch("GenerateParms")]
    public static class Patch_StorytellerComp_RandomMain_GenerateParms
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == VisibleRaidPointsRefs.m_FloatRange_get_RandomInRange)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Stsfld, VisibleRaidPointsRefs.f_ThreatPointsBreakdown_StorytellerRandomFactor);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
