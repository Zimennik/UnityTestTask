using UnityEngine;

namespace _Bludoku.Scripts.PowerUps
{
    public static class PowerUpSaveLoad
    {
        private const string ChargesKeyFormat = "PowerUp_{0}_Charges";
        private const string CooldownKeyFormat = "PowerUp_{0}_Cooldown";

        public static void Save(string id, PowerUpCharges charges)
        {
            PlayerPrefs.SetInt(string.Format(ChargesKeyFormat, id), charges.Charges);
            PlayerPrefs.SetInt(string.Format(CooldownKeyFormat, id), charges.CooldownLeft);
            PlayerPrefs.Save();
        }

        public static void Load(string id, PowerUpCharges charges)
        {
            charges.Restore(
                PlayerPrefs.GetInt(string.Format(ChargesKeyFormat, id), charges.Charges),
                PlayerPrefs.GetInt(string.Format(CooldownKeyFormat, id), charges.CooldownLeft));
        }
    }
}
