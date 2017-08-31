using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour {
	public GameObject PrefabGray1,PrefabGray2,PrefabGray3;
	public Sprite[] Digits;
	public GameObject Pointer;
	public Button[] Interface;
	public Button Next;

	private GameObject[,] face = new GameObject[6,3];

	private int[,] array = new int[6,3] {{8,8,9},{4,4,8},{7,7,1},{6,3,3},{3,2,4},{5,6,2}};
	private GameObject pFace1, pFace2, pFace3;
	private int counter = 2;
	private bool move1 = false, move2 = false, move3 = false;
	private float driver = 0;
	private Quaternion state1, state2, state3;
	private Manager manager;
	private int ctr = 1;
	
	void Start () {
		manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Manager>();
		pFace1 = GameObject.FindGameObjectWithTag("Face1");
		pFace2 = GameObject.FindGameObjectWithTag("Face2");
		pFace3 = GameObject.FindGameObjectWithTag("Face3");
		InitFaces();

		
	}
	

	void Update () {

		if(move1){
			driver += Time.deltaTime;
			float lerpValue = Mathf.InverseLerp(0f,0.5f,driver);
			pFace1.transform.rotation = Quaternion.Lerp(pFace1.transform.localRotation, state1, lerpValue);
			for(int i = 0; i < 6; i++){
				face[i,0].transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,face[i,0].transform.rotation.z * -1);
			}
			if(lerpValue == 1){
				move1 = false;
				driver = 0;
				Check();
			}
		}
		else if(move2){
			driver += Time.deltaTime;
			float lerpValue = Mathf.InverseLerp(0f,0.5f,driver);
			pFace2.transform.rotation = Quaternion.Lerp(pFace2.transform.rotation, state2, lerpValue);
			for(int i = 0; i < 6; i++){
				face[i,1].transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,face[i,1].transform.rotation.z * -1);
			}
			if(lerpValue == 1){
				move2 = false;
				driver = 0;
				Check();
			}
		}
		else if(move3){
			driver += Time.deltaTime;
			float lerpValue = Mathf.InverseLerp(0f,0.5f,driver);
			pFace3.transform.rotation = Quaternion.Lerp(pFace3.transform.rotation, state3, lerpValue);
			for(int i = 0; i < 6; i++){
				face[i,2].transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,face[i,2].transform.rotation.z * -1);
			}
			if(lerpValue == 1){
				move3 = false;
				driver = 0;
				Check();
			}
		}
	}

	// Проверка суммы
	private void Check(){
		int counter = 0;
		for(int i = 0; i < 6; i++){
			int sum = 0;
			for(int j = 0; j < 3; j++){
				sum += array[i,j];
			}
			if(sum == 15){
				counter++;	
				print("Sum = 15, arrray: " + i);
				face[i,0].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
				face[i,1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
				face[i,2].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
			}else{
				face[i,0].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
				face[i,1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
				face[i,2].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
			}
		}
		if(counter == 6){
			print("THE END !!!! FINISH !!!");
			foreach(Button btn in Interface){
				btn.interactable = false;
				Next.gameObject.SetActive(true);
				return;
			}
		}
		foreach(Button btn in Interface){
			btn.interactable = true;
		}
	}

	private void OutArr(int part){
		for(int i = 0; i < 6; i++){
			print(array[i,part]);
		}
	}

	// Смена местами цифр в массиве
	private void Left(int part){
		int last = array[5,part];
		GameObject _last = face[5,part];
		for(int i = 5; i >= 0; i--){
			if(i == 0){
				array[i,part] = last;
				face[i,part] = _last;
				return;
			}
			array[i,part] = array[i - 1, part];
			face[i,part] = face[i - 1,part];
		}
	}

	private void InitFaces(){

		//Инициализация первого циферблата
		for(int i = 0; i < 6; i++){
			GameObject obj = Instantiate(PrefabGray1, new Vector3(0,0,0),Quaternion.Euler(new Vector3(0,0,i * 60)));
			obj.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = Digits[array[i,0]];
			obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,obj.transform.rotation.z * -1);
			obj.transform.parent = pFace1.transform;
			face[i,0] = obj;
		}

		//Инициализация второго циферблата
		for(int i = 0; i < 6; i++){
			GameObject obj = Instantiate(PrefabGray2, new Vector3(0,0,0),Quaternion.Euler(new Vector3(0,0,i * 60)));
			obj.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = Digits[array[i,1]];
			obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,obj.transform.rotation.z * -1);
			obj.transform.parent = pFace2.transform;
			face[i,1] = obj;
		}

		//Инициализация третьего циферблата
		for(int i = 0; i < 6; i++){
			GameObject obj = Instantiate(PrefabGray3, new Vector3(0,0,0),Quaternion.Euler(new Vector3(0,0,i * 60)));
			obj.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = Digits[array[i,2]];
			obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,obj.transform.rotation.z * -1);
			obj.transform.parent = pFace3.transform;
			face[i,2] = obj;
		}

		pFace1.transform.localPosition += new Vector3(0.2f,0,0);
		pFace2.transform.localPosition += new Vector3(0.2f,0,0);
		pFace3.transform.localPosition += new Vector3(0.2f,0,0);

		Pointer.transform.localPosition = new Vector3(0.14f,1.25f,-2f);

	}

	// Прокрутка
	public void Reload(){
		Left(counter);
		if(counter == 0){
			state1.eulerAngles += new Vector3(0,0,60f);
			move1 = true;

		}else if(counter == 1){
			state2.eulerAngles += new Vector3(0,0,60f);
			move2 = true;
		}else if(counter == 2){
			state3.eulerAngles += new Vector3(0,0,60f);
			move3 = true;
		}
		foreach(Button btn in Interface){
			btn.interactable = false;
		}
	}

	// Движение указателя
	public void MovePointer(){
		if(counter == 2){
			counter--;
			Pointer.transform.localPosition = new Vector3(0.14f,0.89f,-2f);
		}else if(counter == 1){
			counter--;
			Pointer.transform.localPosition = new Vector3(0.14f,0.525f,-2f);
		}else if(counter == 0){
			counter = 2;
			Pointer.transform.localPosition = new Vector3(0.14f,1.25f,-2f);
		}
		print(counter);
	}

	public void NextLevel(){
		manager.LoadLevel(1);
	}
}
