﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using GalaSoft.MvvmLight;

namespace Sylapse.MainThreadDispatch
{
    public static class DispatchExtensions
    {
        public static void Dispatch(this ObservableObject observableObject, Action action)
        {
#if PORTABLE
            throw NotSupported();
#else
            MainThreadDispatcher.Instance.Execute(action);
#endif
        }

        public static bool DispatchSet<T>(this ObservableObject observableObject, Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
#if PORTABLE
            throw NotSupported();
#else
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;

            MainThreadDispatcher.Instance.Execute(() => observableObject.RaisePropertyChanged(propertyExpression));
            return true;
#endif
        }

        public static bool DispatchSet<T>(this ObservableObject observableObject, string propertyName, ref T field, T newValue)
        {
#if PORTABLE
            throw NotSupported();
#else
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;

            MainThreadDispatcher.Instance.Execute(() => observableObject.RaisePropertyChanged(propertyName));
            return true;
#endif
        }
        public static bool DispatchSet<T>(this ObservableObject observableObject, ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
#if PORTABLE
            throw NotSupported();
#else
            return observableObject.DispatchSet(propertyName, ref field, newValue);
#endif
        }

        static Exception NotSupported()
        {
            return new PlatformNotSupportedException("Make sure Sylapse.MainThreadDispatcher is added to your platform project. If it is then you are probably using an unsupported platform, please visit https://github.com/Sylapse/MainThreadDispatch to find out about supporting other platforms");
        }
    }
}
