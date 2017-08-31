using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	private Vector3 state;
	private bool _action = false;
	private Level2 level2;
	void Start(){
		level2 = GameObject.FindGameObjectWithTag("Level2").GetComponent<Level2>();
	}
	void OnMouseDown(){	
		_action = true;
	}

	void OnMouseUp(){
		_action = false;
	}

	void Update(){
		if(_action){
			state = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.gameObject.transform.position = new Vector3(state.x,state.y,0);
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Lock"){
			_action = false;	
			level2.EndAnim();
		}
	}
}
