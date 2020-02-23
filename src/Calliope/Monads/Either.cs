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

        public Option<TLeft> MatchLeft(Func<TLeft, TLeft>? leftFunc = null)
        {
            if (_left is Some<TLeft> left)
            {
                if (leftFunc != null)
                {
                    return new Some<TLeft>(leftFunc(left));
                }

                return new Some<TLeft>(left.Value);
            }
            
            return new None<TLeft>();
        }
        
        public Option<T> MatchLeft<T>(Func<TLeft, T> leftFunc)
        {
            if (_left is Some<TLeft> left) return new Some<T>(leftFunc(left));
            return new None<T>();
        }

        public Option<TRight> MatchRight(Func<TRight, TRight>? rightFunc = null)
        {
            if (_right is Some<TRight> right)
            {
                if (rightFunc != null)
                {
                    return new Some<TRight>(rightFunc(right));
                }
                
                return new Some<TRight>(right.Value);
            }
            
            return new None<TRight>();
        }
        
        public Option<T> MatchRight<T>(Func<TRight, T> rightFunc)
        {
            if (_right is Some<TRight> right) return new Some<T>(rightFunc(right));
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