using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

[System.Serializable]
public class LoginCredentials
{
    public string username;
    public string password;
    public string submitButton;
}
public class LoginPage : Singleton<LoginPage>
{
    private void OnEnable()
    {
        StartCoroutine(SetLoginPage());
    }

    private IEnumerator SetLoginPage()
    {
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:5000/fetchParmsLogin", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type",  "application/json");

        yield return request.SendWebRequest();

        LoginCredentials credentials = JsonUtility.FromJson<LoginCredentials>(request.downloadHandler.text);
        var root = this.GetComponent<UIDocument>().rootVisualElement;
        var username = root.Q<TextField>(credentials.username);
        var password = root.Q<TextField>(credentials.password);
        var submitButton = root.Q<Button>(credentials.submitButton);

        submitButton.clicked += () => { OnSubmitButton(username, password); };
    }

    private void OnSubmitButton(TextField username, TextField password)
    {
        if(!string.IsNullOrEmpty(username.value) && !string.IsNullOrEmpty(password.value))
        {
            Debug.Log("Login Button Clicked!");
        }
    }
}