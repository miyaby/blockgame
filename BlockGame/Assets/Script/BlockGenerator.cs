using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour {

	public GameObject iBlockPrefab;
	public GameObject jBlockPrefab;
	public GameObject lBlockPrefab;
	public GameObject oBlockPrefab;
	public GameObject sBlockPrefab;
	public GameObject tBlockPrefab;
	public GameObject zBlockPrefab;

	public static List<GameObject> blocks = new List<GameObject>();

	public bool movingBlock = false;

	// Use this for initialization
	void Start () {
		Debug.Log ("GeneSTART");

		blocks.Add (iBlockPrefab);
		blocks.Add (jBlockPrefab);
		blocks.Add (lBlockPrefab);
		blocks.Add (oBlockPrefab);
		blocks.Add (sBlockPrefab);
		blocks.Add (tBlockPrefab);
		blocks.Add (zBlockPrefab);

		generateBlock ();

//		blocks = new GameObject[iBlockPrefab,jBlockPrefab,lBlockPrefab,oBlockPrefab,sBlockPrefab,tBlockPrefab,zBlockPrefab];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static public void generateBlock(){

		//ランダムに7種類のブロックを生成
		int rand = Random.Range (0,7);
		GameObject block = Instantiate (blocks [rand], new Vector3 (0.2f, 4.2f, 0), Quaternion.Euler (0, 0, 0));
		block.name = "MovingBlock";

		BlockController.movingBlockPos = 205;
		BlockController.allBlocksPos [rand].CopyTo (BlockController.movingBlocksPos,0);
	}
}
