CSharpUtil
===========================
这是一个C#开发的工具类，后续将不断集成常用的工具代码

## 当前包含工具类

1. 命令行解析工具类:[代码目录:/CommandLine/](/CommandLine/)

  此代码来源于:[katana项目](http://katanaproject.codeplex.com/),由于发现此代码很很好用，因此搜集起来

2. 常见的Lock和读写锁的封装:[代码目录:/SyncUtil/](/SyncUtil/)
  这个主要用于需要频繁使用锁的情况。代码主要是对锁进行集中管理，并采用using的方式简化代码编写，并减少错误率
  1. MonitorUtil使用方式：

	````
		MonitorUtil lockObj = new MonitorUtil();
		using(lockObj.GetLock("锁的名字"))
		{
			// 读写操作
		}
	````
  2. ReaderWriterLockUtil使用方式
  
	````
		ReaderWriterLockUtil lockObj = new ReaderWriterLockUtil();
		using(lockObj.GetLock("锁的名字",ReaderWriterLockUtil.LockTypeEnum.Reader))
		{
			// 读写操作
		}
	````

3. 邮件发送工具类:[代码目录:/MailUtil/](/MailUtil/)

  这个代码有一部分是参考自:[NugetGallery项目](https://github.com/NuGet/NuGetGallery)。采用url的形式，对用户配置的发件地址进行解析