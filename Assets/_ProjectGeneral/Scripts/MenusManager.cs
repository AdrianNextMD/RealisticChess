using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : Singleton<MenusManager>
{
	[SerializeField] private List<GameObject> _menus = new List<GameObject>();
	
	public void OpenMenu(GameObject menu)
	{
		for (int i = 0; i < _menus.Count; i++)
		{
			if(menu)
			{
				if (_menus[i].activeSelf)
				{
					_menus[i].SetActive(false);
				}
				if (_menus[i].name == menu.name)
				{
					_menus[i].SetActive(true);
				}
			}
		}
	}

	public void OpenMenu(string menuName = "")
	{
		switch (menuName)
		{
			case "ModeSelectionScreen":
				if (menuName == "ModeSelectionScreen")
				{
					StaticActions.setInitialPositionCamera?.Invoke();
				}
				else
				{
					SpawnManager.Instance.DestroyPieces();
				}
				break;
			
			case "ShopMenuScreen":
				StaticActions.shopPositionCamera?.Invoke();
				break;
		}
			
		for (int i = 0; i < _menus.Count; i++)
		{
			if (_menus[i].activeSelf)
			{
				if(_menus[i].name == "SingleplayerMenu" || _menus[i].name == "ShopMenuScreen" || _menus[i].name == "MultiplayerMenu")
				{
					SpawnManager.Instance.SpawnChessAndBoard(true);
				}
				_menus[i].SetActive(false);
			}
			if (_menus[i].name == menuName)
			{
				_menus[i].SetActive(true);
			}
		}
	}
}
