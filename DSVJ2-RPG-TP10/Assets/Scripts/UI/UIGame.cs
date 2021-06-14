using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerScript;

namespace UIGameScript
{
    public class UIGame : MonoBehaviour
    {
        private Player player;
        [SerializeField] private GameObject inventoryUI;

        [Header("Life bar data")]
        public Image lifeBar;
        private float minLife = 0;
        private float maxLife = 0;
        private float totalLive = 0;

        [Header("Defense bar data")]
        public Canvas defenseBarCanvas;
        public Image defenseBar;
        private float minDefense = 0;
        private float maxDefense = 0;
        private float totalDefense = 0;

        private void Start()
        {
            player = FindObjectOfType<Player>();

            if (player != null)
            {
                maxLife = player.playerData.characterHp;
                maxDefense = player.playerData.characterDefense;
            }
        }

        private void Update()
        {
            totalLive = Mathf.Clamp(player.playerData.characterHp, minLife, maxLife);
            lifeBar.fillAmount = totalLive / maxLife;

            if (inventoryUI.activeSelf)
                UpdateMaxDefense();

            totalDefense = Mathf.Clamp(player.playerData.characterDefense, minDefense, maxDefense);
            defenseBar.fillAmount = totalDefense / maxDefense;

            if (player.playerData.characterDefense > 0)
                defenseBarCanvas.gameObject.SetActive(true);
            else
                defenseBarCanvas.gameObject.SetActive(false);
        }

        private void UpdateMaxDefense()
        {
            maxDefense = player.playerData.characterDefense;
        }
    }
}