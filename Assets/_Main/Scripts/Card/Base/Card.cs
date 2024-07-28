using UnityEngine;
public abstract class Card : MonoBehaviour
{
    public CardColorEnum CardColor;
    public CardTypeEnum CardTypeEnum;
    public virtual void PlayCard() {}
}
