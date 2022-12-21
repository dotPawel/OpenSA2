Shader "My Shaders/World/Shadow" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Background+100" "IGNOREPROJECTOR"="True" "RenderType"="Background" }
 Pass {
  Tags { "QUEUE"="Background+100" "IGNOREPROJECTOR"="True" "RenderType"="Background" }
  BindChannels {
   Bind "color", Color
  }
  ZWrite Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_Texture] { ConstantColor [_Color] combine constant * primary }
 }
}
}