using System;
using System.IO;

namespace RonsHouse.SeoNet
{
    internal abstract class BaseFilter : Stream
    {
        private Stream _sink;
        private long _position;

        public Stream Sink
        {
            get { return _sink; }
            set { _sink = value; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            //_sink.Flush();
            this.Sink.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            //return _sink.Read(buffer, offset, count);
            return this.Sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            //return _sink.Seek(offset, origin);
            return this.Sink.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            //_sink.SetLength(value);
            this.Sink.SetLength(value);
        }

        public override void Close()
        {
            //_sink.Close();
            this.Sink.Close();
        }

        public BaseFilter()
        {
            this.Sink = System.Web.HttpContext.Current.Response.Filter;
        }

        public BaseFilter(Stream sink)
        {
            //_sink = sink;
            this.Sink = sink;
        }

        public abstract override void Write(byte[] buffer, int offset, int count);
    }
}