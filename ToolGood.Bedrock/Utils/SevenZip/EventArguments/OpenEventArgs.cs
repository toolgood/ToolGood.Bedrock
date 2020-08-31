namespace SevenZip
{
    using System;

    /// <summary>
    /// EventArgs used to report the size of unpacked archive data
    /// </summary>
    public sealed class OpenEventArgs : EventArgs
    {
        private readonly ulong _totalSize;

        /// <summary>
        /// Initializes a new instance of the OpenEventArgs class
        /// </summary>
        /// <param name="totalSize">Size of unpacked archive data</param>
        public OpenEventArgs(ulong totalSize)
        {
            _totalSize = totalSize;
        }

        /// <summary>
        /// Gets the size of unpacked archive data
        /// </summary>
        public ulong TotalSize => _totalSize;
    }
}
