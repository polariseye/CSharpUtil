CSharpUtil
===========================
����һ��C#�����Ĺ����࣬���������ϼ��ɳ��õĹ��ߴ���

## ��ǰ����������

1. �����н���������:[����Ŀ¼:/CommandLine/](/CommandLine/)

  �˴�����Դ��:[katana��Ŀ](http://katanaproject.codeplex.com/),���ڷ��ִ˴���ܺܺ��ã�����Ѽ�����

2. ������Lock�Ͷ�д���ķ�װ:[����Ŀ¼:/SyncUtil/](/SyncUtil/)
  �����Ҫ������ҪƵ��ʹ�����������������Ҫ�Ƕ������м��й���������using�ķ�ʽ�򻯴����д�������ٴ�����
  1. MonitorUtilʹ�÷�ʽ��

	````
		MonitorUtil lockObj = new MonitorUtil();
		using(lockObj.GetLock("��������"))
		{
			// ��д����
		}
	````
  2. ReaderWriterLockUtilʹ�÷�ʽ
  
	````
		ReaderWriterLockUtil lockObj = new ReaderWriterLockUtil();
		using(lockObj.GetLock("��������",ReaderWriterLockUtil.LockTypeEnum.Reader))
		{
			// ��д����
		}
	````

3. �ʼ����͹�����:[����Ŀ¼:/MailUtil/](/MailUtil/)

  ���������һ�����ǲο���:[NugetGallery��Ŀ](https://github.com/NuGet/NuGetGallery)������url����ʽ�����û����õķ�����ַ���н���