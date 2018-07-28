using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public int type;
	//How many meters is this weapon effective over
	public float range;
	//Damage per hit
	public float damage;
	
	//Time in seconds to complete a swing
	public float swingSpeed;
	
	//For weapons that change states when wielded
	public GameObject normalMode;
	public GameObject heldMode;

	//List of nodes that the weapons swings thru
	public Transform[] swingNodes;
	
	//GameObject that is holding this weapon
	public GameObject wielder;
	
	//Trigger hitbox that is how this weapon knows to deal damage
	public Collider hitTrigger;
	
	//Should nodes be shown? Useful for setting up weapons
	public bool showNodes;
	
	//Prefabs for particle FX
	public GameObject bloodPrefab;
	public GameObject dustPrefab;
	
	public bool isSwinging;
	GameObject hasHit;
	Vector3 hitPoint;

	void Start() {
		isSwinging = false;
		if(hitTrigger != null) hitTrigger.enabled = false;
			
		int nodeCount = 0;
		foreach(Transform child in transform){
			if(child.gameObject.name.StartsWith("Node")) nodeCount++;	
		}
		
		swingNodes = new Transform[nodeCount];
		for(int i = 0; i < nodeCount; i++){
			swingNodes[i] = transform.Find("Node" + i);
		}
	}


	void OnDrawGizmos() {
		if(showNodes){
			int nodeCount = 0;
			//Count the nodes
			foreach(Transform child in transform){
				if(child.gameObject.name.StartsWith("Node")) nodeCount++;	
			}
			//Add the nodes to the array
			swingNodes = new Transform[nodeCount];
			for(int i = 0; i < nodeCount; i++){
				swingNodes[i] = transform.Find("Node" + i);
			}
			
			//Draw spheres on each node and lines between them
			for(int i = 0; i < swingNodes.Length; i++) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(swingNodes[i].position, 0.1f);
				if(i > 0) Gizmos.DrawLine(swingNodes[i - 1].position, swingNodes[i].position);
				else Gizmos.DrawLine(swingNodes[swingNodes.Length - 1].position, swingNodes[0].position);
				Gizmos.color = Color.green;
				Gizmos.DrawLine(swingNodes[i].position, swingNodes[i].position + (0.5f * swingNodes[i].up));
				Gizmos.color = Color.red;
				Gizmos.DrawLine(swingNodes[i].position, swingNodes[i].position + (0.5f * swingNodes[i].right));
			}
		}
	}

	public void Use(){
        //Debug.Log("Use Signal Recieved");
		if(!isSwinging) StartCoroutine(Swing());
	}

	IEnumerator Swing () {
		hitTrigger.enabled = true;
		isSwinging = true;
		hasHit = null;
		//hitPoint = null;
		float swingTime = 0;
		int swingState = 1;
		
		//Swing thru all nodes
		while(!hasHit && swingState < swingNodes.Length){
			swingTime = 0;
			while(hasHit == null && swingTime < swingSpeed / swingNodes.Length) {
				swingTime += Time.deltaTime;
				transform.localPosition = Vector3.Lerp(swingNodes[swingState - 1].localPosition, swingNodes[swingState].localPosition, swingTime / (swingSpeed / swingNodes.Length));
				transform.localRotation = Quaternion.Lerp(swingNodes[swingState - 1].localRotation, swingNodes[swingState].localRotation, swingTime / (swingSpeed / swingNodes.Length));
				yield return null;
			}
			swingState++;
		}
		
		//If we hit something
		if(hasHit && hasHit.GetComponent<Health>()){
            hasHit.GetComponent<Health>().ApplyDamage(damage, type);
		}
		else if(hasHit){
			
		}
		
		Vector3 endPos = transform.localPosition;
		Quaternion endRot = transform.localRotation;
		
		swingTime = 0;
		while(swingTime < swingSpeed / swingNodes.Length) {
			swingTime += Time.deltaTime;
			transform.localPosition = Vector3.Lerp(endPos, swingNodes[0].localPosition, swingTime / (swingSpeed / swingNodes.Length));
			transform.localRotation = Quaternion.Lerp(endRot, swingNodes[0].localRotation, swingTime / (swingSpeed / swingNodes.Length));
			yield return null;
		}
		
		hitTrigger.enabled = false;
		isSwinging = false;
	}



	void OnTriggerEnter(Collider collider){
		if(collider.gameObject != wielder) hasHit = collider.gameObject;
		hitPoint = collider.ClosestPointOnBounds(transform.position);
	}
}
