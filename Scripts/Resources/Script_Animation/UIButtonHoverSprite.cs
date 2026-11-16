using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRawButtonHover : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("대상 RawImage")]
    public RawImage targetImage;

    [Header("버튼 텍스처")]
    public Texture normalTexture;
    public Texture hoverTexture;
    public Texture pressedTexture;

    private void Reset()
    {

        targetImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        if (targetImage == null) return;

        if (normalTexture != null)
            targetImage.texture = normalTexture;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage == null) return;

        if (hoverTexture != null)
            targetImage.texture = hoverTexture;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage == null) return;

        if (normalTexture != null)
            targetImage.texture = normalTexture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage == null) return;

        if (pressedTexture != null)
            targetImage.texture = pressedTexture;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetImage == null) return;

        if (hoverTexture != null)
            targetImage.texture = hoverTexture;
        else if (normalTexture != null)
            targetImage.texture = normalTexture;
    }
}
