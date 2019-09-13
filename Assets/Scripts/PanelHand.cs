using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PanelHand : MonoBehaviour
{
	[SerializeField]
	private GameObject panelPrefab;
	[SerializeField]
	private GameObject leftHand, rightHand;

	private SteamVR_Behaviour_Pose handComponent;

    // Start is called before the first frame update
    void Start()
    {
		handComponent = GetComponent<SteamVR_Behaviour_Pose>();
        if(handComponent == null)
		{
			Debug.LogError("The PanelHand script needs to be put next to a SteamVR_Behaviour_Pose. Destroying...");
			Destroy(this);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (SteamVR_Actions._default.Create.GetStateDown(handComponent.inputSource))
		{
			Debug.Log("Creating panel...");
			GameObject p = Instantiate(panelPrefab, gameObject.transform.position, gameObject.transform.rotation);
			p.GetComponent<Panel>().SetHands(leftHand, rightHand);
		}
	}
}
