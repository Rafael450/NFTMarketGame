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

        List<string> uris = new List<string>();

        for(int i = 0; i < result.assets.Count; i++)
        {
            if(result.assets[i].image_url != null)
                uris.Add(result.assets[i].image_url);
        }
        
        float radius = 3 / Mathf.Sin(Mathf.PI / uris.Count);
        int numberOfObjects = uris.Count;
        
        for(int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 70, z);
            float angleDegrees = -angle*Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees+90, 180);
            GameObject newObject = Instantiate(prefab, pos, rot);
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(uris[i]);
            await textureRequest.SendWebRequest();
            var Texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            float ratio = (float) Texture.height/(float) Texture.width;
            newObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
            newObject.transform.localScale = new Vector3(4f, 4f*ratio, 0.04f);
            newObject.GetComponent<MeshRenderer>().receiveShadows = false;

        }
        
    }
}
