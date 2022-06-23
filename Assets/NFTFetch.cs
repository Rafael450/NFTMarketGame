using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NFTFetch : MonoBehaviour
{
    public GameObject prefab;

    public class Assets {
        public string image_url;
    }
    
    public class Response {
        public List<Assets> assets;
    }

    async void Start()
    {
        // string account = PlayerPrefs.GetString("Account");

        string account = "0x729326Aca347af56A94d172BBE384e055C46c6Ca";

        // fetch uri from chain
        string uri = "https://testnets-api.opensea.io/api/v1/assets?owner=" + account;

        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        await webRequest.SendWebRequest();

        var result = JsonConvert.DeserializeObject<Response>(webRequest.downloadHandler.text);

        // parse json to get image uri
        string imageUri = result.assets[0].image_url;

        // fetch image and display in game
        for(int i = 0, j = 0; i < result.assets.Count; i++)
        {
            if(result.assets[i].image_url != null)
            {
                GameObject newObject = Instantiate(prefab, new Vector3(3.5f*(i-j),0,0), Quaternion.identity);
                UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(result.assets[i].image_url);
                print(result.assets[i].image_url);
                await textureRequest.SendWebRequest();
                newObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            }
            else
                j++;

        }
        
    }
}
