using System;
using System.Collections;
using UnityEngine;

namespace MidlandFarm.Scripts.Cuttables.Wheat
{
    [RequireComponent(typeof(WheatViewController))]
    public class WheatController : MonoBehaviour, ICuttable
    {
        enum GrowState
        {
            Growing,
            Growed
        }

        public int cutsCount;
        [SerializeField] private float growTime;
        [SerializeField] private GrowState growState;
        [SerializeField] private WheatTrigger wheatTrigger;

        [SerializeField] private WheatStackSpawner _wheatStackSpawner;

        private Coroutine _growCoroutine;
        private WheatViewController _wheatCutController;
        private int currentCuts = 0;

        private void Awake()
        {
            _wheatCutController = GetComponent<WheatViewController>();
            _growCoroutine = StartCoroutine(GrowProcess());

            switch (growState)
            {
                case GrowState.Growing:
                    _wheatCutController.ShowGrowing();
                    break;
                case GrowState.Growed:
                    _wheatCutController.ShowGrowed();
                    break;
            }
        }

        private IEnumerator GrowProcess()
        {
            for (; ; )
            {
                if (growState == GrowState.Growing)
                {
                    yield return new WaitForSeconds(growTime);
                    Grow();
                }
                yield return null;
            }
        }

        private void Grow()
        {
            growState = GrowState.Growed;
            wheatTrigger.ShouldTrigger = true;
            _wheatCutController.PlayGrowAnimation();
            _wheatCutController.ShowGrowed();
        }

        public void Cut(ICutter cutter)
        {
            if (growState == GrowState.Growing)
            {
                return;
            }
            currentCuts++;
            _wheatCutController.PlayCutAnimation();
            if (currentCuts > cutsCount)
            {
                currentCuts = 0;
                growState = GrowState.Growing;
                wheatTrigger.ShouldTrigger = false;
                cutter.Cutted(this);

                _wheatCutController.PlayCutAnimation();
                _wheatCutController.ShowGrowing();

                if (_wheatStackSpawner)
                {
                    _wheatStackSpawner.Spawn();
                }
            }
        }
    }
}
