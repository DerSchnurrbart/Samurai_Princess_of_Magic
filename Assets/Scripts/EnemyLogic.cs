using UnityEngine;
using System.Collections;
using SwordCombatLibrary;

public class EnemyLogic : MonoBehaviour {

    public EnemyType type;
    public float attack_delay;
    public SwordCombat currentInstance;

	void OnTriggerEnter()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        StartCoroutine(delay_attack());
    }

    IEnumerator delay_attack()
    {
        yield return new WaitForSeconds(attack_delay);
        currentInstance.enemy_attacked();
        currentInstance.active_enemies--;
        Destroy(gameObject);
    }
}
