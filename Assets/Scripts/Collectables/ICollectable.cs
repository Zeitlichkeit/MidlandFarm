using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidlandFarm.Scripts.Collectables
{
    public interface ICollectable
    {
        bool Collect();
        void Collectable(bool state);
    }
}