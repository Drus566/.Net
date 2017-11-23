using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Game : MonoBehaviour {

    Player player = new Player();
    private bool checker = false;

    void LateUpdate()
    {
        if (checker)
        {
            StartCoroutine(FindGetReq());
        }
    }

    public void FindGame()
    {
        StartCoroutine(ChangeStatusEnum());
    }

    private IEnumerator ChangeStatusEnum()
    {
        using (UnityWebRequest www = UnityWebRequest.Head("http://localhost:51394/api/user/" + player.ID))
        {
            www.method = "Patch";

            string bodyJson = JsonUtility.ToJson(player);
            byte[] body = new System.Text.UTF8Encoding().GetBytes(bodyJson);

            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();

            if (www.isError)
            {
                print(www.isError);
            }
            else
            {
                print("Find is started");
                print("Form upload complete");
                print(www.downloadHandler.text);
                player = JsonUtility.FromJson<Player>(www.downloadHandler.text);
                checker = true;
            }
        }
    }   

    private IEnumerator FindGetReq()
    {
        checker = false;

        UnityWebRequest getReq = UnityWebRequest.Get("http://localhost:51394/api/user/" + player.ID);
        yield return getReq.Send();

        if (getReq.isError)
        {
            print(getReq.error);
        }
        else
        {
            player = JsonUtility.FromJson<Player>(getReq.downloadHandler.text);
            print(player.Status);
        }
        
        yield return new WaitForSeconds(5);
        if (player.Status == "Awaiting")
        {
            print("The game was found");
        }
        else
        {
            checker = true;
            print("Continue find");
        }
    }
}

public class Player
{
    public int ID = 6;
    public string Username = "Max";
    public string Password = "Max1";
    public string Status = "Active";
    public int Instance_id;
}