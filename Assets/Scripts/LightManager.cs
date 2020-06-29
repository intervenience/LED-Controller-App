
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

namespace LEDController {

    public class LightManager : MonoBehaviour {

        [SerializeField] Button[] effectButtons;

        [SerializeField] ConnectionManager connectionManager;
        [SerializeField] ColorPicker colorPicker;


        [SerializeField] Slider frequency;
        [SerializeField] TextMeshProUGUI frequencyText;
        string activeEffect = "rgb";

        Coroutine frequencyDelay, effectDelay;

        void OnEnable () {
            foreach (Button b in effectButtons) {
                b.onClick.AddListener (() => {
                    //Debug.Log (b.name + " clicked. producing: " + b.GetComponentInChildren<TextMeshProUGUI> ().text);
                    EffectChanged (b.GetComponentInChildren<TextMeshProUGUI> ().text);
                });
            }

            frequency.onValueChanged.AddListener (delegate {
                //Debug.Log ("frequency slider changed");
                if (frequencyDelay != null) {
                    StopCoroutine (frequencyDelay);
                }
                frequencyText.text = frequency.value.ToString ("0.0") + " Hz";
                frequencyDelay = StartCoroutine (UpdateFrequency ());
            });

            frequencyText.text = frequency.value.ToString ("0.0") + " Hz";
        }

        IEnumerator UpdateFrequency () {
            yield return new WaitForSeconds (.5f);
            connectionManager.UpdateFrequency (frequency.value);
        }

        string EffectInput () {
            if (activeEffect == "rainbow" || activeEffect == "xmas" || activeEffect == "random") {
                return string.Format ("{0} {1}", activeEffect, frequency.value);
            } else if (activeEffect == "pulse") {
                return string.Format ("{0} {1} {2} {3} {4}", activeEffect, Convert.ToInt32 (colorPicker.R * 255), Convert.ToInt32 (colorPicker.G * 255), Convert.ToInt32 (colorPicker.B * 255), frequency.value);
            } else if (activeEffect == "rgb") {
                return string.Format ("{0} {1} {2} {3}", activeEffect, Convert.ToInt32 (colorPicker.R * 255), Convert.ToInt32 (colorPicker.G * 255), Convert.ToInt32 (colorPicker.B * 255));
            }
            return "";
        }

        public void EffectChanged (string effect) {
            //Debug.Log ("Checking effect changed " + effect);
            switch (effect) {
                case "Rainbow":
                    activeEffect = "rainbow";
                    break;
                case "Random":
                    activeEffect = "random";
                    break;
                case "Colour pulse":
                    activeEffect = "pulse";
                    break;
                case "Flat colour":
                    activeEffect = "rgb";
                    break;
                case "X-Mas":
                    activeEffect = "xmas";
                    break;
            }
            //Debug.Log ("Checking effect changed post " + activeEffect);

            if (effectDelay != null) {
                StopCoroutine (effectDelay);
            }
            effectDelay = StartCoroutine (UpdateEffectDelay ());
        }

        IEnumerator UpdateEffectDelay () {
            yield return new WaitForSeconds (0.3f);
            connectionManager.UpdateLighting (EffectInput ());
        }

        public void ColourChanged () {
            connectionManager.UpdateLighting (EffectInput ());
        }

    }

}
