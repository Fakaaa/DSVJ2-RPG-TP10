using UnityEngine;
using UnityEngine.UI;
using EnemyAIFSMScript;

public class UIEnemy : MonoBehaviour
{
    [SerializeField] private EnemyAIFSM enemy;
    [SerializeField] public Image lifeBar;
    private float minLife = 0;
    private float maxLife = 0;
    private float totalLive = 0;
    private void Start()
    {
        if (enemy != null)
            maxLife = enemy.enemyData.characterHp;
    }

    // Update is called once per frame
    void Update()
    {
        totalLive = Mathf.Clamp(enemy.enemyData.characterHp, minLife, maxLife);
        lifeBar.fillAmount = totalLive / maxLife;
    }
}
