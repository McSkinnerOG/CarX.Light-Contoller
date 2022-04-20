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

namespace LC_EXP
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class EXP : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_EXP", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public EXP()
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
        public Exposure exp;
        public ConfigEntry<AdaptationMode> EXP_ADAPTATIONMODE;
        public ConfigEntry<float> EXP_ADAPTATIONSPEEDDARKTOLIGHT;
        public ConfigEntry<float> EXP_ADAPTATIONSPEEDLIGHTTODARK;
        public ConfigEntry<bool> EXP_CENTERAROUNDEXPOSURETARGET;
        public ConfigEntry<float> EXP_COMPENSATION;
        public ConfigEntry<float> EXP_FIXEDEXPOSURE;
        public ConfigEntry<Vector2> EXP_HISTOGRAMPERCENTAGES;
        public ConfigEntry<bool> EXP_HISTOGRAMUSECURVEREMAPPING;
        public ConfigEntry<float> EXP_LIMITMAX;
        public ConfigEntry<float> EXP_LIMITMIN;
        public ConfigEntry<LuminanceSource> EXP_LUMINANCESOURCE;
        public ConfigEntry<float> EXP_MASKMAXINTENSITY;
        public ConfigEntry<float> EXP_MASKMININTENSITY;
        public ConfigEntry<MeteringMode> EXP_METERINGMODE;
        public ConfigEntry<ExposureMode> EXP_MODE;
        public ConfigEntry<Vector2> EXP_PROCEDURALCENTER;
        public ConfigEntry<Vector2> EXP_PROCEDURALRADII;
        public ConfigEntry<float> EXP_PROCEDURALSOFTNESS;
        public ConfigEntry<TargetMidGray> EXP_TARGETMIDGRAY;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            EXP_ADAPTATIONMODE = Config.Bind("EXP", "EXP_ADAPTATIONMODE", AdaptationMode.Progressive, "");
            EXP_ADAPTATIONSPEEDDARKTOLIGHT = Config.Bind("EXP", "EXP_ADAPTATIONSPEEDDARKTOLIGHT", 3f, new ConfigDescription("", new AcceptableValueRange<float>(0, 100f)));
            EXP_ADAPTATIONSPEEDLIGHTTODARK = Config.Bind("EXP", "EXP_ADAPTATIONSPEEDLIGHTTODARK", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0, 100f)));
            EXP_CENTERAROUNDEXPOSURETARGET = Config.Bind("EXP", "EXP_CENTERAROUNDEXPOSURETARGET", false, "");
            EXP_COMPENSATION = Config.Bind("EXP", "EXP_COMPENSATION", 0.66f, new ConfigDescription("", new AcceptableValueRange<float>(0, 10f)));
            EXP_FIXEDEXPOSURE = Config.Bind("EXP", "EXP_FIXEDEXPOSURE", 0f, new ConfigDescription("", new AcceptableValueRange<float>(0, 10f)));
            EXP_HISTOGRAMPERCENTAGES = Config.Bind("EXP", "EXP_HISTOGRAMPERCENTAGES", new Vector2(40,90), "");
            EXP_HISTOGRAMUSECURVEREMAPPING = Config.Bind("EXP", "EXP_HISTOGRAMUSECURVEREMAPPING", false, "");
            EXP_LIMITMAX = Config.Bind("EXP", "EXP_LIMITMAX", 2.15f, new ConfigDescription("", new AcceptableValueRange<float>(0, 100f)));
            EXP_LIMITMIN = Config.Bind("EXP", "EXP_LIMITMIN", 0.65f, new ConfigDescription("", new AcceptableValueRange<float>(0, 100f)));
            EXP_LUMINANCESOURCE = Config.Bind("EXP", "EXP_LUMINANCESOURCE", LuminanceSource.ColorBuffer, "");
            EXP_MASKMAXINTENSITY = Config.Bind("EXP", "EXP_MASKMAXINTENSITY", 30f, new ConfigDescription("", new AcceptableValueRange<float>(-100, 100)));
            EXP_MASKMININTENSITY = Config.Bind("EXP", "EXP_MASKMININTENSITY", -30f, new ConfigDescription("", new AcceptableValueRange<float>(-100, 100f)));
            EXP_METERINGMODE = Config.Bind("EXP", "EXP_METERINGMODE", MeteringMode.CenterWeighted, "");
            EXP_MODE = Config.Bind("EXP", "EXP_MODE", ExposureMode.Automatic, "");
            EXP_PROCEDURALCENTER = Config.Bind("EXP", "EXP_PROCEDURALCENTER", new Vector2(0.5f, 0.5f), "");
            EXP_PROCEDURALRADII = Config.Bind("EXP", "EXP_PROCEDURALRADII", new Vector2(0.3f, 0.3f), "");
            EXP_PROCEDURALSOFTNESS = Config.Bind("EXP", "EXP_PROCEDURALSOFTNESS", 0.5f, new ConfigDescription("", new AcceptableValueRange<float>(0, 100f)));
            EXP_TARGETMIDGRAY = Config.Bind("EXP", "EXP_TARGETMIDGRAY", TargetMidGray.Grey125, "");
        }
        public static Volume VOLUMES;
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_EXP();
                }
            }
        }
        public GameObject SUN_GO;
        public void DO_EXP() 
        {
            if (SUN_GO != null)
            {
                if (!SUN_GO.transform.parent.name.Contains("night")) 
                {
                    VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
                    exp = (Exposure)VOLUMES.profile.components[3];
                    exp.adaptationMode.value = EXP_ADAPTATIONMODE.Value;
                    exp.adaptationSpeedDarkToLight.value = EXP_ADAPTATIONSPEEDDARKTOLIGHT.Value;
                    exp.adaptationSpeedLightToDark.value = EXP_ADAPTATIONSPEEDLIGHTTODARK.Value;
                    exp.centerAroundExposureTarget.value = EXP_CENTERAROUNDEXPOSURETARGET.Value;
                    exp.compensation.value = EXP_COMPENSATION.Value;
                    exp.fixedExposure.value = EXP_FIXEDEXPOSURE.Value;
                    exp.histogramPercentages.value = EXP_HISTOGRAMPERCENTAGES.Value;
                    exp.histogramUseCurveRemapping.value = EXP_HISTOGRAMUSECURVEREMAPPING.Value;
                    exp.limitMax.value = EXP_LIMITMAX.Value;
                    exp.limitMin.value = EXP_LIMITMIN.Value;
                    exp.luminanceSource.value = EXP_LUMINANCESOURCE.Value;
                    exp.maskMaxIntensity.value = EXP_MASKMAXINTENSITY.Value;
                    exp.maskMinIntensity.value = EXP_MASKMININTENSITY.Value;
                    exp.meteringMode.value = EXP_METERINGMODE.Value;
                    exp.mode.value = EXP_MODE.Value;
                    exp.proceduralCenter.value = EXP_PROCEDURALCENTER.Value;
                    exp.proceduralRadii.value = EXP_PROCEDURALRADII.Value;
                    exp.proceduralSoftness.value = EXP_PROCEDURALSOFTNESS.Value;
                    exp.targetMidGray.value = EXP_TARGETMIDGRAY.Value;
                }
                if (SUN_GO.transform.parent.name.Contains("night")) 
                {
                    VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
                    exp = (Exposure)VOLUMES.profile.components[5];
                    exp.adaptationMode.value = EXP_ADAPTATIONMODE.Value;
                    exp.adaptationSpeedDarkToLight.value = EXP_ADAPTATIONSPEEDDARKTOLIGHT.Value;
                    exp.adaptationSpeedLightToDark.value = EXP_ADAPTATIONSPEEDLIGHTTODARK.Value;
                    exp.centerAroundExposureTarget.value = EXP_CENTERAROUNDEXPOSURETARGET.Value;
                    exp.compensation.value = EXP_COMPENSATION.Value;
                    exp.fixedExposure.value = EXP_FIXEDEXPOSURE.Value;
                    exp.histogramPercentages.value = EXP_HISTOGRAMPERCENTAGES.Value;
                    exp.histogramUseCurveRemapping.value = EXP_HISTOGRAMUSECURVEREMAPPING.Value;
                    exp.limitMax.value = EXP_LIMITMAX.Value;
                    exp.limitMin.value = EXP_LIMITMIN.Value;
                    exp.luminanceSource.value = EXP_LUMINANCESOURCE.Value;
                    exp.maskMaxIntensity.value = EXP_MASKMAXINTENSITY.Value;
                    exp.maskMinIntensity.value = EXP_MASKMININTENSITY.Value;
                    exp.meteringMode.value = EXP_METERINGMODE.Value;
                    exp.mode.value = EXP_MODE.Value;
                    exp.proceduralCenter.value = EXP_PROCEDURALCENTER.Value;
                    exp.proceduralRadii.value = EXP_PROCEDURALRADII.Value;
                    exp.proceduralSoftness.value = EXP_PROCEDURALSOFTNESS.Value;
                    exp.targetMidGray.value = EXP_TARGETMIDGRAY.Value;
                }
            }
            else {
                SUN_GO = GameObject.Find("sunlight");
            }
            if (SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("night"))
            {
                
            }
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("night")){
                
            } 
        }
    }
}
