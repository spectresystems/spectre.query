using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Spectre.Query.Internal
{
    internal sealed class QueryTranslatorContext
    {
        private readonly ContextStack<ParameterExpression> _parameters;
        private readonly ContextStack<Type> _types;

        public Type Type => _types.Current;
        public ParameterExpression Parameter => _parameters.Current;

        private class ContextStack<T>
        {
            private readonly T _default;
            private readonly Stack<T> _stack;

            public T Current => _stack.Count > 0 ? _stack.Peek() : _default;

            public ContextStack(T @default)
            {
                _default = @default;
                _stack = new Stack<T>();
            }

            public void Push(T item)
            {
                _stack.Push(item);
            }

            public void Pop()
            {
                if (_stack.Count > 0)
                {
                    _stack.Pop();
                }
            }
        }

        public QueryTranslatorContext(Type rootType)
        {
            _types = new ContextStack<Type>(rootType);
            _parameters = new ContextStack<ParameterExpression>(Expression.Parameter(rootType));
        }

        public void PushParameter(Type type)
        {
            _types.Push(type);
            _parameters.Push(Expression.Parameter(type));
        }

        public void PopParameter()
        {
            _types.Pop();
            _parameters.Pop();
        }
    }
}
