using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Modding;
using ModCommon;
using UnityEngine;
using System.Security.Cryptography;
using SFCore;
using SFCore.Generics;

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

    public class CharmHelperExample : FullSettingsMod<CHESaveSettings, CHEGlobalSettings>
    {
        public CharmHelper charmHelper { get; private set; }

        public override string GetVersion() => SFCore.Utils.GetVersion(Assembly.GetExecutingAssembly());

        public override void Initialize()
        {
            Log("Initializing");

            charmHelper = new CharmHelper();
            charmHelper.customCharms = 4;
            charmHelper.customSprites = new Sprite[] { new Sprite(), new Sprite(), new Sprite(), new Sprite() };
            initCallbacks();

            Log("Initialized");
        }

        private void initCallbacks()
        {
            // Hooks
            ModHooks.Instance.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.Instance.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.Instance.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.Instance.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.Instance.ApplicationQuitHook += SaveCHEGlobalSettings;
            ModHooks.Instance.LanguageGetHook += OnLanguageGetHook;
        }

        private void SaveCHEGlobalSettings()
        {
            SaveGlobalSettings();
        }

        #region Charm Names and Descriptions

        private string[] charmNames = { "Charm Name 1", "Charm Name 2", "Charm Name 3", "Charm Name 4" };
        private string[] charmDescriptions = { "Charm Description 1", "Charm Description 2", "Charm Description 3", "Charm Description 4" };

        #endregion

        #region Get/Set Hooks
        private string OnLanguageGetHook(string key, string sheet)
        {
            // There probably is a better way to do this, but for now take this
            #region Custom Charms
            if (key.StartsWith("CHARM_NAME_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return charmNames[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (key.StartsWith("CHARM_DESC_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return charmDescriptions[charmHelper.charmIDs.IndexOf(charmNum)];
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
        }
        #endregion
    }
}
