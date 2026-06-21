using UnityEngine;

namespace GJAM5.SoundEffects
{
    public class ParachuteSoundPlayer : SoundPlayer
    {
        #region Static Declaration

        public static ParachuteSoundPlayer instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        #endregion

        public override AudioClip GetSFX(string soundEffect)
        {
            switch (soundEffect)
            {
                case "AffirmativeNotification":
                    return _soundEffects[0];
                case "FabricFlap":
                    return _soundEffects[1];
                default:
                    return null;
            }
        }
    }
}