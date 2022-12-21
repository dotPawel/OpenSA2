Shader "My Shaders/VertexColor/Diffuse[WOL]" {
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
  }
  Fog { Mode Off }
 }
}
}