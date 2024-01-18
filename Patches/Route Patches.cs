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
            if (!Config.Instance.customPriceEnabled) {
                return;
            }
            var terminal = Object.FindObjectOfType<Terminal>();
            Plugin.Logger.LogDebug(node.name);
            foreach (var noun in terminal.terminalNodes.allKeywords.First(terminalKeyword => terminalKeyword.word == "route").compatibleNouns) {
                if (noun.result == null) {
                    continue;
                }

                if (noun.result.name is "atlas abyssRoute") {
                    noun.result.itemCost = Config.Instance.moonPrice;
                }
            }
        }

        [HarmonyPatch("LoadNewNodeIfAffordable")]
        [HarmonyPrefix]
        static void LoadNewNodeIfAffordablePatch(ref TerminalNode node) {
            if (!Config.Instance.customPriceEnabled) {
                return;
            }
            Plugin.Logger.LogDebug(node.name);
            if (node == null || node.name != "atlas abyssRouteConfirm") {
                return;
            }
            node.itemCost = Math.Abs(Config.Instance.moonPrice);
        }
    }
}