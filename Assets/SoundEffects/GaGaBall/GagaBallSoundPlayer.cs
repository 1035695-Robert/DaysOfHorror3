using UnityEngine;

namespace GJAM5.SoundEffects
{
    public class GagaBallSoundPlayer : SoundPlayer
    {
        #region Static Declaration

        public static GagaBallSoundPlayer instance;

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
                case "PickUp":
                    return _soundEffects[0];
                case "Throw":
                    return _soundEffects[1];
                case "Hit":
                    return _soundEffects[2];
                default:
                    return null;
            }
        }
    }
}