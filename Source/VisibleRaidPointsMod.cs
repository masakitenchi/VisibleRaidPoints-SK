using UnityEngine;
using Verse;
using HarmonyLib;

namespace VisibleRaidPoints
{
    public class VisibleRaidPointsMod : Mod
    {
        public const string PACKAGE_ID = "visibleraidpoints.1trickPonyta";
        public const string PACKAGE_NAME = "Visible Raid Points";

        public static VisibleRaidPointsSettings Settings;

        public VisibleRaidPointsMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<VisibleRaidPointsSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            VisibleRaidPointsSettings.DoSettingsWindowContents(inRect);
        }
    }
}
