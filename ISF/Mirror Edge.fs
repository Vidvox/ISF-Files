/*{
    "CATEGORIES": [
        "Tile Effect"
    ],
    "CREDIT": "by VIDVOX",
    "DESCRIPTION": null,
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Angle",
            "MAX": 1,
            "MIN": 0,
            "NAME": "angle",
            "TYPE": "float"
        },
        {
            "DEFAULT": [
                0,
                0.5
            ],
            "LABEL": "Shift",
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "shift",
            "TYPE": "point2D"
        }
    ],
    "ISFVSN": "2",
    "VSN": null
}
*/


varying vec2 translated_coord;


void main() {
	vec2 loc = translated_coord;
	vec2 modifiedCenter = shift;
	
	loc = mod(loc + modifiedCenter, 1.0);
	
	//	mirror the image so it's repeated 4 times at different reflections
	loc = 2.0 * abs(loc - 0.5);
	
	gl_FragColor = IMG_NORM_PIXEL(inputImage, loc);
}