/*
{
  "CATEGORIES" : [
    "Color Adjustment"
  ],
  "DESCRIPTION" : "Performs a Lift Gamma Gain Saturation CDL",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "lift",
      "TYPE" : "color",
      "DEFAULT" : [
        0.5,
        0.5,
        0.5,
        0.5
      ]
    },
    {
      "NAME" : "gamma",
      "TYPE" : "color",
      "DEFAULT" : [
        0.25,
        0.25,
        0.25,
        0.25
      ]
    },
    {
      "NAME" : "gain",
      "TYPE" : "color",
      "DEFAULT" : [
        0.25,
        0.25,
        0.25,
        0.25
      ]
    },
    {
      "NAME" : "saturation",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 1,
      "MIN" : 0
    }
  ],
  "ISFVSN" : "2",
  "CREDIT" : "VIDVOX"
}
*/



void main()	{
	vec4		color = IMG_THIS_PIXEL(inputImage);
	vec4		scaledLift = (lift - vec4(0.5));
	vec4		scaledGain = gain * 4.0;
	vec4		scaledGamma = max(vec4(0.0001),gamma * 4.0);
	
	color = clamp(color + (scaledLift * (1.0 - color)), 0.0, 1.0);
	color = clamp(color * scaledGain, 0.0, 1.0);
	color = clamp(pow(color, scaledGamma), 0.0, 1.0);
	
	float		satLuma = (0.2126 * color.r) + (0.7152 * color.g) + (0.0722 * color.b);
	
	color.r = clamp(satLuma + (saturation*(color.r - satLuma)), 0.0, 1.0);
	color.g = clamp(satLuma + (saturation*(color.g - satLuma)), 0.0, 1.0);
	color.b = clamp(satLuma + (saturation*(color.b - satLuma)), 0.0, 1.0);

	gl_FragColor = color;
}
