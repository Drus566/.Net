using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityClient;
using UnityEngine.UI;
using System;
public class NewBehaviourScript : MonoBehaviour {
	ChatUnity obj = new ChatUnity();
	public Text Logs;
	void Start(){
		obj.Host = "127.0.0.1";
		obj.Port = 8888;
		obj.UserName = "Drus";
		obj.Message = "My default message";
	}

	void LateUpdate(){
		if(obj.Relay){
			Logs.text += obj.Logs;
			obj.Relay = false;
		}
	}
	public void Connect(){
		obj.Connect();
		Logs.text = "";
		Logs.text += obj.Logs;
	}

	public void SetHost(Text tObj){
		obj.Host = tObj.text;
	}

	public void SetPort(Text tObj){
		obj.Port = Int32.Parse(tObj.text);
	}

	public void SetUserName(Text tObj){
		obj.UserName = tObj.text;
	}

	public void SetMessage(Text tObj){
		obj.Message = tObj.text;
	}

	public void Send(){
		obj.SendMessage();
		Logs.text += obj.Logs;
	}

	public void Disconnect(){
		obj.Disconnect();
		Logs.text += obj.Logs;
	}
}
