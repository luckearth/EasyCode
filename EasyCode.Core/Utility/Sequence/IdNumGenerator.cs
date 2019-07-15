using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace EasyCode.Core.Utility.Sequence
{
    public interface ITimeSource
    {
        /// <summary>
        /// Returns the duration of a single tick.
        /// </summary>
        /// <remarks>
        /// It's up to the <see cref="ITimeSource"/> to define what a 'tick' is; it may be nanoseconds, milliseconds,
        /// seconds or even days or years.
        /// </remarks>
        TimeSpan TickDuration { get; }

        /// <summary>
        /// Returns the current number of ticks for the <see cref="ITimeSource"/>.
        /// </summary>
        /// <returns>The current number of ticks to be used by an <see cref="IdGenerator"/> when creating an Id.</returns>
        /// <remarks>
        /// It's up to the <see cref="ITimeSource"/> to define what a 'tick' is; it may be nanoseconds, milliseconds,
        /// seconds or even days or years.
        /// </remarks>
        long GetTicks();
    }
    /// <summary>
    /// Provides time data to an <see cref="IdGenerator"/>. This timesource uses a <see cref="Stopwatch"/> for timekeeping.
    /// </summary>
    public abstract class StopwatchTimeSource : ITimeSource
    {
        private static readonly Stopwatch _sw = Stopwatch.StartNew();

        /// <summary>
        /// Gets the elapsed time since this <see cref="ITimeSource"/> was initialized.
        /// </summary>
        protected TimeSpan Elapsed { get { return _sw.Elapsed; } }

        /// <summary>
        /// Gets the offset for this <see cref="ITimeSource"/> which is defined as the difference of it's creationdate
        /// and it's epoch which is specified in the object's constructor.
        /// </summary>
        protected TimeSpan Offset { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="StopwatchTimeSource"/> object.
        /// </summary>
        /// <param name="epoch">The epoch to use as an offset from now,</param>
        /// <param name="tickDuration">The duration of a single tick for this timesource.</param>
        public StopwatchTimeSource(DateTimeOffset epoch, TimeSpan tickDuration)
        {
            this.Offset = (DateTimeOffset.UtcNow - epoch);
            this.TickDuration = tickDuration;
        }

        /// <summary>
        /// Returns the duration of a single tick.
        /// </summary>
        public TimeSpan TickDuration { get; private set; }

        /// <summary>
        /// Returns the current number of ticks for the <see cref="DefaultTimeSource"/>.
        /// </summary>
        /// <returns>The current number of ticks to be used by an <see cref="IdGenerator"/> when creating an Id.</returns>
        public abstract long GetTicks();
    }
    /// <summary>
    /// Provides time data to an <see cref="IdGenerator"/>.
    /// </summary>
    /// <remarks>
    /// Unless specified the default duration of a tick for a <see cref="DefaultTimeSource"/> is 1 millisecond.
    /// </remarks>
    public class DefaultTimeSource : StopwatchTimeSource
    {
        /// <summary>
        /// Initializes a new <see cref="DefaultTimeSource"/> object.
        /// </summary>
        /// <param name="epoch">The epoch to use as an offset from now.</param>
        /// <remarks>The default tickduration is 1 millisecond.</remarks>
        public DefaultTimeSource(DateTimeOffset epoch)
            : this(epoch, TimeSpan.FromMilliseconds(1)) { }

        /// <summary>
        /// Initializes a new <see cref="DefaultTimeSource"/> object.
        /// </summary>
        /// <param name="epoch">The epoch to use as an offset from now,</param>
        /// <param name="tickDuration">The duration of a tick for this timesource.</param>
        public DefaultTimeSource(DateTimeOffset epoch, TimeSpan tickDuration)
            : base(epoch, tickDuration) { }

        /// <summary>
        /// Returns the current number of ticks for the <see cref="DefaultTimeSource"/>.
        /// </summary>
        /// <returns>The current number of ticks to be used by an <see cref="IdGenerator"/> when creating an Id.</returns>
        /// <remarks>
        /// Note that a 'tick' is a period defined by the timesource; this may be any valid <see cref="TimeSpan"/>; be
        /// it a millisecond, an hour, 2.5 seconds or any other value.
        /// </remarks>
        public override long GetTicks()
        {
            return (this.Offset.Ticks + this.Elapsed.Ticks) / this.TickDuration.Ticks;
        }
    }
    public interface IIdNumGenerator : IEnumerable<long>
    {
        /// <summary>
        /// Creates a new Id.
        /// </summary>
        /// <returns>Returns an Id.</returns>
        long CreateId();

        /// <summary>
        /// Gets the <see cref="ITimeSource"/> for the <see cref="IIdGenerator{T}"/>.
        /// </summary>
        ITimeSource TimeSource { get; }

        /// <summary>
        /// Gets the epoch for the <see cref="IIdGenerator{T}"/>.
        /// </summary>
        DateTimeOffset Epoch { get; }
    }
    public class IdNumGenerator : IIdNumGenerator
    {
        private static readonly DateTime defaultepoch = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private TimeSpan _ts = new TimeSpan(ticks: 100);

        private static readonly ITimeSource defaulttimesource = new DefaultTimeSource(defaultepoch);

        private int _sequence = 0;
        private long _lastgen = -1;

        private readonly ITimeSource _timesource;
        private readonly DateTimeOffset _epoch;
        private readonly long _generatorId;

        private readonly long MASK_SEQUENCE;
        private readonly long MASK_TIME;
        private readonly long MASK_GENERATOR;

        private readonly int SHIFT_TIME;
        private readonly int SHIFT_GENERATOR;


        // Object to lock() on while generating Id's
        private object genlock = new object();

        /// <summary>
        /// Gets the Id of the generator.
        /// </summary>
        public int Id { get { return (int)_generatorId; } }

        /// <summary>
        /// Gets the epoch for the <see cref="IdGenerator"/>.
        /// </summary>
        public DateTimeOffset Epoch { get { return _epoch; } }


        /// <summary>
        /// Gets the <see cref="ITimeSource"/> for the <see cref="IdGenerator"/>.
        /// </summary>
        public ITimeSource TimeSource { get { return _timesource; } }

        public IdNumGenerator(int generatorId)
        {
            MASK_TIME = 21;
            MASK_GENERATOR = 21;
            MASK_SEQUENCE = 21;

            if (generatorId < 0 || generatorId > MASK_GENERATOR)
                throw new ArgumentOutOfRangeException(string.Format("GeneratorId must be between 0 and {0} (inclusive).", MASK_GENERATOR));

            SHIFT_TIME = 21 + 21;
            SHIFT_GENERATOR = 21;


            _timesource = defaulttimesource;
            _epoch = defaultepoch;
            _generatorId = generatorId;
        }
        public long CreateId()
        {
            lock (genlock)
            {
                var ticks = this.GetTicks();
                var timestamp = ticks & MASK_TIME;
                if (timestamp < _lastgen || ticks < 0)
                    throw new Exception(string.Format("Clock moved backwards or wrapped around. Refusing to generate id for {0} ticks", _lastgen - timestamp));

                if (timestamp == _lastgen)
                {
                    if (_sequence < MASK_SEQUENCE)
                        _sequence++;
                    else
                        throw new Exception("Sequence overflow. Refusing to generate id for rest of tick");
                }
                else
                {
                    _sequence = 0;
                    _lastgen = timestamp;
                }

                unchecked
                {
                    long res = (timestamp << SHIFT_TIME)
                        + (_generatorId << SHIFT_GENERATOR)         // GeneratorId is already masked, we only need to shift
                        + _sequence;
                    return res;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetTicks()
        {
            return _timesource.GetTicks();
        }
        private IEnumerable<long> IdStream()
        {
            while (true)
                yield return this.CreateId();
        }

        public IEnumerator<long> GetEnumerator()
        {
            return this.IdStream().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
