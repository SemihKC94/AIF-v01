using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Processors
{
    public class ProcessEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _processorEffect;

        public void RunEffect(bool on)
        {
            if(on)
            {
                if(!_processorEffect.isPlaying)
                {
                    _processorEffect.Play();
                }
                return;
            }

            _processorEffect.Stop();
        }
    }
}
