/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Color Adjustment"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		}
	]
}*/

void main() {
	vec4		srcPixel = IMG_THIS_PIXEL(inputImage);
	srcPixel.rgb = srcPixel.rgb * srcPixel.a;
	srcPixel.a = 1.0;
	gl_FragColor = srcPixel;
}