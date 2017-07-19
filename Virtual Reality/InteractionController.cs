using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour {
	//set up variables
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private GameObject pickup;
	public Rigidbody attachPoint;
	FixedJoint joint;

	void Awake()
	{
		//set trackedObj from the SteamVR_TrackedObject component of the controller
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void FixedUpdate()
	{
		//set the device reference using the trackedObj index.
		device = SteamVR_Controller.Input((int)trackedObj.index);

		//when the trigger button is pressed and there is an object colling with the sphere collider trigger
		//create a FixedJoint between the controller and the object
		if (joint == null && device.GetTouchDown(triggerButton) && pickup != null)
		{
            pickup.transform.parent = this.transform;
			joint = pickup.AddComponent<FixedJoint>();
			joint.connectedBody = attachPoint;
		}
		//when the trigger button is released destroy the FixedJoint between the controller and the object
		else if (joint != null && device.GetTouchUp(triggerButton))
		{
            pickup.transform.parent = null;
			var go = joint.gameObject;
			var rigidbody = go.GetComponent<Rigidbody>();
			Object.DestroyImmediate(joint);
			joint = null;

			var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
			if (origin != null)
			{
				rigidbody.velocity = origin.TransformVector(device.velocity);
				rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
			}
			else
			{
				rigidbody.velocity = device.velocity;
				rigidbody.angularVelocity = device.angularVelocity;
			}

			rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
		}
	}

	//when the sphere collider trigger collides with an object with a Rigidbody component, set pickup to that object.
    private void OnTriggerEnter(Collider collider) {
        pickup = collider.gameObject;
    }

	//when the sphere collider trigger separates from an object with a Rigidbody component, set pickup to null.
    private void OnTriggerExit(Collider collider) {
        pickup = null;
    }
}