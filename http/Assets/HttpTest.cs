using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HttpTest : MonoBehaviour
{
    public int CardID = 3;
    public string url = "https://my-json-server.typicode.com/s4ru/jsonDB";
    public string apiRickAndMorty = "https://rickandmortyapi.com/api/character";

    [SerializeField]
    private TMP_Text UsernameCard;
    [SerializeField]
    private RawImage[] YourRawImage;
    [SerializeField]
    private TMP_Text[] MycardID;
    [SerializeField]
    private TMP_Text[] CardNumber;
    private User userss;

    void Start()
    {

    }

    public void SendRequest()
    {
        CardID = UnityEngine.Random.Range(1, 5);
        StartCoroutine(GetUsers());
    }
 
    IEnumerator DownloadImage(string MediaUrl, RawImage image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();

        if(request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else if(!request.isHttpError)
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest request = UnityWebRequest.Get(url+"/users"+CardID);
        yield return request.SendWebRequest();

        if(request.isNetworkError)
        {
            Debug.Log("NETWORK ERROR" + request.error);
        }

        else
        {

            if(request.responseCode == 200)
            {
                userss = JsonUtility.FromJson<User>(request.downloadHandler.text);
                UsernameCard.text = userss.username;
                for (int i = 0; i < userss.card.Length; i++)
                {
                    StartCoroutine(GetCharacter(i));
                    StartCoroutine(GetCard(i));
                }
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }


   IEnumerator GetCharacter(int index)
   {
       int characterID = userss.card[index];
       UnityWebRequest request = UnityWebRequest.Get(apiRickAndMorty + "/"+ characterID);
       yield return request.SendWebRequest();

        if(request.isNetworkError)
        {
            Debug.Log(request.error);
        }

        else
        {
            
            if(request.responseCode == 200)
            {
                
                Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);

                StartCoroutine(DownloadImage(character.image, YourRawImage[index]));
                MycardID[index].text = character.name;


            }

            else
            {
                Debug.Log(request.error);
            }
        }
    }

    IEnumerator GetCard(int index)
    {
        int DeckID = userss.card[index];
        UnityWebRequest request = UnityWebRequest.Get(url+"/users/"+CardID);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("NETWORK ERROR:" + request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                userss = JsonUtility.FromJson<User>(request.downloadHandler.text);

                CardNumber[index].text = userss.card[index].ToString();
            }
        }

    }




}
[System.Serializable]
public class UsersList
{
    public List<User> users;
}

public class Worlds
{
    public List<int> worlds;
}

[System.Serializable]
public class User
{
    public int id;
    public string username;
    public int[] card;
}

public class Character
{
    public int id;
    public string name;
    public string image;
}

public class CharacterList
{
    public List<Character> charac;
}