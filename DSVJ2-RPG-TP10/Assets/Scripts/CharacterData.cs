[System.Serializable]
public class CharacterData
{
    public int characterHp;
    public int characterDefense;
    public int characterDamage;

    public float characterSpeed;
    public bool characterAlive;

    public enum AttackType
    {
        Melee,
        Ranged
    }
    public AttackType actualAttackType;
    public float rangeAttack;
    public float coldownAttack;
    public float resetColdown;
    public bool attackReady;
}