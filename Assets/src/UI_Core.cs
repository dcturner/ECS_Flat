using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Core : MonoBehaviour {

    static Camera CAM;
    static Material lineMaterial;
    static float Z = 0f;

    //private static Texture2D _staticRectTexture;
    //private static GUIStyle _staticRectStyle;

    //// Note that this function is only meant to be called from OnGUI() functions.
    //public static void GUIDrawRect(Rect position, Color color)
    //{
    //    if (_staticRectTexture == null)
    //    {
    //        _staticRectTexture = new Texture2D(1, 1);
    //    }

    //    if (_staticRectStyle == null)
    //    {
    //        _staticRectStyle = new GUIStyle();
    //    }

    //    _staticRectTexture.SetPixel(0, 0, color);
    //    _staticRectTexture.Apply();

    //    _staticRectStyle.normal.background = _staticRectTexture;

    //    GUI.Box(position, GUIContent.none, _staticRectStyle);
    //}

    //private void OnGUI()
    //{
    //    GUIDrawRect(new Rect(new Vector2(10f, 10f), new Vector2(1f, 400f)), Color.red);
    //}

    private void Awake()
    {
        CAM = GetComponent<Camera>();
    }
    private void OnPostRender()
    {
        Vector3[] verts = new Vector3[10];
        Color[] colours = new Color[10];

        for (int i = 0; i < 10; i++)
        {
            float X = Screen.width * 0.5f + (Mathf.Sin(Time.realtimeSinceStartup + i*0.2f) * 200f);
            float Y = Screen.height * 0.5f + (Mathf.Cos(Time.realtimeSinceStartup + i*0.2f) * 300f);
            verts[i] = new Vector3(X, Y, Z);
            colours[i] = new Color((i / 10f), 1f, 1f, (i/10f));
        }

        GL.LoadPixelMatrix();
        Draw_ELLIPSE(12,Screen.width*0.5f, Screen.height*0.5f, 200,200, )
    }

    public static void Draw_POLY(Vector3[] _verts, Color[] _colours){
        GL.PushMatrix();
        //GL.LoadPixelMatrix();
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
    public static void Draw_POLY(Vector3[] _verts, Color _colour)
    {
        GL.PushMatrix();
        //GL.LoadPixelMatrix();
        GL.Begin(GL.LINE_STRIP);

        for (int vertIndex = 0; vertIndex < _verts.Length; vertIndex++)
        {
            Vector3 _tempPos = _verts[vertIndex];
            Add_VERT(_tempPos.x, _tempPos.y, _colour);
        }

        GL.End();
        GL.PopMatrix();
    }

    public static void Add_VERT(float _x, float _y, Color _col){
        GL.Color(_col);
        GL.Vertex(new Vector3(_x, _y, 0f));
        GL.Vertex3(_x,_y,Z);
    }
    public static void Draw_RECT(float _x, float _y, float _w, float _h, Color _col){
        GL.PushMatrix();
        //GL.LoadPixelMatrix();

        _x = Screen.width * _x;
        _y = Screen.height * _y;
        _w = Screen.width * _w;
        _h = Screen.height * _h;

        GL.Begin(GL.LINE_STRIP);

        GL.Vertex3(_x, _y, Z);
        GL.Vertex3(_x + _w, _y, Z);
        GL.Vertex3(_x + _w, _y + _h, Z);
        GL.Vertex3(_x, _y + _h, Z);
        GL.Vertex3(_x, _y, Z);

        GL.End();
        GL.PopMatrix();
    }

    public static void Draw_ELLIPSE(int _segments, float _x, float _y, float _w, float _h, Color[] _colour){
        GL.PushMatrix();

        float _DIV = (Mathf.PI * 2) / _segments;
        for (int i = 0; i < _segments; i++)
        {
            float _X = _x + Mathf.Sin(i * _DIV) * _w;
            float _Y = _y + Mathf.Cos(i * _DIV) * _h;
            Add_VERT(_X, _Y, _colour[i % _colour.Length]);
        }

        GL.PopMatrix();
    }
}
