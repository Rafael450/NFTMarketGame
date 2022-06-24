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
        string account = PlayerPrefs.GetString("Account");

        // fetch uri from chain
        // ##########don't forget to add a person's collections too#############
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
                GameObject newObject = Instantiate(prefab, new Vector3(5f*(i-j), 70,0), Quaternion.identity);
                UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(result.assets[i].image_url);
                await textureRequest.SendWebRequest();
                var Texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
                float ratio = Texture.height/Texture.width;
                newObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
                newObject.transform.localScale = new Vector3(4f, 4f*ratio, 0.04f);
            }
            else
                j++;

        }
        
    }
}
