using UnityEngine;

public class LoaderMainUpdatePopup : MonoBehaviour
{
    public string androidAppUrl = "https://play.google.com/store/apps/details?id=com.elermond.realisticchess";
    public string appStoreAppUrl = "https://games.elermond.com/";

    public void CloseAndUpdateMain()
    {
        Application.Quit();

        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL(androidAppUrl);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL(appStoreAppUrl);
        }
    }
}
