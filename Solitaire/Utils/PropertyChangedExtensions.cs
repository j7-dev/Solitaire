using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Solitaire.Utils;

/// <summary>
/// 屬性變更擴充方法。提供響應式程式設計風格的屬性變更通知。
/// Property changed extensions. Provides reactive programming style property change notifications.
/// </summary>
public static class PropertyChangedExtensions
{
    /// <summary>
    /// 屬性可觀察包裝器。將 INotifyPropertyChanged 包裝成 IObservable。
    /// Property observable wrapper. Wraps INotifyPropertyChanged as IObservable.
    /// </summary>
    class PropertyObservable<T> : IObservable<T>
    {
        private readonly INotifyPropertyChanged _target;
        private readonly PropertyInfo _info;

        public PropertyObservable(INotifyPropertyChanged target, PropertyInfo info)
        {
            _target = target;
            _info = info;
        }

        /// <summary>
        /// 訂閱包裝器。管理屬性變更事件的訂閱。
        /// Subscription wrapper. Manages property change event subscription.
        /// </summary>
        class Subscription : IDisposable
        {
            private readonly INotifyPropertyChanged _target;
            private readonly PropertyInfo _info;
            private readonly IObserver<T> _observer;

            public Subscription(INotifyPropertyChanged target, PropertyInfo info, IObserver<T> observer)
            {
                _target = target;
                _info = info;
                _observer = observer;
                _target.PropertyChanged += OnPropertyChanged!;
                // 立即發送當前值
                // Immediately send current value
                _observer.OnNext(((T)_info.GetValue(_target)!));
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                // 當目標屬性變更時，通知觀察者
                // When target property changes, notify observer
                if (e.PropertyName == _info.Name)
                    _observer.OnNext(((T)_info.GetValue(_target)!));
            }

            public void Dispose()
            {
                _target.PropertyChanged -= OnPropertyChanged!;
                _observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return new Subscription(_target, _info, observer);
        }
    }

    /// <summary>
    /// 監聽屬性值的變更。類似於 ReactiveUI 的 WhenAnyValue。
    /// Watches for property value changes. Similar to ReactiveUI's WhenAnyValue.
    /// </summary>
    /// <typeparam name="TModel">模型類型 (Model type)</typeparam>
    /// <typeparam name="TRes">屬性值類型 (Property value type)</typeparam>
    /// <param name="model">要監聽的模型 (Model to watch)</param>
    /// <param name="expr">屬性表達式 (Property expression)</param>
    /// <returns>可觀察的屬性值序列 (Observable sequence of property values)</returns>
    public static IObservable<TRes> WhenAnyValue<TModel, TRes>(this TModel model,
        Expression<Func<TModel, TRes>> expr) where TModel : INotifyPropertyChanged
    {
        var l = (LambdaExpression)expr;
        var ma = (MemberExpression)l.Body;
        var prop = (PropertyInfo)ma.Member;
        return new PropertyObservable<TRes>(model, prop);
    }
}