using UnityEngine;
using System.Collections;

public class AttackPlayer : MonoBehaviour {

	void OnTriggerEnter()
    {
        //TODO: Add enemy attack sound
        Destroy(gameObject);
    }

}
