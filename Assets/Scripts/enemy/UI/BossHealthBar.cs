using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UI {
    public class BossHealthBar : MonoBehaviour
    {
        public TextMeshProUGUI bossName;
        public Slider slider;

        private void Awake() 
        {
            slider = GetComponentInChildren<Slider>();   
        }

        private void Start() 
        {
            DeactiveBossHealthBar();
        }

        #region health bar management
        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void ActiveBossHealthBar()
        {
            slider.gameObject.SetActive(true);
            bossName.gameObject.SetActive(true);
        }

        public void DeactiveBossHealthBar()
        {
            slider.gameObject.SetActive(false);
            bossName.gameObject.SetActive(false);
        }
        #endregion

        #region Boss health management
        public void SetBossMaxHealth(float health)
        {
            slider.maxValue = health;
            slider.value = health;
        }

        public void SetBossHealth(float health)
        {
            slider.value = health;
        }
        #endregion
    }
}
