// Simplified SDF shader:
// - No Shading Option (bevel / bump / env map)
// - No Glow Option
// - Softness is applied on both side of the outline

Shader "TextMeshPro/Mobile/Distance Field SSD"
{

    Properties
    {
        [HDR]_Face 	("Face Color", Color) = (1,1,1,1)        	_FaceDilat 	("Face Dilate", Range(-1,1)) = 0
        	[HDR]_Outlin r	("Outline Color", Color) = (0,0,0,1        
	_OutlineWid 		("Outline Thickness", Range(0,1)) =         
	_OutlineSoftne s	("Outline Softness", Range(0,1)) = 0        
	[HDR]_Underl Color		("Border Color", Color) = (0,0        0,.5)
	_Underlay fsetX 	("Border OffsetX", Range(-1,1        ) = 0
	_Underlay fsetY 	("Border OffsetY", Range(-1,1        ) = 0
	_Underla ilate		("Border Dilate", Range(-1,1        ) = 0
	_UnderlayS tness 	("Border Softness", Range(0,1)         = 0

	_Weigh ormal		("Weight Normal", floa        ) = 0
	_Wei Bold			("Weight Bold", float)        = .5

	_Shad Flags		("Flags", floa        ) = 0
	_Scal atioA		("Scale RatioA", floa        ) = 1
	_Scal atioB		("Scale RatioB", floa        ) = 1
	_Scal atioC		("Scale RatioC", float         = 1

	_ nTex			("Font Atlas", 2D) = "whi        e" {}
	_Textu Width		("Texture Width", float)        = 512
	_Textur eight		("Texture Height", float)        = 512
	_Gradie Scale		("Gradient Scale", floa        ) = 5
	 leX				("Scale X", floa        ) = 1
	 leY				("Scale Y", floa        ) = 1
	_Perspectiv Filter	("Perspective Correction", Range(0, 1)) =        0.875
	_Sh ness			("Sharpness", Range(-1,1)         = 0

	_Vertex fsetX		("Vertex OffsetX", floa        ) = 0
	_Vertex fsetY		("Vertex OffsetY", float         = 0

	_C Rect			("Clip Rect", vector) = (-32767, -32767, 32767,         2767)
	_MaskSo nessX		("Mask SoftnessX", floa        ) = 0
	_MaskSo nessY		("Mask SoftnessY", floa        ) = 0
	_ kTex			("Mask Texture", 2D) = "whi        e" {}
	_Mask verse		("Inverse", floa        ) = 0
	_MaskEd Color		("Edge Color", Color) = (1,        ,1,1)
	_MaskEdgeS ftness	("Edge Softness", Range(0, 1))          0.01
	_MaskWipe ontrol	("Wipe Position", Range(0, 1))          0.5

	_Sten lComp		("Stencil Comparison", Floa        ) = 8
	_ ncil			("Stencil ID", Floa        ) = 0
	_St ilOp			("Stencil Operation", Floa        ) = 0
	_StencilWr teMask	("Stencil Write Mask", Float)        = 255
	_StencilR adMask	("Stencil Read Mask", Float)         5

    _C        ("Cull Mode", Floa        ) = 0
	_Co Mask			("Color Mask", Floa
    )

     15
}

Su
    Sh        der 
        
	            gs {
		"Queue"="Transp            ent"
		"IgnoreProjector"=            rue"
		"RenderType"="Transp        ren        "
	}

	S        en            l
	{
		Ref [_St            cil]
		Comp [_Stenci            omp]
		Pass [_Sten            lOp]
		ReadMask [_StencilRea            ask]
		WriteMask [_StencilWrit        Mas        ]
	}

	Cull [_Cul        Mode]
	ZWri        e Off
	Lighti        g O
        f
            	Fog { M
        de        Off }
	ZTest [unity_GUIZTes        Mode]
	Blend One OneMinusSr        Alpha
	ColorMask [_Color        ask]
        
	            ss {
		CG
            #pragma vertex VertShader
            #pragma fragment PixShader
            #pragma shader_feature __ OUTLINE_ON
            #pragma shader_feature __ UNDERLAY_ON UNDERLAY_INNER

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "TMPro_Properties.cginc"

            #include "TMPro_Mobile.cginc"
            nc"

	        E
    D

    
	}
}

CustomEditor "TMPro.EditorUtilities.TMP_SDFSha
eGUI"
}
