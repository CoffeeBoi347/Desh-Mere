using UnityEngine;
using Amazon;
using Amazon.Auth;
using System.Collections;
using UnityEngine.Networking;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System.Threading.Tasks;

[System.Serializable]
public class AuthCredentials
{
    public string region;
    public string user_pool_id;
    public string client_id;
}

[System.Serializable]
public class AuthAWSCredentials
{
    public string accessKey;
    public string secretAccessKey;
}

public class AuthManager : Singleton<AuthManager>
{
    public string region;
    public string userPoolID;
    public string clientID;

    public string accessKey;
    public string secretAccessKey;
    private AmazonDynamoDBClient dynamoDBClient;

    private void OnEnable()
    {
        StartCoroutine(GetBasicAWSCredentials());
        StartCoroutine(GetAuthCredentials());
    }

    private void Start()
    {
        if(!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretAccessKey))
        {
            var creds = new BasicAWSCredentials(accessKey, secretAccessKey);

            var dynamo = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };

            dynamoDBClient = new AmazonDynamoDBClient(creds, dynamo);
        }
    }

    private IEnumerator GetAuthCredentials()
    {
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:5000/fetchBasicCredentials", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        AuthCredentials authCreds = JsonUtility.FromJson<AuthCredentials>(request.downloadHandler.text);
        region = authCreds.region;
        userPoolID = authCreds.user_pool_id;
        clientID = authCreds.client_id;

        Debug.Log($"Connected to {region} region.");
    }

    private IEnumerator GetBasicAWSCredentials()
    {
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:5000/fetchBasicCreds", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        AuthAWSCredentials awsCreds = JsonUtility.FromJson<AuthAWSCredentials>(request.downloadHandler.text);

        accessKey = awsCreds.accessKey;
        secretAccessKey = awsCreds.secretAccessKey;

        Debug.Log("Fetched AWS Credentials.");
    }
}
