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

namespace LC_AMB
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class AMB : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_AMB", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public AMB()
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
        public AmbientOcclusion amb;
        public ConfigEntry<float> AMB_BLURSHARPNESS;
        public ConfigEntry<bool> AMB_BILATERALUPSAMPLE;
        public ConfigEntry<bool> AMB_DENOISE;
        public ConfigEntry<float> AMB_DENOISERRADIUS;
        public ConfigEntry<int> AMB_DIRECTIONCOUNT;
        public ConfigEntry<float> AMB_DIRECTLIGHTINGSTRENGTH;
        public ConfigEntry<bool> AMB_FULLRESOLUTION;
        public ConfigEntry<float> AMB_GHOSTINGREDUCTION;
        public ConfigEntry<float> AMB_INTENSITY;
        public ConfigEntry<int> AMB_MAXIMUMRADIUSINPIXELS;
        public ConfigEntry<int> AMB_QUALITY;
        public ConfigEntry<float> AMB_RADIUS;
        public ConfigEntry<float> AMB_RAYLENGTH;
        public ConfigEntry<bool> AMB_RAYTRACING;
        public ConfigEntry<int> AMB_SAMPLECOUNT;
        public ConfigEntry<float> AMB_SPATIALBILATERALAGGRESSIVENESS;
        public ConfigEntry<int> AMB_STEPCOUNT;
        public ConfigEntry<bool> AMB_TEMPORALACCUMULATION;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            AMB_BLURSHARPNESS = Config.Bind("AMB", "AMB_BLURSHARPNESS", 0.1f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_BILATERALUPSAMPLE = Config.Bind("AMB", "AMB_BILATERALUPSAMPLE", true, ".");
            AMB_DENOISE = Config.Bind("AMB", "AMB_DENOISE", true, ".");
            AMB_DENOISERRADIUS = Config.Bind("AMB", "AMB_BLURSHARPNESS", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_DIRECTIONCOUNT = Config.Bind("AMB", "AMB_DIRECTIONCOUNT", 2, new ConfigDescription("", new AcceptableValueRange<int>(0, 1000)));
            AMB_DIRECTLIGHTINGSTRENGTH = Config.Bind("AMB", "AMB_DIRECTLIGHTINGSTRENGTH", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_FULLRESOLUTION = Config.Bind("AMB", "AMB_FULLRESOLUTION", false, ".");
            AMB_GHOSTINGREDUCTION = Config.Bind("AMB", "AMB_GHOSTINGREDUCTION", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_INTENSITY = Config.Bind("AMB", "AMB_INTENSITY", 0f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_MAXIMUMRADIUSINPIXELS = Config.Bind("AMB", "AMB_MAXIMUMRADIUSINPIXELS", 40, new ConfigDescription("", new AcceptableValueRange<int>(0, 1000)));
            AMB_QUALITY = Config.Bind("AMB", "AMB_QUALITY", 1, new ConfigDescription("", new AcceptableValueRange<int>(0, 10)));
            AMB_RADIUS = Config.Bind("AMB", "AMB_RADIUS", 2f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_RAYLENGTH = Config.Bind("AMB", "AMB_RAYLENGTH", 3f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_RAYTRACING = Config.Bind("AMB", "AMB_RAYTRACING", false, ".");
            AMB_SAMPLECOUNT = Config.Bind("AMB", "AMB_SAMPLECOUNT", 2, new ConfigDescription("", new AcceptableValueRange<int>(0, 1000)));
            AMB_SPATIALBILATERALAGGRESSIVENESS = Config.Bind("AMB", "AMB_SPATIALBILATERALAGGRESSIVENESS", 0.15f, new ConfigDescription("", new AcceptableValueRange<float>(0f, 1000)));
            AMB_STEPCOUNT = Config.Bind("AMB", "AMB_STEPCOUNT", 6, new ConfigDescription("", new AcceptableValueRange<int>(0, 1000)));
            AMB_TEMPORALACCUMULATION = Config.Bind("AMB", "AMB_TEMPORALACCUMULATION", true, ".");
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
                        DO_AMB();
                    }
                }
            }
        }
        public void DO_AMB() 
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            amb = (AmbientOcclusion)VOLUMES.profile.components[3];
            amb.active = true;
            amb.blurSharpness.value = AMB_BLURSHARPNESS.Value;
            amb.bilateralUpsample = AMB_BILATERALUPSAMPLE.Value;
            amb.denoise = AMB_DENOISE.Value;
            amb.denoiserRadius = AMB_DENOISERRADIUS.Value;
            amb.directionCount = AMB_DIRECTIONCOUNT.Value;
            amb.directLightingStrength.value = AMB_DIRECTLIGHTINGSTRENGTH.Value;
            amb.fullResolution = AMB_FULLRESOLUTION.Value;
            amb.ghostingReduction.value = AMB_GHOSTINGREDUCTION.Value;
            amb.intensity.value = AMB_INTENSITY.Value;
            amb.maximumRadiusInPixels = AMB_MAXIMUMRADIUSINPIXELS.Value;
            amb.quality.value = AMB_QUALITY.Value;
            amb.radius.value = AMB_RADIUS.Value;
            amb.rayLength = AMB_RAYLENGTH.Value;
            amb.rayTracing.value = AMB_RAYTRACING.Value;
            amb.sampleCount = AMB_SAMPLECOUNT.Value;
            amb.spatialBilateralAggressiveness.value = AMB_SPATIALBILATERALAGGRESSIVENESS.Value;
            amb.stepCount = AMB_STEPCOUNT.Value;
            amb.temporalAccumulation.value = AMB_TEMPORALACCUMULATION.Value;
        }
    }
}
