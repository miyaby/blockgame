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

				string compLine = "";
				foreach (int line in getCompLines()) {
					compLine += line.ToString ();
				}
				Debug.Log("COMPLINE = " + compLine);

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

		bool move = true;
		GameObject movingBlock = GameObject.Find ("MovingBlock");
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			foreach (int blockPos in movingBlocksPos) {
				//一番左の列？
				if ((movingBlockPos + blockPos) % 10 == 0)
					move = false;
				//特定のブロックの左隣にすでにブロックが存在している？
				if (blockExistArray [movingBlockPos + blockPos - 1])
					move = false;
			}
			if (move) {
				Vector3 pos = movingBlock.transform.position;
				pos.x -= 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが-1
				movingBlockPos -= 1;
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			foreach (int blockPos in movingBlocksPos) {
				//一番右の列？
				if ((movingBlockPos + blockPos) % 10 == 9)
					move = false;
				//特定のブロックの右隣にすでにブロックが存在している？
				if (blockExistArray [movingBlockPos + blockPos + 1])
					move = false;
			}
			if (move) {
				Vector3 pos = movingBlock.transform.position;
				pos.x += 0.4f;
				movingBlock.transform.position = pos;

				//ポジションが+1
				movingBlockPos += 1;
			}
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			spinBlock ();
		}else {
			return false;
		}

		return true;
	}

	//ブロックを回転させる処理
	void spinBlock(){

		GameObject movingBlock = GameObject.Find ("MovingBlock");

		Vector3 oriEuler = movingBlock.transform.eulerAngles;
		oriEuler.z -= 90;
		if (oriEuler.z < 0)
			oriEuler.z += 360;
		movingBlock.transform.rotation = Quaternion.Euler (oriEuler);

		for (int it = 0; it < 4; it++) {
			movingBlocksPos[it] = applySpin (movingBlocksPos[it]);
		}

		//回転の結果、ブロックが画面外に出てしまっていたら、位置を調整する
		adjustXPosition();
		adjustYPosition();

		Debug.Log ("BLOCKS = "+movingBlocksPos[0]+"/"+movingBlocksPos[1]+"/"+movingBlocksPos[2]+"/"+movingBlocksPos[3]);
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

	//現状動いているブロックが横方向に不正になっていたら位置の調整を行う
	void adjustXPosition(){
		
		GameObject movingBlock = GameObject.Find ("MovingBlock");

		//動いているブロックのX座標
		int movingX = movingBlockPos % 10;
		int adjustX = 0;//調整値
		switch (movingX) {
		case 9://一番右
			foreach (int blockPos in movingBlocksPos) {
				if (blockPos == 2) {
					adjustX = -2;
				}else if (blockPos == -9 || blockPos == 1 || blockPos == 11) {
					if (adjustX != -2)
						adjustX = -1;
				}
			}
			break;
		case 8://右から２番目
			foreach (int blockPos in movingBlocksPos) {
				if (blockPos == 2) {
					adjustX = -1;
				}
			}
			break;
		case 1://左から２番目
			foreach (int blockPos in movingBlocksPos) {
				if (blockPos == -2) {
					adjustX = 1;
				}
			}
			break;
		case 0://一番左
			foreach (int blockPos in movingBlocksPos) {
				if (blockPos == -2) {
					adjustX = 2;
				}else if (blockPos == 9 || blockPos == -1 || blockPos == -11) {
					if (adjustX != 2)
						adjustX = 1;
				}
			}
			break;
		default:
			break;
		}

		//外部(表示)ポジション調整
		Vector3 pos = movingBlock.transform.position;
		pos.x += (0.4f * adjustX);
		movingBlock.transform.position = pos;
		//内部ポジション調整
		movingBlockPos += adjustX;
	}

	//現状動いているブロックが縦方向に不正になっていたら位置の調整を行う
	void adjustYPosition(){

		//TODO:1点先が被ってて、2点先が被ってるときバグる

		GameObject movingBlock = GameObject.Find ("MovingBlock");

		int adjustX = 0;//調整値
		int adjustY = 0;//調整値
		foreach (int blockPos in movingBlocksPos) {

			if (blockExistArray [movingBlockPos + blockPos]) {

				//干渉した座標に22を足して、10で割った余りから2を引いた数が横の調整値
				int x = (int)((blockPos + 22) % 10) - 2;
				//絶対値が大きいものを採用
				if (System.Math.Abs (adjustX) < System.Math.Abs (x))
					adjustX = x;

				//干渉した座標に22を足して、10で割った商から2を引いた数が縦の調整値
				int y = (int)((blockPos + 22) / 10) - 2;
				//絶対値が大きいものを採用
				if (System.Math.Abs (adjustY) < System.Math.Abs (y))
					adjustY = y;

			}
		}

		//外部(表示)ポジション調整
		Vector3 pos = movingBlock.transform.position;
		pos.x -= (0.4f * adjustX);
		pos.y -= (0.4f * adjustY*2);
		movingBlock.transform.position = pos;
		//内部ポジション調整
		movingBlockPos -= adjustX;
		movingBlockPos -= adjustY*10*2;
	}

	///コンプリートした列を取得する
	List<int> getCompLines(){

		int cell = 0;
		bool compLine = true;
		List<int> compLines = new List<int>();

		for (int h = 0; h < height; h++) {
			//コンプフラグリセット
			compLine = true;
			for (int w = 0; w < width; w++) {
				//列チェック。特定のセルにブロックが存在する
				if (blockExistArray [h * width + w]) {
					
				} else {//存在しない
					//コンプフラグをオフ
					compLine = false;
					break;
				}
			}

			if (compLine)
				compLines.Add (h);		
		}

		return compLines;
	}
}