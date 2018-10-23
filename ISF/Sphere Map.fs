/*{
	"DESCRIPTION": "Maps video onto a sphere",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Geometry Adjustment"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"LABEL": "Image Scale",
			"NAME": "imageScale",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.125,
			"MAX": 1.0
		},
		{
			"LABEL": "Radius Scale",
			"NAME": "radiusScale",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.999
		},
		{
			"LABEL": "Rotate",
			"NAME": "pointInput",
			"TYPE": "point2D",
			"DEFAULT": [
				0,
				0
			]
		}
	]
	
}*/



const float pi = 3.14159265359;


void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		rotate = pointInput / RENDERSIZE;
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
