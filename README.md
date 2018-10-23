<p align="center">
<img width="229.5" height="165" src="ISF_light.png">
</p>

# ISF-Files

ISF (Interactive Shader Format) is a file format that describes a GLSL fragment shader, as well as how to execute and interact with it.  More information about the ISF specification- including a test app that can use these files- [can be found here](https://www.github.com/mrRay/ISF_Spec).

This repository is home to our collection of ISF generators and filters.  There are just over two hundred generators and filters in this collection, and everything conforms to ISF 2.0.

Installation Instructions:
* **MacOS**- ISF files should be installed in `/Library/Graphics/ISF` (if you want them to be available for all users on your system) or `~/Library/Graphics/ISF` (if you only want them to be available to your user account).  ISF files installed in either of these locations will be available to all applications: application-specific ISFs should be installed in `/Library/Application Support/<app name>` or `~/Library/Application Support/<app name>`.
* **Windows**- Please consult the instructions for your software package.
* **.nix**- Haven't targeted this platform yet, interested parties please get in touch!
