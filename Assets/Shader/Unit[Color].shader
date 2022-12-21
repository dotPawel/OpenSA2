Shader "My Shaders/World/Unit[Color]" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "normal", Normal
   Bind "texcoord", TexCoord
  }
  Lighting On
  Fog { Mode Off }
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary double }
  SetTexture [_MainTex] { ConstantColor [_Color] combine previous * constant }
 }
}
}