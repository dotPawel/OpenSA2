Shader "My Shaders/World/Surface" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="True" "RenderType"="Background" }
 Pass {
  Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="True" "RenderType"="Background" }
  Lighting On
  ZWrite Off
  Fog { Mode Off }
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary double }
 }
}
Fallback "Diffuse"
}