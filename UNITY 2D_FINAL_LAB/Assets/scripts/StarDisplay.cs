using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    [SerializeField] private Image[] starIcons;   
    [SerializeField] private Sprite filledStar;     
    [SerializeField] private Sprite emptyStar;    

    public void ShowStars(int starCount)
    {
        for (int i = 0; i < starIcons.Length; i++)
        {
            starIcons[i].sprite = i < starCount ? filledStar : emptyStar;
        }
    }
}