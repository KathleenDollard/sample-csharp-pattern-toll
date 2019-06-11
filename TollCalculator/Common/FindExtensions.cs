using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Find
{
    public readonly struct FindResult<TLookFor, TFind>
    {
        public FindResult(TLookFor lookFor)
        {
            LookFor = lookFor;
            FoundValue = default;
            IsFound = false;
        }

        public FindResult(TLookFor lookFor, TFind foundValue)
        {
            LookFor = lookFor;
            FoundValue = foundValue;
            IsFound = true;
        }

        public TLookFor LookFor { get; }
        public TFind FoundValue { get; }
        public bool IsFound { get; }
    }

    public static class FindExtensions
    {
        public static FindResult<TLookFor, TFind> LookFor<TLookFor, TFind>(this TLookFor lookFor)
            => new FindResult<TLookFor, TFind>(lookFor);

        public static FindResult<TLookFor, TFind> IfNotFound<TLookFor, TFind>(
                this FindResult<TLookFor, TFind> previous,
                Func<TLookFor, IResult<TFind>> lookForFunc)
        {
            if (previous.IsFound)
            {
                return previous;
            }
            return lookForFunc(previous.LookFor) is SuccessResult<TFind> successResult
                ? new FindResult<TLookFor, TFind>(previous.LookFor, successResult.Value)
                : previous;
        }

        public static IResult<TFind> GetValueResult<TLookFor, TFind>(
                    this FindResult<TLookFor, TFind> previous)
            => previous.IsFound
                ? Result.Success<TFind>(previous.FoundValue)
                : (IResult<TFind>)Result.Fail<TFind>("Value not found");
    }
}
