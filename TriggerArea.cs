using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerArea : MonoBehaviour {
	GameObject Respawn;

	// Use this for initialization
	void Start () {
		Respawn = GameObject.Find ("RespawnPoint");
	}

	void OnParticleCollision(GameObject other){
		other.transform.position = Respawn.transform.position;
		Debug.Log ("Collider name = " + other.tag);
	}
	// Update is called once per frame
	void Update () {
		
		
	}
}
