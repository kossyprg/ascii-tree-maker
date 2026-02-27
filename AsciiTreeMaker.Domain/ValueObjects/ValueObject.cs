namespace AsciiTreeMaker.Domain.ValueObjects
{
    /// <summary>
    /// ValueObject の基底クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object? obj)
        {
            if (obj is not T vo)
            {
                return false;
            }

            return EqualsCore(vo);
        }

        public static bool operator ==(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return vo1.Equals(vo2);
        }

        public static bool operator !=(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return !vo1.Equals(vo2);
        }

        protected abstract bool EqualsCore(T other);
        protected abstract int GetHashCodeCore();

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }
    }
}
