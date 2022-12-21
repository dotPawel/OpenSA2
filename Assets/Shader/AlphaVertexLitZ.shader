Shader "My Shaders/Transparent/VertexLit with Z" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  ColorMask 0
 }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Lighting On
  Material {
   Ambient [_Color]
   Diffuse [_Color]
  }
  ZTest Always
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  SetTexture [_Texture] { ConstantColor [_Color] combine constant * primary double }
 }
}
}