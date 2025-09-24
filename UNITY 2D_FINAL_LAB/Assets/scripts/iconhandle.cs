using UnityEngine;
using UnityEngine.UI;

public class IconHandle : MonoBehaviour
{
    [SerializeField] private Image[] _icons;
    [SerializeField] private Color _usedColor;

    public void UseShot(int shotNumber)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if (i < shotNumber)
            {
                _icons[i].color = _usedColor;
            }
        }
    }
}
