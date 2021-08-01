using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RealTimeAutoSave
{
    public class VisibleRaidPointsSettings : ModSettings
    {
        public static bool ShowInLabel = false;
        public static bool ShowInText = true;

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("VisibleRaidPoints_ShowPointsInLetterLabel".Translate(), ref ShowInLabel);
            listingStandard.CheckboxLabeled("VisibleRaidPoints_ShowPointsInLetterText".Translate(), ref ShowInText);

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShowInLabel, "ShowInLabel");
            Scribe_Values.Look(ref ShowInText, "ShowInText");
        }
    }
}
