using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidlandFarm.Scripts.Cuttables.Wheat
{
    public class WheatViewController : MonoBehaviour
    {
        public GameObject growedObject;
        public GameObject growingObject;
        public ParticleSystem cutParticles;

        public void PlayCutAnimation()
        {
            cutParticles.Play();
        }

        public void PlayGrowAnimation()
        {

        }

        public void ShowGrowed()
        {
            growingObject.SetActive(false);
            growedObject.SetActive(true);
        }

        public void ShowGrowing()
        {
            growingObject.SetActive(true);
            growedObject.SetActive(false);
        }
    }
}