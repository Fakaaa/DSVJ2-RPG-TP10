using UnityEngine;

public class CharacterAnimator
{
	public Animator animator;

	#region Methods
	public CharacterAnimator(Animator animator)
    {
		this.animator = animator;
    }
	public void UpdateSpeed(float speed)
    {
		animator.SetFloat("speed", speed);
	}
	public void SetArmor(bool hasArmor)
	{
		animator.SetBool("hasArmor", hasArmor);
	}
	public void PlayMeleeAttack()
    {
		animator.SetTrigger("meleeAttack");
    }
	public void PlayRangedAttack()
	{
		animator.SetTrigger("rangedAttack");
	}
	public void PlayReceiveDamage()
	{
		animator.SetTrigger("receiveDamage");
	}
	#endregion
}