# HanumanInstitute.Validators
Common validator functions and other extension methods that are used all the time.

**The main goal of this assembly is to add helper extension methods to validate arguments.**

Tired of repeating this in every method?

    if (value == null) { throw new NullReferenceException(nameof(value)); }
    _field = value;

Now you can instead write this!

    _field = value.CheckNotNull(nameof(value));

Other super useful functions

    value.CheckRange(nameof(value), min: 0);
    var msg = "Value {0} is invalid.".FormatInvariant(value);
    var result = await list.ForEachOrderedAsync(x => DoSomeWorkAsync(x));

## Setup

Add NuGet package [HanumanInstitute.Validators](https://www.nuget.org/packages/HanumanInstitute.Validators/)

Extension methods are available from namespace `System`.

## Validation Methods

#### Object.CheckNotNull(name)

Checks that Object is not not null. Throws an exception for parameter 'name' if it is null. Returns Object if it is valid.

#### String.CheckNotNullOrEmpty(name)

Checks that String is not null or empty.

#### IEnumerable&lt;T>.CheckNotNullOrEmpty(name)

Checks that IEnumerable is not null or empty.

#### Type.CheckAssignableFrom(baseType, name)

Checks that Type can be assigned from baseType (object of same type is valid).

#### Type.CheckDerivesFrom(baseType, name)

Checks that Type derives from baseType (object of same type is invalid).

#### Enum.CheckEnumValid&lt;T>(name)

Checks that an enumeration value is valid. Also works with Flags enumerations.

#### IComparable.CheckRange(name, min, minInclusive, max, maxInclusive)

Checks whether value is within valid range. It throws short and meaningful exceptions based on whether min and max were set.

    myInt.CheckRange(nameof(value), min: -10, max: 10) // myInt must be between -10 and 10.
    myFloat.CheckRange(nameof(value), min: 0, minInclusive: false) // myFloat must be greater than 0.

#### IComparable.IsInRange(min, minInclusive, max, maxInclusive)

Returns whether value is within valid range.

#### RangeClusiveAttribute(min, minInclusive, max, maxInclusive)

Validates that a property is within range, allowing to exclude the minimum or maximum value.

## List Extensions

#### ICollection/IList.AddRange()

[List.AddRange](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.addrange) is notably missing from standard IList interface!! Fixed.

#### IList.AsReadOnly()

[List.AsReadOnly](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.asreadonly) is also missing from standard IList interface.

#### IEnumerable.ForEachAsync(task, callback, maxParallel = 10)

Runs a task on each item of IEnumerable with up to specified parallel tasks. Callback will be invoked once each task is completed, in no particular order.

#### IEnumerable.ForEachOrderedAsync(task, maxParallel = 10)

Runs a task on each item of IEnumerable with up to specified parallel tasks. It will return an IList containing the result of each operation while preserving the order.

    var result = await list.ForEachOrderedAsync(x => DoSomeWorkAsync(x));

#### IList<T>.CastList<TTo, T>()

Creates a casted list that exposes a derived type while maintaining the same references.

## String Extensions

#### Object.ToStringInvariant()

Converts a value to string using InvariantCulture.

#### String.HasValue()

Returns whether the string contains a value. It is the equivalent of !string.IsNullOrEmpty(value).

#### String.Default(defaultValue)

Returns specified default value if the value is null or empty.

#### String.FormatInvariant(args)

Formats a string using invariant culture. This is a shortcut for string.format(CultureInfo.InvariantCulture, ...)

#### String.EqualsInvariant(value)

Returns whether the two string values are equal, using InvariantCultureIgnoreCase. Note that extension methods work on null values.

#### String.Parse&lt;T>()

Parses a string value into specified data type and returns null if conversion fails.

## Command Extensions

#### ICommand.Execute()

Executes a command. This overloads passes null parameter.

#### ICommand.CanExecute()

Returns whether the command can execute in its current state. This overloads passes null parameter.

#### ICommand.ExecuteIfCan()

Executes the command if CanExecute if true.

## Other Extensions

#### Enum.GetFlags()

Returns a list of all enumeration flags that are contained in an enumeration value.

#### ExtensionMethods.CopyAllFields(source, target)

Copies all fields from one instance of a class to another.

#### IComparable.Clamp(min, max)

Forces a value to be within specified range.

#### Type.IsAssignableFromGeneric(genericType)

Returns whether given type is assignable from specified generic base type.
