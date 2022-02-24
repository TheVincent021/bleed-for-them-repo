using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{

	[SerializeField] [Range(0f, 1f)] float followStiffness = 0.2f;
	Transform target;
	Vector3 offset;

	new Transform transform;

	void Awake ()
	{
        transform = base.transform;
		target = GameObject.FindWithTag("Player").transform;
	}

	void Start () 
	{
		offset = transform.position - target.position;
	}
	
	void FixedUpdate () 
	{
		transform.position = Vector3.Lerp(transform.position, target.position + offset, followStiffness);
	}
}
