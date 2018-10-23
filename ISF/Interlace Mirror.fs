/*{
	"DESCRIPTION": "",
	"CREDIT": "by Carter Rosenberg",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Distortion", "Glitch"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "horizontal",
			"LABEL": "Horizontal",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
		{
			"NAME": "vertical",
			"LABEL": "Vertical",
			"TYPE": "bool",
			"DEFAULT": 0.0
		}
	]
	
}*/

void main()
{
	vec2 pixelCoord = isf_FragNormCoord * RENDERSIZE;
	vec2 loc = pixelCoord;
	if (vertical)	{
		if (mod(pixelCoord.x,2.0)>1.0)	{
			loc.y = RENDERSIZE.y - pixelCoord.y;
		}
	}
	if (horizontal)	{
		if (mod(pixelCoord.y,2.0)>1.0)	{
			loc.x = RENDERSIZE.x - pixelCoord.x;
		}	
	}
	gl_FragColor = IMG_PIXEL(inputImage,loc);
}
