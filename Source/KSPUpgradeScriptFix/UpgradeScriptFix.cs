using SaveUpgradePipeline;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KSPUpgradeScriptFix {
  [KSPAddon (KSPAddon.Startup.MainMenu, true)]
  public class UpgradeScriptFix : MonoBehaviour {
    public void Awake () {
      var pipeline = (SaveUpgradePipeline.SaveUpgradePipeline)
        (typeof (KSPUpgradePipeline).GetProperty ("Pipeline", BindingFlags.Static | BindingFlags.NonPublic).GetValue (null));

      foreach(var index in pipeline.upgradeScripts.Where(s => s is v180_ModuleAeroSurface).Select(s => pipeline.upgradeScripts.IndexOf (s)).ToList()) {
        pipeline.upgradeScripts[index] = new v180_ModuleAeroSurfaceFixed(pipeline.upgradeScripts[index]);
      }

      foreach (var index in pipeline.upgradeScripts.Where (s => s is v180_ModuleControlSurface).Select (s => pipeline.upgradeScripts.IndexOf (s)).ToList ()) {
        pipeline.upgradeScripts[index] = new v180_ModuleControlSurfaceFixed (pipeline.upgradeScripts[index]);
      }

      typeof (KSPUpgradePipeline).GetField ("_pipeline", BindingFlags.Static | BindingFlags.NonPublic).SetValue (null, pipeline);

      Debug.Log ("[UpgradeScriptFix] Successfully replaced the v180 upgrade scripts.");
    }
  }

  public class v180_ModuleControlSurfaceFixed : v180_ModuleControlSurface {
    public v180_ModuleControlSurfaceFixed (UpgradeScript parent) {
      var nodeUrlCraft = typeof(UpgradeScript).GetField("nodeUrlCraft", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(parent);
      var nodeUrlSFS = typeof(UpgradeScript).GetField("nodeUrlSFS", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(parent);

      this.nodeUrlCraft = (string)nodeUrlCraft;
      this.nodeUrlSFS = (string)nodeUrlSFS;
      this.ContextMask = parent.ContextMask;
    }

    public override void OnUpgrade (ConfigNode node, LoadContext loadContext, ConfigNode parentNode) {
      ConfigNode[] nodes = node.GetNodes("MODULE");
      int num = nodes.Length;
      while (num-- > 0) {
        string value = nodes[num].GetValue("name");
        if (value == "ModuleControlSurface") {
          ModuleControlSurface baseModule = GetBaseModule(node, loadContext);
          ConvertControlAuthority (nodes[num], baseModule);
          ConvertControlAuthorityAxisGroup (nodes[num]);
        }
      }
    }

    private ModuleControlSurface GetBaseModule (ConfigNode node, LoadContext loadContext) {
      string text;
      if (loadContext == LoadContext.Craft) {
        text = node.GetValue ("part");
        text = text.Substring (0, text.IndexOf ('_'));
      } else {
        text = node.GetValue ("name");
      }
      Part partPrefab = PartLoader.getPartInfoByName(text).partPrefab;
      int count = partPrefab.Modules.Count;
      while (count-- > 0) {
        PartModule partModule = partPrefab.Modules[count];
        if (partModule is ModuleControlSurface moduleControlSurface) {
          return moduleControlSurface;
        }
      }
      return null;
    }
  }

  public class v180_ModuleAeroSurfaceFixed : v180_ModuleAeroSurface {
    public v180_ModuleAeroSurfaceFixed (UpgradeScript parent) {
      var nodeUrlCraft = typeof(UpgradeScript).GetField("nodeUrlCraft", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(parent);
      var nodeUrlSFS = typeof(UpgradeScript).GetField("nodeUrlSFS", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(parent);

      this.nodeUrlCraft = (string)nodeUrlCraft;
      this.nodeUrlSFS = (string)nodeUrlSFS;
      this.ContextMask = parent.ContextMask;
    }

    public override void OnUpgrade (ConfigNode node, LoadContext loadContext, ConfigNode parentNode) {
      ConfigNode[] nodes = node.GetNodes("MODULE");
      int num = nodes.Length;
      while (num-- > 0) {
        string value = nodes[num].GetValue("name");
        if (value == "ModuleAeroSurface") {
          ModuleAeroSurface baseModule = GetBaseModule(node, loadContext);
          ConvertAeroAuthority (nodes[num], baseModule);
          ConvertAeroAuthorityAxisGroup (nodes[num]);
        }
      }
    }

    private ModuleAeroSurface GetBaseModule (ConfigNode node, LoadContext loadContext) {
      string text;
      if (loadContext == LoadContext.Craft) {
        text = node.GetValue ("part");
        text = text.Substring (0, text.IndexOf ('_'));
      } else {
        text = node.GetValue ("name");
      }
      Part partPrefab = PartLoader.getPartInfoByName(text).partPrefab;
      int count = partPrefab.Modules.Count;
      while (count-- > 0) {
        PartModule partModule = partPrefab.Modules[count];
        if (partModule is ModuleAeroSurface moduleAeroSurface) {
          return moduleAeroSurface;
        }
      }
      return null;
    }
  }
}
