namespace Polaris.Utility
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public class DownloadFileUtil
    {
        private int ByteSize = 1024;

        /// <summary>
        /// 下载中的后缀，下载完成去掉
        /// </summary>
        private const string Suffix = ".downloading";

        public event Action<int> ShowDownloadPercent;

        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="url">http地址</param>
        /// <param name="localfile">本地文件</param>
        /// <returns></returns>
        public void DownloadFile(string url, string localfile)
        {
            int ret = 0;
            string localfileReal = localfile;
            string localfileWithSuffix = localfileReal + Suffix;


            long startPosition = 0;
            FileStream writeStream = null;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(localfileReal))
            {
                throw new ArgumentNullException();
            }

            //取得远程文件长度
            long remoteFileLength = GetHttpLength(url);
            if (remoteFileLength == 0)
            {
                throw new Exception("Get Content-Length Fail");
            }

            if (File.Exists(localfileReal))
            {
                throw new IOException("file exist");
            }

            //判断要下载的文件是否存在
            if (File.Exists(localfileWithSuffix))
            {
                writeStream = File.OpenWrite(localfileWithSuffix);
                startPosition = writeStream.Length;
                if (startPosition > remoteFileLength)
                {
                    writeStream.Close();
                    File.Delete(localfileWithSuffix);
                    writeStream = new FileStream(localfileWithSuffix, FileMode.Create);
                }
                else if (startPosition == remoteFileLength)
                {
                    DownloadFileOk(localfileReal, localfileWithSuffix);
                    writeStream.Close();
                    return;
                }
                else
                    writeStream.Seek(startPosition, SeekOrigin.Begin);
            }
            else
                writeStream = new FileStream(localfileWithSuffix, FileMode.Create);

            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                if (startPosition > 0)
                    req.AddRange((int)startPosition);

                rsp = (HttpWebResponse)req.GetResponse();
                using (Stream readStream = rsp.GetResponseStream())
                {
                    byte[] btArray = new byte[ByteSize];
                    long currPostion = startPosition;
                    int contentSize = 0;
                    while ((contentSize = readStream.Read(btArray, 0, btArray.Length)) > 0)
                    {
                        writeStream.Write(btArray, 0, contentSize);
                        currPostion += contentSize;

                        if (ShowDownloadPercent != null)
                            ShowDownloadPercent((int)(currPostion * 100 / remoteFileLength));
                    }
                }
            }
            finally
            {
                if (writeStream != null)
                    writeStream.Close();
                if (rsp != null)
                    rsp.Close();
                if (req != null)
                    req.Abort();

                if (ret == 0)
                    DownloadFileOk(localfileReal, localfileWithSuffix);
            }

            return;
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        private void DownloadFileOk(string localfileReal, string localfileWithSuffix)
        {
            try
            {
                //去掉.downloading后缀
                FileInfo fi = new FileInfo(localfileWithSuffix);
                fi.MoveTo(localfileReal);
            }
            finally
            {
                //通知完成
                if (ShowDownloadPercent != null)
                    ShowDownloadPercent(100);
            }
        }

        // 从文件头得到远程文件的长度
        private long GetHttpLength(string url)
        {
            long length = 0;
            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                rsp = (HttpWebResponse)req.GetResponse();
                if (rsp.StatusCode == HttpStatusCode.OK)
                    length = rsp.ContentLength;
            }
            finally
            {
                if (rsp != null)
                    rsp.Close();
                if (req != null)
                    req.Abort();
            }

            return length;
        }

        public static Task DownloadFileAsync(String url, String localFile)
        {
            var task = new Task(() =>
            {
                var obj = new DownloadFileUtil();
                obj.DownloadFile(url, localFile);
            });
            task.Start();
            return task;
        }
    }
}
