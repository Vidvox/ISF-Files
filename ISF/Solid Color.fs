/*{
	"DESCRIPTION": "demonstrates the use of color-type image inputs",
	"CREDIT": "by Carter Rosenberg",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "Color",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.0,
				0.0,
				1.0
			]
		}
	]
}*/

void main()
{
	gl_FragColor = Color;
}
