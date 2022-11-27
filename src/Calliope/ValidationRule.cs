using System;

namespace Calliope;

public abstract record ValidationRule<T>(Func<T, bool> Check, Func<T, string> ErrorFactory);