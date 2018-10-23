/*{
	"DESCRIPTION": "Posterizes an image",
	"CREDIT": "VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Stylize", "Retro"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "gamma",
			"LABEL": "Gamma",
			"TYPE": "float",
			"DEFAULT": 1.25,
			"MIN": 0.5,
			"MAX": 2.0
		},
		{
			"NAME": "numColors",
			"LABEL": "Quality",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 3.0,
			"MAX": 32.0
		}
	]
	
}*/

void main()	{
	
    vec4	c = IMG_THIS_PIXEL(inputImage);

 	c.rgb = pow(c.rgb, vec3(gamma, gamma, gamma));
 	c.rgb = c.rgb * numColors;
 	c.rgb = floor(c.rgb);
 	c.rgb = c.rgb / numColors;
 	c.rgb = pow(c.rgb, vec3(1.0/gamma));

	gl_FragColor = c;
}
