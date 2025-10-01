using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements; //namespace for storing UI elements of UI document

[System.Serializable]
public class SignUpUIElements
{
    public string token;
    public string username;
    public string email;
    public string submitButton;
    public string loginButton;
}
public class SignUpPage : Singleton<SignUpPage>
{

    private void OnEnable()
    {
        StartCoroutine(SetLogin());
    }

    private IEnumerator SetLogin()
    {
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:5000/fetchParmsLoading", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        SignUpUIElements signUpElements = JsonUtility.FromJson<SignUpUIElements>(request.downloadHandler.text);

        var root = this.GetComponent<UIDocument>().rootVisualElement;
        var token = signUpElements.token;

        Debug.Log($"Token: {token}");
        var username = root.Q<TextField>(signUpElements.username);
        var email = root.Q<TextField>(signUpElements.email);
        var submitButton = root.Q<Button>(signUpElements.submitButton);
        var loginButton = root.Q<Button>(signUpElements.loginButton);

        username.value = "";
        email.value = "";

        submitButton.clicked += () => { OnSubmitButtonClicked(username, email); };

        loginButton.clicked += () => { OnLoginButtonClicked(); };
    }

    void OnSubmitButtonClicked(TextField username, TextField email)
    {
        if (!string.IsNullOrEmpty(username.value) && !string.IsNullOrEmpty(email.value))
        {
            Debug.Log("Submit Button Clicked.");
        }
    }

    void OnLoginButtonClicked()
    {
        Debug.Log("Login Button Clicked.");
    }
}