using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameInitializer : MonoBehaviour
{
    [Header("Game mode dependent objects")]
    [SerializeField] private SingleplayerChessGameController singleplayerControllerPrefab;
    [SerializeField] private MultiplayerChessGameController multiplayerControllerPrefab;
    [SerializeField] private MultiplayerBoard multiplayerBoardPrefab;
    [SerializeField] private SinglePlayerBoard singleplayerBoardPrefab;

    [Header("Scene references")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private CameraSetup cameraSetup;
    [SerializeField] private ChessUIManager uiManager;
    [SerializeField] private Transform boardAnchor;

    public void CreateMultiplayerBoard()
    {
        if (!networkManager.IsRoomFull())
            PhotonNetwork.Instantiate(multiplayerBoardPrefab.name, boardAnchor.position, boardAnchor.rotation);
    }
    
    public void InitializeMultiplayerController()
    {
        MultiplayerBoard board = FindObjectOfType<MultiplayerBoard>();
        ChessUIManager chessUIManager = FindObjectOfType<ChessUIManager>();
        MultiplayerChessGameController controller = Instantiate(multiplayerControllerPrefab);
        controller.SetDependencies(cameraSetup, uiManager, board);
        controller.InitializeGame();
        controller.SetNetworkManager(networkManager);
        networkManager.SetDependencies(controller);
        chessUIManager.SetDependencies(controller);
        board.SetDependencies(controller);
    }

    public void InitializeSingleplayerController()
    {
        Instantiate(singleplayerBoardPrefab, boardAnchor);
        SinglePlayerBoard board = FindObjectOfType<SinglePlayerBoard>();
        ChessUIManager chessUIManager = FindObjectOfType<ChessUIManager>();
        SingleplayerChessGameController controller = Instantiate(singleplayerControllerPrefab);
        controller.SetDependencies(cameraSetup, uiManager, board);
        controller.InitializeGame();
        board.SetDependencies(controller);
        chessUIManager.SetDependencies(controller);
        controller.StartNewGame();
    }

    public bool PieceIsBuyed()
    {
        return StaticActions.CurrentManager.player.currentItemBuyed == -1 && StaticActions.CurrentManager.player.itemInfo.Count == 0 ? false : true;
    }
    
}