using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class WallPainter : MonoBehaviour
{
    [SerializeField] private Color paintColor = Color.red;
    [SerializeField] private int brushSize = 20; 
    [SerializeField] private float brushHardness = 0.5f;
    [SerializeField] private LayerMask wallLayer;
    
    [SerializeField] private Renderer wallRenderer;
    [SerializeField] private TextMeshProUGUI percentageText;

    private Texture2D _texture;
    private int _totalPixels;
    private int _paintedPixels;
    private bool _isInitialized = false;
    private bool[] _paintedMap;

    private void Start()
    {
        if (wallRenderer == null) return;
        
        Texture2D originalTex = (Texture2D)wallRenderer.material.mainTexture;
        int width, height;
        
        if (originalTex != null)
        {
            width = originalTex.width;
            height = originalTex.height;
            _texture = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
            _texture.SetPixels(originalTex.GetPixels());
        }
        else
        {
            width = 512;
            height = 512;
            _texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            
        }

        _texture.Apply();
        wallRenderer.material.mainTexture = _texture;

        _totalPixels = width * height;
        _paintedMap = new bool[_totalPixels];
        _isInitialized = true;
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Painting) return;

        if (Input.GetMouseButton(0))
        {
            Paint();
        }
    }

    private void Paint()
    {
        if (!_isInitialized) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10.0f, wallLayer))
        {
            Vector2 pixelUV = hit.textureCoord;
            int centerX = (int)(pixelUV.x * _texture.width);
            int centerY = (int)(pixelUV.y * _texture.height);
            
            int radiusSquared = brushSize * brushSize;

            for (int x = -brushSize; x < brushSize; x++)
            {
                for (int y = -brushSize; y < brushSize; y++)
                {
                    int distSquared = x * x + y * y;

                    if (distSquared < radiusSquared)
                    {
                        int pX = centerX + x;
                        int pY = centerY + y;

                        if (pX >= 0 && pX < _texture.width && pY >= 0 && pY < _texture.height)
                        {
                            Color currentColor = _texture.GetPixel(pX, pY);
                            
                            float distance = Mathf.Sqrt(distSquared);
                            float strength = 1.0f - (distance / brushSize);
                            
                            strength *= brushHardness;
                            
                            Color blendedColor = Color.Lerp(currentColor, paintColor, strength);
                            _texture.SetPixel(pX, pY, blendedColor);
                            
                            int pixelIndex = pY * _texture.width + pX;

                            if (strength > 0.1f && currentColor != paintColor && !_paintedMap[pixelIndex])
                            {
                                _paintedMap[pixelIndex] = true;
                                _paintedPixels++;
                            }
                        }
                    }
                }
            }

            _texture.Apply();
            UpdatePercentage();
        }
    }

    private void UpdatePercentage()
    {
        float percent = 0.0f;
        
        if (percentageText != null)
        {
            percent = Mathf.Clamp((float)_paintedPixels / _totalPixels * 100f, 0f, 100f);
            percentageText.text = "%" + percent.ToString("F0");
        }

        if (percent == 100.0f)
        {
            FinishPainting();
        }
        
    }

    private void FinishPainting()
    {
        if (GameManager.Instance.State == GameState.Painting) GameManager.Instance.UpdateGameState(GameState.Finished);
    }

    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    public void SetBrushColor(Color color)
    {
        paintColor = color;
    }
}