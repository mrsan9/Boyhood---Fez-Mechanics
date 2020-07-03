using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ParticleSystem))]
public class ParticleSoundSystem : MonoBehaviour
{
    private ParticleSystem  _parentParticleSystem;
        
    private int _numberOfParticles;

    void Start()
    {
        _parentParticleSystem = this.GetComponent<ParticleSystem>();      
       
    }

    private void Update()
    {
        var count = _parentParticleSystem.particleCount;
        if (count > _numberOfParticles)
        {
            SoundManager.instance.PlayClip(EAudioClip.PARTICLE,1);
        }

        _numberOfParticles = count;
    }



}