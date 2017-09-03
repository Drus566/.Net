using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellar : MonoBehaviour {
	private Vector3 state;
	private Vector3 stateOne;
	private Vector3 areaPos;
	private bool _action = false, _action1 = false, anim = false;
	private bool part1 = true;
	private float driver = 0;
	private GameObject door;
	private float val = 0;
	private float temp;
	private GameObject end;

	void Start(){
		end = GameObject.FindGameObjectWithTag("End");
		door = GameObject.FindGameObjectWithTag("Door");
		end.SetActive(false);
	}
	void OnMouseDown(){	
		if(part1){
			_action = true;
		}else if(!part1 && !anim){
			_action1 = true;
			stateOne = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}	
	}

	void OnMouseUp(){
		_action = false;
		_action1 = false;
	}
	void Update(){
		if(_action){
			state = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.gameObject.transform.position = new Vector3(state.x,state.y,0);
		}
		if(_action1){
			state = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector3 dir = state - areaPos;
       		dir.Normalize();
			float rot = Mathf.Atan2(dir.x, dir.y * -1) * Mathf.Rad2Deg;
        	transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);


			float x = Mathf.Abs(transform.rotation.z - temp);
			val += x * 100;
			temp = transform.rotation.z;
			float procent = (val * 100 / 360);
			float dig = ((3 * procent)/100) + 1.5f;
			door.transform.position = new Vector3(door.transform.position.x,dig,door.transform.position.z);
			
			if(val > 360){
				_action1 = false;
				gameObject.GetComponent<Collider2D>().enabled = false;
				end.SetActive(true);
			}
			
		}
		if(anim){
			driver += Time.deltaTime;
			float lerpValue = Mathf.InverseLerp(0f,1.5f,driver);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0),lerpValue);
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, areaPos,lerpValue);
			if(lerpValue == 1){
				anim = false;
				driver = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Area"){
			_action = false;
			anim = true;
			part1 = false;
			areaPos = col.gameObject.transform.position;
			print("GGW");
		}
	}
}
