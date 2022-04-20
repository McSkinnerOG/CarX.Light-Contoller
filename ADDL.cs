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

namespace LC_ADDL
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class ADDL: BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_ADDL", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public ADDL()
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
        //ADD-LIGHT
        public HDAdditionalLightData ADD_LIGHT;
        public ConfigEntry<bool> ADD_LIGHT_AFFECTDIFFUSE;
        public ConfigEntry<bool> ADD_LIGHT_AFFECTSPECULAR;
        public ConfigEntry<bool> ADD_LIGHT_AFFECTSVOLUMETRIC;
        public ConfigEntry<bool> ADD_LIGHT_ALWAYSDRAWDYNAMICSHADOWS;
        public ConfigEntry<float> ADD_LIGHT_ANGULARDIAMETER; 
        public ConfigEntry<bool> ADD_LIGHT_APPLYRANGEATTENUATION;
        public ConfigEntry<int> ADD_LIGHT_AREALIGHTEMISSIVEMESHLAYER;
        public ConfigEntry<MotionVectorGenerationMode> ADD_LIGHT_AREALIGHTEMISSIVEMESHMOTIONVECTORGENERATIONMODE;
        public ConfigEntry<ShadowCastingMode> ADD_LIGHT_AREALIGHTEMISSIVEMESHSHADOWCASTINGMODE; 
        public ConfigEntry<float> ADD_LIGHT_AREALIGHTSHADOWCONE; 
        public ConfigEntry<AreaLightShape> ADD_LIGHT_AREALIGHTSHAPE;
        public ConfigEntry<float> ADD_LIGHT_ASPECTRATIO;
        public ConfigEntry<float> ADD_LIGHT_BARNDOORANGLE;
        public ConfigEntry<float> ADD_LIGHT_BARNDOORLENGTH;
        public ConfigEntry<int> ADD_LIGHT_BLOCKERSAMPLECOUNT;
        public ConfigEntry<float> ADD_LIGHT_CACHEDSHADOWANGLEUPDATETHRESHOLD;
        public ConfigEntry<float> ADD_LIGHT_CACHEDSHADOWTRANSLATIONUPDATETHRESHOLD;
        public ConfigEntry<Color> ADD_LIGHT_COLOR;
        public ConfigEntry<bool> ADD_LIGHT_COLORSHADOW; 
        public ConfigEntry<float> ADD_LIGHT_CUSTOMSPOTLIGHTSHADOWCONE;
        public ConfigEntry<bool> ADD_LIGHT_DISPLAYAREALIGHTEMISSIVEMESH; 
        public ConfigEntry<float> ADD_LIGHT_DISTANCE; 
        public ConfigEntry<bool> ADD_LIGHT_ENABLESPOTREFLECTOR; 
        public ConfigEntry<int> ADD_LIGHT_EVSMBLURPASSES; 
        public ConfigEntry<float> ADD_LIGHT_EVSMEXPONENT; 
        public ConfigEntry<float> ADD_LIGHT_EVSMLIGHTLEAKBIAS; 
        public ConfigEntry<float> ADD_LIGHT_EVSMVARIANCEBIAS; 
        public ConfigEntry<float> ADD_LIGHT_FADEDISTANCE; 
        public ConfigEntry<int> ADD_LIGHT_FILTERSAMPLECOUNT; 
        public ConfigEntry<int> ADD_LIGHT_FILTERSIZETRACED; 
        public ConfigEntry<bool> ADD_LIGHT_FILTERTRACEDSHADOW; 
        public ConfigEntry<float> ADD_LIGHT_FLAREFALLOFF;
        public ConfigEntry<float> ADD_LIGHT_FLARESIZE; 
        public ConfigEntry<Color> ADD_LIGHT_FLARETINT; 
        public ConfigEntry<bool> ADD_LIGHT_INCLUDEFORRAYTRACING; 
        public ConfigEntry<float> ADD_LIGHT_INNERSPOTPERCENT;
        public ConfigEntry<float> ADD_LIGHT_INTENSITY;
        public ConfigEntry<bool> ADD_LIGHT_INTERACTSWITHSKY;
        public ConfigEntry<int> ADD_LIGHT_KERNELSIZE;
        public ConfigEntry<int> ADD_LIGHT_LIGHTANGLE;
        public ConfigEntry<int> ADD_LIGHT_LIGHTDIMMER; 
        public ConfigEntry<LightLayerEnum> ADD_LIGHT_LIGHTLAYERSMASK;
        public ConfigEntry<float> ADD_LIGHT_LIGHTSHADOWRADIUS;
        public ConfigEntry<LightUnit> ADD_LIGHT_LIGHTUNIT;
        public ConfigEntry<bool> ADD_LIGHT_LINKSHADOWLAYERS; 
        public ConfigEntry<float> ADD_LIGHT_LUXATDISTANCE; 
        public ConfigEntry<float> ADD_LIGHT_MAXDEPTHBIAS; 
        public ConfigEntry<float> ADD_LIGHT_MAXSMOOTHNESS; 
        public ConfigEntry<float> ADD_LIGHT_MINFILTERSIZE;
        public ConfigEntry<bool> ADD_LIGHT_NONLIGHTMAPPEDONLY; 
        public ConfigEntry<float> ADD_LIGHT_NORMALBIAS; 
        public ConfigEntry<int> ADD_LIGHT_NUMRAYTRACINGSAMPLES;
        public ConfigEntry<bool> ADD_LIGHT_PENUMBRATINT;
        public ConfigEntry<bool> ADD_LIGHT_PRESERVECACHEDSHADOW; 
        public ConfigEntry<float> ADD_LIGHT_RANGE;
        public ConfigEntry<bool> ADD_LIGHT_RAYTRACECONTACTSHADOW;
        public ConfigEntry<bool> ADD_LIGHT_SEMITRANSPARENTSHADOW; 
        public ConfigEntry<float> ADD_LIGHT_SHADOWDIMMER; 
        public ConfigEntry<float> ADD_LIGHT_SHADOWFADEDISTANCE; 
        public ConfigEntry<float> ADD_LIGHT_SHADOWNEARPLANE;
        public ConfigEntry<Color> ADD_LIGHT_SHADOWTINT; 
        public ConfigEntry<ShadowUpdateMode> ADD_LIGHT_SHADOWUPDATEMODE;
        public ConfigEntry<float> ADD_LIGHT_SHAPEHEIGHT; 
        public ConfigEntry<float> ADD_LIGHT_SHAPERADIUS;
        public ConfigEntry<float> ADD_LIGHT_SHAPEWIDTH;
        public ConfigEntry<float> ADD_LIGHT_SLOPEBIAS; 
        public ConfigEntry<float> ADD_LIGHT_SOFTNESSSCALE; 
        public ConfigEntry<float> ADD_LIGHT_SPOTIESCUTOFFPERCENT;
        public ConfigEntry<SpotLightShape> ADD_LIGHT_SPOTLIGHTSHAPE;
        public ConfigEntry<float> ADD_LIGHT_SUNLIGHTCONEANGLE;
        public ConfigEntry<Color> ADD_LIGHT_SURFACETINT;
        public ConfigEntry<HDLightType> ADD_LIGHT_TYPE;
        public ConfigEntry<bool> ADD_LIGHT_UPDATEUPONLIGHTMOVEMENT;
        public ConfigEntry<bool> ADD_LIGHT_USECUSTOMSPOTLIGHTSHADOWCONE;
        public ConfigEntry<bool> ADD_LIGHT_USERAYTRACEDSHADOWS;
        public ConfigEntry<bool> ADD_LIGHT_USESCREENSPACESHADOWS; 
        public ConfigEntry<float> ADD_LIGHT_VOLUMETRICDIMMER; 
        public ConfigEntry<float> ADD_LIGHT_VOLUMETRICFADEDISTANCE;
        public ConfigEntry<float> ADD_LIGHT_VOLUMETRICSHADOWDIMMER;
        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            ADD_LIGHT_AFFECTDIFFUSE = Config.Bind("ADD_LIGHT", "AFFECTDIFFUSE", true, "");
            ADD_LIGHT_AFFECTSPECULAR = Config.Bind("ADD_LIGHT", "AFFECTSPECULAR", true, "");
            ADD_LIGHT_AFFECTSVOLUMETRIC = Config.Bind("ADD_LIGHT", "AFFECTSVOLUMETRIC", false, "");
            ADD_LIGHT_ALWAYSDRAWDYNAMICSHADOWS = Config.Bind("ADD_LIGHT", "ALWAYSDRAWDYNAMICSHADOWS", false, "");
            ADD_LIGHT_ANGULARDIAMETER = Config.Bind("ADD_LIGHT", "ANGULARDIAMETER", 0.5f, "");
            ADD_LIGHT_APPLYRANGEATTENUATION = Config.Bind("ADD_LIGHT", "APPLYRANGEATTENUATION", true, "");
            ADD_LIGHT_AREALIGHTEMISSIVEMESHLAYER = Config.Bind("ADD_LIGHT", "AREALIGHTEMISSIVEMESHLAYER", -1, "");
            ADD_LIGHT_AREALIGHTEMISSIVEMESHMOTIONVECTORGENERATIONMODE = Config.Bind("ADD_LIGHT", "AREALIGHTEMISSIVEMESHMOTIONVECTORGENERATIONMODE", MotionVectorGenerationMode.Camera, "");
            ADD_LIGHT_AREALIGHTEMISSIVEMESHSHADOWCASTINGMODE = Config.Bind("ADD_LIGHT", "AREALIGHTEMISSIVEMESHSHADOWCASTINGMODE", ShadowCastingMode.Off, "");
            ADD_LIGHT_AREALIGHTSHADOWCONE = Config.Bind("ADD_LIGHT", "AREALIGHTSHADOWCONE", 120f, "");
            ADD_LIGHT_AREALIGHTSHAPE = Config.Bind("ADD_LIGHT", "AREALIGHTSHAPE", AreaLightShape.Rectangle, "");
            ADD_LIGHT_ASPECTRATIO = Config.Bind("ADD_LIGHT", "ASPECTRATIO", 1f, "");
            ADD_LIGHT_BARNDOORANGLE = Config.Bind("ADD_LIGHT", "BARNDOORANGLE", 90f, "");
            ADD_LIGHT_BARNDOORLENGTH = Config.Bind("ADD_LIGHT", "BARNDOORLENGTH", 0.05f, "");
            ADD_LIGHT_BLOCKERSAMPLECOUNT = Config.Bind("ADD_LIGHT", "BLOCKERSAMPLECOUNT", 24, "");
            ADD_LIGHT_CACHEDSHADOWANGLEUPDATETHRESHOLD = Config.Bind("ADD_LIGHT", "CACHEDSHADOWANGLEUPDATETHRESHOLD", 0.5f, "");
            ADD_LIGHT_CACHEDSHADOWTRANSLATIONUPDATETHRESHOLD = Config.Bind("ADD_LIGHT", "CACHEDSHADOWTRANSLATIONUPDATETHRESHOLD", 0.01f, "");
            ADD_LIGHT_COLOR = Config.Bind("ADD_LIGHT", "COLOR", new Color(1, 1, 1, 1), "");
            ADD_LIGHT_COLORSHADOW = Config.Bind("ADD_LIGHT", "COLORSHADOW", true, "");
            ADD_LIGHT_CUSTOMSPOTLIGHTSHADOWCONE = Config.Bind("ADD_LIGHT", "CUSTOMSPOTLIGHTSHADOWCONE", 30f, "");
            ADD_LIGHT_DISPLAYAREALIGHTEMISSIVEMESH = Config.Bind("ADD_LIGHT", "DISPLAYAREALIGHTEMISSIVEMESH", false, "");
            ADD_LIGHT_DISTANCE = Config.Bind("ADD_LIGHT", "DISTANCE", 150000000000f, "");
            ADD_LIGHT_ENABLESPOTREFLECTOR = Config.Bind("ADD_LIGHT", "ENABLESPOTREFLECTOR", false, "");
            ADD_LIGHT_EVSMBLURPASSES = Config.Bind("ADD_LIGHT", "EVSMBLURPASSES", 0, "");
            ADD_LIGHT_EVSMEXPONENT = Config.Bind("ADD_LIGHT", "EVSMEXPONENT", 15f, "");
            ADD_LIGHT_EVSMLIGHTLEAKBIAS = Config.Bind("ADD_LIGHT", "EVSMLIGHTLEAKBIAS", 0f, "");
            ADD_LIGHT_EVSMVARIANCEBIAS = Config.Bind("ADD_LIGHT", "EVSMVARIANCEBIAS", 0f, "");
            ADD_LIGHT_FADEDISTANCE = Config.Bind("ADD_LIGHT", "FADEDISTANCE", 10000f, "");
            ADD_LIGHT_FILTERSAMPLECOUNT = Config.Bind("ADD_LIGHT", "FILTERSAMPLECOUNT", 16, "");
            ADD_LIGHT_FILTERSIZETRACED = Config.Bind("ADD_LIGHT", "FILTERSIZETRACED", 16, "");
            ADD_LIGHT_FILTERTRACEDSHADOW = Config.Bind("ADD_LIGHT", "FILTERTRACEDSHADOW", true, "");
            ADD_LIGHT_FLAREFALLOFF = Config.Bind("ADD_LIGHT", "FLAREFALLOFF", 4f, "");
            ADD_LIGHT_FLARESIZE = Config.Bind("ADD_LIGHT", "FLARESIZE", 2f, "");
            ADD_LIGHT_FLARETINT = Config.Bind("ADD_LIGHT", "FLARETINT", new Color(1, 1, 1, 1), "");
            ADD_LIGHT_INCLUDEFORRAYTRACING = Config.Bind("ADD_LIGHT", "INCLUDEFORRAYTRACING", true, "");
            ADD_LIGHT_INNERSPOTPERCENT = Config.Bind("ADD_LIGHT", "INNERSPOTPERCENT", 0f, "");
            ADD_LIGHT_INTENSITY = Config.Bind("ADD_LIGHT", "INTENSITY", 35f, "");
            ADD_LIGHT_INTERACTSWITHSKY = Config.Bind("ADD_LIGHT", "INTERACTSWITHSKY", true, "");
            ADD_LIGHT_KERNELSIZE = Config.Bind("ADD_LIGHT", "KERNELSIZE", 5, "");
            ADD_LIGHT_LIGHTANGLE = Config.Bind("ADD_LIGHT", "LIGHTANGLE", 1, "");
            ADD_LIGHT_LIGHTDIMMER = Config.Bind("ADD_LIGHT", "LIGHTDIMMER", 1, "");
            ADD_LIGHT_LIGHTLAYERSMASK = Config.Bind("ADD_LIGHT", "LIGHTLAYERSMASK", LightLayerEnum.LightLayerDefault, "");
            ADD_LIGHT_LIGHTSHADOWRADIUS = Config.Bind("ADD_LIGHT", "LIGHTSHADOWRADIUS", 0.5f, "");
            ADD_LIGHT_LIGHTUNIT = Config.Bind("ADD_LIGHT", "LIGHTUNIT", LightUnit.Lux, "");
            ADD_LIGHT_LINKSHADOWLAYERS = Config.Bind("ADD_LIGHT", "LINKSHADOWLAYERS", true, "");
            ADD_LIGHT_LUXATDISTANCE = Config.Bind("ADD_LIGHT", "LUXATDISTANCE", 1f, "");
            ADD_LIGHT_MAXDEPTHBIAS = Config.Bind("ADD_LIGHT", "MAXDEPTHBIAS", 0.001f, "");
            ADD_LIGHT_MAXSMOOTHNESS = Config.Bind("ADD_LIGHT", "MAXSMOOTHNESS", 0.99f, "");
            ADD_LIGHT_MINFILTERSIZE = Config.Bind("ADD_LIGHT", "MINFILTERSIZE", 0.01f, "");
            ADD_LIGHT_NONLIGHTMAPPEDONLY = Config.Bind("ADD_LIGHT", "NONLIGHTMAPPEDONLY", false, "");
            ADD_LIGHT_NORMALBIAS = Config.Bind("ADD_LIGHT", "NORMALBIAS", 0.75f, "");
            ADD_LIGHT_NUMRAYTRACINGSAMPLES = Config.Bind("ADD_LIGHT", "NUMRAYTRACINGSAMPLES", 4, "");
            ADD_LIGHT_PENUMBRATINT = Config.Bind("ADD_LIGHT", "PENUMBRATINT", false, "");
            ADD_LIGHT_PRESERVECACHEDSHADOW = Config.Bind("ADD_LIGHT", "PRESERVECACHEDSHADOW", false, "");
            ADD_LIGHT_RANGE = Config.Bind("ADD_LIGHT", "RANGE", 10f, "");
            ADD_LIGHT_RAYTRACECONTACTSHADOW = Config.Bind("ADD_LIGHT", "RAYTRACECONTACTSHADOW", false, "");
            ADD_LIGHT_SEMITRANSPARENTSHADOW = Config.Bind("ADD_LIGHT", "SEMITRANSPARENTSHADOW", false, "");
            ADD_LIGHT_SHADOWDIMMER = Config.Bind("ADD_LIGHT", "SHADOWDIMMER", 1f, "");
            ADD_LIGHT_SHADOWFADEDISTANCE = Config.Bind("ADD_LIGHT", "SHADOWFADEDISTANCE", 10000f, "");
            ADD_LIGHT_SHADOWNEARPLANE = Config.Bind("ADD_LIGHT", "SHADOWNEARPLANE", 0.1f, "");
            ADD_LIGHT_SHADOWTINT = Config.Bind("ADD_LIGHT", "SHADOWTINT", new Color(0, 0, 0, 1), "");
            ADD_LIGHT_SHADOWUPDATEMODE = Config.Bind("ADD_LIGHT", "SHADOWUPDATEMODE", ShadowUpdateMode.EveryFrame, "");
            ADD_LIGHT_SHAPEHEIGHT = Config.Bind("ADD_LIGHT", "SHAPEHEIGHT", 0.5f, "");
            ADD_LIGHT_SHAPERADIUS = Config.Bind("ADD_LIGHT", "SHAPERADIUS", 0f, "");
            ADD_LIGHT_SHAPEWIDTH = Config.Bind("ADD_LIGHT", "SHAPEWIDTH", 0.5f, "");
            ADD_LIGHT_SLOPEBIAS = Config.Bind("ADD_LIGHT", "SLOPEBIAS", 0.5f, "");
            ADD_LIGHT_SOFTNESSSCALE = Config.Bind("ADD_LIGHT", "SOFTNESSSCALE", 1f, "");
            ADD_LIGHT_SPOTIESCUTOFFPERCENT = Config.Bind("ADD_LIGHT", "SPOTIESCUTOFFPERCENT", 100f, "");
            ADD_LIGHT_SPOTLIGHTSHAPE = Config.Bind("ADD_LIGHT", "SPOTLIGHTSHAPE", SpotLightShape.Cone, "");
            ADD_LIGHT_SUNLIGHTCONEANGLE = Config.Bind("ADD_LIGHT", "SUNLIGHTCONEANGLE", 0.5f, "");
            ADD_LIGHT_SURFACETINT = Config.Bind("ADD_LIGHT", "SURFACETINT", new Color(1, 1, 1, 1), "");
            ADD_LIGHT_TYPE = Config.Bind("ADD_LIGHT", "TYPE", HDLightType.Directional, "");
            ADD_LIGHT_UPDATEUPONLIGHTMOVEMENT = Config.Bind("ADD_LIGHT", "UPDATEUPONLIGHTMOVEMENT", false, "");
            ADD_LIGHT_USECUSTOMSPOTLIGHTSHADOWCONE = Config.Bind("ADD_LIGHT", "USECUSTOMSPOTLIGHTSHADOWCONE", false, "");
            ADD_LIGHT_USERAYTRACEDSHADOWS = Config.Bind("ADD_LIGHT", "USERAYTRACEDSHADOWS", false, "");
            ADD_LIGHT_USESCREENSPACESHADOWS = Config.Bind("ADD_LIGHT", "USESCREENSPACESHADOWS", false, "");
            ADD_LIGHT_VOLUMETRICDIMMER = Config.Bind("ADD_LIGHT", "VOLUMETRICDIMMER", 0f, "");
            ADD_LIGHT_VOLUMETRICFADEDISTANCE = Config.Bind("ADD_LIGHT", "VOLUMETRICFADEDISTANCE", 10000f, "");
            ADD_LIGHT_VOLUMETRICSHADOWDIMMER = Config.Bind("ADD_LIGHT", "VOLUMETRICSHADOWDIMMER", 0f, "");
        }
        public static Volume VOLUMES;
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_ADDL();
                }
            }
        }
        public void DO_ADDL()
        {
            ADD_LIGHT = GameObject.Find("sunlight").GetComponent<HDAdditionalLightData>();
            ADD_LIGHT.affectDiffuse = ADD_LIGHT_AFFECTDIFFUSE.Value;
            ADD_LIGHT.affectSpecular = ADD_LIGHT_AFFECTSPECULAR.Value;
            ADD_LIGHT.affectsVolumetric = ADD_LIGHT_AFFECTSVOLUMETRIC.Value;
            ADD_LIGHT.alwaysDrawDynamicShadows = ADD_LIGHT_ALWAYSDRAWDYNAMICSHADOWS.Value;
            ADD_LIGHT.angularDiameter = ADD_LIGHT_ANGULARDIAMETER.Value;
            ADD_LIGHT.applyRangeAttenuation = ADD_LIGHT_APPLYRANGEATTENUATION.Value;
            ADD_LIGHT.areaLightEmissiveMeshLayer = ADD_LIGHT_AREALIGHTEMISSIVEMESHLAYER.Value;
            ADD_LIGHT.areaLightEmissiveMeshMotionVectorGenerationMode = ADD_LIGHT_AREALIGHTEMISSIVEMESHMOTIONVECTORGENERATIONMODE.Value;
            ADD_LIGHT.areaLightEmissiveMeshShadowCastingMode = ADD_LIGHT_AREALIGHTEMISSIVEMESHSHADOWCASTINGMODE.Value;
            ADD_LIGHT.areaLightShadowCone = ADD_LIGHT_AREALIGHTSHADOWCONE.Value;
            ADD_LIGHT.areaLightShape = ADD_LIGHT_AREALIGHTSHAPE.Value;
            ADD_LIGHT.aspectRatio = ADD_LIGHT_ASPECTRATIO.Value;
            ADD_LIGHT.barnDoorAngle = ADD_LIGHT_BARNDOORANGLE.Value;
            ADD_LIGHT.barnDoorLength = ADD_LIGHT_BARNDOORLENGTH.Value;
            ADD_LIGHT.blockerSampleCount = ADD_LIGHT_BLOCKERSAMPLECOUNT.Value;
            ADD_LIGHT.cachedShadowAngleUpdateThreshold = ADD_LIGHT_CACHEDSHADOWANGLEUPDATETHRESHOLD.Value;
            ADD_LIGHT.cachedShadowTranslationUpdateThreshold = ADD_LIGHT_CACHEDSHADOWTRANSLATIONUPDATETHRESHOLD.Value;
            ADD_LIGHT.color = ADD_LIGHT_COLOR.Value;
            ADD_LIGHT.colorShadow = ADD_LIGHT_COLORSHADOW.Value;
            ADD_LIGHT.customSpotLightShadowCone = ADD_LIGHT_CUSTOMSPOTLIGHTSHADOWCONE.Value;
            ADD_LIGHT.displayAreaLightEmissiveMesh = ADD_LIGHT_DISPLAYAREALIGHTEMISSIVEMESH.Value;
            ADD_LIGHT.distance = ADD_LIGHT_DISTANCE.Value;
            ADD_LIGHT.enableSpotReflector = ADD_LIGHT_ENABLESPOTREFLECTOR.Value;
            ADD_LIGHT.evsmBlurPasses = ADD_LIGHT_EVSMBLURPASSES.Value;
            ADD_LIGHT.evsmExponent = ADD_LIGHT_EVSMEXPONENT.Value;
            ADD_LIGHT.evsmLightLeakBias = ADD_LIGHT_EVSMLIGHTLEAKBIAS.Value;
            ADD_LIGHT.evsmVarianceBias = ADD_LIGHT_EVSMVARIANCEBIAS.Value;
            ADD_LIGHT.fadeDistance = ADD_LIGHT_FADEDISTANCE.Value;
            ADD_LIGHT.filterSampleCount = ADD_LIGHT_FILTERSAMPLECOUNT.Value;
            ADD_LIGHT.filterSizeTraced = ADD_LIGHT_FILTERSIZETRACED.Value;
            ADD_LIGHT.filterTracedShadow = ADD_LIGHT_FILTERTRACEDSHADOW.Value;
            ADD_LIGHT.flareFalloff = ADD_LIGHT_FLAREFALLOFF.Value;
            ADD_LIGHT.flareSize = ADD_LIGHT_FLARESIZE.Value;
            ADD_LIGHT.flareTint = ADD_LIGHT_FLARETINT.Value;
            ADD_LIGHT.includeForRayTracing = ADD_LIGHT_INCLUDEFORRAYTRACING.Value;
            ADD_LIGHT.innerSpotPercent = ADD_LIGHT_INNERSPOTPERCENT.Value;
            ADD_LIGHT.intensity = ADD_LIGHT_INTENSITY.Value;
            ADD_LIGHT.interactsWithSky = ADD_LIGHT_INTERACTSWITHSKY.Value;
            ADD_LIGHT.kernelSize = ADD_LIGHT_KERNELSIZE.Value;
            ADD_LIGHT.lightAngle = ADD_LIGHT_LIGHTANGLE.Value;
            ADD_LIGHT.lightDimmer = ADD_LIGHT_LIGHTDIMMER.Value;
            ADD_LIGHT.lightlayersMask = ADD_LIGHT_LIGHTLAYERSMASK.Value;
            ADD_LIGHT.lightShadowRadius = ADD_LIGHT_LIGHTSHADOWRADIUS.Value;
            ADD_LIGHT.lightUnit = ADD_LIGHT_LIGHTUNIT.Value;
            ADD_LIGHT.linkShadowLayers = ADD_LIGHT_LINKSHADOWLAYERS.Value;
            ADD_LIGHT.luxAtDistance = ADD_LIGHT_LUXATDISTANCE.Value;
            ADD_LIGHT.maxDepthBias = ADD_LIGHT_MAXDEPTHBIAS.Value;
            ADD_LIGHT.maxSmoothness = ADD_LIGHT_MAXSMOOTHNESS.Value;
            ADD_LIGHT.minFilterSize = ADD_LIGHT_MINFILTERSIZE.Value;
            ADD_LIGHT.nonLightmappedOnly = ADD_LIGHT_NONLIGHTMAPPEDONLY.Value;
            ADD_LIGHT.normalBias = ADD_LIGHT_NORMALBIAS.Value;
            ADD_LIGHT.numRayTracingSamples = ADD_LIGHT_NUMRAYTRACINGSAMPLES.Value;
            ADD_LIGHT.penumbraTint = ADD_LIGHT_PENUMBRATINT.Value;
            ADD_LIGHT.preserveCachedShadow = ADD_LIGHT_PRESERVECACHEDSHADOW.Value;
            ADD_LIGHT.range = ADD_LIGHT_RANGE.Value;
            ADD_LIGHT.rayTraceContactShadow = ADD_LIGHT_RAYTRACECONTACTSHADOW.Value;
            ADD_LIGHT.semiTransparentShadow = ADD_LIGHT_SEMITRANSPARENTSHADOW.Value;
            ADD_LIGHT.shadowDimmer = ADD_LIGHT_SHADOWDIMMER.Value;
            ADD_LIGHT.shadowFadeDistance = ADD_LIGHT_SHADOWFADEDISTANCE.Value;
            ADD_LIGHT.shadowNearPlane = ADD_LIGHT_SHADOWNEARPLANE.Value;
            ADD_LIGHT.shadowTint = ADD_LIGHT_SHADOWTINT.Value;
            ADD_LIGHT.shadowUpdateMode = ADD_LIGHT_SHADOWUPDATEMODE.Value;
            ADD_LIGHT.shapeHeight = ADD_LIGHT_SHAPEHEIGHT.Value;
            ADD_LIGHT.shapeRadius = ADD_LIGHT_SHAPERADIUS.Value;
            ADD_LIGHT.shapeWidth = ADD_LIGHT_SHAPEWIDTH.Value;
            ADD_LIGHT.slopeBias = ADD_LIGHT_SLOPEBIAS.Value;
            ADD_LIGHT.softnessScale = ADD_LIGHT_SOFTNESSSCALE.Value;
            ADD_LIGHT.spotIESCutoffPercent = ADD_LIGHT_SPOTIESCUTOFFPERCENT.Value;
            ADD_LIGHT.spotLightShape = ADD_LIGHT_SPOTLIGHTSHAPE.Value;
            ADD_LIGHT.sunLightConeAngle = ADD_LIGHT_SUNLIGHTCONEANGLE.Value;
            ADD_LIGHT.surfaceTint = ADD_LIGHT_SURFACETINT.Value;
            ADD_LIGHT.type = ADD_LIGHT_TYPE.Value;
            ADD_LIGHT.updateUponLightMovement = ADD_LIGHT_UPDATEUPONLIGHTMOVEMENT.Value;
            ADD_LIGHT.useCustomSpotLightShadowCone = ADD_LIGHT_USECUSTOMSPOTLIGHTSHADOWCONE.Value;
            ADD_LIGHT.useRayTracedShadows = ADD_LIGHT_USERAYTRACEDSHADOWS.Value;
            ADD_LIGHT.useScreenSpaceShadows = ADD_LIGHT_USESCREENSPACESHADOWS.Value;
            ADD_LIGHT.volumetricDimmer = ADD_LIGHT_VOLUMETRICDIMMER.Value;
            ADD_LIGHT.volumetricFadeDistance = ADD_LIGHT_VOLUMETRICFADEDISTANCE.Value;
            ADD_LIGHT.volumetricShadowDimmer = ADD_LIGHT_VOLUMETRICSHADOWDIMMER.Value;
        }
    } 
}
