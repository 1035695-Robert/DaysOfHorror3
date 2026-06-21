using UnityEngine;
using UnityEngine.Audio;

namespace GJAM5.SoundEffects
{
    /// <summary>
    /// This is the base class the all SoundPlayer scripts inherit from
    /// </summary>
    public abstract class SoundPlayer : MonoBehaviour
    {
        #region Variables

        [Header("Sound Playing")]

        [SerializeField] protected AudioClip[] _soundEffects;

        [SerializeField] protected AudioSource _audioSource;

        [SerializeField] protected AudioMixerGroup _audioMixerGroup;

        [SerializeField] protected GameObject _instantiatedAudioSource;

        #endregion

        #region Methods

        public virtual void PlaySFXClipAt(string soundEffect, Vector3 pos, float volume, bool pitchShift)
        {
            Debug.Log("We're playing a SFX");

            // Ransaked code
            AudioSource aSource = Instantiate(_instantiatedAudioSource).GetComponent<AudioSource>();
            aSource.gameObject.transform.position = pos;
            aSource.clip = GetSFX(soundEffect);
            aSource.volume = volume;

            if (pitchShift == true)
            {
                aSource.pitch = Random.Range(0.8f, 1.2f);
            }

            aSource.PlayOneShot(aSource.clip);
            Destroy(aSource.gameObject, aSource.clip.length);
        }

        public abstract AudioClip GetSFX(string soundEffect);

        #endregion
    }
}