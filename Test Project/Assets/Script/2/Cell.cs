using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell : MonoBehaviour {
	private Level2 level2;
	void Start(){
		level2 = GameObject.FindGameObjectWithTag("Level2").GetComponent<Level2>();
	}

	void OnMouseDown(){
		// Если это не координаты финиша и начальной позиции конфеты то...
		if(!level2.RandomBlocks && level2.GetSetBlock()){
			if(gameObject.name != "0 , 0" && gameObject.name != "5 , 7"){
				RebuildString(gameObject.name);
				gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			}
		}
	}

	void OnMouseUp(){
		if(!level2.RandomBlocks && level2.GetSetBlock()){
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
	void RebuildString(string input){
		int x = 0;
		int y = 0;
		int counter = 0;
		foreach(char c in input){
			if(Char.IsDigit(c)){
				if(counter == 0){
					counter++;
					//print(c);
					x = Convert.ToInt32(c.ToString());
				}else if(counter == 1){
					//print(c);
					y = Convert.ToInt32(c.ToString());
				}
			}
		}
		level2.SetCell(x,y);
	}
}
