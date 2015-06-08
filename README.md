# Basic game geometry lib for MonoGame

It is based on part of [Physics2D.Net](http://code.google.com/p/physics2d/) library, which deals with 2D geometry for games like rectangle intersections and such. 

Lots of changes were made in process, but most of the time things should behave the same. There are few exceptions, like 
`BoundingRectangle.FromIntersection` method — for disjointed rectangles it returns `BoundingRectangle.Empty` now as opposed to negative sized rectangle. As for API itself, it is pretty self-explaining: methods do what you expect them to do based on their name (I hope).

All in all, if you want to use it, you better adapt source code for your needs.


### Unit tests NuGet references

You may notice that NuGet packages are not in the repository, so do not forget to set up package restoration in Visual Studio:

Tools menu → Options → Package Manager → General → "Allow NuGet to download missing packages during build" should be selected. 

If you have a build server then it needs to be setup with an environment variable 'EnableNuGetPackageRestore' set to true.

If you do not use Visual Studio, then I guess that you already know how to restore packages from console.