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

namespace LC_FOG
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class FOG : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_FOG", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public FOG()
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
        public Fog fog;
        public ConfigEntry<Color> FOG_ALBEDO;
        public ConfigEntry<bool> FOG_ALBEDO_HDR;
        public ConfigEntry<float> FOG_ANISOTROPY;
        public ConfigEntry<float> FOG_BASEHEIGHT;
        public ConfigEntry<Color> FOG_COLOR;
        public ConfigEntry<bool> FOG_COLOR_HDR;
        public ConfigEntry<FogColorMode> FOG_COLORMODE;
        public ConfigEntry<FogDenoisingMode> FOG_DENOISINGMODE;
        public ConfigEntry<float> FOG_DEPTHEXTENT;
        public ConfigEntry<bool> FOG_DIRECTIONALLIGHTSONLY;
        public ConfigEntry<bool> FOG_ENABLEVOLUMETRICFOG;
        public ConfigEntry<bool> FOG_FILTER;
        public ConfigEntry<FogControl> FOG_FOGCONTROLMODE;
        public ConfigEntry<float> FOG_GLOBALLIGHTPROBEDIMMER;
        public ConfigEntry<float> FOG_MAXFOGDISTANCE;
        public ConfigEntry<float> FOG_MAXIMUMHEIGHT;
        public ConfigEntry<float> FOG_MEANFREEPATH;
        public ConfigEntry<float> FOG_MIPFOGFAR;
        public ConfigEntry<float> FOG_MIPFOGMAXMIP;
        public ConfigEntry<float> FOG_MIPFOGNEAR;
        public ConfigEntry<float> FOG_M_RESOLUTIONDEPTHRATIO;
        public ConfigEntry<int> FOG_QUALITY;
        public ConfigEntry<float> FOG_SCREENRESOLUTIONPERCENTAGE;
        public ConfigEntry<float> FOG_SLICEDISTRIBUTIONUNIFORMITY;
        public ConfigEntry<Color> FOG_TINT;
        public ConfigEntry<bool> FOG_TINT_HDR;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            FOG_ALBEDO = Config.Bind("FOG", "FOG_ALBEDO", new Color(1, 1, 1, 1), "");
            FOG_ALBEDO_HDR = Config.Bind("FOG", "FOG_ALBEDO_HDR", false, "");
            FOG_ANISOTROPY = Config.Bind("FOG", "FOG_ANISOTROPY", 0.71f,"");
            FOG_BASEHEIGHT = Config.Bind("FOG", "FOG_BASEHEIGHT", -100f, "");
            FOG_COLOR = Config.Bind("FOG", "FOG_COLOR", new Color(0.5f, 0.5f, 0.5f, 1),"");
            FOG_COLOR_HDR = Config.Bind("FOG", "FOG_COLOR_HDR", false, "");
            FOG_COLORMODE = Config.Bind("FOG", "FOG_COLORMODE", FogColorMode.SkyColor, "");
            FOG_DENOISINGMODE = Config.Bind("FOG", "FOG_DENOISINGMODE", FogDenoisingMode.Gaussian, "");
            FOG_DEPTHEXTENT = Config.Bind("FOG", "FOG_DEPTHEXTENT", 64f, "");
            FOG_DIRECTIONALLIGHTSONLY = Config.Bind("FOG", "FOG_DIRECTIONALLIGHTSONLY", false, "");
            FOG_ENABLEVOLUMETRICFOG = Config.Bind("FOG", "FOG_ENABLEVOLUMETRICFOG", false, "");
            FOG_FILTER = Config.Bind("FOG", "FOG_FILTER", false, "");
            FOG_FOGCONTROLMODE = Config.Bind("FOG", "FOG_FOGCONTROLMODE", FogControl.Balance, "");
            FOG_GLOBALLIGHTPROBEDIMMER = Config.Bind("FOG", "FOG_GLOBALLIGHTPROBEDIMMER", 1f, ""); ;
            FOG_MAXFOGDISTANCE = Config.Bind("FOG", "FOG_MAXFOGDISTANCE", 5000f, "");
            FOG_MAXIMUMHEIGHT = Config.Bind("FOG", "FOG_MAXIMUMHEIGHT", 400f, "");
            FOG_MEANFREEPATH = Config.Bind("FOG", "FOG_MEANFREEPATH", 1000f, "");
            FOG_MIPFOGFAR = Config.Bind("FOG", "FOG_MIPFOGFAR", 1000f, "");
            FOG_MIPFOGMAXMIP = Config.Bind("FOG", "FOG_MIPFOGMAXMIP", 0.5f, "");
            FOG_MIPFOGNEAR = Config.Bind("FOG", "FOG_MIPFOGNEAR", 0f, "");
            FOG_M_RESOLUTIONDEPTHRATIO = Config.Bind("FOG", "FOG_M_RESOLUTIONDEPTHRATIO", 0.666f,"");
            FOG_QUALITY = Config.Bind("FOG", "FOG_QUALITY", 1, "");
            FOG_SCREENRESOLUTIONPERCENTAGE = Config.Bind("FOG", "FOG_SCREENRESOLUTIONPERCENTAGE", 12.5f,"");
            FOG_SLICEDISTRIBUTIONUNIFORMITY = Config.Bind("FOG", "FOG_SLICEDISTRIBUTIONUNIFORMITY", 0.75f,"");
            FOG_TINT = Config.Bind("FOG", "FOG_TINT", new Color(1, 1, 1, 1), "");
            FOG_TINT_HDR = Config.Bind("FOG", "FOG_TINT_HDR", false, "");
        }
        public static Volume VOLUMES;
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_FOG();
                }
            }
        }
        public void DO_FOG() 
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            fog = (Fog)VOLUMES.profile.components[2];
            fog.active = true;
            fog.albedo.value = FOG_ALBEDO.Value;
            fog.albedo.hdr = FOG_ALBEDO_HDR.Value;
            fog.anisotropy.value = FOG_ANISOTROPY.Value;
            fog.baseHeight.value = FOG_BASEHEIGHT.Value;
            fog.color.value = FOG_COLOR.Value;
            fog.color.hdr = FOG_COLOR_HDR.Value;
            fog.colorMode.value = FOG_COLORMODE.Value;
            fog.denoisingMode.value = FOG_DENOISINGMODE.Value;
            fog.depthExtent.value = FOG_DEPTHEXTENT.Value;
            fog.directionalLightsOnly.value = FOG_DIRECTIONALLIGHTSONLY.Value;
            fog.enableVolumetricFog.value = FOG_ENABLEVOLUMETRICFOG.Value;
            fog.filter.value = FOG_FILTER.Value;
            fog.fogControlMode = FOG_FOGCONTROLMODE.Value; ;
            fog.globalLightProbeDimmer.value = FOG_GLOBALLIGHTPROBEDIMMER.Value;
            fog.maxFogDistance.value = FOG_MAXFOGDISTANCE.Value;
            fog.maximumHeight.value = FOG_MAXIMUMHEIGHT.Value;
            fog.meanFreePath.value = FOG_MEANFREEPATH.Value;
            fog.mipFogFar.value = FOG_MIPFOGFAR.Value;
            fog.mipFogMaxMip.value = FOG_MIPFOGMAXMIP.Value;
            fog.mipFogNear.value = FOG_MIPFOGNEAR.Value;
            fog.m_ResolutionDepthRatio.value = FOG_M_RESOLUTIONDEPTHRATIO.Value;
            fog.quality.value = FOG_QUALITY.Value;
            fog.screenResolutionPercentage.value = FOG_SCREENRESOLUTIONPERCENTAGE.Value;
            fog.sliceDistributionUniformity.value = FOG_SLICEDISTRIBUTIONUNIFORMITY.Value;
            fog.tint.value = FOG_TINT.Value;
            fog.tint.hdr = FOG_TINT_HDR.Value;
        }
    }
}
