using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	public float timeOut = 1.0f;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		Debug.Log ("ConSTART");
	}
	
	// Update is called once per frame
	void Update () {

		DownKeyCheck ();

		timeElapsed += Time.deltaTime;

		if(timeElapsed >= timeOut) {

			GameObject movingBlock = GameObject.Find ("MovingBlock");
			if (movingBlock) {
				Vector3 pos = movingBlock.transform.position;
				pos.y -= 0.4f;
				movingBlock.transform.position = pos;
			}

			timeElapsed = 0.0f;
		}
	}

	void DownKeyCheck(){
//		if (Input.anyKeyDown) {
//			foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
//				if (Input.GetKeyDown (code)) {
//					//処理を書く
//					Debug.Log (code);
//					break;
//				}
//			}
//		}

		GameObject movingBlock = GameObject.Find ("MovingBlock");
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Vector3 pos = movingBlock.transform.position;
			pos.x -= 0.4f;
			movingBlock.transform.position = pos;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Vector3 pos = movingBlock.transform.position;
			pos.x += 0.4f;
			movingBlock.transform.position = pos;
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			Vector3 oriEuler = movingBlock.transform.eulerAngles;
			oriEuler.z -= 90;
			if (oriEuler.z < 0)
				oriEuler.z += 360;
			movingBlock.transform.rotation = Quaternion.Euler (oriEuler);
			

		}else {
			Debug.Log ("ELSE");
		}
	}
}
