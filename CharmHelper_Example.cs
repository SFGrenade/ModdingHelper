using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Modding;
using ModCommon;
using UnityEngine;
using System.Security.Cryptography;
using SFCore;

namespace InvalidNamespaceLol
{
    public class CHESaveSettings : ModSettings
    {
        // Better charms, insert default values here
        public List<bool> gotCharms = new List<bool>() { true, true, true, true };
        public List<bool> newCharms = new List<bool>() { false, false, false, false };
        public List<bool> equippedCharms = new List<bool>() { false, false, false, false };
        public List<int> charmCosts = new List<int>() { 1, 1, 1, 1 };
    }

    public class CHEGlobalSettings : ModSettings
    {
    }

    public class CharmHelperExample : Mod<CHESaveSettings, CHEGlobalSettings>
    {
        internal static CharmHelperExample Instance;

        public CharmHelper charmHelper { get; private set; }

        // Thx to 56
        public override string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            string ver = asm.GetName().Version.ToString();

            SHA1 sha1 = SHA1.Create();
            FileStream stream = File.OpenRead(asm.Location);

            byte[] hashBytes = sha1.ComputeHash(stream);

            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            stream.Close();
            sha1.Clear();

            string ret = $"{ver}-{hash.Substring(0, 6)}";

            return ret;
        }

        public override void Initialize()
        {
            Log("Initializing");
            Instance = this;

            initGlobalSettings();
            charmHelper = new CharmHelper();
            charmHelper.customCharms = 4;
            charmHelper.customSprites = new Sprite[] { new Sprite(), new Sprite(), new Sprite(), new Sprite() };
            initCallbacks();

            Log("Initialized");
        }

        private void initGlobalSettings()
        {
            // Found in a project, might help saving, don't know, but who cares
            // Global Settings
        }

        private void initSaveSettings(SaveGameData data)
        {
            // Charms
            Settings.gotCharms = Settings.gotCharms;
            Settings.newCharms = Settings.newCharms;
            Settings.equippedCharms = Settings.equippedCharms;
            Settings.charmCosts = Settings.charmCosts;
        }

        private void initCallbacks()
        {
            // Hooks
            ModHooks.Instance.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.Instance.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.Instance.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.Instance.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.Instance.AfterSavegameLoadHook += initSaveSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveCHEGlobalSettings;
            ModHooks.Instance.LanguageGetHook += OnLanguageGetHook;
        }

        private void SaveCHEGlobalSettings()
        {
            SaveGlobalSettings();
        }

        #region Get/Set Hooks
        private string OnLanguageGetHook(string key, string sheet)
        {
            //Log($"Sheet: {sheet}; Key: {key}");
            // There probably is a better way to do this, but for now take this
            #region Custom Charms
            if (key.StartsWith("CHARM_NAME_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return "CHARM NAME";
                }
            }
            if (key.StartsWith("CHARM_DESC_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return "CHARM DESC";
                }
            }
            #endregion
            return Language.Language.GetInternal(key, sheet);
        }

        private bool OnGetPlayerBoolHook(string target)
        {
            if (Settings.BoolValues.ContainsKey(target))
            {
                return Settings.BoolValues[target];
            }
            #region Custom Charms
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            #endregion
            return PlayerData.instance.GetBoolInternal(target);
        }
        private void OnSetPlayerBoolHook(string target, bool val)
        {
            if (Settings.BoolValues.ContainsKey(target))
            {
                Settings.BoolValues[target] = val;
                return;
            }
            #region Custom Charms
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            #endregion
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private int OnGetPlayerIntHook(string target)
        {
            if (Settings.IntValues.ContainsKey(target))
            {
                return Settings.IntValues[target];
            }
            #region Custom Charms
            if (target.StartsWith("charmCost_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.charmCosts[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            #endregion
            return PlayerData.instance.GetIntInternal(target);
        }
        private void OnSetPlayerIntHook(string target, int val)
        {
            if (Settings.IntValues.ContainsKey(target))
            {
                Settings.IntValues[target] = val;
            }
            else
            {
                PlayerData.instance.SetIntInternal(target, val);
            }
            //Log("Int  set: " + target + "=" + val.ToString());
        }
        #endregion
    }
}
