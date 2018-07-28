using UnityEngine;
using System.Collections;

public interface DeathNotified
{
    void OnDeath();
}

public class Health : MonoBehaviour {
	public float maxHealth;
	public float curHealth;

    public float[] damageMults = {1, 1, 1};

	void Start() {
		curHealth = maxHealth;
	}



	public bool ApplyDamage(float amount) {
		curHealth -= amount;
		if(curHealth < 0){
			//gameObject.SetActive(false);
            foreach (DeathNotified thing in GetComponents<DeathNotified>())
            {
                thing.OnDeath();
            }
			return true;
		}
		return false;
	}

    public bool ApplyDamage(float amount, int weaponType)
    {
        return ApplyDamage(amount * damageMults[weaponType]);
    }

	public float Heal(float amount) {
		curHealth += amount;
		if(curHealth > maxHealth) {
			float change = curHealth - maxHealth;
			curHealth = maxHealth;
			return change;
		}
		return 0;
	}
	
}
