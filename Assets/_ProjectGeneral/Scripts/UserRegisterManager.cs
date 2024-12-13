using NaughtyAttributes;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UserRegisterManager : MonoBehaviour
{
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject registerScreen;
    [SerializeField] private Image loginScreenBtn;
    [SerializeField] private Image registerScreenBtn;
    
    [SerializeField] private Color activeColor;
    [SerializeField] private Color activeTextColor;
    [SerializeField] private Color inactiveColor;

    [BoxGroup("Register UI")]
    [SerializeField] private InputField usernameField;
    [BoxGroup("Register UI")]
    [SerializeField] private GameObject usernameCheckUI;
    [BoxGroup("Register UI")]
    [SerializeField] private InputField emailField;
    [BoxGroup("Register UI")]
    [SerializeField] private GameObject emailCheckUI;
    [BoxGroup("Register UI")]
    [SerializeField] private InputField passwordField;
    [BoxGroup("Register UI")]
    [SerializeField] private GameObject passwordCheckUI;
    [BoxGroup("Register UI")]
    [SerializeField] private Button registerBtn;
    [BoxGroup("Register UI")]
    [SerializeField] private Text registerBtnText;

    public bool general;

    private void Start()
    {
        ScreenSelect();
        registerBtn.interactable = false;
        usernameCheckUI.SetActive(false);
        emailCheckUI.SetActive(false);
        passwordCheckUI.SetActive(false);
    }

    public void ScreenSelect(string screen = "login")
    {
        if(screen.Contains("register"))
        {
            loginScreen.SetActive(false);
            registerScreen.SetActive(true);

            loginScreenBtn.GetComponent<Button>().interactable = true;
            loginScreenBtn.color = activeColor;
            loginScreenBtn.transform.Find("Text").GetComponent<Text>().color = activeTextColor;

            registerScreenBtn.GetComponent<Button>().interactable = false;
            registerScreenBtn.color = inactiveColor;
            registerScreenBtn.transform.Find("Text").GetComponent<Text>().color = inactiveColor;
        }
        else
        {
            registerScreen.SetActive(false);
            loginScreen.SetActive(true);

            loginScreenBtn.GetComponent<Button>().interactable = false;
            loginScreenBtn.color = inactiveColor;
            loginScreenBtn.transform.Find("Text").GetComponent<Text>().color = inactiveColor;

            registerScreenBtn.GetComponent<Button>().interactable = true;
            registerScreenBtn.color = activeColor;
            registerScreenBtn.transform.Find("Text").GetComponent<Text>().color = activeTextColor;
        }
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

    private bool emailCheck;
    public void EmailFieldCheck(string value)
    {
        if (IsValid(value))
        {
            emailCheck = true;
            emailCheckUI.SetActive(true);
        }
        else
        {
            emailCheck = false;
            emailCheckUI.SetActive(false);
        }
        CheckAllToRegister();
    }

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
        if (passwordCheck && usernameCheck && emailCheck)
        {
            registerBtn.interactable = true;
            registerBtnText.color = activeTextColor;
        }
        else
        {
            registerBtn.interactable = false;
            registerBtnText.color = inactiveColor;
        }
    }

    public void RegisterNow()
    {
        registerBtn.interactable = false;
        StartCoroutine(ES3CloudManager.Instance.ES3UserRegister(usernameField.text, passwordField.text, emailField.text, general));
        //gameObject.SetActive(false);
    }

}
