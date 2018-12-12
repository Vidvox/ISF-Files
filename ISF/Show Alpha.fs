/*
{
  "CATEGORIES" : [
    "Utility"
  ],
  "DESCRIPTION" : "Maps the alpha to grayscale",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/

void main()	{
	vec4		inputPixelColor = IMG_THIS_PIXEL(inputImage);
	inputPixelColor.rgb = vec3(inputPixelColor.a);
	inputPixelColor.a = 1.0;
	gl_FragColor = inputPixelColor;
}
