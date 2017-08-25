using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	public float timeOut = 1.0f;
	private float timeElapsed;

	//縦横のマス数
	static int width = 10;
	static int height = 20;
	//全マスにブロックが存在しているかどうか判定
	static bool[] boolArray = new bool[width*height];

	//移動中ブロックの存在しているポイント
	public static int movingBlockPos = 0;

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

			//movingBlockPosが10(=最下段)に着いたらストップ
			if (movingBlockPos < 10) {//-10すると他のブロックにぶつかる際の条件も必要
				if(movingBlock)
					movingBlock.name = "StopBlock";
				BlockGenerator.generateBlock ();
				return;
			}
				
			if (movingBlock) {
				Vector3 pos = movingBlock.transform.position;
				pos.y -= 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが-10
				movingBlockPos -= 10;
			}

			timeElapsed = 0.0f;
		}
	}

	void DownKeyCheck(){

		GameObject movingBlock = GameObject.Find ("MovingBlock");
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			//一番左の列でない
			if (movingBlockPos % 10 != 0) {
				Vector3 pos = movingBlock.transform.position;
				pos.x -= 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが-1
				movingBlockPos -= 1;
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			//一番右の列でない
			if (movingBlockPos % 10 != 9) {
				Vector3 pos = movingBlock.transform.position;
				pos.x += 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが+1
				movingBlockPos += 1;
			}
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
