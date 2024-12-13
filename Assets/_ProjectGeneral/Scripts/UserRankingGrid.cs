using UnityEngine;
using UnityEngine.UI;

public class UserRankingGrid : MonoBehaviour
{
    [SerializeField] private Text _userName;
    [SerializeField] private Text _userRank;
    [SerializeField] private Text _userWins;
    [SerializeField] private Text _userLoss;
    [SerializeField] private Text _arrayInTop;

    public void SetState(bool state)
    {
        if(state)
        {
            gameObject.SetActive(true);

            _userName.text = "";
            _userRank.text = "";
            _userWins.text = "";
            _userLoss.text = "";

            _arrayInTop.text = "";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
