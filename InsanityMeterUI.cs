using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace InsanityMeter
{
    public class InsanityMeterUI : MonoBehaviour
    {
        public static InsanityMeterUI Instance { get; private set; }

        private Image image;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            image = GetComponent<Image>();
        }

        public void UpdatePercentage(float insanityPercentage)
        {
            image.fillAmount = insanityPercentage;
        }
    }
}
