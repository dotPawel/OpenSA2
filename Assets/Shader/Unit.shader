Shader "My Shaders/World/Unit" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
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
 }
}
Fallback "Diffuse"
}