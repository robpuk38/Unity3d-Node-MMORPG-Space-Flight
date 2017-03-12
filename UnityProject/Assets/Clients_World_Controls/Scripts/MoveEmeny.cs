using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEmeny : MonoBehaviour {

	public Transform player;
	public float rotationalTime = 0.5f;
	public float MoveSpeed = 10f;
	public float detectionDistance = 20f;
	public float RaycastOffset = 2.5f;

	private void Update()
	{   
		//turn ();
		Move ();
		Pathfinding();
	}

	void Move()
	{

		transform.position += transform.forward *MoveSpeed * Time.deltaTime;
	}

	void Turn()
	{
		Vector3 pos = player.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(pos,Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation,rotation,rotationalTime  * Time.deltaTime);

	}

	void Pathfinding()
	{
		RaycastHit hit;
		Vector3 RayOffset = Vector3.zero;
		Vector3 Left = transform.position - transform.right * RaycastOffset;
		Vector3 Right = transform.position + transform.right * RaycastOffset;
		Vector3 Up = transform.position + transform.up * RaycastOffset;
		Vector3 Down = transform.position - transform.up * RaycastOffset;

		Debug.DrawRay (Right, transform.forward * detectionDistance, Color.gray);
		Debug.DrawRay (Left, transform.forward * detectionDistance, Color.gray);
		Debug.DrawRay (Up, transform.forward * detectionDistance, Color.gray);
		Debug.DrawRay (Down, transform.forward * detectionDistance, Color.gray);

		if(Physics.Raycast(Left,transform.forward,out hit, detectionDistance))
		{
			RayOffset += Vector3.right;
		}
		else  if(Physics.Raycast(Right,transform.forward,out hit, detectionDistance))
		{
			RayOffset -= Vector3.right;
		}
		else  if(Physics.Raycast(Up,transform.forward,out hit, detectionDistance))
		{
			RayOffset -= Vector3.up;
		}
		else  if(Physics.Raycast(Down,transform.forward,out hit, detectionDistance))
		{
			RayOffset += Vector3.up;
		}

		if (RayOffset != Vector3.zero) {
			transform.Rotate (RayOffset * 5f * Time.deltaTime);
		} 
		else 
		{
			Turn ();
		}
	}

}
