/*
{
  "CATEGORIES" : [
    "Masking"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "width",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.125,
      "LABEL" : "Size",
      "MIN" : 0
    },
    {
      "NAME" : "maskOffset",
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
      ],
      "LABEL" : "Mask Offset"
    },
    {
      "NAME" : "alpha1",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0,
      "LABEL" : "Alpha 1"
    },
    {
      "NAME" : "alpha2",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0,
      "LABEL" : "Alpha 2"
    },
    {
      "NAME" : "randomThreshold",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0,
      "LABEL" : "Threshold"
    },
    {
      "NAME" : "seed1",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.24391,
      "MIN" : 0,
      "LABEL" : "Random Seed"
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/


//	glsl doesn't include random functions
//	this is a pseudo-random function
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


void main() {
	
	vec4 out_color = IMG_THIS_NORM_PIXEL(inputImage);
	float alphaAdjust = alpha2;
	vec2 coord = isf_FragNormCoord * RENDERSIZE;
	vec2 shift = maskOffset * RENDERSIZE;
	float size = width * max(RENDERSIZE.x,RENDERSIZE.y);
	vec2 gridIndex = vec2(0.0);

	if (size == 0.0)	{
		size = 1.0 / max(RENDERSIZE.x,RENDERSIZE.y);
	}
	
	gridIndex = floor((shift + coord) / size);
	float value = rand(fract((0.1247+seed1)*gridIndex));
	if (value < randomThreshold)
		alphaAdjust = alpha1;
	
	out_color.a *= alphaAdjust;
	
	gl_FragColor = out_color;
}