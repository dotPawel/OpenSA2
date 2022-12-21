Shader "My Shaders/Solid Color/Transparent" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  Color [_Color]
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
 }
}
}