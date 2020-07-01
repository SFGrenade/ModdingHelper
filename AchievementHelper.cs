using System;
using GlobalEnums;
using UnityEngine;
using Logger = Modding.Logger;

namespace InvalidNamespaceLol
{
    public class AchievementHelper
    {
        public AchievementHelper()
        {
            On.UIManager.RefreshAchievementsList += OnUIManagerRefreshAchievementsList;
            On.AchievementHandler.Awake += OnAchievementHandlerAwake;
        }

        private void initAchievements(AchievementsList list)
        {
            string Ach1_Key = "Achievement_Key_Here";
            string Ach1_Text_Key = "Achievement_Text_Convo_Here";
            string Ach1_Title_Key = "Achievement_Title_Convo_Here";
            Sprite Ach1_Sprite = new Sprite();

            Achievement ach1 = new Achievement();
            ach1.key = Ach1_Key;
            ach1.type = AchievementType.Normal;
            ach1.earnedIcon = Ach1_Sprite;
            ach1.unearnedIcon = Ach1_Sprite;
            ach1.localizedText = Ach1_Text_Key;
            ach1.localizedTitle = Ach1_Title_Key;

            bool containsAch1 = false;

            foreach (var ach in list.achievements)
            {
                if (ach.key == Ach1.key)
                    containsAch1 = true;
            }

            if (!containsAch1)
            {
                list.achievements.Add(Ach1);
            }
        }
        private void OnAchievementHandlerAwake(On.AchievementHandler.orig_Awake orig, AchievementHandler self)
        {
            orig(self);
            initAchievements(self.achievementsList);
        }
        private void OnUIManagerRefreshAchievementsList(On.UIManager.orig_RefreshAchievementsList orig, UIManager self)
        {
            initAchievements(GameManager.instance.achievementHandler.achievementsList);
            initMenuAchievements(self);
            orig(self);

            On.UIManager.RefreshAchievementsList -= OnUIManagerRefreshAchievementsList;
        }
        private void initMenuAchievements(UIManager manager)
        {
            // Stolen from the game
            GameManager gm = GameManager.instance;

            AchievementsList achievementsList = gm.achievementHandler.achievementsList;
            int count = achievementsList.achievements.Count;
            for (int j = 0; j < count; j++)
            {
                Achievement achievement2 = achievementsList.achievements[j];
                if (manager.menuAchievementsList.FindAchievement(achievement2.key) == null)
                {
                    MenuAchievement menuAchievement2 = UnityEngine.Object.Instantiate<MenuAchievement>(manager.menuAchievementsList.menuAchievementPrefab);
                    menuAchievement2.transform.SetParent(manager.achievementListRect.transform, false);
                    menuAchievement2.name = achievement2.key;
                    manager.menuAchievementsList.AddMenuAchievement(menuAchievement2);
                    UpdateMenuAchievementStatus(achievement2, menuAchievement2, manager.hiddenIcon);
                }
            }
            manager.menuAchievementsList.MarkInit();
        }
        private void UpdateMenuAchievementStatus(Achievement ach, MenuAchievement menuAch, Sprite hiddenIcon)
        {
            // Stolen from the game
            try
            {
                if (GameManager.instance.IsAchievementAwarded(ach.key))
                {
                    menuAch.icon.sprite = ach.earnedIcon;
                    menuAch.icon.color = Color.white;
                    menuAch.title.text = Language.Language.Get(ach.localizedTitle, "Achievements");
                    menuAch.text.text = Language.Language.Get(ach.localizedText, "Achievements");
                }
                else if (ach.type == AchievementType.Normal)
                {
                    menuAch.icon.sprite = ach.earnedIcon;
                    menuAch.icon.color = new Color(0.57f, 0.57f, 0.57f, 0.57f);
                    menuAch.title.text = Language.Language.Get(ach.localizedTitle, "Achievements");
                    menuAch.text.text = Language.Language.Get(ach.localizedText, "Achievements");
                }
                else
                {
                    menuAch.icon.sprite = hiddenIcon;
                    menuAch.icon.color = new Color(0.57f, 0.57f, 0.57f, 0.57f);
                    menuAch.title.text = Language.Language.Get("HIDDEN_ACHIEVEMENT_TITLE", "Achievements");
                    menuAch.text.text = Language.Language.Get("HIDDEN_ACHIEVEMENT", "Achievements");
                }
            }
            catch (Exception ex)
            {
                if (ach.type == AchievementType.Normal)
                {
                    menuAch.icon.sprite = ach.earnedIcon;
                    menuAch.icon.color = new Color(0.57f, 0.57f, 0.57f, 0.57f);
                    menuAch.title.text = Language.Language.Get(ach.localizedTitle, "Achievements");
                    menuAch.text.text = Language.Language.Get(ach.localizedText, "Achievements");
                }
                else
                {
                    menuAch.icon.sprite = hiddenIcon;
                    menuAch.title.text = Language.Language.Get("HIDDEN_ACHIEVEMENT_TITLE", "Achievements");
                    menuAch.text.text = Language.Language.Get("HIDDEN_ACHIEVEMENT", "Achievements");
                }
            }
        }
    }
}
