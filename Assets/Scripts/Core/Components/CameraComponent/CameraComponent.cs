using System.Collections;
using System.Collections.Generic;
using SidiaGameJam.Components;
using Cinemachine;
using UnityEngine;

public class CameraComponent : ComponentBase
{
    public CharacterSpawn characterSpawn;
    private CinemachineVirtualCamera _virtualCamera;

    protected override void Awake()
    {
        base.Awake();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        characterSpawn.OnSpawnCharacter += ChangeCameraTarget;
    }

    public void ChangeCameraTarget(GameObject newTarget)
    {
        _virtualCamera.Follow = newTarget.transform;
    }
}
