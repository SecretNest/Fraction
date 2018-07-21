# Fraction
Add fraction support.

# Known Issue
Using an instance built by default parameterless constructor will cause exception or miscalculation. To avoid this, always use parameter-based constructors. This will not be fixed due to consideration about running speed.

# Example
```c#
Fraction a = new Fraction(2, 3);   // 2/3
Fraction b = 100;  // 100
Fraction c = new Fraction("50 1/2"); //50 1/2
Fraction d = a + b;
```

# Nuget
https://www.nuget.org/packages/fraction/
