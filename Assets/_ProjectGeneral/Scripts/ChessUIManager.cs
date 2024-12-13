using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ChessUIManager : Singleton<ChessUIManager>
{
	[Header("Dependencies")]
	public NetworkManager networkManager;
	[BoxGroup("User Log UI")] public GameObject registerLoginUI;

	[Header("Buttons")]
	[SerializeField] private Button whiteTeamButtonButton;
	[SerializeField] private Button blackTeamButtonButton;

	[SerializeField] private Image whiteTeamImage;
	[SerializeField] private Image blackTeamImage;

	[Header("Texts")]
	[SerializeField] private Text finishText;
	[SerializeField] private Text moneyText;
	[SerializeField] private Text winsText;
	[SerializeField] private Text losesText;
	[SerializeField] private Text rankText;

	[BoxGroup("User UI"), SerializeField] private Text userName;

	[Header("Other UI")]
	[SerializeField] private Dropdown gameLevelSelection;
	[SerializeField] public GameObject demoChessBoard;
	
	private ChessGameController chessController;
	[SerializeField] private PlayerData _playerData;

	[Space(1f)]
	[SerializeField] private GameInitializer _gameInitializer;

	private void Start()
	{
		gameLevelSelection.AddOptions(Enum.GetNames(typeof(ChessLevel)).ToList());
		OnGameLaunched();
	}

	public void UpdatePlayer(bool loggedIn)
    {
		if(loggedIn)
		{
			_playerData.LoadSave();
			Money();
			UpdateUserRank();
			userName.text = ES3.Load<string>("UserName");
		}
		else
        {
			_playerData.LoadSave();
			Money();
			UpdateUserRank();
			userName.text = ES3.Load<string>("UserName");
		}
		
		SpawnManager.Instance.SpawnChessAndBoard(true);
	}

	[Button]
	public void TestAddWins()
    {
		Wins(200);
		UpdateUserRank();
	}

	[Button]
	public void TestAddLoses()
	{
		Loss(100);
		UpdateUserRank();
	}

	[Button]
	public void TestAddMoney()
	{
		Money(10000);
	}

	public void SetDependencies(ChessGameController chessController)
	{
		this.chessController = chessController;
	}

	public int Money(int _add = 0, int _remove = 0, int _standartValue = 6000)
    {
		if (_add != 0 && _add > 0)
        {
			ES3.Save("Money", ES3.Load("Money", _standartValue) + _add);
		}
		else if (_remove != 0 && _remove > 0)
        {
			ES3.Save("Money", ES3.Load("Money", _standartValue) - _remove);
		}
		
		var val = ES3.Load("Money", _standartValue);
		moneyText.text = "$" + val.ToString("#,##0");
		return val;
	}

	private int Wins(int _add = 0, int _remove = 0, int _standartValue = 0)
	{
		if (_add != 0 && _add > 0)
		{
			ES3.Save("Wins", ES3.Load("Wins", _standartValue) + _add);
		}
		else if (_remove != 0 && _remove > 0)
		{
			ES3.Save("Wins", ES3.Load("Wins", _standartValue) - _remove);
		}
		if (_add > 0 || _remove > 0) ES3CloudManager.Instance.SyncUserData();

		var val = ES3.Load("Wins", _standartValue);
		winsText.text = "Wins: " + val.ToString("#,##0");
		return val;
	}

	private int Loss(int _add = 0, int _remove = 0, int _standartValue = 0)
	{
		if (_add != 0 && _add > 0)
		{
			ES3.Save("Loss", ES3.Load("Loss", _standartValue) + _add);
		}
		else if (_remove != 0 && _remove > 0)
		{
			ES3.Save("Loss", ES3.Load("Loss", _standartValue) - _remove);
		}
		if (_add > 0 || _remove > 0) ES3CloudManager.Instance.SyncUserData();

		var val = ES3.Load("Loss", _standartValue);
		losesText.text = "Loss: " + val.ToString("#,##0");
		return val;
	}

	public void UpdateUserRank()
    {
		rankText.text = "Rank: " + (1 * (Wins() + 1)) / (Loss() + 2);
    }

	public void RestartGame()
    {
		// AdsManager.Instance.ShowAd("RestartGame", Ads.INTERSTITIAL, () =>
		// {
		// 	if (chessController)
		// 	{
		// 		chessController.RestartGame();
		// 		print("RestartGame");
		// 	}
		// });
	}

	public void KillTheGame(bool online = false)
	{
		UiUtils.Instance.ShowLoading(true, "Loading...");
		if (chessController)
		{
			chessController.KillGame();
			Destroy(chessController.gameObject);
			print("Kill The Game");

			if(online) MenusManager.Instance.OpenMenu("ModeSelectionScreen");
			else
            {
				MenusManager.Instance.OpenMenu("ModeSelectionScreen");
				UiUtils.Instance.ShowLoading(false);
			}
		}
	}

	internal void OnGameLaunched()
	{
		MenusManager.Instance.OpenMenu("ModeSelectionScreen");
	}

	public void OnSinglePlayerModeSelected()
	{
		if (!_gameInitializer.PieceIsBuyed())
		{
			UiUtils.Instance.SimplePopup(true, "You need to buy a pieces");
			return;
		}
		else
		{
			SpawnManager.Instance.DestroyPieces();
			MenusManager.Instance.OpenMenu("");
			_gameInitializer.InitializeSingleplayerController();
		}
	}

	public void OnMultiPlayerModeSelected()
	{
		//connectionStatus.gameObject.SetActive(true);
		if (StaticActions.CurrentManager.player.currentItemBuyed == -1 &&
		    StaticActions.CurrentManager.player.itemInfo.Count == 0)
		{
			UiUtils.Instance.SimplePopup(true, "You need to buy a pieces");
			return;
		}
		MenusManager.Instance.OpenMenu("ConnectScreen");
	}

	public void OnMenuShopSelected()
	{
		MenusManager.Instance.OpenMenu("ShopMenuScreen");
	}

	internal void OnGameFinished(string winner)
	{
		MenusManager.Instance.OpenMenu("GameOverScreen");
		
		if(PhotonNetwork.InRoom)
        {
			if (networkManager.chessGameController.localPlayer.team.ToString() == winner)
			{
				finishText.text = "You won!";
				Wins(1);
			}
			else
			{
				// AdsManager.Instance.ShowAd("RestartGame", Ads.INTERSTITIAL, () =>
				// {
				// 	finishText.text = "You Lose!";
				// 	Loss(1);
				// });
			}
		}
		else
        {
			finishText.text = chessController.activePlayer.team + " Win!";
		}
	}

	public void OnConnect()
	{
		UiUtils.Instance.ShowLoading(true, "Connecting...");
		networkManager.SetPlayerLevel((ChessLevel)gameLevelSelection.value);
		networkManager.Connect();
	}

	internal void ShowTeamSelectionScreen()
	{
		MenusManager.Instance.OpenMenu("TeamSelectionScreen");
	}

	public void OnGameStarted()
	{
		if (PhotonNetwork.InRoom) MenusManager.Instance.OpenMenu("MultiplayerMenu");
		else MenusManager.Instance.OpenMenu("SingleplayerMenu");
	}

	public void BackToMenu()
	{
		// AdsManager.Instance.ShowAd("BackToMenu", Ads.INTERSTITIAL, () =>
		// {
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
				PhotonNetwork.Disconnect();
				KillTheGame(true);
			}
			else
			{
				KillTheGame(false);
			}
		// });
	}

	public void BackBtn(GameObject menu)
    {
		//StartCoroutine(SpawnManager.Instance.SpawnCheesAndBoard(true));

		KillTheGame();
		MenusManager.Instance.OpenMenu(menu.name);
	}

	public void UpdateMoney()
	{
		Money();
	}

	public void SelectTeam(int team)
	{
		networkManager.SetPlayerTeam(team);
	}

	internal void RestrictTeamChoice(TeamColor occpiedTeam)
	{
		Button buttonToDeactivate = occpiedTeam == TeamColor.White ? whiteTeamButtonButton : blackTeamButtonButton;
		buttonToDeactivate.interactable = false;
	}

}
