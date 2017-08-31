using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Level2 : MonoBehaviour {

	public int Rows,Columns;
	public int CountBlockage;
	public GameObject Cell;
	public GameObject Block;
	public GameObject Sweet;
	public GameObject Finish;
	public Button Next;
	public Button SetBlocksBtn;
	public bool RandomBlocks = true;
	
	private GameObject cellsParent, sweetParent, blockParent, parent; // Родительские объекты
	private GameObject key, _lock; // Предметы
	private GameObject sweet;
	private Vector3 way;
	private int[,] map;
	private int x,y;
	private float step = 0.4f;
	private bool move = false;
	private float driver = 0;
	private Manager manager;
	private bool setBlocks = false;


	void Start () {
		manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Manager>();
		parent = GameObject.FindGameObjectWithTag("Parent");
		cellsParent = GameObject.FindGameObjectWithTag("CellParent");
		sweetParent = GameObject.FindGameObjectWithTag("SweetParent");
		blockParent = GameObject.FindGameObjectWithTag("BlockParent");
		key = GameObject.FindGameObjectWithTag("Key");
		_lock = GameObject.FindGameObjectWithTag("Lock");
		key.SetActive(false);
		InitMap();
		if(!RandomBlocks){
			SetBlocksBtn.gameObject.SetActive(true);
		}
	}
	void LateUpdate(){
		// Плавноe движение
		if(move){
			driver += Time.deltaTime;
			float lerpValue = Mathf.InverseLerp(0f,0.5f,driver);
			sweet.transform.position = Vector3.Lerp(sweet.transform.position, way, lerpValue);
			if(lerpValue == 1){
				move = false;
				driver = 0;
			}
		}
	}

	private void InitMap(){
		// 0 - можно ходить, 1 - препятствие, 2 - финиш
		map = new int[Rows,Columns];
		int counter = Random.Range(4,8);
		for(int i = 0; i < Rows; i++){
			for(int j = 0; j < Columns; j++){

				// Инициализация конфеты
				if(i == 0 && j == 0 && map[i,j] == 0){ 
					x = i; y = j;
					sweet = Instantiate(Sweet,new Vector2(i,j),Quaternion.identity);
					sweet.transform.parent = sweetParent.transform;
				}

				// Инициализация префаба клеток
				GameObject obj = Instantiate(Cell,new Vector2(step * i,step * j),Quaternion.identity);
				obj.transform.parent = cellsParent.transform;
				obj.name = i + " , " + j;

				// Инициализация массива карты.
				map[i,j] = 0;
				
				// Инициализация Финиша
				if(i == Rows - 1 && j == Columns - 1){
					GameObject _finish = Instantiate(Finish, new Vector3(i * step, j * step, -0.1f), Quaternion.identity);
					_finish.transform.parent = sweetParent.transform;
					
					map[i,j] = 2;
				}

				// Инициализация блока
				if(RandomBlocks){
					if(counter <= 0){
						if(CountBlockage > 0){
							if(i != Rows - 1 && j != Columns - 1){
								// инициализация префаба блоков
								GameObject block = Instantiate(Block,new Vector3(step * i, step * j, -0.1f), Quaternion.identity);
								block.transform.parent = blockParent.transform;
								block.name = i + "" + j + "b";

								// 1 - Блок в массиве карты
								map[i,j] = 1;

								counter = Random.Range(4,8);
								CountBlockage--;
							}
						}
					}else{
						counter--;
					}
				}
				//print(counter);
			}
		}
		// Сдвиг для того, чтобы карта встала в игровой автомат
		parent.transform.position -= new Vector3(2.2f,1.15f,0);
		//OutMap();
	}

	public void InitBlocks(){
		for(int i = 0; i < Rows; i++){
			for(int j = 0; j < Columns; j++){
				if(map[i,j] == 1){
					GameObject block = Instantiate(Block,new Vector3(step * i, step * j, -0.1f), Quaternion.identity);
					block.transform.parent = blockParent.transform;
					block.name = i + "" + j + "b";
				}
			}
		}
		blockParent.transform.localPosition -= new Vector3(2.2f,1.15f,0);
	}
	private void OutMap(){
		for(int i = 0; i < Rows; i++){
			for(int j = 0; j < Columns; j++){
				print(map[i,j] + " : [" + i + ", " + j + "]");
			}
		}
	}

	// --------------------------------------------------------
	// Кнопки

	// -2.2f и -1.15f это сдвиг позиции конфеты
	// Вверх
	public void Up(){
		if(move) return;
		for(int i = y; i < Columns; i++){
			if(map[x,i] == 1){
				way = new Vector3(x * step,(i - 1) * step) - new Vector3(2.2f,1.15f);
				move = true;
				y = i - 1;
				return;
			}else if( i == Columns - 1){
				way = new Vector3(x * step, i * step) - new Vector3(2.2f,1.15f);
				move = true;
				y = i;
				if(map[x,y] == 2){
					End();
					return;
				}
				return;
			}
		}
	}
	// Вниз
	public void Down(){
		if(move) return;
		for(int i = y; i >= 0; i--){
			if(map[x,i] == 1){
				way = new Vector3(x * step, (i + 1) * step) - new Vector3(2.2f,1.15f);
				move = true;
				y = i + 1;
				return;
			}else if(i == 0){
				way = new Vector3(x * step, i * step) - new Vector3(2.2f,1.15f);
				move = true;
				y = i;
				if(map[x,y] == 2){
					End();
					return;
				}
				return;
			}
		}
	}
	// Влево
	public void Left(){
		if(move) return;
		for(int i = x; x >= 0; i--){
			if(map[i,y] == 1){
				way = new Vector3((i + 1) * step, y * step) - new Vector3(2.2f,1.15f);
				move = true;
				x = i + 1;
				return;
			}else if( i == 0){
				way = new Vector3(i * step, y * step) - new Vector3(2.2f,1.15f);
				move = true;
				x = i;
				if(map[x,y] == 2){
					End();
					return;
				}
				return;
			}
		}
	}

	// Вправо
	public void Right(){
		if(move) return;
		for(int i = x; i < Rows; i++){
			if(map[i,y] == 1){
				way = new Vector3((i - 1) * step, y * step) - new Vector3(2.2f,1.15f);
				move = true;
				x = i - 1;
				return;
			}else if( i == Rows - 1){
				way = new Vector3(i * step, y * step) - new Vector3(2.2f,1.15f);
				move = true;
				x = i;
				if(map[x,y] == 2){
					End();
					print("END");
					return;
				}
				return;
			}
		}
	}
	
	// Когда пройдена игра в автомате
	private void End(){
		print("END");
		sweet.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Click");
		key.SetActive(true);
		StartCoroutine(Wait(3));
	}

	// Метод для вывода координат (Для удобства)
	public void OutCoord(){
		print(x + "," + y + " = " + map[x,y]);
	}

	// Отключение Sweet спустя анимацию
	private IEnumerator Wait(float time){
		yield return new WaitForSeconds(time);
		sweet.SetActive(false);
	}

	// Нажатие на ключ
	public void EndAnim(){
		_lock.GetComponent<Animator>().SetTrigger("Click");
		Next.gameObject.SetActive(true);
		key.GetComponent<Animator>().SetTrigger("Click");
	}

	// После нажатия кнопки для перехода на следующий уровень
	public void NextLevel(){
		manager.LoadLevel(2);
	}

	// Установка блока на карте
	public int SetCell(int x, int y){
		return map[x,y] = 1;
	}

	// Возврат булевой перменной для установки блоков
	public bool GetSetBlock(){
		return setBlocks;
	}
	
	public void SetSetBlock(bool val){
		setBlocks = val;
	}
}
