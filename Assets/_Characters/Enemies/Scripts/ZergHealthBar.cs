using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class ZergHealthBar : MonoBehaviour
    {

        RawImage healthBarRawImage = null;
        Zerg zerg = null;

        // Use this for initialization
        void Start()
        {
            zerg = GetComponentInParent<Zerg>();
            healthBarRawImage = GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            float xValue = -(zerg.healthAsPercentage / 2f) - 0.5f;
            healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}