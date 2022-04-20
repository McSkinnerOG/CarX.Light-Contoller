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

namespace LC_SUN
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class SUN : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_SUN", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public SUN()
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
        public Light C_LIGHT;
        public GameObject SUN_GO;
        public ConfigEntry<Color> SUN_LIGHT_COLOR;
        public ConfigEntry<float> SUN_LIGHT_INNER_SPOT_ANGLE; 
        public ConfigEntry<float> SUN_LIGHT_INTENSITY;
        public ConfigEntry<LightShadowCasterMode> SUN_LIGHT_SHADOW_CASTERMODE;
        public ConfigEntry<LightRenderMode> SUN_LIGHT_RENDERMODE;
        public ConfigEntry<LightShape> SUN_LIGHT_SHAPE;
        public ConfigEntry<float> SUN_LIGHT_SPOT_ANGLE;
        public ConfigEntry<LightType> SUN_LIGHT_TYPE;
        //SHADOW
        public ConfigEntry<float> SHADOW_BIAS;
        public ConfigEntry<int> SHADOW_RES_CUSTOM;
        public ConfigEntry<float> SHADOW_NEAR_PLANE;
        public ConfigEntry<float> SHADOW_NORMAL_BIAS;
        public ConfigEntry<LightShadowResolution> SHADOW_RES;
        public ConfigEntry<LightShadows> SHADOWS;
        public ConfigEntry<float> SHADOW_SOFTNESS;
        public ConfigEntry<float> SHADOW_SOFTNESS_FADE;
        public ConfigEntry<float> SHADOW_STRENGTH;
         
        public ConfigEntry<float> LIGHT_ROT_X;
        public ConfigEntry<float> LIGHT_ROT_Y; 

        public ConfigEntry<string> LIGHT_NAME;
        public ConfigEntry<bool> NIGHT;
        public ConfigEntry<bool> USE_CUSTOM_COLOR;
        public DayNightController DNC;
        
        public ConfigEntry<bool> REFLECTION_PROBES;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            USE_CUSTOM_COLOR = Config.Bind("LIGHT", "CUSTOMS TOGGLE", false, "Custom Color Use toggle."); 
            // LIGHT
            SUN_LIGHT_COLOR = Config.Bind("LIGHT", "Light Color", new Color(1, 1, 1), new ConfigDescription("Color/Hue emmited by the 'sun'."));
            SUN_LIGHT_INNER_SPOT_ANGLE = Config.Bind("LIGHT", "Light Inner Spot Angle", 21.8021f, new ConfigDescription("Light Inner Spot Angle.", new AcceptableValueRange<float>(0, 360f)));
            SUN_LIGHT_INTENSITY = Config.Bind("LIGHT", "Light Intensity", 1f, new ConfigDescription("Light Intensity.", new AcceptableValueRange<float>(1, 1000f)));
            SUN_LIGHT_SHADOW_CASTERMODE = Config.Bind("LIGHT", "Light Shadow Caster", LightShadowCasterMode.Everything, "");
            SUN_LIGHT_RENDERMODE = Config.Bind("LIGHT", "Light Render Mode", LightRenderMode.Auto, "");
            SUN_LIGHT_SHAPE = Config.Bind("LIGHT", "Light Shape", LightShape.Cone, "");
            SUN_LIGHT_SPOT_ANGLE = Config.Bind("LIGHT", "Spot Angle", 30f, new ConfigDescription("Light Spot Angle.", new AcceptableValueRange<float>(0f, 360f)));
            SUN_LIGHT_TYPE = Config.Bind("LIGHT", "Light Type", LightType.Directional, "");
            // SHADOW
            SHADOW_BIAS = Config.Bind("SHADOW", "Light Shadow Bias", 0.05f, new ConfigDescription("Shadow Bias.", new AcceptableValueRange<float>(0.01f, 1f)));
            SHADOW_RES_CUSTOM = Config.Bind("SHADOW", "Shadow Custom Res", -1, new ConfigDescription("Shadow Custom Res.", new AcceptableValueRange<int>(-1, 1024)));
            SHADOW_NEAR_PLANE = Config.Bind("SHADOW", "Shadow Near Plane", 0.2f, new ConfigDescription("Shadow Near Plane.", new AcceptableValueRange<float>(0.01f, 10)));
            SHADOW_NORMAL_BIAS = Config.Bind("SHADOW", "Shadow Normal Bias", 0.4f, new ConfigDescription("Shadow Normal Bias.", new AcceptableValueRange<float>(0.01f, 10)));
            SHADOW_RES = Config.Bind("SHADOW", "Shadow Res", LightShadowResolution.FromQualitySettings, "");
            SHADOWS = Config.Bind("SHADOW", "Shadow Type", LightShadows.Soft, "");
            SHADOW_SOFTNESS = Config.Bind("SHADOW", "Shadow Softness", 4f, new ConfigDescription("Shadow Softness.", new AcceptableValueRange<float>(0f, 1024f)));
            SHADOW_SOFTNESS_FADE = Config.Bind("SHADOW", "Shadow Softness Fade", 1f, new ConfigDescription("Shadow Softness Fade.", new AcceptableValueRange<float>(0f, 1024f)));
            SHADOW_STRENGTH = Config.Bind("SHADOW", "Shadow Stength", 1f, new ConfigDescription("Light Stength.", new AcceptableValueRange<float>(0f, 1024f)));
 
            NIGHT = Config.Bind("LIGHT", "NIGHT TOGGLE", false, "Night toggle.");
            LIGHT_NAME = Config.Bind(new ConfigDefinition("LIGHT", "Light Scene"), "vp_airfield", new ConfigDescription("Lighting Scene to use!", new AcceptableValueList<string>("vp_airfield", "vp_atron", "vp_bathurst", "vp_ebisu", "vp_fiorano", "vp_irwindale", "vp_japan", "vp_losAngeles", "vp_pacifichills", "vp_parking", "vp_petersburg", "vp_redring", "vp_redrock", "vp_showroom", "vp_silverstone", "vp_winterfell", "vp_nring", "vp_mondello")));
            
            LIGHT_ROT_X = Config.Bind("LIGHT", "Light X Rotation", 45f, new ConfigDescription("Light Rotation.", new AcceptableValueRange<float>(0, 360f)));
            LIGHT_ROT_Y = Config.Bind("LIGHT", "Light Y Rotation", 45f, new ConfigDescription("Light Rotation.", new AcceptableValueRange<float>(0, 360f)));
            REFLECTION_PROBES = Config.Bind("LIGHT", "PROBES TOGGLE", false, "Probes toggle.");
        }
        public GameObject[] reflectionProbes;
        public static Volume VOLUMES;
        public void DO_REFS() 
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            reflectionProbes = (from x in UnityEngine.Object.FindObjectsOfType<ReflectionProbe>(true) select x.gameObject).ToArray<GameObject>();
            if (reflectionProbes.ToList().Count > 0) { foreach (GameObject gameObject in reflectionProbes) { gameObject.SetActive(REFLECTION_PROBES.Value); } }
        }
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_LIGHT();
                }
            }
        }
        public void DO_LIGHT()
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            DNC = GameObject.Find("lighting_controller").GetComponent<DayNightController>();
            if (NIGHT.Value == true)
            {
                DNC.TryApplyMode(LIGHT_NAME.Value + "_night", !NIGHT.Value);
            }
            else 
            {
                DNC.TryApplyMode(LIGHT_NAME.Value, !NIGHT.Value);
            } 
            C_LIGHT = GameObject.Find("sunlight").GetComponent<Light>();
            SUN_GO = GameObject.Find("sunlight"); 
            SUN_GO.transform.eulerAngles = new Vector3(LIGHT_ROT_X.Value, LIGHT_ROT_Y.Value, 0f);
            C_LIGHT.color = SUN_LIGHT_COLOR.Value;
            C_LIGHT.innerSpotAngle = SUN_LIGHT_INNER_SPOT_ANGLE.Value;
            C_LIGHT.intensity = SUN_LIGHT_INTENSITY.Value;
            C_LIGHT.lightShadowCasterMode = SUN_LIGHT_SHADOW_CASTERMODE.Value;
            C_LIGHT.renderMode = SUN_LIGHT_RENDERMODE.Value;
            C_LIGHT.shadowBias = SHADOW_BIAS.Value; 
            C_LIGHT.shadowCustomResolution = SHADOW_RES_CUSTOM.Value;
            C_LIGHT.shadowNearPlane = SHADOW_NEAR_PLANE.Value;
            C_LIGHT.shadowNormalBias = SHADOW_NORMAL_BIAS.Value;
            C_LIGHT.shadowResolution = SHADOW_RES.Value;
            C_LIGHT.shadows = SHADOWS.Value;
            C_LIGHT.shadowStrength = SHADOW_STRENGTH.Value;
            C_LIGHT.shape = SUN_LIGHT_SHAPE.Value;
            C_LIGHT.spotAngle = SUN_LIGHT_SPOT_ANGLE.Value;
            C_LIGHT.type = SUN_LIGHT_TYPE.Value; 
        }

    }
}
