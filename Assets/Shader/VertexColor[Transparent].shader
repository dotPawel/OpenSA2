Shader "My Shaders/VertexColor/Transparent" {
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  BindChannels {
   Bind "color", Color
  }
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
 }
}
}