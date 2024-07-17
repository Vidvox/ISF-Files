/*{
    "CATEGORIES": [
        "Utility"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Sets the alpha to a constant value",
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
        	"NAME": "alpha",
        	"TYPE": "float"
        }
    ],
    "ISFVSN": "2"
}
*/

void main()	{
	vec4		inputPixelColor = IMG_THIS_PIXEL(inputImage);
	inputPixelColor.a = alpha;
	gl_FragColor = inputPixelColor;
}
