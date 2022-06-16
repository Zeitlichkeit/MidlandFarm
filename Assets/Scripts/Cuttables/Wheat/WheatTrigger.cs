using System;
using System.Collections;
using UnityEngine;

namespace MidlandFarm.Scripts.Cuttables.Wheat
{
    public class WheatTrigger : MonoBehaviour
    {
        public WheatController Wheat { get => wheat; }
        public bool ShouldTrigger { get => _shouldTrigger; set => _shouldTrigger = value; }

        private bool _shouldTrigger = true;

        [SerializeField] private WheatController wheat;
    }
}
