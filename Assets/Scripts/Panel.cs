using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Panel : MonoBehaviour
{
	[SerializeField]
	private SteamVR_Input_Sources leftInput, rightInput;
	[SerializeField]
	private GameObject leftHand, rightHand;

	private List<GameObject> collidings = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CheckInputForHand(leftInput);
		CheckInputForHand(rightInput);
	}

	private void CheckInputForHand(SteamVR_Input_Sources inputHand)
	{
		GameObject handObject;
		handObject = (inputHand == leftInput) ? leftHand : rightHand;
		
		// DESTROY
		if (SteamVR_Actions._default.Destroy.GetStateDown(inputHand) && collidings.Contains(handObject))
		{
			Destroy(gameObject);
		}
		
		// MOVE
		if (SteamVR_Actions._default.Grab.GetStateDown(inputHand) && collidings.Contains(handObject))
		{
			transform.parent = handObject.transform;
		}
		if (SteamVR_Actions._default.Grab.GetLastStateUp(inputHand) && collidings.Contains(handObject))
		{
			transform.parent = null;
		}

		// SCALE
		Vector2 joystickPos = SteamVR_Actions._default.Scale.GetAxis(inputHand);

		float speed = 0.005f;

		if (Mathf.Abs(joystickPos.x) > 0.1f && collidings.Contains(handObject))
		{
			transform.localScale = transform.localScale + new Vector3(joystickPos.x * speed, 0f, 0f);
		}
		if (Mathf.Abs(joystickPos.y) > 0.1f && collidings.Contains(handObject))
		{
			transform.localScale = transform.localScale + new Vector3(0f, joystickPos.y * speed, 0f);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		Debug.Log(collider.gameObject.name);
		if (collider.gameObject == leftHand || collider.gameObject == rightHand)
		{
			collidings.Add(collider.gameObject);
		}
	}
	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject == leftHand || collider.gameObject == rightHand)
		{
			collidings.Remove(collider.gameObject);
		}
	}

	public void SetHands(GameObject _leftHand, GameObject _rightHand)
	{
		leftHand = _leftHand;
		rightHand = _rightHand;
	}
}
