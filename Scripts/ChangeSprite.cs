using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [System.Serializable]
    public class Sprites
    {
        public string name;
        public Sprite sprite;
    }

    SpriteRenderer Renderer;
    [SerializeField] public List<Sprites> spritesList;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeSpriteRenderer(Sprite sprite)
    {
        Renderer.sprite = sprite;
    }
}
