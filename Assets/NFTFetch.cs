using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NFTFetch : MonoBehaviour
{
    public class Assets {
        public string image_url;
    }
    
    public class Response {
        public List<Assets> assets;
    }

    async void Start()
    {
        // string account = PlayerPrefs.GetString("Account");
        string account = "0x3B02935B6717b012f8240AA3a3A1Be0eCD37B315";

        // fetch uri from chain
        string URL = "https://testnets-api.opensea.io/api/v1/assets?owner=0x3B02935B6717b012f8240AA3a3A1Be0eCD37B315";
        print("URL: " + URL);

        // fetch json from URL
        UnityWebRequest webRequest = UnityWebRequest.Get(URL);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        var op = await webRequest.SendWebRequest();
        if(webRequest.result == UnityWebRequest.Result.Success)
            print(true);
        else
        {
            print(false);
        }

        var result = JsonConvert.DeserializeObject<Response>(webRequest.downloadHandler.text);
        print(result.assets[0].image_url);

        // parse json to get image uri
        string imageUri = "GetResponseHeaders";
        print("imageUri: " + imageUri);

        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }
}
