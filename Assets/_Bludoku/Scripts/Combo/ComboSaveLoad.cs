using UnityEngine;

namespace _Bludoku.Scripts.Combo
{
    public static class ComboSaveLoad
    {
        private const string LevelKey = "ComboLevel";
        private const string MovesWithoutClearKey = "ComboMovesWithoutClear";

        public static void Save(ComboSystem combo)
        {
            PlayerPrefs.SetInt(LevelKey, combo.Level);
            PlayerPrefs.SetInt(MovesWithoutClearKey, combo.MovesWithoutClear);
            PlayerPrefs.Save();
        }

        public static void Load(ComboSystem combo)
        {
            combo.Restore(PlayerPrefs.GetInt(LevelKey, 0), PlayerPrefs.GetInt(MovesWithoutClearKey, 0));
        }
    }
}
