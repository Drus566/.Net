using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject[] LevelsPrefabs;
	void Start(){
		GameObject o = Instantiate(LevelsPrefabs[0],new Vector3(0,0,0),Quaternion.identity);
	}

	public void LoadLevel(int level){
		GameObject obj = Instantiate(LevelsPrefabs[level],new Vector3(0,0,0),Quaternion.identity);
		GameObject dObj = GameObject.FindGameObjectWithTag("Level" + level);
		Destroy(dObj.gameObject);
	}

	private IEnumerator Coroutine(int level){
		yield return new WaitForSeconds(2);
	}
}
