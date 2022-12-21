Shader "My Shaders/VertexColor/Diffuse[Color]" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
  BindChannels {
   Bind "color", Color
   Bind "normal", Normal
  }
  Lighting On
  Fog { Mode Off }
  ColorMaterial AmbientAndDiffuse
  SetTexture [_Texture] { ConstantColor [_Color] combine constant * primary double }
 }
}
}