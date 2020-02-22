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
        
        public Option<T> MatchLeftOptional<T>(Func<TLeft, T> leftFunc)
        {
            if (_left is Some<TLeft> left) return new Some<T>(leftFunc(left));
           return new None<T>();
        }
        
        public Option<T> MatchRightOptional<T>(Func<TRight, T>? rightFunc)
        {
            if (_right is Some<TRight> right) return new Some<T>(rightFunc!(right));
            return new None<T>();
        }
        
        public void DoRight(Action<TRight> rightAction)
        {
            if (_right is Some<TRight> right)
            {
                rightAction(right.Value);
            }
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }
}