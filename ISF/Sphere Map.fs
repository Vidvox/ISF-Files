/*
{
  "CATEGORIES" : [
    "Geometry Adjustment"
  ],
  "DESCRIPTION" : "Maps video onto a sphere",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "imageScale",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "LABEL" : "Image Scale",
      "MIN" : 0.125
    },
    {
      "NAME" : "radiusScale",
      "TYPE" : "float",
      "MAX" : 1.9990000000000001,
      "DEFAULT" : 1,
      "LABEL" : "Radius Scale",
      "MIN" : 0
    },
    {
      "NAME" : "pointInput",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "DEFAULT" : [
        0,
        0
      ],
      "LABEL" : "Rotate",
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/



const float pi = 3.14159265359;


void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		rotate = pointInput;
 	vec2 		p = 2.0 * isf_FragNormCoord.xy - 1.0;
 	float		aspect = RENDERSIZE.x / RENDERSIZE.y;
 	p.x = p.x * aspect;
 	
 	float		r = sqrt(dot(p,p)) * (2.0-radiusScale);
 	if (r < 1.0)	{
		vec2 uv;
    	float f = imageScale * (1.0-sqrt(1.0-r))/(r);
    	uv.x = mod(p.x*f + rotate.x,1.0);
    	uv.y = mod(p.y*f + rotate.y,1.0);
    	inputPixelColor = IMG_NORM_PIXEL(inputImage, uv);
	}


	//	both of these are also the same
	//inputPixelColor = IMG_NORM_PIXEL(inputImage, loc);
	
	gl_FragColor = inputPixelColor;
}
