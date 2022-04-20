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

namespace LC_ENV
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class ENV : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "LC_ENV", AUTHOR = "ValidUser", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0";
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        public ENV()
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
        public VisualEnvironment env;
        public ConfigEntry<SkyAmbientMode> ENV_SKYAMBIENTMODE;
        public ConfigEntry<SkyType> ENV_SKYTYPE;

        public ConfigEntry<KeyCode> RELOAD_KEY;
        public void InitConfig()
        {
            RELOAD_KEY = Config.Bind("KEYBINDS", "RELOAD Keybind", KeyCode.N, "RELOAD Key.");
            ENV_SKYAMBIENTMODE = Config.Bind("ENV", "SKYAMBIENTMODE", SkyAmbientMode.Dynamic, "");
            ENV_SKYTYPE = Config.Bind("ENV", "SKYTYPE", SkyType.HDRI, "");
        }
        public static Volume VOLUMES;
        public void Update()
        {
            if (!SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("empty"))
            {
                if (Input.GetKeyDown(RELOAD_KEY.Value))
                {
                    DO_ENV();
                }
            }
        }
        public void DO_ENV() 
        {
            VOLUMES = GameObject.Find("sunlight").GetComponentInParent<Volume>();
            env = (VisualEnvironment)VOLUMES.profile.components[1];
            env.active = true;
            env.skyAmbientMode.value = SkyAmbientMode.Dynamic;
            env.skyType.value = (int)SkyType.HDRI;
        }
    }
}