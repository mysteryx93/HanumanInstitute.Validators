# HanumanInstitute.Validators
Common validator functions and other extension methods that are used all the time.

The main goal of this assembly is to add helper extension methods to validate arguments.

Tired of repeating this in every method?

    if (value == null) { throw new NullReferenceException(nameof(value)); }
    _field = value;

Now you can instead write this!

    _field = value.CheckNotNull(nameof(value));

## Setup

Add NuGet package HanumanInstitute.Validators

(optional) Add .editorconfig file to every project. This will tell FxCop to stop null warnings after calling these methods.

    [*.cs]
    # EditorConfig is awesome: https://EditorConfig.org
    dotnet_code_quality.CA1062.null_check_validation_methods = CheckNotNull|CheckNotNullOrEmpty|CheckAssignableFrom|CheckDerivesFrom

To use in a file

    using HanumanInstitute.Validators;

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

