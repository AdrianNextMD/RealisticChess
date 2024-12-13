using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ES3CloudManager : Singleton<ES3CloudManager>
{
    [SerializeField] private string _filePath = "chessData.es3";
    [SerializeField] private string _globalFilePath = "chessGlobalData.es3";

    [SerializeField] private string _serverUri;
    [SerializeField] private string _apiKey;

    private UnityAction _logOut;

    [SerializeField, BoxGroup("UI Logged In")] private GameObject registerLoginPart;

    [SerializeField, BoxGroup("UI Logged Out")] private GameObject loggedInPart;
    [SerializeField, BoxGroup("UI Logged Out")] private Text loggedName;

    private bool startSyncedData = false;

    private void Start()
    {
        _logOut += LogOutAction;
        SyncUserData();
    }

    private void OnApplicationPause(bool pause)
    {
        if(startSyncedData && pause) SyncUserData();
    }

    private void OnApplicationQuit()
    {
        SyncUserData();
    }

    public void SetLoggedState(bool loggedIn)
    {
        registerLoginPart.SetActive(!loggedIn);
        loggedInPart.SetActive(loggedIn);

        if(ES3.KeyExists("UserName"))
        {
            loggedName.text = "Logged in as: " + ES3.Load<string>("UserName");
        }
        else
        {
            ES3.Save("UserName", "ID:" + SystemInfo.deviceUniqueIdentifier);
            loggedName.text = "Logged in as: " + ES3.Load<string>("UserName");
        }
        
        ChessUIManager.Instance.UpdatePlayer(loggedIn);
    }

    [Button]
    public void SyncUserData()
    {
        if (ES3.KeyExists("UserPass"))
        {
            var user = ES3.Load<string>("UserName");
            var pass = ES3.Load<string>("UserPass");

            StartCoroutine(ES3SyncConnect(user, pass));
            SetLoggedState(true);
        }
        else
        {
            SetLoggedState(false);
        }
    }

    public IEnumerator SyncGlobalFileData(bool download = false, bool upload = false)
    {
        var cloud = new ES3Cloud(_serverUri, _apiKey);

        if (!download && !upload)
        {
            // Synchronise global data local file, but make it global for all users.
            yield return StartCoroutine(cloud.Sync(_globalFilePath));

            Debug.Log("cloud.encoding " + cloud.encoding);
            Debug.Log("cloud.data " + cloud.data);
            Debug.Log("cloud.text " + cloud.text);
            //Debug.Log("cloud.downloadProgress " + cloud.downloadProgress);
            //Debug.Log("cloud.uploadProgress " + cloud.uploadProgress);
        }
        else if(upload)
        {
            // Upload global data local file, but make it global for all users.
            yield return StartCoroutine(cloud.UploadFile(_globalFilePath));
        }
        else if (download)
        {
            // Download global data local file, but make it global for all users.
            yield return StartCoroutine(cloud.DownloadFile(_globalFilePath));
        }

        if (cloud.isError)
        {
            Debug.LogError(cloud.error);
            UiUtils.Instance.SimplePopup(true, cloud.error);
        }
    }

    public IEnumerator ES3SyncConnect(string user = null, string password = null, bool general = false, ES3Settings settings = null)
    {
        UiUtils.Instance.ShowLoading(true, "Sync...");
        // Create a new ES3Cloud object with the URL to our ES3.php file.
        var cloud = new ES3Cloud(_serverUri, _apiKey);

        // Synchronise a local file with the cloud for a particular user.
        yield return StartCoroutine(cloud.Sync(_filePath, user, password));

        if (cloud.errorCode == 3)
        {
            var mess = "User not found or incorect password/username, please contact us for help > ";
            UiUtils.Instance.SimplePopup(true, mess);
            yield break;
        }

        if (cloud.isError)
        {
            Debug.Log(cloud.error);
            UiUtils.Instance.SimplePopup(true, cloud.error);
        }
        else
        {
            UiUtils.Instance.ShowLoading(false);
            if (general) ChessUIManager.Instance.registerLoginUI.SetActive(false);
            ChessUIManager.Instance.UpdatePlayer(true);
            SetLoggedState(true);
            //UiUtils.Instance.SimplePopup(true, "Successfully Signed In, you're welcome!");
        }
        startSyncedData = true;

        StartCoroutine(SyncGlobalFileData());
    }

    public IEnumerator ES3UserLogin(string user = null, string password = null, bool general = false, ES3Settings settings = null)
    {
        UiUtils.Instance.ShowLoading(true, "Logging in...");
        // Create a new ES3Cloud object with the URL to our ES3.php file.
        var cloud = new ES3Cloud(_serverUri, _apiKey);

        ES3.DeleteFile(_filePath);
        ES3.DeleteFile(_globalFilePath);

        // Synchronise a local file with the cloud for a particular user.
        yield return StartCoroutine(cloud.DownloadFile(_filePath, user, password));

        if (cloud.errorCode == 3)
        {
            UiUtils.Instance.ShowLoading(false);
            var mess = "User not found, please contact us for help!";
            UiUtils.Instance.SimplePopup(true, mess);
            yield break;
        }

        if (cloud.isError)
        {
            UiUtils.Instance.ShowLoading(false);
            Debug.Log(cloud.error);
            UiUtils.Instance.SimplePopup(true, cloud.error);
        }
        else
        {
            UiUtils.Instance.ShowLoading(false);
            if(general) ChessUIManager.Instance.registerLoginUI.SetActive(false);
            ChessUIManager.Instance.UpdatePlayer(true);
            SetLoggedState(true);
            UiUtils.Instance.SimplePopup(true, "Successfully Signed In, you're welcome!");
        }

        StartCoroutine(SyncGlobalFileData());
    }

    public IEnumerator ES3UserRegister(string user = null, string password = null, string email = null, bool general = false, ES3Settings settings = null)
    {
        UiUtils.Instance.ShowLoading(true, "Creating account...");
        // Create a new ES3Cloud object with the URL to our custom ES3.php file.
        var es3Cloud = new ES3Cloud(_serverUri, _apiKey);

        ES3.FileExists(_filePath);
        ES3.FileExists(_globalFilePath);

        // Add the scene name to our POST data so we can use this in our custom PHP script.
        es3Cloud.AddPOSTField("scene", SceneManager.GetActiveScene().name);
        if (!string.IsNullOrEmpty(email)) ES3.Save("Email", email); //es3Cloud.AddPOSTField("Email", email);

        //settings.path = _filePath;
        if (!es3Cloud.isError)
        {
            Debug.Log(es3Cloud.downloadProgress);
            ES3.Save("UserName", user);
            ES3.Save("UserPass", password);
        }

        // Upload a file, and our scene POST field will also be received by the PHP script.
        yield return StartCoroutine(es3Cloud.UploadFile(_filePath, user, password));

        if (es3Cloud.isError)
        {
            Debug.Log(es3Cloud.error);
            UiUtils.Instance.SimplePopup(true, es3Cloud.error);
        }
        else
        {
            UiUtils.Instance.ShowLoading(false);
            if (general) ChessUIManager.Instance.registerLoginUI.SetActive(false);
            ChessUIManager.Instance.UpdatePlayer(true);
            SetLoggedState(true);
            UiUtils.Instance.SimplePopup(true, "Successfully Registered, You're Welcome!");
        }

        StartCoroutine(SyncGlobalFileData());
    }

    public void LogOut()
    {
        UiUtils.Instance.ShowPopup(true, "Are you sure to log out?", _logOut);
    }

    private void LogOutAction()
    {
        StartCoroutine(ES3LogOut());
    }

    private IEnumerator ES3LogOut()
    {
        UiUtils.Instance.ShowLoading(true, "Logging out...");

        var user = ES3.Load<string>("UserName");
        var pass = ES3.Load<string>("UserPass");

        // Create a new ES3Cloud object with the URL to our ES3.php file.
        var cloud = new ES3Cloud(_serverUri, _apiKey);

        // Synchronise a local file with the cloud for a particular user.
        yield return StartCoroutine(cloud.UploadFile(_filePath, user, pass));

        if (cloud.errorCode == 3)
        {
            UiUtils.Instance.ShowLoading(false);
            var mess = "Error on logging out, please contact us for help > elermond.gm@gmail.com!";
            UiUtils.Instance.SimplePopup(true, mess);
            yield break;
        }

        if (cloud.isError)
        {
            UiUtils.Instance.ShowLoading(false);
            Debug.Log(cloud.error);
            UiUtils.Instance.SimplePopup(true, cloud.error);
        }
        else
        {
            yield return StartCoroutine(SyncGlobalFileData());

            ES3.DeleteFile(_filePath);
            ES3.DeleteFile(_globalFilePath);

            SetLoggedState(false);

            UiUtils.Instance.ShowLoading(false);
            UiUtils.Instance.SimplePopup(true, "Logged Out, be carefully your data now is not saving, for this you need to Log In/Register!");
        }
    }

}
