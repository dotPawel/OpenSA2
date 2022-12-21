Shader "My Shaders/World/AiplaneShadow" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="True" "RenderType"="Overlay" }
 Pass {
  Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="True" "RenderType"="Overlay" }
  BindChannels {
   Bind "color", Color
  }
  ZTest Always
  ZWrite Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_Texture] { ConstantColor [_Color] combine constant * primary }
 }
}
}