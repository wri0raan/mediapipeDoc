using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHoverOverlay : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("기본 이미지(아래)")]
    public RawImage normalImage;

    [Header("Hover 오버레이(위)")]
    public GameObject hoverOverlayObject;
    public RawImage hoverOverlayImage;

    [Header("텍스처")]
    public Texture normalTexture;
    public Texture hoverTexture;
    public Texture pressedTexture;

    void Reset()
    {
        normalImage = GetComponent<RawImage>();
    }

    void Awake()
    {
        if (normalImage && normalTexture)
            normalImage.texture = normalTexture;

        if (hoverOverlayObject)
            hoverOverlayObject.SetActive(false);

        if (hoverOverlayImage && hoverTexture)
        {
            hoverOverlayImage.texture = hoverTexture;
            hoverOverlayImage.raycastTarget = false;
            hoverOverlayImage.SetNativeSize();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverOverlayObject) hoverOverlayObject.SetActive(true);
        if (hoverOverlayImage && hoverTexture) hoverOverlayImage.texture = hoverTexture;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverOverlayObject) hoverOverlayObject.SetActive(false);
        if (normalImage && normalTexture) normalImage.texture = normalTexture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hoverOverlayObject) hoverOverlayObject.SetActive(true);
        if (hoverOverlayImage && pressedTexture) hoverOverlayImage.texture = pressedTexture;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hoverOverlayObject) hoverOverlayObject.SetActive(true);
        if (hoverOverlayImage && hoverTexture) hoverOverlayImage.texture = hoverTexture;
    }
}
