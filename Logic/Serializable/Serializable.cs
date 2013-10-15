using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Regulus.Project.Crystal
{
    namespace Game
    {
        [Serializable]
        public class Value<T> where T : IComparable 
        {
            T _Min;
            T _Max;
            public Value(T min,T max)
            {
                
                _Set(min, max);
            }

            private void _Set(T min, T max)
            {
                _Min = min;
                _Max = max;

                if (_Min.CompareTo(_Max) > 0)
                {
                    _Min = _Max;
                }
            }

            static T Add<T>(T a, T b)
            {
                //TODO: re-use delegate!
                // declare the parameters
                var paramA = System.Linq.Expressions.Expression.Parameter(typeof(T), "a");
                var paramB = System.Linq.Expressions.Expression.Parameter(typeof(T), "b");
                // add the parameters together
                System.Linq.Expressions.BinaryExpression body = System.Linq.Expressions.Expression.Add(paramA, paramB);                
                // compile it
                var add = System.Linq.Expressions.Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
                // call it
                return add(a, b);
            }

            static T Sub<T>(T a, T b)
            {
                //TODO: re-use delegate!
                // declare the parameters
                var paramA = System.Linq.Expressions.Expression.Parameter(typeof(T), "a");
                var paramB = System.Linq.Expressions.Expression.Parameter(typeof(T), "b");
                // add the parameters together
                System.Linq.Expressions.BinaryExpression body = System.Linq.Expressions.Expression.Subtract(paramA, paramB);                
                    // compile it
                Func<T, T, T> add = System.Linq.Expressions.Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
                // call it
                return add(a, b);
            }

            public static Value<T> operator +(Value<T> value1, Value<T> value2)
            {
                T value = Add(value1._Min, value2._Min);
                return new Value<T>(value, value1._Max);
            }

            public static Value<T> operator -(Value<T> value1, Value<T> value2)
            {
                T value = Sub(value1._Min, value2._Min);
                return new Value<T>(value, value1._Max);
            }
            public static implicit operator T(Value<T> value)
            {
                return value._Min;
            }
        }
    };

    [Serializable]
    public class EnergyGroup
    {
        public Energy Energy;
        public int Round;
        public Guid Owner;
    }

    [Serializable]
    public class Energy
    {        
        public int Red { get; private set; }
        public int Yellow { get; private set; }
        public int Green { get; private set; }
        public int Power { get; private set; }
        int _TotalMax;

        public Energy(int total_max)
        {                
            _TotalMax = total_max;
        }
        public bool _CheckTotal()
        {
            return Red + Yellow + Green <= _TotalMax;
        }

        public void IncRed()
        {
            if (_CheckTotal())
                Red++;
        }

        public void IncYellow()
        {
            if (_CheckTotal())
                Yellow++;
        }
        public void IncGreen()
        {
            if (_CheckTotal())
                Green++;
        }

        public void IncPower()
        {
            if (Power == 0)
                Power++;
        }

        public void Consume(int r, int y, int g, int p)
        {
            if (Check(r, y, g, p))
            {
                Red -= r;
                Yellow -= y;
                Green -= g;
                Power -= p;
            }
        }
        public bool Check(int r, int y, int g, int p)
        {
            return Red >= r && Yellow >= y && Green >= g && Power >= p;
        }
    };
    [Serializable]
    public class Pet
    {                
        public Guid Id { get; set; }
        public Guid Owner { get; set; }
        public Energy Energy { get; set; }
    }
	[Serializable]
	public class AccountInfomation
	{
		public string Name { get; set; }
		public string Password { get; set; }
		public Guid Id { get; set; }
	}

    [Serializable]
    public class ActorInfomation
    {
        public Guid Id { get; set; }
    }
    [Serializable]
    public enum UserStatus
    {
        None,
        Verify,
        Parking,
        Adventure,
        Battle,
    }
    

	[Serializable]
	public enum LoginResult
	{
        Success,
        Fail,
        Repeat
	}

    [Serializable]
    public enum BattlerSide
    { 
        Blue,Red
    }

    
    [Serializable]
    public class BattlerInfomation
    {
        public Guid Id;
        public BattlerSide Side;
    }

    [Serializable]
    public class BattleRequester
    {
        
        public BattleRequester()
        {
            Battlers = new List<BattlerInfomation>();
        }
        public List<BattlerInfomation> Battlers { get; private set; }
    }
}