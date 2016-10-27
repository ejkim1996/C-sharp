using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour {
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private GameObject pickup;
	public Rigidbody attachPoint;
	FixedJoint joint;

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void FixedUpdate()
	{
		device = SteamVR_Controller.Input((int)trackedObj.index);
		if (joint == null && device.GetTouchDown(triggerButton) && pickup != null)
		{
            pickup.transform.parent = this.transform;
			pickup.transform.position = attachPoint.transform.position;

			joint = pickup.AddComponent<FixedJoint>();
			joint.connectedBody = attachPoint;
		}
		else if (joint != null && device.GetTouchUp(triggerButton) && pickup != null)
		{
            pickup.transform.parent = null;
			var go = joint.gameObject;
			var rigidbody = go.GetComponent<Rigidbody>();
			Object.DestroyImmediate(joint);
			joint = null;
			Object.Destroy(go, 15.0f);

			// We should probably apply the offset between trackedObj.transform.position
			// and device.transform.pos to insert into the physics sim at the correct
			// location, however, we would then want to predict ahead the visual representation
			// by the same amount we are predicting our render poses.

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

    private void OnTriggerEnter(Collider collider) {
        pickup = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider) {
        pickup = null;
    }
	// // Use this for initialization
	// void Start () {
	// 	trackedObj = GetComponent<SteamVR_TrackedObject>();
	// }
	
	// // Update is called once per frame
	// void Update () {
	// 	device = SteamVR_Controller.Input((int)trackedObj.index);

    //     if (device.GetPressDown(triggerButton) && pickup != null) {
    //         pickup.transform.parent = this.transform;
    //         pickup.GetComponent<Rigidbody>().useGravity = false;
    //     }
    //     if (device.GetPressUp(triggerButton) && pickup != null) {
    //         pickup.transform.parent = null;
    //         pickup.GetComponent<Rigidbody>().useGravity = true;
    //     }
	// }

    // private void OnTriggerEnter(Collider collider) {
    //     pickup = collider.gameObject;
    // }

    // private void OnTriggerExit(Collider collider) {
    //     pickup = null;
    // }
}