using UnityEngine;
using System.Collections;

public class AttackPlayer : MonoBehaviour {

	void OnTriggerEnter()
    {
        Destroy(gameObject);
    }

}
