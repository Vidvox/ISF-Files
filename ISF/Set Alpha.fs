/*{
	"DESCRIPTION": "Sets the alpha channel of the image",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Color Adjustment"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "newAlpha",
			"LABEL": "New Alpha",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/

void main()	{
	vec4		inputPixelColor = IMG_THIS_NORM_PIXEL(inputImage);
	
	inputPixelColor.a = newAlpha;
	
	gl_FragColor = inputPixelColor;
}
