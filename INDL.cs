using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace LC_INDL
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class INDL : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_INDL", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public INDL()
        {
            log = Logger;
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }
        public void Start()
        {
            harmony.PatchAll(assembly);
            InitConfig();
        }
        #endregion
        public IndirectLightingController indl;
        public ConfigEntry<LightLayerEnum> INDIRECTDIFFUSELIGHTINGLAYERS;
        public ConfigEntry<float> INDIRECTDIFFUSELIGHTINGMULTIPLIER;
        public ConfigEntry<LightLayerEnum> REFLECTIONLIGHTINGLAYERS;
        public ConfigEntry<float> REFLACTIONLIGHTINGMULTIPLIER;
        public ConfigEntry<float> REFLECTIONPROBEINTENSITYMULTIPLIER;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            INDIRECTDIFFUSELIGHTINGLAYERS = Config.Bind("IND_LIGHT", "Light Scene", LightLayerEnum.Everything, "");
            INDIRECTDIFFUSELIGHTINGMULTIPLIER = Config.Bind("IND_LIGHT", "Indirect Light Multiplier", 2f, new ConfigDescription("Indirect Light Multiplier.", new AcceptableValueRange<float>(0f, 100f)));
            REFLECTIONLIGHTINGLAYERS = Config.Bind("IND_LIGHT", "REFLECTIONLIGHTINGLAYERS", LightLayerEnum.Everything, "");
            REFLACTIONLIGHTINGMULTIPLIER = Config.Bind("IND_LIGHT", "Indirect Reflection Multiplier", 1f, new ConfigDescription("Indirect Reflection Multiplier.", new AcceptableValueRange<float>(0f, 100f)));
            REFLECTIONPROBEINTENSITYMULTIPLIER = Config.Bind("IND_LIGHT", "Indirect Reflection Probe Multiplier", 1.5f, new ConfigDescription("Indirect Reflection Probe Multiplier.", new AcceptableValueRange<float>(0f, 100f)));
        }
        public static Volume VOLUMES;
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    if (SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("night"))
                    {
                        DO_INDL();
                    }
                }
            }
        }
        public void DO_INDL() 
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            indl = (IndirectLightingController)VOLUMES.profile.components[4];
            indl.active = true;
            indl.indirectDiffuseLightingLayers.value = INDIRECTDIFFUSELIGHTINGLAYERS.Value;
            indl.indirectDiffuseLightingMultiplier.value = INDIRECTDIFFUSELIGHTINGMULTIPLIER.Value;
            indl.reflectionLightingLayers.value = REFLECTIONLIGHTINGLAYERS.Value;
            indl.reflectionLightingMultiplier.value = REFLACTIONLIGHTINGMULTIPLIER.Value;
            indl.reflectionProbeIntensityMultiplier.value = REFLECTIONPROBEINTENSITYMULTIPLIER.Value;
        }


    }
}
