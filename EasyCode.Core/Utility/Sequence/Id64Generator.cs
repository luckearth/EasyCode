using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EasyCode.Core.Utility.Sequence
{
    /// <summary>
    /// Generated id is composed of
    /// <list type="bullet">
    /// <item><description>time - 41 bits (millisecond precision w/ a custom epoch gives us 69 years)</description></item>
    /// <item><description>configured machine id - 10 bits (5 bit worker id, 5 bits datacenter id) - gives us up to 1024 machines</description></item>
    /// <item><description>sequence number - 12 bits - rolls over every 4096 per machine (with protection to avoid rollover in the same ms)</description></item>
    /// </list>
    /// </summary>
    public class Id64Generator : IIdGenerator<long>
    {
        #region Private Constant

        /// <summary>
        /// 1 January 1970. Used to calculate timestamp (in milliseconds)
        /// </summary>
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        // 基准时间
        private const long Epoch = 1351606710465L;

        /// <summary>
        /// Number of bits allocated for a worker id in the generated identifier. 5 bits indicates values from 0 to 31
        /// </summary>
        private const int WorkerIdBits = 5;

        /// <summary>
        /// Datacenter identifier this worker belongs to. 5 bits indicates values from 0 to 31
        /// </summary>
        private const int DatacenterIdBits = 5;

        /// <summary>
        /// Generator identifier. 10 bits indicates values from 0 to 1023
        /// </summary>
        private const int GeneratorIdBits = 10;

        /// <summary>
        /// Maximum generator identifier
        /// </summary>
        private const long MaxGeneratorId = -1 ^ (-1L << GeneratorIdBits);

        /// <summary>
        /// Maximum worker identifier
        /// </summary>
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);

        /// <summary>
        /// Maximum datacenter identifier
        /// </summary>
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        /// <summary>
        /// Number of bits allocated for sequence in the generated identifier
        /// </summary>
        private const int SequenceBits = 12;

        private const int WorkerIdShift = SequenceBits;

        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;

        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        #endregion Private Constant

        #region Private Fields

        /// <summary>
        /// Object used as a monitor for threads synchronization.
        /// </summary>
        private readonly object monitor = new object();

        /// <summary>
        /// The timestamp used to generate last id by the worker
        /// </summary>
        private long lastTimestamp = -1L;

        private long sequence = 0;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Indicates how many times the given generator had to wait 
        /// for next millisecond <see cref="TillNextMillis"/> since startup.
        /// </summary>
        public int NextMillisecondWait { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public Id64Generator(int generatorId = 0, int sequence = 0)
            : this(
                (int)(generatorId & MaxWorkerId),
                (int)(generatorId >> WorkerIdBits & MaxDatacenterId),
                sequence)
        {
            // sanity check for generatorId
            if (generatorId > MaxGeneratorId || generatorId < 0)
            {
                throw new InvalidOperationException(
                    string.Format("Generator Id can't be greater than {0} or less than 0", MaxGeneratorId));
            }
        }

        public Id64Generator(int workerId, int datacenterId, int sequence = 0)
        {
            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new InvalidOperationException(
                    string.Format("Worker Id can't be greater than {0} or less than 0", MaxWorkerId));
            }

            // sanity check for datacenterId
            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new InvalidOperationException(
                    string.Format("Datacenter Id can't be greater than {0} or less than 0", MaxDatacenterId));
            }

            WorkerId = workerId;
            DatacenterId = datacenterId;
            this.sequence = sequence;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The identifier of the worker
        /// </summary>
        public long WorkerId { get; private set; }

        /// <summary>
        /// Identifier of datacenter the worker belongs to
        /// </summary>
        public long DatacenterId { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public long GenerateId()
        {
            lock (monitor)
            {
                return NextId();
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            while (true)
            {
                yield return GenerateId();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #endregion Public Methods

        #region Private Properties

        private static long CurrentTime
        {
            get { return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; }
        }

        #endregion Private Properties

        #region Public Methods
        #endregion Public Methods

        #region Private Static Methods
        #endregion Private Static Methods

        #region Private Methods

        private long TillNextMillis(long lastTimestamp)
        {
            NextMillisecondWait++;

            var timestamp = CurrentTime;

            SpinWait.SpinUntil(() => (timestamp = CurrentTime) > lastTimestamp);

            return timestamp;
        }

        private long NextId()
        {
            var timestamp = CurrentTime;

            if (timestamp < lastTimestamp)
            {
                throw new InvalidOperationException(string.Format("Clock moved backwards. Refusing to generate id for {0} milliseconds", (lastTimestamp - timestamp)));
            }

            if (lastTimestamp == timestamp)
            {
                sequence = (sequence + 1) & SequenceMask;
                if (sequence == 0)
                {
                    timestamp = TillNextMillis(lastTimestamp);
                }
            }
            else
            {
                sequence = 0;
            }

            lastTimestamp = timestamp;
            return ((timestamp - Epoch) << TimestampLeftShift) |
                (DatacenterId << DatacenterIdShift) |
                (WorkerId << WorkerIdShift) |
                sequence;
        }

        #endregion Private Methods
    }
}
