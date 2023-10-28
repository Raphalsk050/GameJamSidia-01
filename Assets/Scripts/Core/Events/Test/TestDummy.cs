using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private readonly float _maxLife = 100f;
    private float _currentLife = 100f;

    public void DebugMessage()
    {
        Debug.Log("The event was triggered");
    }

    public void TakeDamage(float value)
    {
        if (_currentLife - value >= 0f) _currentLife -= value;

        Debug.Log($"TOOK DAMAGE => {_currentLife}");
    }

    public void Heal(float value)
    {
        if (_currentLife + value <= _maxLife) _currentLife += value;

        Debug.Log($"HEAL => {_currentLife}");
    }
}