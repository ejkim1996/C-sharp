using UnityEngine;
using System.Collections;

public class MoveElevator : MonoBehaviour {

	public Transform groundFloor;
	public Transform topFloor;

	public GameObject player;

	public float speed = 1.0F;
	private float startTime;
	private float totalDistance;

	public bool isMoving = false;

	void Start () {

	}
	
	void Update () {

		if (Input.GetKeyUp("space") && isMoving == false){
			startTime = Time.time;
			totalDistance = Vector3.Distance(groundFloor.position, topFloor.position);
			isMoving = true;

			player.transform.SetParent(transform);
		}

		float distCovered = (Time.time - startTime) * speed;
		float progress = distCovered / totalDistance;
		
		if (isMoving){
			transform.position = Vector3.Lerp(groundFloor.position, topFloor.position, progress);
		}

		if (Mathf.Abs(progress) > .999 && isMoving){
			isMoving = false;
		}
	}
}
