Shader "Custom/CubeEdgeHighlight"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _EdgeThickness ("Edge Thickness", Range(0.001, 0.1)) = 0.02
        _SideColor ("Side Color", Color) = (1,1,1,0.2)
        _SideOpacity ("Side Opacity", Range(0, 1)) = 0.2
    }
    
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        LOD 100
        
        // Ensure the backfaces are rendered
        Cull Off
        // For proper transparency
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 localPos : TEXCOORD0;
            };

            float4 _EdgeColor;
            float _EdgeThickness;
            float4 _SideColor;
            float _SideOpacity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.localPos = v.vertex.xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // For a cube, local positions range from -0.5 to 0.5 in each axis
                float3 absPos = abs(i.localPos);
                
                // Check if we're close to any edge
                // For a cube, an edge occurs when two coordinates are at their maximum (0.5)
                float maxDist = max(max(absPos.x, absPos.y), absPos.z);
                
                // How close we are to being on an edge - a value close to 1 when two or more coordinates are near 0.5
                float edgeFactor = 0;
                if (maxDist > 0.5 - _EdgeThickness) {
                    // Count how many dimensions are close to the edge
                    int nearEdgeCount = 0;
                    if (absPos.x > 0.5 - _EdgeThickness) nearEdgeCount++;
                    if (absPos.y > 0.5 - _EdgeThickness) nearEdgeCount++;
                    if (absPos.z > 0.5 - _EdgeThickness) nearEdgeCount++;
                    
                    // We want to highlight when at least two dimensions are near edge
                    if (nearEdgeCount >= 2) {
                        // Calculate how much into the edge region we are
                        float edgeDistance = min(
                            min(0.5 - absPos.x, 0.5 - absPos.y),
                            0.5 - absPos.z
                        );
                        edgeFactor = 1.0 - saturate(edgeDistance / _EdgeThickness);
                    }
                }
                
                // Blend between side color and edge color based on edge factor
                float4 finalColor = lerp(_SideColor, _EdgeColor, edgeFactor);
                // Set the alpha based on whether we're on an edge or a side
                finalColor.a = lerp(_SideOpacity, _EdgeColor.a, edgeFactor);
                
                return finalColor;
            }
            ENDCG
        }
    }
}

