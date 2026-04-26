# Decode.Extensions

General purpose extension methods for **.NET basic types**, designed to simplify common operations like type conversion, enum parsing, and validation.

## 📦 Installation

```bash
dotnet add package Decode.Extensions
```

## 🛠️ Usage

### 1. Object Extensions (Type Conversion)

The library provides a suite of methods to handle type conversion safely and concisely.

#### Basic Conversion (`To<T>`)
Converts an object to a target type.
```csharp
string input = "100";
int value = input.To<int>();
```

#### Safe Conversion with Default (`ToOrDefault<T>`)
Returns the default value of `T` if the input is null or conversion fails.
```csharp
string? invalidInput = "abc";
int value = invalidInput.ToOrDefault<int>(); // returns 0
```

#### Safe Conversion to Nullable (`ToOrNull<T>`)
Returns `null` if the input is null or conversion fails (for value types).
```csharp
string? input = "not a number";
int? value = input.ToOrNull<int>(); // returns null
```

### 2. Enum Extensions

#### Parse to Enum (`ToEnum<T>`)
```csharp
string statusStr = "Active";
var status = statusStr.ToEnum<UserStatus>();
```

#### Safe Parse to Enum (`ToEnumOrNull<T>`)
```csharp
string? invalidStatus = "UnknownValue";
var status = invalidStatus.ToEnumOrNull<UserStatus>(); // returns null
```

### 3. Validation

#### Check if Parseable (`IsParseableTo<T>`)
Returns `true` if the conversion is possible without throwing an exception.
```csharp
object rawValue = "123";
if (rawValue.IsParseableTo<int>()) 
{
    // safe to convert
}
```

## 📄 License
MIT License.
