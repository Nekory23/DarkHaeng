using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyUI {
    public class EnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        CanvasGroup canvasGroup;
        float timeUntilHidden = 0;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update() 
        {
            if (slider == null)
                return;

            timeUntilHidden -= Time.deltaTime;
            if (timeUntilHidden <= 0) {
                timeUntilHidden = 0;
                fadeOut();
            }
            else {
                canvasGroup.alpha = 1;
            }

            if (slider.value <= 0)
                Destroy(slider.gameObject);
        }

        private void fadeOut()
        {
            if (slider == null && canvasGroup == null)
                return;
            if (slider.gameObject.activeInHierarchy)
                canvasGroup.alpha -= Time.deltaTime;
        }

        
        #region Health management
        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilHidden = 3;
        }

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
        }
        #endregion
    }
}
