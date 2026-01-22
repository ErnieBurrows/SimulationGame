Shader "Custom/AlwaysOnTopPreview"
{
    Properties
    {
        _Color ("Color", Color) = (0,1,0,0.35)
    }
    SubShader
    {
        Tags { "Queue"="Transparent+50" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always

        Pass
        {
            Color [_Color]
        }
    }
}
