/*{
  "ISFVSN": 2,
  "CATEGORIES": ["Geometry Adjustment"],
  "CREDIT": "ProjectileObjects modifed, based on VIDVOX crop",
  "DESCRIPTION": "ISF crop adjustment created to replace VIDVOX Quartz Composer FX, includes four sided feathering",
  "INPUTS": [
    {
      "NAME": "inputImage",
      "TYPE": "image",
      "DEFAULT": null
    },
    {
      "NAME": "Top",
      "TYPE": "float",
      "DEFAULT": 1.0
    },
    {
      "NAME": "Bottom",
      "TYPE": "float",
      "DEFAULT": 0.0
    },
    {
      "NAME": "Left",
      "TYPE": "float",
      "DEFAULT": 0.0
    },
    {
      "NAME": "Right",
      "TYPE": "float",
      "DEFAULT": 1.0
    },
    {
      "NAME": "TopFeather",
      "TYPE": "float",
      "DEFAULT": 0.0
    },
    {
      "NAME": "BottomFeather",
      "TYPE": "float",
      "DEFAULT": 0.0
    },
    {
      "NAME": "LeftFeather",
      "TYPE": "float",
      "DEFAULT": 0.0
    },
    {
      "NAME": "RightFeather",
      "TYPE": "float",
      "DEFAULT": 0.0
    }
  ],
  "PASSES": [
    {
      "NAME": "crop",
      "TARGET": "myOutputColor",
      "MAIN": "main"
    }
  ]
}
*/

void main() {
    // Get the texture coordinates
    vec2 uv = isf_FragNormCoord;

    // Define variables to control cropping and feathering for each side
    float topCrop = Bottom;
    float topFeather = Bottom * BottomFeather;
    float bottomCrop = Top;
    float bottomFeather = Top * TopFeather;
    float leftCrop = Left;
    float leftFeather = Left * LeftFeather;
    float rightCrop = Right;
    float rightFeather = Right * RightFeather;

    // Calculate the distance to the edge of the cropping area for each side
    float topDistance = topCrop - uv.y;
    float bottomDistance = uv.y - bottomCrop;
    float leftDistance = leftCrop - uv.x;
    float rightDistance = uv.x - rightCrop;

    // Calculate the feathering factor for each side
    float topFeatherFactor = smoothstep(0.0, topFeather, topDistance);
    float bottomFeatherFactor = smoothstep(0.0, bottomFeather, bottomDistance);
    float leftFeatherFactor = smoothstep(0.0, leftFeather, leftDistance);
    float rightFeatherFactor = smoothstep(0.0, rightFeather, rightDistance);

    // Apply the feathering factors to the color and alpha
    vec4 color = IMG_NORM_PIXEL(inputImage, uv);
    color.a = mix(color.a, 0.0, topFeatherFactor);
    color.a = mix(color.a, 0.0, bottomFeatherFactor);
    color.a = mix(color.a, 0.0, leftFeatherFactor);
    color.a = mix(color.a, 0.0, rightFeatherFactor);

    // Set the output color
    gl_FragColor = color;
}
