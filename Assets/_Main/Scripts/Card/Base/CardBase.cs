using UnityEngine;
public abstract class Card : MonoBehaviour
{
    public CardColorEnum CardColor { get; set; }
    public CardTypeEnum CardTypeEnum { get; set; }
    public virtual void PlayCard() {}
}
