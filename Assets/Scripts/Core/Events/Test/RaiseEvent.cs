using SidiaGameJam.Events;
using UnityEngine;

public class RaiseEvent : MonoBehaviour
{
    [SerializeField] private SoEventOneParamRelated<float> doDamageToDummy;
    //[SerializeField] private SOEventOneParam<float> _doHealToDummy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) doDamageToDummy.InvokeAction(1f, gameObject);
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            _doHealToDummy.InvokeAction(1f);
        }*/
    }
}