using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK.VFX
{
    public class MouthOvenVFXControler : MonoBehaviour
    {
        #region Attributes

        [SerializeField] private ParticleSystem[] _fireBurstEmmiters = { };

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Plays a burn effect
        /// </summary>
        public void BurnEffect()
        {
            for(int i = 0; i < _fireBurstEmmiters.Length; i++)
            {
                if(_fireBurstEmmiters[i] != null)
                {
                    if (_fireBurstEmmiters[i].isPlaying)
                    {
                        _fireBurstEmmiters[i].Stop();
                    }

                    _fireBurstEmmiters[i].Play();
                }
            } 
        }

        #endregion

        #endregion
    }
}