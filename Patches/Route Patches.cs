using System;
using System.Linq;

using HarmonyLib;

using Object = UnityEngine.Object;

namespace AtlasAbyss.Patches {
    [HarmonyPatch(typeof(Terminal))]
    public class RoutePatches {

        [HarmonyPatch("LoadNewNode")]
        [HarmonyPrefix]
        private static void LoadNewNodePatchBefore(ref TerminalNode node) {
            var terminal = Object.FindObjectOfType<Terminal>();

            foreach (var noun in terminal.terminalNodes.allKeywords.First(terminalKeyword => terminalKeyword.word == "route").compatibleNouns) {
                if (noun.result == null) {
                    continue;
                }

                if (noun.result.name is "atlasabyssRoute") {
                    noun.result.itemCost = Config.Instance.moonPrice;
                }
            }
        }

        [HarmonyPatch("LoadNewNodeIfAffordable")]
        [HarmonyPrefix]
        static void LoadNewNodeIfAffordablePatch(ref TerminalNode node) {
            if (node == null || node.name != "atlasabyssRouteConfirm") {
                return;
            }
            node.itemCost = Math.Abs(Config.Instance.moonPrice);
        }
    }
}