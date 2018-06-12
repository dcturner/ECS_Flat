using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Core : MonoBehaviour
{

    static Camera CAM;
    static float WOBBLE = 2f;
    static float WOBBLE_RATE = 3.0f;
    static Material lineMaterial;
    static float Z = 0f;

    private void Awake()
    {
        CAM = GetComponent<Camera>();
    }
    private void OnPostRender()
    {
        Vector3[] verts = new Vector3[10];
        Color[] colours = new Color[10];
        float _w2 = Screen.width * 0.5f;
        float _h2 = Screen.height * 0.5f;

        for (int i = 0; i < 10; i++)
        {
            float X = Screen.width * 0.5f + (Mathf.Sin(Time.realtimeSinceStartup + i * 0.2f) * 200f);
            float Y = Screen.height * 0.5f + (Mathf.Cos(Time.realtimeSinceStartup + i * 0.2f) * 300f);
            verts[i] = new Vector3(X, Y, Z);
            colours[i] = new Color((i / 10f), 1f, 1f, (i / 10f));
        }

        GL.LoadPixelMatrix();
        int ellipseCount = 20;
        for (int i = 0; i < ellipseCount; i++)
        {
            float factor = i / (float)ellipseCount;
            Draw_ELLIPSE_HATCHED(36, _w2, _h2, 200 + (i * 20), 200 + (i * 20), 0.05f, Time.realtimeSinceStartup * (i * 0.01f), 0.95f, new Color(1f, 1f, 1f, 1f - factor));
            //Draw_ELLIPSE_WIRE(36, Screen.width * 0.5f, Screen.height * 0.5f, 200 + (i * 6), 200 + (i * 6), new Color(1f, 1f, 1f, 1f - factor));
        }

        int count = 50;
        for (int i = 0; i < count; i++)
        {
            float _X = _w2 / 2f + ((_w2 / count) * i);
            float _Y = Screen.height * 0.45f;
            float randStrength = Mathf.PerlinNoise(0, Time.realtimeSinceStartup + i * 0.02f);
            Draw_LINE(_X, _Y, _X, _Y + (Mathf.PerlinNoise(0, randStrength) * _h2 / 2f), Color.Lerp(Color.yellow, Color.cyan, randStrength));
        }
        float helix_w = 150;
        float helix_h = 200;
        float xOffset = 5f;
        float yOffset = 2.5f;
        int helixCount = 40;
        for (int i = 0; i < helixCount; i++)
        {
            float wNoise = PNOISE_TIME(0.2f, i, helix_w * 0.25f, helix_w);
            float hNoise = PNOISE_TIME(0.2f, i + 2, helix_h * 0.25f, helix_h);
            Draw_LINE(
                _w2 + Mathf.Sin((Time.realtimeSinceStartup + i) * xOffset) * wNoise,
                _h2 + Mathf.Cos((Time.realtimeSinceStartup + i) * yOffset) * hNoise,
                _w2 + Mathf.Sin((Time.realtimeSinceStartup + i + 0.05f) * xOffset) * wNoise,
                _h2 + Mathf.Cos((Time.realtimeSinceStartup + i + 0.05f) * yOffset) * hNoise,
                new Color(1f, 1f, 1f, PNOISE_TIME(0.3f, i, 0f, 1f, i)));
        }
    }

    #region POLY
    public static void Draw_POLY_WIRE(Vector3[] _verts, Color[] _colours)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        for (int vertIndex = 0; vertIndex < _verts.Length; vertIndex++)
        {
            Vector3 _tempPos = _verts[vertIndex];
            Color _tempColour = _colours[vertIndex % _colours.Length];
            Add_VERT(_tempPos.x, _tempPos.y, _tempColour);
        }

        GL.End();
        GL.PopMatrix();
    }
    public static void Draw_POLY_WIRE(Vector3[] _verts, Color _colour)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        for (int vertIndex = 0; vertIndex < _verts.Length; vertIndex++)
        {
            Vector3 _tempPos = _verts[vertIndex];
            Add_VERT(_tempPos.x, _tempPos.y, _colour);
        }

        GL.End();
        GL.PopMatrix();
    }
    #endregion
    #region RECT
    public static void Draw_RECT_WIRE(float _x, float _y, float _w, float _h, Color _col)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        GL.Vertex3(_x, _y, Z);
        GL.Vertex3(_x + _w, _y, Z);
        GL.Vertex3(_x + _w, _y + _h, Z);
        GL.Vertex3(_x, _y + _h, Z);
        GL.Vertex3(_x, _y, Z);

        GL.End();
        GL.PopMatrix();
    }
    public static void Draw_RECT_WIRE_CENTER(float _x, float _y, float _w, float _h, Color _col)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        float _w2 = _w * 0.5f;
        float _h2 = _h * 0.5f;

        GL.Vertex3(_x - _w2, _y - _h2, Z);
        GL.Vertex3(_x + _w2, _y, Z);
        GL.Vertex3(_x + _w, _y + _h, Z);
        GL.Vertex3(_x, _y + _h, Z);
        GL.Vertex3(_x, _y, Z);

        GL.End();
        GL.PopMatrix();
    }
    #endregion
    #region ELLIPSE
    public static void Draw_ELLIPSE_WIRE(int _segments, float _x, float _y, float _w, float _h, Color[] _colour)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        float _DIV = (Mathf.PI * 2) / _segments;
        for (int i = 0; i <= _segments; i++)
        {
            float _X = _x + Mathf.Sin(i * _DIV) * _w;
            float _Y = _y + Mathf.Cos(i * _DIV) * _h;
            Add_VERT(_X, _Y, _colour[i % _colour.Length]);
        }
        GL.End();
        GL.PopMatrix();
    }
    public static void Draw_ELLIPSE_WIRE(int _segments, float _x, float _y, float _w, float _h, Color _colour)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINE_STRIP);

        float _DIV = (Mathf.PI * 2) / _segments;
        for (int i = 0; i <= _segments; i++)
        {
            float _X = _x + Mathf.Sin(i * _DIV) * _w;
            float _Y = _y + Mathf.Cos(i * _DIV) * _h;
            Add_VERT(_X, _Y, _colour);
        }
        GL.End();
        GL.PopMatrix();
    }

    public static void Draw_ELLIPSE_HATCHED(int _segments, float _x, float _y, float _w, float _h, float _hatchOffset, float _rot, float _offsetRadius, Color _colour)
    {
        float _DIV = (Mathf.PI * 2f) / _segments;

        for (int i = 0; i < _segments; i++)
        {
            Draw_LINE(
                _x + Mathf.Sin(_rot + _DIV * i) * _w,
                _y + Mathf.Cos(_rot + _DIV * i) * _h,
                _x + Mathf.Sin(_rot + _DIV * i + _hatchOffset) * _w * _offsetRadius,
                _y + Mathf.Cos(_rot + _DIV * i + _hatchOffset) * _h * _offsetRadius,
                _colour
            );
        }
    }
    #endregion
    public static void Draw_LINE(float _startX, float _startY, float _endX, float _endY, Color _colour)
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINES);

        Add_VERT(_startX, _startY, _colour);
        Add_VERT(_endX, _endY, _colour);

        GL.End();
        GL.PopMatrix();
    }
    public static void Add_VERT(float _x, float _y, Color _col)
    {
        GL.Color(_col);
        GL.Vertex(new Vector3(Mathf.Round(_x + Mathf.PerlinNoise(Time.realtimeSinceStartup * WOBBLE_RATE, _x) * WOBBLE), Mathf.Round(_y + Mathf.PerlinNoise(Time.realtimeSinceStartup * WOBBLE_RATE, _y) * WOBBLE), 0f));
        //GL.Vertex3(_x, _y, Z);
    }
    public static float PNOISE(float _seed1, float _seed2, float _min, float _max)
    {
        return _min + Mathf.PerlinNoise(_seed1, _seed2) * (_max - _min);
    }
    public static float PNOISE_TIME(float _rate, float _seed, float _min, float _max, float _timeOffset = 0f)
    {
        return _min + Mathf.PerlinNoise((Time.realtimeSinceStartup + _timeOffset) * _rate, _seed) * (_max - _min);
    }
}
