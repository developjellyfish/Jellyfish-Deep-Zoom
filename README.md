## At First
This Project moved from CodePlex. 
and, I'm sorry about it is not maintaining.
http://jellyfishdz.codeplex.com/

## Summary
Jellyfish Deep Zoom is open source toolkit for dynamic deep zoom application using for silverlight development that oriented to most of system integrators and web agencies.

## How to use
For example, when we think about the project similar with Hard Rock Memorabilia, we want to sort, search and pinpoint appropriate image object in Deep Zoom canvas. If we wanted to make such a thing with Silverlight Deep Zoom feature, we have to develop with complicated relative axis calculation in Deep Zoom canvas. Even their needs are clear.

So, we want to develop common framework that is useful for most of web agencies to achieve more spreading situation of unique silverlight Deep Zoom application. 

Jellyfish Deep Zoom has two main parts. 1st one is client side libray(.dll) that can use for Deep Zoom development easily for client side, 2nd one is server side deep zoom slicing application that can use for dynamic generation of deep zoom images and collections.

Jellyfish Deep Zoom was developed by second factory co., ltd, JAPAN.
We hope that helps !


## ReleaseNote
update 2010/4/15 ------------------------------------
There was a bug in the Jellyfish server side DZC (DeepZoomCollection) generation code. When viewing a composition through a Silverlight 4 client, the image resolution would not update after zooming in. Prior versions of Silverlight were unaffected.

The code that creates the collection.xml file was generating incorrect values for the image width and height attributes for the original image data.

The fix just affects the source code only. Please download the updated source code at the link below.
http://jellyfishdz.codeplex.com/SourceControl/list/changesets

Special thanks to DanCory for pointing out the error in the source code and providing a patch. 

------------------------------------
As we promised at our session of MIX09, now we'd like to release 1.1 beta of Jellyfish Deep Zoom.

Please check our session recording and PowerPoint file about Jellyfish for your better understanding.
Deep Zoom++ : Build Dynamic Deep Zoom Applications with Open Source http://videos.visitmix.com/MIX09/C07F 

This 1.1 beta release has several addition features and fixes :
full screen
improvement of behavior about next / previous buttons
Re-organized UI
more reusable example of XAML synchronization and SubImage selection
Re-skinned web administrator (MIX09 style)
minor bug fixes ;)

With this release's solution, you can follow our step by switching XAML.

Please check "app.xaml.cs" and find "this.RootVisual".

// this.RootVisual = new Step09_ChangeSelectableSkin();
this.RootVisual = new Step10_Dynamic();

You can switch by removing commented out //.

--- install ----------------
1) Please download from "Downloads & Files" of "Jellyfish Deep Zoom 1.0 "
http://jellyfishdz.codeplex.com/Release/ProjectReleases.aspx?ReleaseId=18362

-- 1a) DataBase files
-- 1b) DeepZoom Materials in sl/out/source_files 
(or sourcefiles 1 (separated) and sourcefiles 2 (separated) )

1') Download from "Downloads & Files" of "Jellyfish Deep Zoom 1.1 Beta "
http://jellyfishdz.codeplex.com/Release/ProjectReleases.aspx?ReleaseId=25392

-- 1c) deep zoom tiles for semi-dynamic / dynamic mode 
-- 1d) DeepZoom Materials for StaticImages 

2) Download from "Source Code"

3) Please unzip archive to anywhere, and place all files under "jellyfish_development\jellyfishDZApp" to your destination. (Please do not place these files into path that includes spaces. for example, "visual studio 2008". We are fixing this problem, but not yet fixed.)

4) Please place unpacked image source files (1b) to "\jellyfishDZApp.Web\sl\out\source_files".

5) Please place and overwrite unpacked 5 folders and files under "other_files" folder (1c) to "\jellyfishDZApp.Web\sl\out".

6) Please place unpacked 4 DB files under "DBFiles" folder (1a) to "\jellyfishDZApp.Web\App_Data".

7) Please place unpacked GeneratedImages folder (1d) to jellyfishDZApp.Web\ClientBin.

8) Boot your Visual Studio 2008, configure start page for solution.
If you check from Step 1 to Step 4 by Vusual Studio, set "Start Page" to "jellyfishDZApp.Web\jellyfishDZAppTestPage.html".

If you check from Step 5 to Step 10 by Vusual Studio, set "Start Page" to "jellyfishDZApp.Web\sl\TestPage.html".

For web adminitrator, set "Start Page" to "jellyfishDZApp.Web\admin\Default.aspx".

9) And, please set your step by app.xaml.cs.

10) please right-clicks in "jellyfishDZApp.Web" project and select "set start-up project".

11) Hit F5 please !

--- about source code ----------------
DzcConverter
(Bin\DzcConv\DzcConverter.exe, DzcConverterImg2Sdi.dll, DzcConverterSdi2Coll.dll)
and PhotoZoomConverter
(Bin\DziConv\SDImageConverter.exe, Img2Sdi.dll, Sdi2Coll.dll)
are not change from ver.1.0.0
So, if you want to check these source code, please check below folder of "Source Code"
\jellyfish_release
jellyfish.dll is changed from ver.1.0.0.
There is no upward compatibility. 

if you want to check source code, please.
