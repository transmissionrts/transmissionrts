using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkedMessanger {
	
	//just a random number
	private short chatChannel;

	public string lastMessageA;
	public string lastMessageB;
	public string lastMessage;
	private static NetworkedMessanger _networkedMessanger;
	public static NetworkedMessanger Instance {
		get {
			if(_networkedMessanger == null) {
				_networkedMessanger = new NetworkedMessanger(233);
			}
			return _networkedMessanger;
		}
	}

	private NetworkedMessanger(short chatChannel = 131) {
		this.chatChannel = chatChannel;
		//if the client is also the server
		if (NetworkServer.active) 
		{
			//registering the server handler
			NetworkServer.RegisterHandler(chatChannel, ServerReceiveMessage);

		}

		//registering the client handler
		NetworkManager.singleton.client.RegisterHandler (chatChannel, ReceiveMessage);

	}


	private void ReceiveMessage(NetworkMessage message)
	{
		//reading message
		string inMsg = message.ReadMessage<StringMessage> ().value;
		if(inMsg.Contains(PlayerId.PlayerA.ToString())) {
			lastMessageA = inMsg;
		}

		if(inMsg.Contains(PlayerId.PlayerB.ToString())) {
			lastMessageB = inMsg;
		}
				
		lastMessage = inMsg;
	}

	private void ServerReceiveMessage(NetworkMessage message)
	{
		StringMessage myMessage = new StringMessage ();
		//we are using the connectionId as player name only to exemplify
		myMessage.value = message.ReadMessage<StringMessage> ().value;

		//sending to all connected clients
		NetworkServer.SendToAll (chatChannel, myMessage);
	}

	public void SendMessage (string message)
	{
		if(NetworkManager.singleton == null) {
			return;
		}

		StringMessage myMessage = new StringMessage ();
		myMessage.value = message + "-" + NetworkServer.active.ToString();

		//sending to server
		NetworkManager.singleton.client.Send (chatChannel, myMessage);
	}
}
