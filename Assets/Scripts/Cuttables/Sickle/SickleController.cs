using System;
using System.Collections;
using UnityEngine;

namespace MidlandFarm.Scripts.Cuttables.Sickle
{
    public class SickleController : MonoBehaviour, ICutter
    {

        private ICuttable _cuttable;

        private void Awake()
        {

        }

        public void Cutted(ICuttable cuttable)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            var cuttable = other.gameObject.GetComponent<ICuttable>();
            if (cuttable != null)
            {
                if (cuttable == _cuttable || _cuttable == null)
                {
                    cuttable.Cut(this);
                }
            }
        }

        public void SetTargetCuttable(ICuttable cuttable)
        {
            _cuttable = cuttable;
        }
    }
}
