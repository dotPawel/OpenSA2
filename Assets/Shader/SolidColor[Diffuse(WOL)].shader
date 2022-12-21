Shader "My Shaders/Solid Color/Diffuse[WOL]" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="True" "RenderType"="Geometry" }
  Color [_Color]
  Fog { Mode Off }
 }
}
}