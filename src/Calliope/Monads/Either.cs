using System;

namespace Calliope.Monads
{
    public class Either<TLeft, TRight>
    {
        private readonly Option<TLeft> _left;
        private readonly Option<TRight> _right;

        public Either(TLeft left)
        {
            _left = left;
            _right = None.Value;
        }

        public Either(TRight right)
        {
            _left = None.Value;
            _right = right;
        }

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
        {
            if (_left is Some<TLeft> left) return leftFunc(left);
            if (_right is Some<TRight> right) return rightFunc(right);
            
            // logically this shouldn't ever be hit, but you never know.
            throw new ArgumentException("Neither left nor right is set.");
        }

        public void DoRight(Action<TRight> rightAction)
        {
            if (_right is Some<TRight> right)
            {
                rightAction(right.Value);
            }
        }
        
        /*
         I don't know if I actually need these or not.
        public TLeft LeftOrDefault() => Match(l => l, r => default);
        public TRight RightOrDefault() => Match(l => default, r => r);
        */

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }
}