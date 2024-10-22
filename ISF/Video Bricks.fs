/*
{
  "CATEGORIES" : [
    "Tile Effect"
  ],
  "DESCRIPTION" : "Tiles the incoming image using a basic brick pattern",
  "ISFVSN" : "2",
  "INPUTS" : [
	{
		"NAME": "inputImage",
		"TYPE": "image"
	},
    {
      "NAME" : "brickSize",
      "LABEL" : "Brick Size",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "MIN" : 0
    },
    {
      "NAME" : "borderSize",
      "LABEL" : "Border Size",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.05,
      "MIN" : 0
    },
    {
      "NAME" : "borderColor",
      "LABEL" : "Border Color",
      "TYPE" : "color",
      "DEFAULT" : [
        0,
        0,
        0,
        1
      ]
    },
	{
		"NAME": "crop",
		"LABEL" : "Crop Image",
		"TYPE": "bool",
		"DEFAULT": 0.0
	},
    {
      "NAME" : "brickOffset",
      "LABEL" : "Brick Offset",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "DEFAULT" : [
        0,
        0
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "CREDIT" : "adapted from patriciogv bricks.fs by vidvox"
}
*/

// Based on Brick example from https://thebookofshaders.com/09/
// Author @patriciogv ( patriciogonzalezvivo.com ) - 2015



vec2 brickTile(vec2 _st, float _zoom){
    _st *= _zoom;

    // Here is where the offset is happening
    _st.x += step(1., mod(_st.y,2.0)) * 0.5;

    return fract(_st);
}

float box(vec2 _st, vec2 _size){
    _size = vec2(0.5)-_size*0.5;
    vec2 uv = smoothstep(_size,_size+vec2(1e-4),_st);
    uv *= smoothstep(_size,_size+vec2(1e-4),vec2(1.0)-_st);
    return uv.x*uv.y;
}

void main(void){
    vec2 st = isf_FragNormCoord;
    vec4 color = borderColor;
    float brickCount = (brickSize == 0.0) ? max(RENDERSIZE.x,RENDERSIZE.y) : 1.0 / brickSize;
    //	Apply the offset
    st += brickOffset;
    // Apply the brick tiling
    st = brickTile(st,brickCount);

	float val = box(st,vec2(1.0 - borderSize));
	
	if (val > 0.0)	{
		//	if we aren't cropping, zoom in st a bit
		if (crop == false && borderSize < 1.0)	{
			st = (st - 0.5) / (1.0 - borderSize) + 0.5;
		}
	    color = IMG_NORM_PIXEL(inputImage, st);
    }
    else	{
    	color = borderColor;
    }

    // Uncomment to see the space coordinates
    //color.rgb = vec3(st,0.0);

    gl_FragColor = color;
}

