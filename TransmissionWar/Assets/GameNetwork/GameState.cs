using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace TransmissionNetworking {
	
	public class GameState : MonoBehaviour {

		// Set up starting board
		public const int INIT = 0;

		// Players choose a command for the pigeon
		public const int SELECT_COMMAND = 1;

		// Players select a soldier to send command to
		public const int SELECT_RECIPIENT = 2;

		// Check if there are adjacent enemies with pigeons
		public const int CHECK_FOR_POSSIBLE_INTERCEPTIONS = 3;
			
		// If there are possible interceptions, choose to execute your own command or intercept a command
		private const int CHOOSE_INTERCEPTIONS = 4;

		// Commands are executed
		public const int EXECUTE_COMMAND = 5;

		// Check if there are enemy soldiers on the same tile
		public const int CHECK_FOR_COMBAT = 6;
			
		// Combat animation initiated, dice is rolled
		public const int ROLL_DICE = 7;

		// Player with lowest number dies
		public const int KILL_LOSER = 8;

		// Has a soldier reached the end?
		public const int CHECK_FOR_WIN = 9;

		private int _state;

		private int _networkState;

		//just a random number
		private const short chatMessage = 131;

		public int state
		{
			get { return _state; }
		}

		public int timerDelay = 5000;

		private Timer _timer;


		// Use this for initialization
		void Start () {

			//if the client is also the server
			if (NetworkServer.active) 
			{
				//registering the server handler
				NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
				initTimer();
			}

			//registering the client handler
			NetworkManager.singleton.client.RegisterHandler (chatMessage, ReceiveMessage);

		}

		void initTimer() {
			_state = INIT;
			_timer = new Timer();
			_timer.set(timerDelay);
			_timer.start();
		}

		// Update is called once per frame
		void Update () {
			if(NetworkServer.active) {
				updateAsServer();	
			}
				
			if(state != _networkState) {
				_state = _networkState;
				updateState(_state);
			}
		}
			
		void updateAsServer() {
			if(_timer.expired()) {
				_timer.set(timerDelay);
				_timer.start();
				SendMessage((_networkState + 1) % 9);
			}
		}

		void updateState(int state) {

			switch(state) {

			case SELECT_COMMAND: 
				Debug.Log("SELECT_COMMAND");
				break;
			case SELECT_RECIPIENT:
				Debug.Log("SELECT_RECIPIENT");
				break;
			case CHECK_FOR_POSSIBLE_INTERCEPTIONS:
				Debug.Log("CHECK_FOR_POSSIBLE_INTERCEPTIONS");
				break;
			case CHOOSE_INTERCEPTIONS:
				Debug.Log("CHOOSE_INTERCEPTIONS");
				break;
			case EXECUTE_COMMAND:
				Debug.Log("EXECUTE_COMMAND");
				break;
			case CHECK_FOR_COMBAT:
				Debug.Log("CHECK_FOR_COMBAT");
				break;
			case ROLL_DICE:
				Debug.Log("ROLL_DICE");
				break;
			case KILL_LOSER:
				Debug.Log("KILL_LOSER");
				break;
			case CHECK_FOR_WIN:
				Debug.Log("CHECK_FOR_WIN");
				break;

				default:
				Debug.Log("Unknown state: " + state);
				break;
			}

		}

		private void ReceiveMessage(NetworkMessage message)
		{
			//reading message
			_networkState = message.ReadMessage<IntegerMessage> ().value;
		}

		private void ServerReceiveMessage(NetworkMessage message)
		{
			IntegerMessage myMessage = new IntegerMessage ();
			//we are using the connectionId as player name only to exemplify
			myMessage.value = message.ReadMessage<IntegerMessage> ().value;

			//sending to all connected clients
			NetworkServer.SendToAll (chatMessage, myMessage);
		}

		public void SendMessage (int stateID)
		{
			IntegerMessage myMessage = new IntegerMessage ();
			myMessage.value = stateID;
	
			//sending to server
			NetworkManager.singleton.client.Send (chatMessage, myMessage);
		}

	}

}