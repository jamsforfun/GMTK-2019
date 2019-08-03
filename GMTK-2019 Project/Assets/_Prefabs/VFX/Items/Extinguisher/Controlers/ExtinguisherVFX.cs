using UnityEngine;

namespace GMTK.VFX
{
    public class ExtinguisherVFX : MonoBehaviour
    {
        #region Attributes

        [SerializeField]private float _blastLength = 1.0f;
        [SerializeField] private ParticleSystem[] _parametrableEffects = { };

        public float blastLength
        {
            get { return _blastLength; }
            set { SetBlastLength(value); }
        }

        #endregion

        #region Methods

        private void Update()
        {
            blastLength = _blastLength;
        }


        /// <summary>
        /// Plays the effect
        /// </summary>
        public void Play()
        {
            if (!_parametrableEffects[0].isPlaying)
            {
                _parametrableEffects[0].Play();
            }
        }


        /// <summary>
        /// Stops playing the effect
        /// </summary>
        public void Stop()
        {
            if (_parametrableEffects[0].isPlaying)
            {
                _parametrableEffects[0].Stop();
            }
        }

        /// <summary>
        /// Sets the length of the blast
        /// </summary>
        /// <param name="length"></param>
        public void SetBlastLength(float length)
        {
            if(length < 0.0f)
            {
                length = 0.0f;
            }

            _blastLength = length;

            ParticleSystem.MainModule main;
            ParticleSystem.MinMaxCurve lifetime = new ParticleSystem.MinMaxCurve();

            lifetime.constantMax = length * 0.1f;
            lifetime.constantMin = length * 0.95f;

            for (int i = 0; i < _parametrableEffects.Length; i++)
            {
                main = _parametrableEffects[i].main;
                main.startLifetime = lifetime;
            }

        }

        #endregion
    }
}

