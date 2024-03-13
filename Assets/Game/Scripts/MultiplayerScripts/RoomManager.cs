using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    void Awake()
    {
        if(instance) // we are checking if one roomManager exists already or not, if it does then we'll destroy it
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)
        {
            // PhotonPrefabs, Resources naam ke folder ke andar banaya hua folder hai, PlayerControllerManager uske andar ek prefab hai
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
