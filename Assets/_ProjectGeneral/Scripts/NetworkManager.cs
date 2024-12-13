using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviourPunCallbacks
{
	private const string LEVEL = "level";
	private const string TEAM = "team";
	private const byte MAX_PLAYERS = 2;
	[SerializeField] private ChessUIManager uiManager;
	[SerializeField] private GameInitializer gameInitializer;
	[HideInInspector] public MultiplayerChessGameController chessGameController;

	private ChessLevel playerLevel;

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void SetDependencies(MultiplayerChessGameController chessGameController)
	{
		this.chessGameController = chessGameController;
	}

	public void Connect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }, MAX_PLAYERS);
			//PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.NickName = ES3.Load<string>("UserName");
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	#region Photon Callbacks

	public override void OnConnectedToMaster()
	{
		Debug.Log($"Connected to server. Looking for random room with level {playerLevel}");
		PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }, MAX_PLAYERS);
		//PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log($"Joining random room failed becuse of {message}. Creating new one with player level {playerLevel}");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			CustomRoomPropertiesForLobby = new string[] { LEVEL },
			MaxPlayers = MAX_PLAYERS,
			CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }
		});

		UiUtils.Instance.ShowLoading(false);
		//PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined a room with level: {(ChessLevel)PhotonNetwork.CurrentRoom.CustomProperties[LEVEL]}");
		gameInitializer.CreateMultiplayerBoard();
		PrepareTeamSelectionOptions();
		uiManager.ShowTeamSelectionScreen();

		UiUtils.Instance.ShowLoading(false);
		UiUtils.Instance.SimplePopup(true, "Please select your chess color!");
	}

	public override void OnLeftRoom()
	{
		UiUtils.Instance.ShowLoading(false);
	}

	private void PrepareTeamSelectionOptions()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			UiUtils.Instance.OnlinePopup(false);
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);

			if (player.CustomProperties.ContainsKey(TEAM))
			{
				var occupiedTeam = player.CustomProperties[TEAM];
				uiManager.RestrictTeamChoice((TeamColor)occupiedTeam);
			}
		}
		else
        {
			UiUtils.Instance.OnlinePopup(true, 0, "Waiting for a player ...");
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log($"Player {newPlayer.ActorNumber} entered a room!");
		UiUtils.Instance.OnlinePopup(true, 4f, newPlayer.NickName + " entered the room!");
	}

	#endregion

	public void SetPlayerLevel(ChessLevel level)
	{
		playerLevel = level;
		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { LEVEL, level } });
	}

	public void SetPlayerTeam(int teamInt)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
			if (player.CustomProperties.ContainsKey(TEAM))
			{
				var occupiedTeam = player.CustomProperties[TEAM];
				teamInt = (int)occupiedTeam == 0 ? 1 : 0;
			}
		}

		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, teamInt } });
		gameInitializer.InitializeMultiplayerController();
		chessGameController.SetLocalPlayer((TeamColor)teamInt);
		chessGameController.StartNewGame();
		chessGameController.SetupCamera((TeamColor)teamInt);
	}

	internal bool IsRoomFull()
	{
		bool state = PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
		if(state) GetComponent<PhotonView>().RPC("OnGameStarted", RpcTarget.AllBuffered);
		return state;
	}

	[PunRPC]
	private void OnGameStarted()
    {
		UiUtils.Instance.OnlinePopup(true, 6f, "Game is started!");
	}
}
