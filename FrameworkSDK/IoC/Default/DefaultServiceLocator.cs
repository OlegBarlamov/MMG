﻿using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkSDK.Localization;
using JetBrains.Annotations;

namespace FrameworkSDK.IoC.Default
{
	internal class DefaultServiceLocator : IServiceLocator
	{
		private ConstructorFinder ConstructorFinder { get; }
		private DependencyResolver DependencyResolver { get; }

		private readonly Dictionary<int, List<RegistrationInfo>> _mapping = new Dictionary<int, List<RegistrationInfo>>();

		private bool _isDisposed;

		public DefaultServiceLocator([NotNull, ItemNotNull] IReadOnlyList<RegistrationInfo> registrations)
		{
			if (registrations == null) throw new ArgumentNullException(nameof(registrations));

			ConstructorFinder = new ConstructorFinder(this);
			DependencyResolver = new DependencyResolver(this);

			foreach (var registrationInfo in registrations)
			{
				var code = GetCode(registrationInfo);
				if (!_mapping.ContainsKey(code))
					_mapping.Add(code, new List<RegistrationInfo>());

				_mapping[code].Add(registrationInfo);
			}
		}

		public void Dispose()
		{
			_isDisposed = true;

			var allDisposable = FindAllDisposableSingletones();

			var exceptions = new List<Exception>();
			foreach (var disposable in allDisposable)
			{
				try
				{
					disposable.Dispose();
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			if (exceptions.Count < 1)
				throw new AggregateException(Strings.Exceptions.Ioc.DisposeServicesException, exceptions);
		}

		public T Resolve<T>()
		{
			CheckDisposed();

			var type = typeof(T);
			return (T) Resolve(type);
		}

		public object Resolve(Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			CheckDisposed();

			var regInfos = GetRegInfos(type);
			var lastReg = regInfos.Last();

			return ResolveRegInfo(lastReg);
		}

		public IReadOnlyList<T> ResolveMultiple<T>()
		{
			CheckDisposed();

			var type = typeof(T);
			return ResolveMultiple(type).Cast<T>().ToArray();
		}

		public IReadOnlyList<object> ResolveMultiple([NotNull] Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			CheckDisposed();

			var regInfos = GetRegInfos(type);
			var result = regInfos.Select(ResolveRegInfo);
			return result.ToArray();
		}

		public bool IsServiceRegistered<T>()
		{
			CheckDisposed();

			var type = typeof(T);
			return IsServiceRegistered(type);
		}

		public bool IsServiceRegistered(Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			CheckDisposed();

			var code = GetCode(type);
			return _mapping.ContainsKey(code);
		}

		private IEnumerable<IDisposable> FindAllDisposableSingletones()
		{
			var singletonesRegInfos = _mapping.Values.SelectMany(list => list)
				.Where(info => info.ResolveType == ResolveType.Singletone);

			var disposableCashedObjects = singletonesRegInfos
				.Where(info => info.CashedInstance is IDisposable)
				.Cast<IDisposable>();

			return disposableCashedObjects;
		}

		private object ResolveRegInfo(RegistrationInfo regIngo)
		{
			if (regIngo.CashedInstance != null && regIngo.ResolveType == ResolveType.Singletone)
				return regIngo.CashedInstance;

			var instance = CreateInstance(regIngo.ImplType);
			if (regIngo.ResolveType == ResolveType.Singletone)
				regIngo.CashedInstance = instance;

			return instance;
		}

		private IReadOnlyList<RegistrationInfo> GetRegInfos(Type type)
		{
			var code = GetCode(type);
			if (!_mapping.TryGetValue(code, out var regInfos) || regInfos.Count < 1)
				throw new FrameworkIocException(Strings.Exceptions.Ioc.TypeNotRegisteredException);

			return regInfos;
		}

		private object CreateInstance(Type type)
		{
			try
			{
				var constructor = ConstructorFinder.GetConstructor(type);
				var dependencies = DependencyResolver.ResolveDependencies(constructor);
				return constructor.Invoke(dependencies);
			}
			catch (Exception e)
			{
				throw new FrameworkIocException(Strings.Exceptions.Ioc.ResolvingTypeException, e);
			}
		}

		private static int GetCode([NotNull] Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			return type.GetHashCode();
		}

		private static int GetCode([NotNull] RegistrationInfo registrationInfo)
		{
			if (registrationInfo == null) throw new ArgumentNullException(nameof(registrationInfo));

			var type = registrationInfo.Type;
			return GetCode(type);
		}

		private void CheckDisposed()
		{
			if (_isDisposed)
				throw new ObjectDisposedException(nameof(DefaultServiceContainer));
		}
	}
}