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
		int rand = Random.Range (0,6);
		GameObject block = Instantiate (blocks [0], new Vector3 (0.2f, 3.8f, 0), Quaternion.Euler (0, 0, 0));
		block.name = "MovingBlock";

		BlockController.movingBlockPos = 195;
		BlockController.allBlocksPos [0].CopyTo (BlockController.movingBlocksPos,0);
	}
}
