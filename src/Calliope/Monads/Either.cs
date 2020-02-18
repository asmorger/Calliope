using System;

namespace Calliope.Monads
{
    public class Either<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private readonly bool _isLeft;
        private readonly TLeft? _left;
        private readonly TRight? _right;

        public Either(TLeft left)
        {
            _left = left;
            _isLeft = true;
        }

        public Either(TRight right)
        {
            _right = right;
            _isLeft = false;
        }

        public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
        {
            if (leftFunc == null) throw new ArgumentNullException(nameof(leftFunc));
            if (rightFunc == null) throw new ArgumentNullException(nameof(rightFunc));

            return _isLeft ? leftFunc(_left!) : rightFunc(_right!);
        }

        public void DoRight(Action<TRight> rightAction)
        {
            if (rightAction == null) throw new ArgumentNullException(nameof(rightAction));

            if (!_isLeft) rightAction(_right!);
        }

        public TLeft LeftOrDefault() => Match(l => l, r => default);
        public TRight RightOrDefault() => Match(l => default, r => r);

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
    }
}