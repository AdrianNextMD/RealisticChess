using NaughtyAttributes;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UserLoginManager : MonoBehaviour
{
    [SerializeField] private Color activeColor;
    [SerializeField] private Color activeTextColor;
    [SerializeField] private Color inactiveColor;

    [BoxGroup("Login UI")]
    [SerializeField] private InputField usernameField;
    [BoxGroup("Login UI")]
    [SerializeField] private GameObject usernameCheckUI;
    [BoxGroup("Login UI")]
    //[SerializeField] private InputField emailField;
    //[BoxGroup("Login UI")]
    //[SerializeField] private GameObject emailCheckUI;
    //[BoxGroup("Login UI")]
    [SerializeField] private InputField passwordField;
    [BoxGroup("Login UI")]
    [SerializeField] private GameObject passwordCheckUI;
    [BoxGroup("Login UI")]
    [SerializeField] private Button loginBtn;
    [BoxGroup("Login UI")]
    [SerializeField] private Text loginBtnText;

    public bool general;

    private void Start()
    {
        loginBtn.interactable = false;
        usernameCheckUI.SetActive(false);
        passwordCheckUI.SetActive(false);
    }

    private bool usernameCheck;
    public void UsernameFieldCheck(string value)
    {
        if (value.Length >= 3)
        {
            usernameCheck = true;
            usernameCheckUI.SetActive(true);
        }
        else
        {
            usernameCheck = false;
            usernameCheckUI.SetActive(false);
        }
        CheckAllToRegister();
    }

    //private bool emailCheck;
    //public void EmailFieldCheck(string value)
    //{
    //    if (IsValid(value))
    //    {
    //        emailCheck = true;
    //        emailCheckUI.SetActive(true);
    //    }
    //    else
    //    {
    //        emailCheck = false;
    //        emailCheckUI.SetActive(false);
    //    }
    //    CheckAllToRegister();
    //}

    public bool IsValid(string emailaddress)
    {
        Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        Match match = regex.Match(emailaddress);
        if (match.Success)
            return true;
        else
            return false;
    }

    private bool passwordCheck;
    public void PasswordFieldCheck(string value)
    {
        if (value.Length >= 8)
        {
            passwordCheck = true;
            passwordCheckUI.SetActive(true);
        }
        else
        {
            passwordCheck = false;
            passwordCheckUI.SetActive(false);
        }
        CheckAllToRegister();
    }

    private void CheckAllToRegister()
    {
        if (passwordCheck && usernameCheck) //&& emailCheck)
        {
            loginBtn.interactable = true;
            loginBtnText.color = activeTextColor;
        }
        else
        {
            loginBtn.interactable = false;
            loginBtnText.color = inactiveColor;
        }
    }

    public void LoginNow()
    {
        loginBtn.interactable = false;
        StartCoroutine(ES3CloudManager.Instance.ES3UserLogin(usernameField.text, passwordField.text, general));
        //gameObject.SetActive(false);
    }

}
