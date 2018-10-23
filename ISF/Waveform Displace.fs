/*
{
  "CATEGORIES" : [
    "Distortion Effect", "Audio Reactive"
  ],
  "DESCRIPTION" : "Displaces image with audio waveform",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "audio",
      "TYPE" : "image",
      "LABEL" : "Audio Waveform"
    },
    {
      "NAME" : "displaceX",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "MIN" : 0,
      "LABEL" : "Displace X"
    },
    {
      "NAME" : "displaceY",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "MIN" : 0,
      "LABEL" : "Displace Y"
    },
    {
      "NAME" : "detailX",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0,
      "LABEL" : "Detail X"
    },
    {
      "NAME" : "detailY",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0,
      "LABEL" : "Detail Y"
    }
  ],
  "PASSES" : [
    {
      "DESCRIPTION" : "Renderpass 0"
    }
  ],
  "CREDIT" : "icalvin102 (calvin@icalvin.de)"
}
*/

void main()	{

	vec4 inputPixelColor;
	vec2 uv = isf_FragNormCoord.xy;
	vec2 waveLoc = fract((uv)*vec2(detailX, detailY));

	vec2 wave = vec2(IMG_NORM_PIXEL(audio, vec2(waveLoc.x, 0.0)).r, IMG_NORM_PIXEL(audio, vec2(waveLoc.y, 1.0)).r)-.5;
	wave *= vec2(displaceY, displaceX);

	inputPixelColor = IMG_NORM_PIXEL(inputImage, uv + wave.yx);

	gl_FragColor = inputPixelColor;
}
