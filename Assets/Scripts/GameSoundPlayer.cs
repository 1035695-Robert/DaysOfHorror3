using UnityEngine;

namespace GJAM5.SoundEffects
{
    public class GameSoundPlayer : SoundPlayer
    {
        public override AudioClip GetSFX(string soundEffect)
        {
            switch (soundEffect)
            {
                case "HighPitchDrone":
                    return _soundEffects[0];
                case "LowPitchDrone":
                    return _soundEffects[1];
                default:
                    return _soundEffects[0];
            }
        }
    }
}