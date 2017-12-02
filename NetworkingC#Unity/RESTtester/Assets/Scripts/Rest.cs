using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Rest : MonoBehaviour {

	public InputField UrlField;
    public InputField UserStatus;
    public InputField[] UserField;
	public Text Logs;

	private string serverApi = "http://localhost:51394/api/";

    User user = new User();
    List<User> users = new List<User>();

    Instance instance = new Instance();
    List<Instance> instances = new List<Instance>();
	
	void Start(){
		UrlField.text = "user/1";

        UserField[0].text = "DefaultName";
        UserField[1].text = "DefaultPass";
        UserField[2].text = "DefaultStatus";
    }

    public void Get(){
		StartCoroutine(GetEnum());
	}

    public void Post()
    {
        StartCoroutine(PostEnum());
    }

    /*public void Put()
    {
        StartCoroutine(PutEnum());
    }*/

    public void Patch()
    {
        StartCoroutine(PatchEnum());
    }

    public void Delete()
    {
        StartCoroutine(DeleteEnum());
    }

    private IEnumerator GetEnum(){
        string url = UrlField.text;

		//Если запрос на коллекцию пользователей
		if(!IsNumberContains(url)){
			UnityWebRequest getReq = UnityWebRequest.Get(serverApi + url);
			yield return getReq.Send();

			if(getReq.isError){
				print(getReq.error);
				Logs.text += getReq.error + "\r\n\r\n";
			}else{
                print(getReq.downloadHandler.text);
                // юзеры
                if(getReq.downloadHandler.text != "[]")
                {
                    if (url.ToLower().Contains("user"))
                    {
                        //Для каждого элемента из коллекции элементов
                        foreach (string part in MiniJsonParse(getReq.downloadHandler.text))
                        {
                            print(part);
                            User user = JsonUtility.FromJson<User>(part);
                            users.Add(user);
                            Logs.text += user.Output();
                        }
                    }
                    // инстансы
                    else if (url.ToLower().Contains("instance"))
                    {
                        foreach (string part in MiniJsonParse(getReq.downloadHandler.text))
                        {
                            Instance instance = JsonUtility.FromJson<Instance>(part);
                            instances.Add(instance);
                            Logs.text += instance.Output();
                        }
                    }
                }
                else
                {
                    print("Collection is empty");
                    Logs.text += "Collection is empty";
                }
			}
		//Если запрос на конкретного пользователя
		}else{
			UnityWebRequest getReq = UnityWebRequest.Get(serverApi + url);
			yield return getReq.Send();
         
			if(getReq.isError){
				print(getReq.error);
				Logs.text += getReq.error + "\r\n\r\n";
			}else{
                if(getReq.downloadHandler.text != "null")
                {
                    if (url.ToLower().Contains("user"))
                    {
                        user = JsonUtility.FromJson<User>(getReq.downloadHandler.text);
                        Logs.text += user.Output();
                    }else if (url.ToLower().Contains("instance")){
                        instance = JsonUtility.FromJson<Instance>(getReq.downloadHandler.text);
                        Logs.text += instance.Output();
                    }
                }
                else
                {
                    string id = "";
                    foreach (char c in url)
                    {
                        if (Char.IsNumber(c))
                        {
                            id += c;
                        }
                    }
                    Logs.text += String.Format("The object on ID : " + id + " is null \r\n\r\n");
                }
			}
		}
	}

    private IEnumerator PostEnum()
    {
        WWWForm form = new WWWForm();
        form.AddField("Username", UserField[0].text);
        form.AddField("Password", UserField[1].text);
        form.AddField("Status", UserField[2].text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:51394/api/user", form))
        {

            yield return www.Send();

            if (www.isError)
            {
                print(www.isError);
                Logs.text += www.isError + "\r\n\r\n";
            }
            else
            {
                print("Form upload complete");
                Logs.text += www.downloadHandler.text + " объект создан\r\n\r\n";
            }
        }
    }

    private IEnumerator PutEnum()
    {
        string url = UrlField.text;
        User user = new User(UserField[0].text, UserField[1].text, UserField[2].text);

        using (UnityWebRequest www = UnityWebRequest.Put(serverApi + url, JsonUtility.ToJson(user)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.Send();

            if (www.isError)
            {
               print(www.isError);
               Logs.text += www.isError + "\r\n\r\n";
            }
            else
            {
               print("Form upload complete");
               Logs.text += www.downloadHandler.text + " изменен\r\n\r\n";
            }
        } 
    }

    private IEnumerator PatchEnum()
    {
        string url = UrlField.text;
        User user = new User();
        user.Status = UserStatus.text;

        using (UnityWebRequest getReq = UnityWebRequest.Head(serverApi + url)) {

            getReq.method = "Patch";
            
            string bodyJson = JsonUtility.ToJson(user);
            byte[] body = new System.Text.UTF8Encoding().GetBytes(bodyJson);

            getReq.uploadHandler = (UploadHandler) new UploadHandlerRaw(body);
            getReq.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            getReq.SetRequestHeader("Content-Type", "application/json");

            yield return getReq.Send();

            if (getReq.isError)
            {
                print(getReq.isError);
                Logs.text += getReq.isError + "\r\n\r\n";
            }
            else
            {
                print("Form upload complete");
                Logs.text += getReq.downloadHandler.text + " изменен\r\n\r\n";
            }
        }
    }

    private IEnumerator DeleteEnum()
    {
        string url = UrlField.text;
        UnityWebRequest getReq = UnityWebRequest.Delete(serverApi + UrlField.text);
        yield return getReq.Send();

        if (getReq.isError)
        {
            print(getReq.error);
            Logs.text += getReq.error + "\r\n\r\n";
        }
        else
        {
            if (!IsNumberContains(url))
            {
                Logs.text += "All was deleted\r\n\r\n";
            }
            else
            {
                string id = "";
                foreach (char c in url)
                {
                    if (Char.IsNumber(c))
                    {
                        id += c;
                    }
                }
                Logs.text += String.Format("The object on ID : " + id + " deleted\r\n\r\n");
            }
        }
    }


    //Проверка на наличие в строке цифр (если в методе Get есть цифры, то значит это 
    //запрос на конкретного пользователя по его айди, иначе на коллекцию
    private bool IsNumberContains(string input)
    {
        foreach(char c in input)
        {
            if (Char.IsNumber(c))
            {
                return true;
            }
        }
        return false;
    }

    //Небольшой парсер для метода Get (в том случае если он возвращает коллекцию)
    private string[] MiniJsonParse(string message)
    {
        string workMess = message;
        workMess = workMess.Trim(new char[] { '[', ']' });

        string[] parts = workMess.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

        if(parts.Length > 1)
        {
            for (int i = 1; i < parts.Length - 1; i++)
            {
                parts[i] = "{" + parts[i] + "}";
            }
            parts[0] = parts[0] + "}";
            parts[parts.Length - 1] = "{" + parts[parts.Length - 1];
        }
        return parts;
    }
}

[Serializable]
public class User{

	public int ID;
	public string Username;
	public string Password;
	public string Status;
    public int? Instance_id;

    public User(){}
    
    public User(string name, string pass, string stat)
    {
        Username = name;
        Password = pass;
        Status = stat;
    }
    public string Output(){
        string message;
        return message = "ID : " + ID +
                            "\r\n Username : " + Username +
                            "\r\n Password : " + Password +
                            "\r\n Status : " + Status + 
                            "\r\n Instance_id : " + Instance_id +
                            "\r\n\r\n";
    }
}

[Serializable]
public class Instance{

	public int ID;
	public int player1Id;
	public int player2Id;

    public string Output()
    {
        string message;
        return message = "ID : " + ID +
            "\r\n First player ID : " + player1Id +
            "\r\n Second player ID : " + player2Id + "\r\n\r\n";
    }
}
