using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {
	private Vector3 state;
	private bool _action = false;
	void OnMouseDown(){	
		_action = true;
	}

	void OnMouseUp(){
		_action = false;
	}

	void Update(){
		if(_action){
			state = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(state.x > 2f && state.x < 4){
				this.gameObject.transform.position = new Vector3(state.x,this.gameObject.transform.position.y,0);
			}
		}
	}
}
