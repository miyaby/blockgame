using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockController : MonoBehaviour {

	public float timeOut = 1.0f;
	private float timeElapsed;

	//縦横のマス数
	static int width = 10;
	static int height = 20;
	//全マスにブロックが存在しているかどうか判定
	static bool[] blockExistArray = Enumerable.Repeat<bool>(false, width*height).ToArray();
//	new bool[width*height];

	//移動中ブロックの存在しているポイント
	public static int movingBlockPos = 0;

	//中心ブロックの座標を０とした時の４ブロックのポジション
	public static int[] movingBlocksPos = new int[] { 0, 0, 0, 0 };
	static int[] iBlocksPos = new int[] { 0, -1, 1, 2 };
	static int[] jBlocksPos = new int[] { 0, 1, 2, 10 };
	static int[] lBlocksPos = new int[] { 0, -10, 1, 2 };
	static int[] oBlocksPos = new int[] { 0, -10, -9, 1 };
	static int[] sBlocksPos = new int[] { 0, -9, 1, 10 };
	static int[] tBlocksPos = new int[] { 0, -10, -1, 1 };
	static int[] zBlocksPos = new int[] { 0, -10, -9, -1 };
	public static int[][] allBlocksPos = new int[][] {
		iBlocksPos,
		jBlocksPos,
		lBlocksPos,
		oBlocksPos,
		sBlocksPos,
		tBlocksPos,
		zBlocksPos
	};

	// Use this for initialization
	void Start () {
		Debug.Log ("ConSTART");
	}
	
	// Update is called once per frame
	void Update () {

		if (DownKeyCheck ())
			return;

		timeElapsed += Time.deltaTime;

		if(timeElapsed >= timeOut) {

			GameObject movingBlock = GameObject.Find ("MovingBlock");

			bool conflictBlock = false;
			foreach (int blockPos in movingBlocksPos) {
				//-10すると他のブロックにぶつかる?
				if (movingBlockPos + blockPos >= 10 && movingBlockPos + blockPos < 200 && !conflictBlock)
					conflictBlock = blockExistArray [movingBlockPos + blockPos - 10];
				//最下段に達した?
				if (movingBlockPos + blockPos < 10)
					conflictBlock = true;
			}

			//他のブロックにぶつかる・最下段に達したらストップ
			if (conflictBlock) {
				if (movingBlock)
					movingBlock.name = "StopBlock";

				Debug.Log ("FIX.BLOCKS = " + movingBlocksPos [0] + "/" + movingBlocksPos [1] + "/" + movingBlocksPos [2] + "/" + movingBlocksPos [3]);

				//ブロックがある箇所のbool値をtrueに
				blockExistArray [movingBlockPos] = true;
				blockExistArray [movingBlockPos + movingBlocksPos [1]] = true;
				blockExistArray [movingBlockPos + movingBlocksPos [2]] = true;
				blockExistArray [movingBlockPos + movingBlocksPos [3]] = true;

				BlockGenerator.generateBlock ();

				//ログ
				string exitsBlock = "";
				for (int i = 0; i < 200; i++) {
					if (blockExistArray [i]) {
						exitsBlock = exitsBlock + i.ToString ();
						exitsBlock = exitsBlock + ",";
					}
				}
//				Debug.Log ("FIX.BLOCKS = "+movingBlocksPos[0]+"/"+movingBlocksPos[1]+"/"+movingBlocksPos[2]+"/"+movingBlocksPos[3]);
				Debug.Log ("FIX.BLOCKS = " + exitsBlock);

			} else {
				Vector3 pos = movingBlock.transform.position;
				pos.y -= 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが-10
				movingBlockPos -= 10;
			}

			timeElapsed = 0.0f;
		}
	}

	//押されているキーを取得
	bool DownKeyCheck(){

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

			for (int it = 0; it < 4; it++) {
				movingBlocksPos[it] = applySpin (movingBlocksPos[it]);
			}
			Debug.Log ("BLOCKS = "+movingBlocksPos[0]+"/"+movingBlocksPos[1]+"/"+movingBlocksPos[2]+"/"+movingBlocksPos[3]);
		}else {
			return false;
		}

		return true;
	}

	//回転時のブロック座標修正メソッド
	int applySpin(int oriPos){
		
		switch (oriPos) {
		case 0:
			return 0;
		case 1:
			return -10;
		case 2:
			return -20;
		case 9:
			return 11;
		case 10:
			return 1;
		case 11:
			return -9;
		case 20:
			return 2;
		case -1:
			return 10;
		case -2:
			return 20;
		case -9:
			return -11;
		case -10:
			return -1;
		case -11:
			return 9;
		case -20:
			return -2;
		default:
			return 0;
		}
	}
}