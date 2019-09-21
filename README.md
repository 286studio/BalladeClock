# 286studio_clock
叙事曲2风格的iOS闹钟App

Unity版本2019.2.0f

打开项目后，先打开Scenes/APP。

然后File->Build Settings:

确认platform是否为iOS。如果不是，点击iOS，在点switch platform。

确认Scenes in Build中是否有Scenes/APP在最顶端，如果没有就点击Add Open Scenes，并移到最顶端。

Compress method要选成LZ4HC，不然包体会很大。

点击Build，稍等几分钟，XCode Project就会生成。

先不要打开XCode Project。先去下载XCode修改文件，下载地址：https://github.com/wwu39/286studio_clock_xcode

复制com.unity.mobile.notifications

打开Xcode Project所在的文件夹，打开Libraries，粘贴，覆盖。

复制UnityAppController.mm覆盖Classes中的UnityAppController.mm

现在用XCode打开XCode Project。把Sounds文件夹拖到左边栏Libraries文件夹下，选上copy items if needed。

准备完成。

如果需要实机测试，把左上角的Generic iOS Device改成自己的手机，点击运行即可。

如果需要iOS模拟器测试，Google Unity iOS simulator。
