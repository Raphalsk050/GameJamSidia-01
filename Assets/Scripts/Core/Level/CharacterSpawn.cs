using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public UnityAction<GameObject> OnSpawnCharacter;
    public bool changeCameraTarget;
    
    private void Start()
    {
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        var newCharacterInstance = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        
        if(changeCameraTarget)
            OnSpawnCharacter?.Invoke(newCharacterInstance);
    }
    
    public void OnObjectDead()
    {
        SpawnCharacter();
        Debug.Log("AAAAAAAAA");
    }
}
