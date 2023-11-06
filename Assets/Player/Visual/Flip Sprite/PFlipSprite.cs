using UnityEngine;

public class PFlipSprite : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Properties")]
    [SerializeField] private bool invert;
    private bool _flip;
    public bool Flip => _flip;
    public void SetFlip(bool value)
    {
        _flip = invert ? !value : value;
        sprite.transform.localRotation = _flip ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }
    public void Look(float direction) => SetFlip(direction > 0);
}