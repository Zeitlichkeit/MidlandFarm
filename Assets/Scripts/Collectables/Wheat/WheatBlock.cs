using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidlandFarm.Scripts.Collectables.Wheat
{
    public class WheatBlock : MonoBehaviour, ICollectable
    {
        public float price = 15;

        private bool _isCollectable = true;

        public bool Collect()
        {
            if (_isCollectable)
            {
                return true;
            }
            return false;
        }

        public void Collectable(bool state)
        {
            _isCollectable = state;
        }
    }
}
