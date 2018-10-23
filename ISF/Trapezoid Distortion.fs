/*{
	"DESCRIPTION": "Warps the video into a trapezoid shape",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Distortion Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"LABEL": "Top Width",
			"NAME": "topWidth",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"LABEL": "Bottom Width",
			"NAME": "bottomWidth",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"LABEL": "Height",
			"NAME": "heightScale",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
	
}*/

void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		loc = isf_FragNormCoord.xy;
	if (heightScale > 0.0)	{
		float	heightDivisor = 1.0 / heightScale;
		loc.y = loc.y * heightDivisor + (1.0 - heightDivisor) / 2.0;
		float		currentLineWidth = mix(bottomWidth,topWidth,loc.y);
		if (currentLineWidth > 0.0)	{
			float		lwDivisor = 1.0 / currentLineWidth;
			loc.x = loc.x * lwDivisor + (1.0 - lwDivisor) / 2.0;
	
			if ((loc.x >= 0.0)&&(loc.x <= 1.0)&&(loc.y >= 0.0)&&(loc.y <= 1.0))	{
				inputPixelColor = IMG_NORM_PIXEL(inputImage,loc);	
			}
		}
	}
	gl_FragColor = inputPixelColor;
}
