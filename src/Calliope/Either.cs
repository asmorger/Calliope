using System;

namespace Calliope
{
    public class Either<TLeft, TRight>
    {
        private readonly Optional<TLeft> _left;
        private readonly Optional<TRight> _right;

        public Either(TLeft left)
        {
            _left = left;
            _right = Optional<TRight>.None;
        }

        public Either(TRight right)
        {
            _left = Optional<TLeft>.None;
            _right = right;
        }

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
        {
            if (_left.IsSome(out var left)) return leftFunc(left);
            if (_right.IsSome(out var right)) return rightFunc(right);
            
            // logically this shouldn't ever be hit, but you never know.
            throw new ArgumentException("Neither left nor right is set.");
        }

        public Optional<TLeft> MatchLeft(Func<TLeft, TLeft> leftFunc = null)
        {
            if (_left.IsSome(out var left))
            {
                if (leftFunc != null)
                {
                    return leftFunc(left);
                }

                return left;
            }
            
            return Optional<TLeft>.None;
        }
        
        public Optional<T> MatchLeft<T>(Func<TLeft, T> leftFunc)
        {
            if (_left.IsSome(out var left)) return leftFunc(left);
            return Optional<T>.None;
        }

        public Optional<TRight> MatchRight(Func<TRight, TRight> rightFunc = null)
        {
            if (_right.IsSome(out var right))
            {
                if (rightFunc != null)
                {
                    return rightFunc(right);
                }
                
                return right;
            }
            
            return Optional<TRight>.None;
        }
        
        public Optional<T> MatchRight<T>(Func<TRight, T> rightFunc)
        {
            if (_right.IsSome(out var right)) return rightFunc(right);
            return Optional<T>.None;
        }
        
        public void DoRight(Action<TRight> rightAction)
        {
            if (_right.IsSome(out var right))
            {
                rightAction(right);
            }
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }
}