//*****************************************************************************
//
//  File:       StreamAdapter.cs
//
//  Contents:   Helper class that wraps a .NET stream class as a COM IStream
//
//*****************************************************************************

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Windows.Win32.System.Com
{
    /// <summary>
    /// Helper class that wraps a .NET stream class as a COM IStream
    /// </summary>
    internal class StreamAdapter : IStream
    {
        private Stream _stream;

        /// <summary>
        /// Initializes a new instance of the IStream StreamAdapter
        /// </summary>
        /// <param name="stream"></param>
        public StreamAdapter(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _stream = stream;
        }

        #region IStream Members

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="streamCopy"></param>
        /// <returns></returns>
        public HRESULT Clone(out IStream streamCopy)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public HRESULT Commit(STGC flags)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="targetStream"></param>
        /// <param name="bufferSize"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesWrittenPtr"></param>
        /// <returns></returns>
        public unsafe HRESULT CopyTo(IStream targetStream, ulong bufferSize, ulong* buffer, ulong* bytesWrittenPtr)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="byteCount"></param>
        /// <param name="lockType"></param>
        /// <returns></returns>
        public HRESULT LockRegion(ulong offset, ulong byteCount, LOCKTYPE lockType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write the UInt32 value of the total number of bytes read into the buffer to pcbRead
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="cb"></param>
        /// <param name="pcbRead"></param>
        /// <returns></returns>
        public unsafe HRESULT Read(void* pv, uint cb, uint* pcbRead)
        {
            Span<byte> buffer = new Span<byte>(pv, checked((int)cb));
#if NET48
            byte[] array = buffer.ToArray();
            int val = _stream.Read(array, 0, array.Length);
#else
            int val = _stream.Read(buffer);
#endif
            if (pcbRead is not null)
                *pcbRead = (uint)val;
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <returns></returns>
        public HRESULT Revert()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write the new Int64 position of the stream to newPositionPtr
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <param name="newPositionPtr"></param>
        /// <returns></returns>
        public unsafe HRESULT Seek(long offset, SeekOrigin origin, ulong* newPositionPtr)
        {
            //SeekOrigin begin = origin;
            //switch (origin)
            //{
            //    case 0:
            //        begin = SeekOrigin.Begin;
            //        break;

            //    case 1:
            //        begin = SeekOrigin.Current;
            //        break;

            //    case 2:
            //        begin = SeekOrigin.End;
            //        break;

            //    default:
            //        throw new ArgumentOutOfRangeException("origin");
            //}
            long val = _stream.Seek(offset, origin);
            if (newPositionPtr is not null)
                *newPositionPtr = (ulong)val;
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Set the length for the stream
        /// </summary>
        /// <param name="libNewSize"></param>
        /// <returns></returns>
        public HRESULT SetSize(ulong libNewSize)
        {
            _stream.SetLength((long)libNewSize);
            return HRESULT.S_OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pstatstg"></param>
        /// <param name="grfStatFlag"></param>
        /// <returns></returns>
        public unsafe HRESULT Stat(STATSTG* pstatstg, STATFLAG grfStatFlag)
        {
            STATSTG streamStats = *pstatstg;
            streamStats.type = (uint)STGTY.STGTY_STREAM; // 2;
            streamStats.cbSize = (ulong)_stream.Length;
            streamStats.grfMode = 0;
            if (_stream.CanRead && _stream.CanWrite)
            {
                streamStats.grfMode |= STGM.STGM_READWRITE;
            }
            else if (_stream.CanRead)
            {
                //streamStats.grfMode = streamStats.grfMode;
            }
            else
            {
                if (!_stream.CanWrite)
                {
                    throw new IOException("StreamObjectDisposed");
                }
                streamStats.grfMode |= STGM.STGM_WRITE;
            }
            *pstatstg = streamStats;
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="byteCount"></param>
        /// <param name="lockType"></param>
        /// <returns></returns>
        public HRESULT UnlockRegion(ulong offset, ulong byteCount, uint lockType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write UInt32 value cb to pcbWritten
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="cb"></param>
        /// <param name="pcbWritten"></param>
        /// <returns></returns>
        public unsafe HRESULT Write(void* pv, uint cb, uint* pcbWritten)
        {
            var buffer = new ReadOnlySpan<byte>(pv, checked((int)cb));
#if NET48
            byte[] array = buffer.ToArray();
            _stream.Write(array, 0, array.Length);
#else
            _stream.Write(buffer);
#endif
            if (pcbWritten is not null)
                *pcbWritten = cb;
            return HRESULT.S_OK;
        }

#endregion
    }
}
