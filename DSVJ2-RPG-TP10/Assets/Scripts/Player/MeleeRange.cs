using UnityEngine;
using PlayerScript;

public class MeleeRange : MonoBehaviour
{
    [SerializeField] private Player player;
    public int GetDamagePlayer()
    {
        return player.playerData.characterDamage;
    }
}
