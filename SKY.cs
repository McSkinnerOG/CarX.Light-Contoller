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

namespace LC_SKY
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class SKY : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_SKY", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public SKY()
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
        public HDRISky sky; 
        public ConfigEntry<BackplateType> SKY_BACKPLATETYPE;
        public ConfigEntry<float> SKY_BLENDAMOUNT;
        public ConfigEntry<float> SKY_DESIREDLUXVALUE;
        public ConfigEntry<bool> SKY_DIRLIGHTSHADOW;
        public ConfigEntry<bool> SKY_ENABLEBACKPLATE;
        public ConfigEntry<bool> SKY_ENABLEDISTORTION;
        public ConfigEntry<float> SKY_EXPOSURE; 
        public ConfigEntry<float> SKY_GROUNDLEVEL;
        public ConfigEntry<bool> SKY_INCLUDESUNINBAKING;
        public ConfigEntry<float> SKY_MULTIPLIER;
        public ConfigEntry<float> SKY_PLATEROTATION;
        public ConfigEntry<Vector2> SKY_PLATETEXOFFSET;
        public ConfigEntry<float> SKY_PLATETEXROTATION;
        public ConfigEntry<bool> SKY_POINTLIGHTSHADOW;
        public ConfigEntry<bool> SKY_PROCEDURAL;
        public ConfigEntry<float> SKY_PROJECTIONDISTANCE;
        public ConfigEntry<bool> SKY_RECTLIGHTSHADOW;
        public ConfigEntry<float> SKY_ROTATION;
        public ConfigEntry<Vector2> SKY_SCALE;
        public ConfigEntry<int> SKY_SCROLLDIRECTION;
        public ConfigEntry<float> SKY_SCROLLSPEED;
        public ConfigEntry<Color> SKY_SHADOWTINT;
        public ConfigEntry<SkyIntensityMode> SKY_SKYINTENSITYMODE;
        public ConfigEntry<EnvironmentUpdateMode> SKY_UPDATEMODE;
        public ConfigEntry<int> SKY_UPDATEPERIOD;
        public ConfigEntry<Vector3> SKY_UPPERHEMISPHERELUXCOLOR;
        public ConfigEntry<float> SKY_UPPERHEMISPHERELUXVALUE;
        public ConfigEntry<bool> SKY_UPPERHEMISPHEREONLY;
        public ConfigEntry<bool> SKY_MOVER;
        public ConfigEntry<float> SKY_ROTATION_SPEED;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            SKY_BACKPLATETYPE = Config.Bind("SKY", "SKY_BACKPLATETYPE", BackplateType.Disc, "");
            SKY_BLENDAMOUNT = Config.Bind("SKY", "SKY_BLENDAMOUNT", 0f, "");
            SKY_DESIREDLUXVALUE = Config.Bind("SKY", "SKY_DESIREDLUXVALUE", 20000f, "");
            SKY_DIRLIGHTSHADOW = Config.Bind("SKY", "SKY_DIRLIGHTSHADOW", false, "");
            SKY_ENABLEBACKPLATE = Config.Bind("SKY", "SKY_ENABLEBACKPLATE", false, "");
            SKY_ENABLEDISTORTION = Config.Bind("SKY", "SKY_ENABLEDISTORTION", false, "");
            SKY_EXPOSURE = Config.Bind("SKY", "SKY_EXPOSURE", 1.6087f, "");
            SKY_GROUNDLEVEL = Config.Bind("SKY", "SKY_GROUNDLEVEL", 0f, "");
            SKY_INCLUDESUNINBAKING = Config.Bind("SKY", "SKY_INCLUDESUNINBAKING", false, "");
            SKY_MULTIPLIER = Config.Bind("SKY", "SKY_MULTIPLIER", 1f, "");
            SKY_PLATEROTATION = Config.Bind("SKY", "SKY_PLATEROTATION", 0f, "");
            SKY_PLATETEXOFFSET = Config.Bind("SKY", "SKY_PLATETEXOFFSET", new Vector2(0.0f, 0.0f), "");
            SKY_PLATETEXROTATION = Config.Bind("SKY", "SKY_PLATETEXROTATION", 1f, new ConfigDescription("Sky Plate Texture Rotation.", new AcceptableValueRange<float>(0f, 1024f)));
            SKY_POINTLIGHTSHADOW = Config.Bind("SKY", "SKY_POINTLIGHTSHADOW", false, "");
            SKY_PROCEDURAL = Config.Bind("SKY", "SKY_PROCEDURAL", false, "");
            SKY_PROJECTIONDISTANCE = Config.Bind("SKY", "SKY_PROJECTIONDISTANCE", 16f, "");
            SKY_RECTLIGHTSHADOW = Config.Bind("SKY", "SKY_RECTLIGHTSHADOW", false, "");
            SKY_ROTATION = Config.Bind("SKY", "SKY_ROTATION", 45.4f, "");
            SKY_SCALE = Config.Bind("SKY", "SKY_SCALE", new Vector2(32, 32), "");
            SKY_SCROLLDIRECTION = Config.Bind("SKY", "SKY_SCROLLDIRECTION", 0, "");
            SKY_SCROLLSPEED = Config.Bind("SKY", "SKY_SCROLLSPEED", 2f, "");
            SKY_SHADOWTINT = Config.Bind("SKY", "SKY_SHADOWTINT", new Color(0.500f, 0.500f, 0.500f, 1f), "");
            SKY_SKYINTENSITYMODE = Config.Bind("SKY", "SKY_SKYINTENSITYMODE", SkyIntensityMode.Exposure, "");
            SKY_UPDATEMODE = Config.Bind("SKY", "SKY_UPDATEMODE", EnvironmentUpdateMode.OnChanged, "");
            SKY_UPDATEPERIOD = Config.Bind("SKY", "SKY_UPDATEPERIOD", 0, "");
            SKY_UPPERHEMISPHERELUXCOLOR = Config.Bind("SKY", "SKY_UPPERHEMISPHERELUXCOLOR", new Vector3(0.3f, 0.4f, 0.5f), "");
            SKY_UPPERHEMISPHERELUXVALUE = Config.Bind("SKY", "SKY_UPPERHEMISPHERELUXVALUE", 0.3784f, "");
            SKY_UPPERHEMISPHEREONLY = Config.Bind("SKY", "SKY_UPPERHEMISPHEREONLY", true, "");
            SKY_MOVER = Config.Bind("SKY", "SKY_MOVER", false, "");
            SKY_ROTATION_SPEED = Config.Bind("SKY", "SKY_ROTATION_SPEED", 0.1f, new ConfigDescription("", new AcceptableValueRange<float>(0.001f, 0.1f)));
            //EXP_MASKMININTENSITY = Config.Bind("EXP", "EXP_MASKMININTENSITY", -30f, new ConfigDescription("", new AcceptableValueRange<float>(-100, 100f)));
        }
        public static Volume VOLUMES;
        public void Update() 
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_SKY();
                }
                if (SKY_MOVER.Value == true) 
                {
                    SkyMover();
                }
            }
        }

        public void SkyMover()
        { 
            sky.rotation.Override(sky.rotation.value += SKY_ROTATION_SPEED.Value); 
        }


        public void DO_SKY()
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            sky = (HDRISky)VOLUMES.profile.components[0];
            sky.active = true;
            sky.backplateType.value = SKY_BACKPLATETYPE.Value;
            sky.blendAmount.value = SKY_BLENDAMOUNT.Value;
            sky.desiredLuxValue.value = SKY_DESIREDLUXVALUE.Value;
            sky.dirLightShadow.value = SKY_DIRLIGHTSHADOW.Value;
            sky.enableBackplate.value = SKY_ENABLEBACKPLATE.Value;
            sky.enableDistortion.value = SKY_ENABLEDISTORTION.Value;
            sky.exposure.value = SKY_EXPOSURE.Value;
            sky.groundLevel.value = SKY_GROUNDLEVEL.Value;
            sky.includeSunInBaking.value = SKY_INCLUDESUNINBAKING.Value;
            sky.multiplier.value = SKY_MULTIPLIER.Value;
            sky.plateRotation.value = SKY_PLATEROTATION.Value;
            sky.plateTexOffset.value = SKY_PLATETEXOFFSET.Value;
            sky.plateTexRotation.value = SKY_PLATETEXROTATION.Value;
            sky.pointLightShadow.value = SKY_POINTLIGHTSHADOW.Value;
            sky.procedural.value = SKY_PROCEDURAL.Value;
            sky.projectionDistance.value = SKY_PROJECTIONDISTANCE.Value;
            sky.rectLightShadow.value = SKY_RECTLIGHTSHADOW.Value;
            sky.rotation.value = SKY_ROTATION.Value;
            sky.scale.value = SKY_SCALE.Value;
            sky.scrollDirection.value = SKY_SCROLLDIRECTION.Value;
            sky.scrollSpeed.value = SKY_SCROLLSPEED.Value;
            sky.shadowTint.value = SKY_SHADOWTINT.Value;
            sky.skyIntensityMode.value = SKY_SKYINTENSITYMODE.Value;
            sky.updateMode.value = SKY_UPDATEMODE.Value;
            sky.updatePeriod.value = SKY_UPDATEPERIOD.Value;
            sky.upperHemisphereLuxColor.value = SKY_UPPERHEMISPHERELUXCOLOR.Value;
            sky.upperHemisphereLuxValue.value = SKY_UPPERHEMISPHERELUXVALUE.Value;
            sky.upperHemisphereOnly.value = SKY_UPPERHEMISPHEREONLY.Value;
        }
    }
}
