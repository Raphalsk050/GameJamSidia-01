using SidiaGameJam.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatController = SidiaGameJam.Controller.CharacterController;

public class CatQueueComponent : MonoBehaviour
{
    public List<GameObject> Cats = new();
    public MainInputAction inputAction;

    private static int _selectedCatIndex;
    private static List<CatController> _characterControllers = new();
    private static List<AIBrainComponent> _aiBrains = new();

    private void Start()
    {
        Initialize();
        ChangeSelectedCat(0);
    }

    private void Initialize()
    {
        inputAction = new MainInputAction();
        for (int i = 0; i < Cats.Count; i++)
        {
            var cat = Cats[i];
            _characterControllers.Add(cat.GetComponent<CatController>());
            _aiBrains.Add(cat.GetComponent<AIBrainComponent>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(Cats[_selectedCatIndex].transform.position, 1.2f);
    }

    public static void ChangeToNextCatIndex()
    {
        int newIndex = (_selectedCatIndex + 1) % _characterControllers.Count;
        _selectedCatIndex = newIndex;
        ChangeSelectedCat(_selectedCatIndex);
    }

    public static void ChangeSelectedCat(int catIndex)
    {
        int catIndexPosition = 1;

        for(int i = 0;i < _characterControllers.Count;i++)
        {
            var aiBrain = _aiBrains[i];

            if (i == catIndex)
            {
                aiBrain.enabled = false;

                //to enable the controlled cat input
                _characterControllers[i].EnableInput();
            }

            else
            {
                _characterControllers[i].DisableInput();
                aiBrain.enabled = true;
                aiBrain.distanceTolerance = catIndexPosition;
                aiBrain.target = _characterControllers[catIndex].transform;
                catIndexPosition++;
            }
        }
    }
}
