# Fixed64

`Fixed64` 是一个 64 位定点数结构体，使用 128 位 `Int128` 存储，其中高 64 位为整数部分，低 64 位为小数部分（Q64.64 格式）。该结构体提供极高精度的小数计算，避免浮点数的精度误差，适用于游戏逻辑、物理模拟等需要确定性计算的场景。

- **命名空间**：`SimplexLab.Fixed`
- **存储格式**：Q64.64（1 位符号 + 63 位整数 + 64 位小数）
- **精度**：约 5.42 × 10⁻²⁰（即 1/2⁶⁴）
- **表示范围**：约 -9,223,372,036,854,775,808 ~ +9,223,372,036,854,775,807.999999999999999999

> [!WARNING]
> Fixed64 使用 `Int128` / `UInt128` 类型，需要在 .NET 7 及更高版本才被支持。

## 结构定义

```csharp
public partial struct Fixed64 : IFixed<Fixed64>
```

`Fixed64` 实现了 `IFixed<Fixed64>` 接口，该接口继承自：
- `IMinMaxValue<Fixed64>` — 提供最大/最小值
- `IComparable` / `IComparable<Fixed64>` — 提供比较功能
- `IEquatable<Fixed64>` — 提供相等性判断

## 构造函数

### Fixed64()

```csharp
public Fixed64()
```

**功能**：创建值为 0 的定点数。

**示例**：

```csharp
var f = new Fixed64();
// f == Fixed64.Zero
```

---

### `Fixed64(int value)`

```csharp
public Fixed64(int value)
```

**功能**：从整数创建定点数。

**参数**：
- `value`：整数值

**示例**：

```csharp
var f = new Fixed64(42);
// f 表示定点数 42
```

---

### `Fixed64(float value)`

```csharp
public Fixed64(float value)
```

**功能**：从浮点数创建定点数。内部转换为 `double` 后处理。

**参数**：
- `value`：浮点数值

**示例**：

```csharp
var f = new Fixed64(3.14f);
```

---

### `Fixed64(double value)`

```csharp
public Fixed64(double value)
```

**功能**：从双精度浮点数创建定点数。将浮点数乘以小数部分乘数（2⁶⁴）并四舍五入，限制在有效范围内。

**参数**：
- `value`：双精度浮点数值

**示例**：

```csharp
var f = new Fixed64(3.14159);
```

---

### `Fixed64(Fixed64 other)`

```csharp
public Fixed64(Fixed64 other)
```

**功能**：从另一个定点数创建定点数（拷贝构造）。

**参数**：
- `other`：另一个定点数

**示例**：

```csharp
var a = new Fixed64(10);
var b = new Fixed64(a);
// b == a
```

## 常量属性

| 属性 | 类型 | 近似值 | 说明 |
|------|------|--------|------|
| `MaxValue` | `Fixed64` | ~9.22 × 10¹⁸ | 定点数能表示的最大值 |
| `MinValue` | `Fixed64` | ~-9.22 × 10¹⁸ | 定点数能表示的最小值 |
| `AdditiveIdentity` | `Fixed64` | 0 | 加法单位元，x + 0 = x |
| `MultiplicativeIdentity` | `Fixed64` | 1 | 乘法单位元，x × 1 = x |
| `Zero` | `Fixed64` | 0 | 零值 |
| `Half` | `Fixed64` | 0.5 | 二分之一 |
| `One` | `Fixed64` | 1 | 一 |
| `NegativeOne` | `Fixed64` | -1 | 负一 |
| `Two` | `Fixed64` | 2 | 二 |
| `Ln2` | `Fixed64` | ~0.6931 | 自然对数 ln(2) |
| `Ln10` | `Fixed64` | ~2.3026 | 自然对数 ln(10) |
| `NaN` | `Fixed64` | — | 非数字（Not a Number） |
| `Epsilon` | `Fixed64` | ~1.86 × 10⁻⁹ | 最小精度 |
| `E` | `Fixed64` | ~2.71828 | 自然常数 e |
| `PI` | `Fixed64` | ~3.14159 | 圆周率 π |
| `Half_PI` | `Fixed64` | ~1.5708 | π/2 |
| `Quarter_PI` | `Fixed64` | ~0.7854 | π/4 |
| `Two_PI` | `Fixed64` | ~6.28319 | 2π |
| `TPN1` | `Fixed64` | 0.1 | 10⁻¹ |
| `TPN2` | `Fixed64` | 0.01 | 10⁻² |
| `TPN3` | `Fixed64` | 0.001 | 10⁻³ |
| `TPN4` | `Fixed64` | 0.0001 | 10⁻⁴ |
| `N180` | `Fixed64` | 180 | 180 度，用于角度转换 |
| `N360` | `Fixed64` | 360 | 360 度，用于角度转换 |
| `PositiveInfinity` | `Fixed64` | +∞ | 正无穷大 |
| `NegativeInfinity` | `Fixed64` | -∞ | 负无穷大 |
| `DegToRad` | `Fixed64` | ~0.01745 | 角度转弧度系数（π/180） |
| `RadToDeg` | `Fixed64` | ~57.29578 | 弧度转角度系数（180/π） |

**示例**：

```csharp
var pi = Fixed64.PI;
var halfPi = Fixed64.Half_PI;
var degToRad = Fixed64.DegToRad;

// NaN 检测
var nan = Fixed64.NaN;
Console.WriteLine(nan.IsNaN()); // True
```

**注意事项**：
- `NaN` 的内部原始值为 `Int128.MinValue`，任何与 `NaN` 的运算结果均为 `NaN`。
- `PositiveInfinity` 的内部原始值为 `Int128.MaxValue`。
- `NegativeInfinity` 的内部原始值为 `Int128.MinValue + 1`。
- `Epsilon` 的内部原始值为 `(Int128)8 << 32`（即 34359738368），表示定点数比较时的精度阈值。
- `S_MAX_RAW_VALUE` 和 `S_MIN_RAW_VALUE` 使用属性（而非 `const`），以避免跨 partial 文件静态初始化顺序不确定的问题。

## 类型转换

### 隐式转换（从其他类型到 Fixed64）

#### `implicit operator Fixed64(byte n)`

```csharp
public static implicit operator Fixed64(byte n)
```

**功能**：将 `byte` 隐式转换为 `Fixed64`。

**参数**：
- `n`：byte 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed64 f = (byte)42; // 隐式转换
```

---

#### `implicit operator Fixed64(short n)`

```csharp
public static implicit operator Fixed64(short n)
```

**功能**：将 `short` 隐式转换为 `Fixed64`。

**参数**：
- `n`：short 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed64 f = (short)-100; // 隐式转换
```

---

#### `implicit operator Fixed64(int n)`

```csharp
public static implicit operator Fixed64(int n)
```

**功能**：将 `int` 隐式转换为 `Fixed64`。

**参数**：
- `n`：int 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed64 f = 42; // 隐式转换，最常用
```

### 显式转换（从其他类型到 Fixed64）

#### `explicit operator Fixed64(long n)`

```csharp
public static explicit operator Fixed64(long n)
```

**功能**：将 `long` 显式转换为 `Fixed64`。内部通过 `double` 中转处理。

**参数**：
- `n`：long 值

**返回值**：对应的定点数

**注意事项**：`long` 范围可能超出 `Fixed64` 的表示范围，超出部分会被钳制。

**示例**：

```csharp
Fixed64 f = (Fixed64)123456789L;
```

---

#### `explicit operator Fixed64(float n)`

```csharp
public static explicit operator Fixed64(float n)
```

**功能**：将 `float` 显式转换为 `Fixed64`。

**参数**：
- `n`：float 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed64 f = (Fixed64)3.14f;
```

---

#### `explicit operator Fixed64(double n)`

```csharp
public static explicit operator Fixed64(double n)
```

**功能**：将 `double` 显式转换为 `Fixed64`。

**参数**：
- `n`：double 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed64 f = (Fixed64)3.14159265358979;
```

### 显式转换（从 Fixed64 到其他类型）

#### `explicit operator byte(Fixed64 n)`

```csharp
public static explicit operator byte(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `byte`。

**参数**：
- `n`：定点数

**返回值**：转换后的 byte 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(42);
byte b = (byte)f; // 42
```

---

#### `explicit operator short(Fixed64 n)`

```csharp
public static explicit operator short(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `short`。

**参数**：
- `n`：定点数

**返回值**：转换后的 short 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(-100);
short s = (short)f; // -100
```

---

#### `explicit operator int(Fixed64 n)`

```csharp
public static explicit operator int(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `int`。

**参数**：
- `n`：定点数

**返回值**：转换后的 int 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(3.14);
int i = (int)f; // 3（截断小数部分）
```

---

#### `explicit operator long(Fixed64 n)`

```csharp
public static explicit operator long(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `long`。

**参数**：
- `n`：定点数

**返回值**：转换后的 long 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(42);
long l = (long)f; // 42
```

---

#### `explicit operator float(Fixed64 n)`

```csharp
public static explicit operator float(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `float`。

**参数**：
- `n`：定点数

**返回值**：转换后的 float 值

**注意事项**：如果值为 `NaN`，返回 `float.NaN`。

**示例**：

```csharp
var f = new Fixed64(3.14);
float fl = (float)f; // ~3.14
```

---

#### `explicit operator double(Fixed64 n)`

```csharp
public static explicit operator double(Fixed64 n)
```

**功能**：将 `Fixed64` 显式转换为 `double`。

**参数**：
- `n`：定点数

**返回值**：转换后的 double 值

**注意事项**：
- 如果值为 `NaN`，返回 `double.NaN`
- 如果值为正无穷，返回 `double.PositiveInfinity`
- 如果值为负无穷，返回 `double.NegativeInfinity`

**示例**：

```csharp
var f = new Fixed64(3.14);
double d = (double)f; // ~3.14
```

### 转换方法

#### `byte ToByte()`

```csharp
public byte ToByte()
```

**功能**：将定点数转换为 `byte` 类型。

**返回值**：转换后的 byte 值

**注意事项**：如果当前值是 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(42);
byte b = f.ToByte(); // 42
```

---

#### `short ToShort()`

```csharp
public short ToShort()
```

**功能**：将定点数转换为 `short` 类型。

**返回值**：转换后的 short 值

**注意事项**：如果当前值是 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(-100);
short s = f.ToShort(); // -100
```

---

#### `int ToInt()`

```csharp
public int ToInt()
```

**功能**：将定点数转换为 `int` 类型。

**返回值**：转换后的 int 值

**注意事项**：如果当前值是 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(3.14);
int i = f.ToInt(); // 3
```

---

#### `long ToLong()`

```csharp
public long ToLong()
```

**功能**：将定点数转换为 `long` 类型。

**返回值**：转换后的 long 值

**注意事项**：如果当前值是 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed64(42);
long l = f.ToLong(); // 42
```

---

#### `float ToFloat()`

```csharp
public float ToFloat()
```

**功能**：将定点数转换为 `float` 类型。

**返回值**：转换后的 float 值

**注意事项**：如果当前值是 `NaN`，返回 `float.NaN`。

**示例**：

```csharp
var f = new Fixed64(3.14);
float fl = f.ToFloat(); // ~3.14
```

---

#### `double ToDouble()`

```csharp
public double ToDouble()
```

**功能**：将定点数转换为 `double` 类型。

**返回值**：转换后的 double 值

**注意事项**：
- 如果当前值是 `NaN`，返回 `double.NaN`
- 如果当前值是正无穷，返回 `double.PositiveInfinity`
- 如果当前值是负无穷，返回 `double.NegativeInfinity`

**示例**：

```csharp
var f = new Fixed64(3.14);
double d = f.ToDouble(); // ~3.14

var inf = Fixed64.PositiveInfinity;
double dInf = inf.ToDouble(); // double.PositiveInfinity
```

---

#### `static Fixed64 FromRaw(Int128 value)`

```csharp
public static Fixed64 FromRaw(Int128 value)
```

**功能**：从原始内部值创建定点数。直接使用给定的 128 位原始值作为内部存储。

**参数**：
- `value`：原始内部值（Q64.64 格式的 128 位整数）

**返回值**：创建的定点数

**注意事项**：此方法绕过常规的类型转换和范围检查，应谨慎使用。

**示例**：

```csharp
// 创建值为 1.0 的定点数（Int128.One << 64）
var one = Fixed64.FromRaw(Int128.One << 64);
```

---

#### `static Int128 ToRaw(Fixed64 value)`

```csharp
public static Int128 ToRaw(Fixed64 value)
```

**功能**：获取定点数的原始内部存储值。

**参数**：
- `value`：定点数

**返回值**：原始内部值（Q64.64 格式的 128 位整数）

**示例**：

```csharp
var f = new Fixed64(1);
Int128 raw = Fixed64.ToRaw(f); // Int128.One << 64
```

---

#### `Fixed64 Integral()`

```csharp
public Fixed64 Integral()
```

**功能**：获取当前定点数的整数部分（截断小数部分）。

**返回值**：整数部分的定点数

**注意事项**：如果当前值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed64(3.14);
var integral = f.Integral(); // 3
```

---

#### `static Fixed64 Integral(Fixed64 n)`

```csharp
public static Fixed64 Integral(Fixed64 n)
```

**功能**：获取指定定点数的整数部分。

**参数**：
- `n`：定点数

**返回值**：整数部分的定点数

**注意事项**：如果输入值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed64(-3.14);
var integral = Fixed64.Integral(f); // -3
```

---

#### `Fixed64 Fractional()`

```csharp
public Fixed64 Fractional()
```

**功能**：获取当前定点数的小数部分。

**返回值**：小数部分的定点数

**注意事项**：如果当前值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed64(3.14);
var frac = f.Fractional(); // ~0.14
```

---

#### `static Fixed64 Fractional(Fixed64 n)`

```csharp
public static Fixed64 Fractional(Fixed64 n)
```

**功能**：获取指定定点数的小数部分。

**参数**：
- `n`：定点数

**返回值**：小数部分的定点数

**注意事项**：如果输入值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed64(-3.14);
var frac = Fixed64.Fractional(f); // ~0.14（始终为非负）
```

## 算术运算符

### 加法 `operator +`

#### `operator +(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 operator +(Fixed64 a, Fixed64 b)
```

**功能**：两个定点数相加。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：相加后的结果

**注意事项**：
- `NaN` 参与加法，结果为 `NaN`
- 正无穷 + 负无穷 = `NaN`
- 溢出时返回对应方向的无穷大

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(4);
var c = a + b; // 7
```

---

#### `operator +(Fixed64 a, int b)`

```csharp
public static Fixed64 operator +(Fixed64 a, int b)
```

**功能**：定点数加整数。

**参数**：
- `a`：定点数
- `b`：整数

**返回值**：相加后的结果

**示例**：

```csharp
var a = new Fixed64(3.5);
var c = a + 2; // 5.5
```

---

#### `operator +(int a, Fixed64 b)`

```csharp
public static Fixed64 operator +(int a, Fixed64 b)
```

**功能**：整数加定点数。

**参数**：
- `a`：整数
- `b`：定点数

**返回值**：相加后的结果

**示例**：

```csharp
var b = new Fixed64(3.5);
var c = 2 + b; // 5.5
```

### 减法 `operator -`

#### `operator -(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 operator -(Fixed64 a, Fixed64 b)
```

**功能**：两个定点数相减。

**参数**：
- `a`：被减数
- `b`：减数

**返回值**：相减后的结果

**注意事项**：
- `NaN` 参与减法，结果为 `NaN`
- 正无穷 - 正无穷 = `NaN`
- 负无穷 - 负无穷 = `NaN`
- 溢出时返回对应方向的无穷大

**示例**：

```csharp
var a = new Fixed64(10);
var b = new Fixed64(3);
var c = a - b; // 7
```

---

#### `operator -(Fixed64 a, int b)`

```csharp
public static Fixed64 operator -(Fixed64 a, int b)
```

**功能**：定点数减整数。

**参数**：
- `a`：被减数（定点数）
- `b`：减数（整数）

**返回值**：相减后的结果

**示例**：

```csharp
var a = new Fixed64(5.5);
var c = a - 2; // 3.5
```

---

#### `operator -(int a, Fixed64 b)`

```csharp
public static Fixed64 operator -(int a, Fixed64 b)
```

**功能**：整数减定点数。

**参数**：
- `a`：被减数（整数）
- `b`：减数（定点数）

**返回值**：相减后的结果

**示例**：

```csharp
var b = new Fixed64(3.5);
var c = 10 - b; // 6.5
```

### 乘法 `operator *`

#### `operator *(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 operator *(Fixed64 a, Fixed64 b)
```

**功能**：两个定点数相乘。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：相乘后的结果

**注意事项**：
- `NaN` 参与乘法，结果为 `NaN`
- 无穷大 × 0 = `NaN`
- 同号相乘结果为正，异号相乘结果为负
- 溢出时返回对应方向的无穷大

**实现原理**：将操作数分解为整数部分和小数部分分别相乘（共四项），小数×小数部分使用 `UInt128` 存储以避免溢出，合并后检查进位和符号。

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(4);
var c = a * b; // 12
```

---

#### `operator *(Fixed64 a, int b)`

```csharp
public static Fixed64 operator *(Fixed64 a, int b)
```

**功能**：定点数乘整数。

**参数**：
- `a`：定点数
- `b`：整数

**返回值**：相乘后的结果

**示例**：

```csharp
var a = new Fixed64(3.5);
var c = a * 2; // 7
```

---

#### `operator *(int a, Fixed64 b)`

```csharp
public static Fixed64 operator *(int a, Fixed64 b)
```

**功能**：整数乘定点数。

**参数**：
- `a`：整数
- `b`：定点数

**返回值**：相乘后的结果

**示例**：

```csharp
var b = new Fixed64(3.5);
var c = 2 * b; // 7
```

### 除法 `operator /`

#### `operator /(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 operator /(Fixed64 a, Fixed64 b)
```

**功能**：两个定点数相除。

**参数**：
- `a`：被除数
- `b`：除数

**返回值**：相除后的结果

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**注意事项**：
- `NaN` 参与除法，结果为 `NaN`
- 0 / 0 = `NaN`
- 非零 / 0 = 对应方向的无穷大
- 任何有限数 / 无穷大 = 0
- 无穷大 / 无穷大 = `NaN`

**实现原理**：逐位除法算法，2 的幂除数有优化路径，含四舍五入。

**示例**：

```csharp
var a = new Fixed64(12);
var b = new Fixed64(4);
var c = a / b; // 3
```

---

#### `operator /(Fixed64 a, int b)`

```csharp
public static Fixed64 operator /(Fixed64 a, int b)
```

**功能**：定点数除以整数。

**参数**：
- `a`：被除数
- `b`：除数（整数）

**返回值**：相除后的结果

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var a = new Fixed64(7);
var c = a / 2; // 3.5
```

---

#### `operator /(int a, Fixed64 b)`

```csharp
public static Fixed64 operator /(int a, Fixed64 b)
```

**功能**：整数除以定点数。

**参数**：
- `a`：被除数（整数）
- `b`：除数

**返回值**：相除后的结果

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var b = new Fixed64(2);
var c = 7 / b; // 3.5
```

### 取模 `operator %`

#### `operator %(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 operator %(Fixed64 a, Fixed64 b)
```

**功能**：计算两个定点数相除的余数。

**参数**：
- `a`：被除数
- `b`：除数

**返回值**：余数

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**注意事项**：
- 如果有任何一个输入是 `NaN`，返回 `NaN`
- 如果被除数是无穷大，返回 `NaN`
- 如果被除数是最小值或除数为 -1，返回 0
- 如果除数是最大值、最小值或无穷大，返回被除数

**示例**：

```csharp
var a = new Fixed64(7);
var b = new Fixed64(3);
var c = a % b; // 1
```

---

#### `operator %(Fixed64 a, int b)`

```csharp
public static Fixed64 operator %(Fixed64 a, int b)
```

**功能**：定点数对整数取模。

**参数**：
- `a`：被除数
- `b`：除数（整数）

**返回值**：余数

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var a = new Fixed64(7);
var c = a % 3; // 1
```

---

#### `operator %(int a, Fixed64 b)`

```csharp
public static Fixed64 operator %(int a, Fixed64 b)
```

**功能**：整数对定点数取模。

**参数**：
- `a`：被除数（整数）
- `b`：除数

**返回值**：余数

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var b = new Fixed64(3);
var c = 7 % b; // 1
```

### 取反 `operator -`

#### `operator -(Fixed64 n)`

```csharp
public static Fixed64 operator -(Fixed64 n)
```

**功能**：返回定点数的相反数。

**参数**：
- `n`：要取反的定点数

**返回值**：取反后的结果

**注意事项**：
- `NaN` 取反返回 `NaN`
- 零取反返回零
- 正无穷取反返回负无穷，反之亦然

**示例**：

```csharp
var a = new Fixed64(3);
var b = -a; // -3
```

## 比较运算符

### `operator ==`

```csharp
public static bool operator ==(Fixed64 a, Fixed64 b)
```

**功能**：判断两个定点数是否相等。

**返回值**：如果相等返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(3);
bool equal = a == b; // true
```

---

### `operator !=`

```csharp
public static bool operator !=(Fixed64 a, Fixed64 b)
```

**功能**：判断两个定点数是否不相等。

**返回值**：如果不相等返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `true`。

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(4);
bool notEqual = a != b; // true
```

---

### `operator >`

```csharp
public static bool operator >(Fixed64 a, Fixed64 b)
```

**功能**：判断第一个定点数是否大于第二个。

**返回值**：如果大于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed64(5);
var b = new Fixed64(3);
bool greater = a > b; // true
```

---

### `operator <`

```csharp
public static bool operator <(Fixed64 a, Fixed64 b)
```

**功能**：判断第一个定点数是否小于第二个。

**返回值**：如果小于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(5);
bool less = a < b; // true
```

---

### `operator >=`

```csharp
public static bool operator >=(Fixed64 a, Fixed64 b)
```

**功能**：判断第一个定点数是否大于等于第二个。

**返回值**：如果大于等于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed64(5);
var b = new Fixed64(5);
bool ge = a >= b; // true
```

---

### `operator <=`

```csharp
public static bool operator <=(Fixed64 a, Fixed64 b)
```

**功能**：判断第一个定点数是否小于等于第二个。

**返回值**：如果小于等于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed64(5);
var b = new Fixed64(5);
bool le = a <= b; // true
```

## 数学方法

### 绝对值

#### `Fixed64 Abs()`

```csharp
public Fixed64 Abs()
```

**功能**：获取当前定点数的绝对值。

**返回值**：绝对值

**注意事项**：
- 如果当前值是 `NaN`，返回 `NaN`
- 如果当前值是正数，返回原值
- 如果当前值是负无穷，返回正无穷
- 如果当前值是最小值（`MinValue`），返回最大值（`MaxValue`）

**示例**：

```csharp
var a = new Fixed64(-3);
var b = a.Abs(); // 3
```

---

#### `static Fixed64 Abs(Fixed64 n)`

```csharp
public static Fixed64 Abs(Fixed64 n)
```

**功能**：获取指定定点数的绝对值。

**参数**：
- `n`：要计算绝对值的定点数

**返回值**：绝对值

**示例**：

```csharp
var result = Fixed64.Abs(new Fixed64(-5)); // 5
```

### 符号

#### `int Sign()`

```csharp
public int Sign()
```

**功能**：获取当前定点数的符号。

**返回值**：正数返回 1，负数返回 -1，零返回 0

**异常**：如果当前值是 `NaN`，抛出 `ArithmeticException`

**示例**：

```csharp
var a = new Fixed64(-3);
int s = a.Sign(); // -1

var b = Fixed64.Zero;
int s2 = b.Sign(); // 0
```

---

#### `static int Sign(Fixed64 n)`

```csharp
public static int Sign(Fixed64 n)
```

**功能**：获取指定定点数的符号。

**参数**：
- `n`：要获取符号的定点数

**返回值**：正数返回 1，负数返回 -1，零返回 0

**异常**：如果输入值是 `NaN`，抛出 `ArithmeticException`

**示例**：

```csharp
int s = Fixed64.Sign(new Fixed64(42)); // 1
```

---

#### `static bool IsSigns(Fixed64 a, Fixed64 b)`

```csharp
public static bool IsSigns(Fixed64 a, Fixed64 b)
```

**功能**：判断两个定点数的符号是否相同。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：如果符号相同返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = new Fixed64(3);
var b = new Fixed64(5);
bool same = Fixed64.IsSigns(a, b); // true

var c = new Fixed64(-3);
bool diff = Fixed64.IsSigns(a, c); // false
```

### 平方根

#### `Fixed64 Sqrt()`

```csharp
public Fixed64 Sqrt()
```

**功能**：计算当前定点数的平方根。使用二进制搜索算法，两轮迭代（64 位精度）。

**返回值**：平方根值

**注意事项**：
- 如果当前值是 `NaN`，返回 `NaN`
- 如果当前值是正无穷，返回正无穷
- 如果当前值是 0，返回 0
- 如果当前值是负数，返回 `NaN`（非抛出异常）

**示例**：

```csharp
var a = new Fixed64(16);
var b = a.Sqrt(); // 4

var c = new Fixed64(2);
var d = c.Sqrt(); // ~1.4142
```

---

#### `static Fixed64 Sqrt(Fixed64 n)`

```csharp
public static Fixed64 Sqrt(Fixed64 n)
```

**功能**：计算指定定点数的平方根。

**参数**：
- `n`：要计算平方根的定点数

**返回值**：平方根值

**示例**：

```csharp
var result = Fixed64.Sqrt(new Fixed64(9)); // 3
```

### 立方根

#### `Fixed64 Cbrt()`

```csharp
public Fixed64 Cbrt()
```

**功能**：计算当前定点数的立方根。使用公式 ∛x = e^(ln(|x|)/3)。

**返回值**：立方根值

**注意事项**：
- 如果当前值是 `NaN`，返回 `NaN`
- 如果当前值是 0，返回 0
- 支持负数的立方根：∛(-x) = -∛(x)

**示例**：

```csharp
var a = new Fixed64(27);
var b = a.Cbrt(); // 3

var c = new Fixed64(-8);
var d = c.Cbrt(); // -2
```

---

#### `static Fixed64 Cbrt(Fixed64 n)`

```csharp
public static Fixed64 Cbrt(Fixed64 n)
```

**功能**：计算指定定点数的立方根。

**参数**：
- `n`：要计算立方根的定点数

**返回值**：立方根值

**示例**：

```csharp
var result = Fixed64.Cbrt(new Fixed64(8)); // 2
```

### 幂运算

#### `Fixed64 Pow(int n)`

```csharp
public Fixed64 Pow(int n)
```

**功能**：计算当前定点数的整数次幂。使用快速幂算法。

**参数**：
- `n`：指数（整数）

**返回值**：幂值

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 任何数的 0 次幂返回 1
- 0 的负数次幂返回正无穷
- -1 的无穷次幂返回 1
- 纯小数（|x| < 1）的正无穷次幂返回 0，负无穷次幂返回正无穷
- 非纯小数的正无穷次幂返回正无穷，负无穷次幂返回 0

**示例**：

```csharp
var a = new Fixed64(2);
var b = a.Pow(10); // 1024

var c = new Fixed64(3);
var d = c.Pow(-1); // ~0.3333（即 1/3）
```

---

#### `Fixed64 Pow(Fixed64 n)`

```csharp
public Fixed64 Pow(Fixed64 n)
```

**功能**：计算当前定点数的任意次幂。

**参数**：
- `n`：指数（定点数）

**返回值**：幂值

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 任何数的 0 次幂返回 1
- 负数的小数次幂返回 `NaN`
- 0 的负数次幂返回正无穷
- 对于小数指数，使用公式 m^n = e^(n × ln(m))

**示例**：

```csharp
var a = new Fixed64(4);
var b = a.Pow(new Fixed64(0.5)); // 2（即 √4）

var c = new Fixed64(2);
var d = c.Pow(new Fixed64(3)); // 8
```

---

#### `static Fixed64 Pow(Fixed64 m, int n)`

```csharp
public static Fixed64 Pow(Fixed64 m, int n)
```

**功能**：计算指定定点数的整数次幂。

**参数**：
- `m`：底数
- `n`：指数

**返回值**：幂值

**示例**：

```csharp
var result = Fixed64.Pow(new Fixed64(2), 8); // 256
```

---

#### `static Fixed64 Pow(Fixed64 m, Fixed64 n)`

```csharp
public static Fixed64 Pow(Fixed64 m, Fixed64 n)
```

**功能**：计算指定底数的指定次幂。

**参数**：
- `m`：底数
- `n`：指数

**返回值**：幂值

**示例**：

```csharp
var result = Fixed64.Pow(new Fixed64(9), new Fixed64(0.5)); // 3
```

### 2 的幂相关

#### `bool IsPowerOfTwo()`

```csharp
public bool IsPowerOfTwo()
```

**功能**：判断当前定点数是否为 2 的幂。

**返回值**：如果是 2 的幂返回 `true`，否则返回 `false`

**注意事项**：非正数不可能是 2 的幂。

**示例**：

```csharp
var a = new Fixed64(4);
bool isPow = a.IsPowerOfTwo(); // true

var b = new Fixed64(3);
bool isPow2 = b.IsPowerOfTwo(); // false
```

---

#### `static bool IsPowerOfTwo(Fixed64 value)`

```csharp
public static bool IsPowerOfTwo(Fixed64 value)
```

**功能**：判断指定定点数是否为 2 的幂。

**参数**：
- `value`：要判断的定点数

**返回值**：如果是 2 的幂返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsPowerOfTwo(new Fixed64(8)); // true
```

---

#### `Fixed64 ClosestPowerOfTwo()`

```csharp
public Fixed64 ClosestPowerOfTwo()
```

**功能**：计算最接近当前定点数的 2 的幂。

**返回值**：最接近的 2 的幂

**注意事项**：非正数返回 1。

**示例**：

```csharp
var a = new Fixed64(5);
var b = a.ClosestPowerOfTwo(); // 4

var c = new Fixed64(6);
var d = c.ClosestPowerOfTwo(); // 8
```

---

#### `static Fixed64 ClosestPowerOfTwo(Fixed64 value)`

```csharp
public static Fixed64 ClosestPowerOfTwo(Fixed64 value)
```

**功能**：计算最接近指定定点数的 2 的幂。

**参数**：
- `value`：要计算的定点数

**返回值**：最接近的 2 的幂

**示例**：

```csharp
var result = Fixed64.ClosestPowerOfTwo(new Fixed64(5)); // 4
```

---

#### `Fixed64 NextPowerOfTwo()`

```csharp
public Fixed64 NextPowerOfTwo()
```

**功能**：计算大于当前定点数的最小 2 的幂。

**返回值**：下一个 2 的幂

**示例**：

```csharp
var a = new Fixed64(5);
var b = a.NextPowerOfTwo(); // 8
```

---

#### `static Fixed64 NextPowerOfTwo(Fixed64 value)`

```csharp
public static Fixed64 NextPowerOfTwo(Fixed64 value)
```

**功能**：计算大于指定定点数的最小 2 的幂。

**参数**：
- `value`：要计算的定点数

**返回值**：下一个 2 的幂

**示例**：

```csharp
var result = Fixed64.NextPowerOfTwo(new Fixed64(5)); // 8
```

### 指数函数

#### `Fixed64 Exp()`

```csharp
public Fixed64 Exp()
```

**功能**：计算 e 的当前定点数次幂（e^n）。

**返回值**：e^n 的值

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是 0，返回 1
- 如果输入是正无穷，返回正无穷
- 如果输入是负无穷，返回 0

**实现原理**：
1. 分解 x = k × ln(2) + r，其中 |r| ≤ 0.5 × ln(2)
2. 计算 e^r 的泰勒级数展开
3. 计算 2^k × e^r 得到最终结果（k 可达 63）

**示例**：

```csharp
var a = new Fixed64(1);
var b = a.Exp(); // ~2.71828（即 e）

var c = new Fixed64(0);
var d = c.Exp(); // 1
```

---

#### `static Fixed64 Exp(Fixed64 m)`

```csharp
public static Fixed64 Exp(Fixed64 m)
```

**功能**：计算 e 的指定定点数次幂。

**参数**：
- `m`：指数

**返回值**：e^m 的值

**示例**：

```csharp
var result = Fixed64.Exp(new Fixed64(2)); // ~7.38906（即 e²）
```

### 对数函数

#### `Fixed64 Log()`

```csharp
public Fixed64 Log()
```

**功能**：计算当前定点数的自然对数（以 e 为底）。

**返回值**：自然对数值

**注意事项**：
- 如果输入是 `NaN` 或负数，返回 `NaN`
- 如果输入是 0，返回负无穷
- 如果输入是正无穷，返回正无穷

**实现原理**：先计算 Log2()，再乘以 ln(2)。

**示例**：

```csharp
var a = new Fixed64(2.71828);
var b = a.Log(); // ~1.0（即 ln(e)）

var c = new Fixed64(1);
var d = c.Log(); // 0
```

---

#### `static Fixed64 Log(Fixed64 n)`

```csharp
public static Fixed64 Log(Fixed64 n)
```

**功能**：计算指定定点数的自然对数。

**参数**：
- `n`：要计算自然对数的定点数

**返回值**：自然对数值

**示例**：

```csharp
var result = Fixed64.Log(new Fixed64(10)); // ~2.3026
```

---

#### `Fixed64 Log2()`

```csharp
public Fixed64 Log2()
```

**功能**：计算当前定点数的以 2 为底的对数。

**返回值**：以 2 为底的对数值

**注意事项**：
- 如果输入是 `NaN` 或负数，返回 `NaN`
- 如果输入是 0，返回负无穷
- 如果输入是正无穷，返回正无穷

**实现原理**：归一化到 [1, 2) 区间，使用二进制搜索算法计算对数的小数部分，迭代 64 次。

**示例**：

```csharp
var a = new Fixed64(8);
var b = a.Log2(); // 3

var c = new Fixed64(1);
var d = c.Log2(); // 0
```

---

#### `static Fixed64 Log2(Fixed64 n)`

```csharp
public static Fixed64 Log2(Fixed64 n)
```

**功能**：计算指定定点数的以 2 为底的对数。

**参数**：
- `n`：要计算对数的定点数

**返回值**：以 2 为底的对数值

**示例**：

```csharp
var result = Fixed64.Log2(new Fixed64(16)); // 4
```

---

#### `Fixed64 Log10()`

```csharp
public Fixed64 Log10()
```

**功能**：计算当前定点数的以 10 为底的对数。

**返回值**：以 10 为底的对数值

**注意事项**：
- 如果输入是 `NaN` 或负数，返回 `NaN`
- 如果输入是 0，返回负无穷
- 如果输入是正无穷，返回正无穷

**实现原理**：使用 Log() / ln(10) 计算。

**示例**：

```csharp
var a = new Fixed64(100);
var b = a.Log10(); // 2

var c = new Fixed64(1000);
var d = c.Log10(); // 3
```

---

#### `static Fixed64 Log10(Fixed64 n)`

```csharp
public static Fixed64 Log10(Fixed64 n)
```

**功能**：计算指定定点数的以 10 为底的对数。

**参数**：
- `n`：要计算对数的定点数

**返回值**：以 10 为底的对数值

**示例**：

```csharp
var result = Fixed64.Log10(new Fixed64(100)); // 2
```

### 倒数

#### `Fixed64 Reciprocal()`

```csharp
public Fixed64 Reciprocal()
```

**功能**：计算当前定点数的倒数（1/x）。

**返回值**：倒数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是 0，返回正无穷
- 如果输入是无穷大，返回 0

**示例**：

```csharp
var a = new Fixed64(4);
var b = a.Reciprocal(); // 0.25

var c = new Fixed64(0.5);
var d = c.Reciprocal(); // 2
```

---

#### `static Fixed64 Reciprocal(Fixed64 n)`

```csharp
public static Fixed64 Reciprocal(Fixed64 n)
```

**功能**：计算指定定点数的倒数。

**参数**：
- `n`：要计算倒数的定点数

**返回值**：倒数

**示例**：

```csharp
var result = Fixed64.Reciprocal(new Fixed64(5)); // 0.2
```

## 取整方法

### 四舍五入

#### `Fixed64 Round()`

```csharp
public Fixed64 Round()
```

**功能**：将当前定点数四舍五入到最近的整数。

**返回值**：四舍五入后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷，返回正无穷
- 如果输入是负无穷，返回负无穷
- 小数部分等于 0.5 时，向偶数取整（银行家舍入法）
- Q64.64 格式下半值点为 `Int128.One << 63`

**示例**：

```csharp
var a = new Fixed64(3.5);
var b = a.Round(); // 4

var c = new Fixed64(2.5);
var d = c.Round(); // 2（向偶数取整）

var e = new Fixed64(3.4);
var f = e.Round(); // 3
```

---

#### `int RoundToInt()`

```csharp
public int RoundToInt()
```

**功能**：将当前定点数四舍五入到最近的整数，并返回 `int` 类型。

**返回值**：四舍五入后的整数值

**注意事项**：
- 如果输入是 `NaN`，返回 0
- 如果输入是正无穷，返回 `int.MaxValue`
- 如果输入是负无穷，返回 `int.MinValue`

**示例**：

```csharp
var a = new Fixed64(3.7);
int b = a.RoundToInt(); // 4
```

---

#### `static Fixed64 Round(Fixed64 n)`

```csharp
public static Fixed64 Round(Fixed64 n)
```

**功能**：将指定定点数四舍五入到最近的整数。

**参数**：
- `n`：要四舍五入的定点数

**返回值**：四舍五入后的定点数

**示例**：

```csharp
var result = Fixed64.Round(new Fixed64(3.6)); // 4
```

---

#### `static int RoundToInt(Fixed64 n)`

```csharp
public static int RoundToInt(Fixed64 n)
```

**功能**：将指定定点数四舍五入到最近的整数，并返回 `int` 类型。

**参数**：
- `n`：要四舍五入的定点数

**返回值**：四舍五入后的整数值

**示例**：

```csharp
int result = Fixed64.RoundToInt(new Fixed64(3.6)); // 4
```

### 向下取整

#### `Fixed64 Floor()`

```csharp
public Fixed64 Floor()
```

**功能**：返回小于或等于当前定点数的最大整数。

**返回值**：向下取整后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是负无穷，返回负无穷
- 如果输入是正无穷，返回正无穷

**示例**：

```csharp
var a = new Fixed64(3.7);
var b = a.Floor(); // 3

var c = new Fixed64(-3.2);
var d = c.Floor(); // -4
```

---

#### `int FloorToInt()`

```csharp
public int FloorToInt()
```

**功能**：返回小于或等于当前定点数的最大整数，并返回 `int` 类型。

**返回值**：向下取整后的整数值

**注意事项**：
- 如果输入是 `NaN`，返回 0
- 如果输入是负无穷，返回 `int.MinValue`

**示例**：

```csharp
var a = new Fixed64(3.7);
int b = a.FloorToInt(); // 3
```

---

#### `static Fixed64 Floor(Fixed64 n)`

```csharp
public static Fixed64 Floor(Fixed64 n)
```

**功能**：返回小于或等于指定定点数的最大整数。

**参数**：
- `n`：要向下取整的定点数

**返回值**：向下取整后的定点数

**示例**：

```csharp
var result = Fixed64.Floor(new Fixed64(3.7)); // 3
```

---

#### `static int FloorToInt(Fixed64 n)`

```csharp
public static int FloorToInt(Fixed64 n)
```

**功能**：返回小于或等于指定定点数的最大整数，并返回 `int` 类型。

**参数**：
- `n`：要向下取整的定点数

**返回值**：向下取整后的整数值

**示例**：

```csharp
int result = Fixed64.FloorToInt(new Fixed64(3.7)); // 3
```

### 向上取整

#### `Fixed64 Ceil()`

```csharp
public Fixed64 Ceil()
```

**功能**：返回大于或等于当前定点数的最小整数。

**返回值**：向上取整后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷，返回正无穷
- 如果没有小数部分，直接返回原值

**示例**：

```csharp
var a = new Fixed64(3.2);
var b = a.Ceil(); // 4

var c = new Fixed64(-3.7);
var d = c.Ceil(); // -3
```

---

#### `int CeilToInt()`

```csharp
public int CeilToInt()
```

**功能**：返回大于或等于当前定点数的最小整数，并返回 `int` 类型。

**返回值**：向上取整后的整数值

**注意事项**：
- 如果输入是 `NaN`，返回 0
- 如果输入是正无穷，返回 `int.MaxValue`

**示例**：

```csharp
var a = new Fixed64(3.2);
int b = a.CeilToInt(); // 4
```

---

#### `static Fixed64 Ceil(Fixed64 n)`

```csharp
public static Fixed64 Ceil(Fixed64 n)
```

**功能**：返回大于或等于指定定点数的最小整数。

**参数**：
- `n`：要向上取整的定点数

**返回值**：向上取整后的定点数

**示例**：

```csharp
var result = Fixed64.Ceil(new Fixed64(3.2)); // 4
```

---

#### `static int CeilToInt(Fixed64 n)`

```csharp
public static int CeilToInt(Fixed64 n)
```

**功能**：返回大于或等于指定定点数的最小整数，并返回 `int` 类型。

**参数**：
- `n`：要向上取整的定点数

**返回值**：向上取整后的整数值

**示例**：

```csharp
int result = Fixed64.CeilToInt(new Fixed64(3.2)); // 4
```

## 最值与限制

### 最小值 Min

#### `static Fixed64 Min(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 Min(Fixed64 a, Fixed64 b)
```

**功能**：返回两个定点数中的较小值。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：两个数中的较小值

**示例**：

```csharp
var result = Fixed64.Min(new Fixed64(3), new Fixed64(5)); // 3
```

---

#### `static Fixed64 Min(Fixed64 a, Fixed64 b, Fixed64 c)`

```csharp
public static Fixed64 Min(Fixed64 a, Fixed64 b, Fixed64 c)
```

**功能**：返回三个定点数中的最小值。

**参数**：
- `a`、`b`、`c`：三个定点数

**返回值**：三个数中的最小值

**示例**：

```csharp
var result = Fixed64.Min(new Fixed64(3), new Fixed64(1), new Fixed64(5)); // 1
```

---

#### `static Fixed64 Min(Fixed64 a, Fixed64 b, Fixed64 c, Fixed64 d)`

```csharp
public static Fixed64 Min(Fixed64 a, Fixed64 b, Fixed64 c, Fixed64 d)
```

**功能**：返回四个定点数中的最小值。

**参数**：
- `a`、`b`、`c`、`d`：四个定点数

**返回值**：四个数中的最小值

**示例**：

```csharp
var result = Fixed64.Min(new Fixed64(3), new Fixed64(1), new Fixed64(5), new Fixed64(2)); // 1
```

---

#### `static Fixed64 Min(params Fixed64[] fixeds)`

```csharp
public static Fixed64 Min(params Fixed64[] fixeds)
```

**功能**：返回多个定点数中的最小值。

**参数**：
- `fixeds`：定点数数组

**返回值**：多个数中的最小值

**异常**：如果数组为空，抛出 `ArgumentException`

**注意事项**：如果数组包含 `NaN`，返回 `NaN`。

**示例**：

```csharp
var result = Fixed64.Min(new Fixed64(3), new Fixed64(1), new Fixed64(5), new Fixed64(2), new Fixed64(4)); // 1
```

### 最大值 Max

#### `static Fixed64 Max(Fixed64 a, Fixed64 b)`

```csharp
public static Fixed64 Max(Fixed64 a, Fixed64 b)
```

**功能**：返回两个定点数中的较大值。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：两个数中的较大值

**示例**：

```csharp
var result = Fixed64.Max(new Fixed64(3), new Fixed64(5)); // 5
```

---

#### `static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c)`

```csharp
public static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c)
```

**功能**：返回三个定点数中的最大值。

**参数**：
- `a`、`b`、`c`：三个定点数

**返回值**：三个数中的最大值

**示例**：

```csharp
var result = Fixed64.Max(new Fixed64(3), new Fixed64(1), new Fixed64(5)); // 5
```

---

#### `static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c, Fixed64 d)`

```csharp
public static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c, Fixed64 d)
```

**功能**：返回四个定点数中的最大值。

**参数**：
- `a`、`b`、`c`、`d`：四个定点数

**返回值**：四个数中的最大值

**示例**：

```csharp
var result = Fixed64.Max(new Fixed64(3), new Fixed64(1), new Fixed64(5), new Fixed64(2)); // 5
```

---

#### `static Fixed64 Max(params Fixed64[] fixeds)`

```csharp
public static Fixed64 Max(params Fixed64[] fixeds)
```

**功能**：返回多个定点数中的最大值。

**参数**：
- `fixeds`：定点数数组

**返回值**：多个数中的最大值

**异常**：如果数组为空，抛出 `ArgumentException`

**注意事项**：如果数组包含 `NaN`，返回 `NaN`。

**示例**：

```csharp
var result = Fixed64.Max(new Fixed64(3), new Fixed64(1), new Fixed64(5), new Fixed64(2), new Fixed64(4)); // 5
```

### 范围限制

#### `static Fixed64 Clamp(Fixed64 value, Fixed64 min, Fixed64 max)`

```csharp
public static Fixed64 Clamp(Fixed64 value, Fixed64 min, Fixed64 max)
```

**功能**：将定点数限制在指定的最小值和最大值之间。

**参数**：
- `value`：要限制的定点数
- `min`：最小值
- `max`：最大值

**返回值**：限制在 [min, max] 范围内的结果

**示例**：

```csharp
var result1 = Fixed64.Clamp(new Fixed64(5), new Fixed64(0), new Fixed64(10)); // 5
var result2 = Fixed64.Clamp(new Fixed64(-3), new Fixed64(0), new Fixed64(10)); // 0
var result3 = Fixed64.Clamp(new Fixed64(15), new Fixed64(0), new Fixed64(10)); // 10
```

---

#### `static Fixed64 Clamp01(Fixed64 value)`

```csharp
public static Fixed64 Clamp01(Fixed64 value)
```

**功能**：将定点数限制在 [0, 1] 范围内。

**参数**：
- `value`：要限制的定点数

**返回值**：限制在 [0, 1] 范围内的结果

**示例**：

```csharp
var result1 = Fixed64.Clamp01(new Fixed64(0.5)); // 0.5
var result2 = Fixed64.Clamp01(new Fixed64(-1));  // 0
var result3 = Fixed64.Clamp01(new Fixed64(2));   // 1
```

## 状态检测

### `bool IsNaN()`

```csharp
public bool IsNaN()
```

**功能**：检测当前定点数是否为 `NaN`。

**返回值**：如果是 `NaN` 返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.NaN;
bool isNan = a.IsNaN(); // true

var b = new Fixed64(3);
bool isNan2 = b.IsNaN(); // false
```

---

### `static bool IsNaN(Fixed64 value)`

```csharp
public static bool IsNaN(Fixed64 value)
```

**功能**：检测指定定点数是否为 `NaN`。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是 `NaN` 返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsNaN(Fixed64.NaN); // true
```

---

### `bool IsZero()`

```csharp
public bool IsZero()
```

**功能**：检测当前定点数是否为零。

**返回值**：如果是零返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.Zero;
bool isZero = a.IsZero(); // true
```

---

### `static bool IsZero(Fixed64 n)`

```csharp
public static bool IsZero(Fixed64 n)
```

**功能**：检测指定定点数是否为零。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是零返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsZero(new Fixed64(0)); // true
```

---

### `bool IsMin()`

```csharp
public bool IsMin()
```

**功能**：检测当前定点数是否为最小值（`MinValue`）。

**返回值**：如果是最小值返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.MinValue;
bool isMin = a.IsMin(); // true
```

---

### `static bool IsMin(Fixed64 n)`

```csharp
public static bool IsMin(Fixed64 n)
```

**功能**：检测指定定点数是否为最小值。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是最小值返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsMin(Fixed64.MinValue); // true
```

---

### `bool IsMax()`

```csharp
public bool IsMax()
```

**功能**：检测当前定点数是否为最大值（`MaxValue`）。

**返回值**：如果是最大值返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.MaxValue;
bool isMax = a.IsMax(); // true
```

---

### `static bool IsMax(Fixed64 n)`

```csharp
public static bool IsMax(Fixed64 n)
```

**功能**：检测指定定点数是否为最大值。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是最大值返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsMax(Fixed64.MaxValue); // true
```

---

### `bool IsInfinity()`

```csharp
public bool IsInfinity()
```

**功能**：检测当前定点数是否为无穷大（正无穷或负无穷）。

**返回值**：如果是无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.PositiveInfinity;
bool isInf = a.IsInfinity(); // true
```

---

### `static bool IsInfinity(Fixed64 value)`

```csharp
public static bool IsInfinity(Fixed64 value)
```

**功能**：检测指定定点数是否为无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsInfinity(Fixed64.NegativeInfinity); // true
```

---

### `bool IsPositiveInfinity()`

```csharp
public bool IsPositiveInfinity()
```

**功能**：检测当前定点数是否为正无穷大。

**返回值**：如果是正无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.PositiveInfinity;
bool isPosInf = a.IsPositiveInfinity(); // true
```

---

### `static bool IsPositiveInfinity(Fixed64 value)`

```csharp
public static bool IsPositiveInfinity(Fixed64 value)
```

**功能**：检测指定定点数是否为正无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是正无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsPositiveInfinity(Fixed64.PositiveInfinity); // true
```

---

### `bool IsNegativeInfinity()`

```csharp
public bool IsNegativeInfinity()
```

**功能**：检测当前定点数是否为负无穷大。

**返回值**：如果是负无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = Fixed64.NegativeInfinity;
bool isNegInf = a.IsNegativeInfinity(); // true
```

---

### `static bool IsNegativeInfinity(Fixed64 value)`

```csharp
public static bool IsNegativeInfinity(Fixed64 value)
```

**功能**：检测指定定点数是否为负无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是负无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsNegativeInfinity(Fixed64.NegativeInfinity); // true
```

---

### `bool IsPositive()`

```csharp
public bool IsPositive()
```

**功能**：检测当前定点数是否为正数（包括 0）。

**返回值**：如果是正数（含 0）返回 `true`，否则返回 `false`

**注意事项**：零被视为正数。

**示例**：

```csharp
var a = new Fixed64(3);
bool isPos = a.IsPositive(); // true

var b = Fixed64.Zero;
bool isPos2 = b.IsPositive(); // true
```

---

### `static bool IsPositive(Fixed64 value)`

```csharp
public static bool IsPositive(Fixed64 value)
```

**功能**：检测指定定点数是否为正数（包括 0）。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是正数（含 0）返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsPositive(new Fixed64(-3)); // false
```

---

### `bool IsNegative()`

```csharp
public bool IsNegative()
```

**功能**：检测当前定点数是否为负数。

**返回值**：如果是负数返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = new Fixed64(-3);
bool isNeg = a.IsNegative(); // true
```

---

### `static bool IsNegative(Fixed64 value)`

```csharp
public static bool IsNegative(Fixed64 value)
```

**功能**：检测指定定点数是否为负数。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是负数返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsNegative(new Fixed64(3)); // false
```

---

### `bool IsFractional()`

```csharp
public bool IsFractional()
```

**功能**：检测当前定点数是否有小数部分。

**返回值**：如果有小数部分返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = new Fixed64(3.14);
bool isFrac = a.IsFractional(); // true

var b = new Fixed64(3);
bool isFrac2 = b.IsFractional(); // false
```

---

### `static bool IsFractional(Fixed64 value)`

```csharp
public static bool IsFractional(Fixed64 value)
```

**功能**：检测指定定点数是否有小数部分。

**参数**：
- `value`：要检测的定点数

**返回值**：如果有小数部分返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed64.IsFractional(new Fixed64(3.14)); // true
```

## 三角函数

### 正弦 Sin

#### `static Fixed64 Sin(Fixed64 radian)`

```csharp
public static Fixed64 Sin(Fixed64 radian)
```

**功能**：计算指定弧度的正弦值。使用泰勒级数展开（到 x²⁷/27! 项，共 13 个系数）。

**参数**：
- `radian`：弧度值

**返回值**：正弦值，范围在 [-1, 1] 之间

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`

**实现原理**：先规范化到 [0, 2π]，再缩减到 [0, π/2]，使用泰勒级数展开。

**示例**：

```csharp
var sin0 = Fixed64.Sin(Fixed64.Zero);       // 0
var sinPi2 = Fixed64.Sin(Fixed64.Half_PI);  // 1
var sinPi = Fixed64.Sin(Fixed64.PI);        // 0
```

---

#### `static Fixed64 FastSin(Fixed64 radian)`

```csharp
public static Fixed64 FastSin(Fixed64 radian)
```

**功能**：使用查表法快速计算正弦值。速度比 `Sin` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：正弦值，范围在 [-1, 1] 之间

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`
- 误差大于 `Sin` 函数
- 查找表大小为 205888 项，使用 `Lazy<Int128[]>` 延迟加载

**示例**：

```csharp
var result = Fixed64.FastSin(Fixed64.Half_PI); // ~1（精度略低）
```

---

#### `static Fixed64 Asin(Fixed64 value)`

```csharp
public static Fixed64 Asin(Fixed64 value)
```

**功能**：计算指定值的反正弦值。

**参数**：
- `value`：正弦值，范围在 [-1, 1] 之间

**返回值**：反正弦值，范围在 [-π/2, π/2] 之间

**异常**：当输入值不在 [-1, 1] 范围内时，抛出 `ArgumentOutOfRangeException`

**实现原理**：`Half_PI - Acos(value)`

**示例**：

```csharp
var result = Fixed64.Asin(Fixed64.One); // π/2
```

### 余弦 Cos

#### `static Fixed64 Cos(Fixed64 radian)`

```csharp
public static Fixed64 Cos(Fixed64 radian)
```

**功能**：计算指定弧度的余弦值。内部使用 `Sin(radian + π/2)` 实现。

**参数**：
- `radian`：弧度值

**返回值**：余弦值，范围在 [-1, 1] 之间

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`

**示例**：

```csharp
var cos0 = Fixed64.Cos(Fixed64.Zero);   // 1
var cosPi2 = Fixed64.Cos(Fixed64.Half_PI); // 0
var cosPi = Fixed64.Cos(Fixed64.PI);    // -1
```

---

#### `static Fixed64 FastCos(Fixed64 radian)`

```csharp
public static Fixed64 FastCos(Fixed64 radian)
```

**功能**：使用快速正弦函数计算余弦值。速度比 `Cos` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：余弦值，范围在 [-1, 1] 之间

**注意事项**：误差大于 `Cos` 函数。

**示例**：

```csharp
var result = Fixed64.FastCos(Fixed64.Zero); // ~1
```

---

#### `static Fixed64 Acos(Fixed64 value)`

```csharp
public static Fixed64 Acos(Fixed64 value)
```

**功能**：计算指定值的反余弦值。

**参数**：
- `value`：余弦值，范围在 [-1, 1] 之间

**返回值**：反余弦值，范围在 [0, π] 之间

**异常**：当输入值不在 [-1, 1] 范围内时，抛出 `ArgumentOutOfRangeException`

**实现原理**：`Atan(Sqrt(1 - value²) / value)`，负值加 π。

**示例**：

```csharp
var result = Fixed64.Acos(Fixed64.Zero); // π/2
var result2 = Fixed64.Acos(Fixed64.One);  // 0
```

### 正切 Tan

#### `static Fixed64 Tan(Fixed64 radian)`

```csharp
public static Fixed64 Tan(Fixed64 radian)
```

**功能**：计算指定弧度的正切值。使用泰勒级数展开（到 x²⁷ 项，共 13 个系数）。

**参数**：
- `radian`：弧度值

**返回值**：正切值

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`
- 如果输入是 0，返回 0
- 如果输入是 π/2，返回 `MaxValue`
- 如果输入是 -π/2，返回 `MinValue`
- 如果输入是 π/4，返回 1
- 其值越接近 ±π/2 误差越大
- 接近 ±π/2 时使用余切倒数提高精度

**示例**：

```csharp
var tan0 = Fixed64.Tan(Fixed64.Zero);         // 0
var tanPi4 = Fixed64.Tan(Fixed64.Quarter_PI); // 1
```

---

#### `static Fixed64 FastTan(Fixed64 radian)`

```csharp
public static Fixed64 FastTan(Fixed64 radian)
```

**功能**：使用查表法快速计算正切值。速度比 `Tan` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：正切值

**注意事项**：
- 误差大于 `Tan` 函数，但计算速度更快
- 查找表大小为 65537 项，使用 `Lazy<Int128[]>` 延迟加载，线性插值

**示例**：

```csharp
var result = Fixed64.FastTan(Fixed64.Quarter_PI); // ~1
```

---

#### `static Fixed64 Atan(Fixed64 z)`

```csharp
public static Fixed64 Atan(Fixed64 z)
```

**功能**：计算指定值的反正切值。

**参数**：
- `z`：正切值

**返回值**：反正切值，范围在 [-π/2, π/2] 之间

**注意事项**：
- 如果输入是 0，返回 0
- 大于 1 时取倒数计算，迭代最多 60 次

**示例**：

```csharp
var result = Fixed64.Atan(Fixed64.One); // π/4
```

---

#### `static Fixed64 Atan2(Fixed64 y, Fixed64 x)`

```csharp
public static Fixed64 Atan2(Fixed64 y, Fixed64 x)
```

**功能**：计算 y/x 的反正切值，考虑象限。

**参数**：
- `y`：y 坐标
- `x`：x 坐标

**返回值**：反正切值，范围在 [-π, π] 之间

**注意事项**：
- 如果 x 为 0 且 y > 0，返回 π/2
- 如果 x 为 0 且 y = 0，返回 0
- 如果 x 为 0 且 y < 0，返回 -π/2

**示例**：

```csharp
var result = Fixed64.Atan2(new Fixed64(1), new Fixed64(1)); // π/4
var result2 = Fixed64.Atan2(new Fixed64(1), new Fixed64(-1)); // 3π/4
```

## Unity 风格工具方法

### `static Fixed64 MoveTowards(Fixed64 current, Fixed64 target, Fixed64 maxDelta)`

```csharp
public static Fixed64 MoveTowards(Fixed64 current, Fixed64 target, Fixed64 maxDelta)
```

**功能**：将当前值向目标值移动，每次移动的最大距离为 `maxDelta`。

**参数**：
- `current`：当前值
- `target`：目标值
- `maxDelta`：最大移动距离

**返回值**：移动后的值

**注意事项**：如果当前值与目标值的差的绝对值小于等于 `maxDelta`，直接返回目标值。

**示例**：

```csharp
var current = new Fixed64(0);
var target = new Fixed64(10);
var maxDelta = new Fixed64(3);

var result1 = Fixed64.MoveTowards(current, target, maxDelta); // 3
var result2 = Fixed64.MoveTowards(new Fixed64(9), target, maxDelta); // 10（到达目标）
```

---

### `static Fixed64 MoveTowardsAngle(Fixed64 current, Fixed64 target, Fixed64 maxDelta)`

```csharp
public static Fixed64 MoveTowardsAngle(Fixed64 current, Fixed64 target, Fixed64 maxDelta)
```

**功能**：将当前角度向目标角度移动，考虑角度环绕。

**参数**：
- `current`：当前角度（度）
- `target`：目标角度（度）
- `maxDelta`：最大移动角度

**返回值**：移动后的角度

**示例**：

```csharp
var current = new Fixed64(350);
var target = new Fixed64(10);
var maxDelta = new Fixed64(5);

var result = Fixed64.MoveTowardsAngle(current, target, maxDelta); // 355（沿最短路径移动）
```

---

### `static Fixed64 Repeat(Fixed64 t, Fixed64 length)`

```csharp
public static Fixed64 Repeat(Fixed64 t, Fixed64 length)
```

**功能**：将值限制在 [0, length) 范围内，超出则循环。

**参数**：
- `t`：输入值
- `length`：范围长度

**返回值**：重复后的值，范围在 [0, length) 之间

**示例**：

```csharp
var result1 = Fixed64.Repeat(new Fixed64(5), new Fixed64(3)); // 2
var result2 = Fixed64.Repeat(new Fixed64(-1), new Fixed64(3)); // 2
var result3 = Fixed64.Repeat(new Fixed64(3), new Fixed64(3));  // 0
```

---

### `static Fixed64 DeltaAngle(Fixed64 current, Fixed64 target)`

```csharp
public static Fixed64 DeltaAngle(Fixed64 current, Fixed64 target)
```

**功能**：计算两个角度之间的最小差值。

**参数**：
- `current`：当前角度（度）
- `target`：目标角度（度）

**返回值**：角度差，范围在 [-180, 180] 之间

**示例**：

```csharp
var result1 = Fixed64.DeltaAngle(new Fixed64(10), new Fixed64(350)); // -20
var result2 = Fixed64.DeltaAngle(new Fixed64(10), new Fixed64(30));  // 20
```

---

### `static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed, Fixed64 deltaTime)`

```csharp
public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed, Fixed64 deltaTime)
```

**功能**：使用阻尼算法平滑地将一个值过渡到目标值。

**参数**：
- `current`：当前值
- `target`：目标值
- `currentVelocity`：当前速度（引用传递，会被修改）
- `smoothTime`：平滑时间，值越小过渡越快
- `maxSpeed`：最大速度
- `deltaTime`：时间步长

**返回值**：平滑过渡后的值

**注意事项**：`smoothTime` 会被钳制到不小于 `TPN4`（0.0001）。

**示例**：

```csharp
var current = new Fixed64(0);
var target = new Fixed64(100);
var velocity = Fixed64.Zero;
var smoothTime = new Fixed64(0.3);
var maxSpeed = Fixed64.MaxValue;
var deltaTime = new Fixed64(0.016); // ~60fps

// 每帧调用
current = Fixed64.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);
```

---

### `static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed)`

```csharp
public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed)
```

**功能**：平滑阻尼（使用默认时间步长 0.01）。

**参数**：
- `current`：当前值
- `target`：目标值
- `currentVelocity`：当前速度（引用传递，会被修改）
- `smoothTime`：平滑时间
- `maxSpeed`：最大速度

**返回值**：平滑过渡后的值

**示例**：

```csharp
var current = new Fixed64(0);
var target = new Fixed64(100);
var velocity = Fixed64.Zero;
current = Fixed64.SmoothDamp(current, target, ref velocity, new Fixed64(0.3), Fixed64.MaxValue);
```

---

### `static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime)`

```csharp
public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime)
```

**功能**：平滑阻尼（使用默认最大速度 `MinValue` 和默认时间步长 0.01）。

**参数**：
- `current`：当前值
- `target`：目标值
- `currentVelocity`：当前速度（引用传递，会被修改）
- `smoothTime`：平滑时间

**返回值**：平滑过渡后的值

**示例**：

```csharp
var current = new Fixed64(0);
var target = new Fixed64(100);
var velocity = Fixed64.Zero;
current = Fixed64.SmoothDamp(current, target, ref velocity, new Fixed64(0.3));
```

## 角度/弧度转换

### `static Fixed64 NormalizeRadian(Fixed64 radian)`

```csharp
public static Fixed64 NormalizeRadian(Fixed64 radian)
```

**功能**：将弧度值规范化到 [0, 2π] 范围内。

**参数**：
- `radian`：弧度值

**返回值**：规范化后的弧度值

**示例**：

```csharp
var result1 = Fixed64.NormalizeRadian(new Fixed64(7)); // ~0.7168（7 - 2π）
var result2 = Fixed64.NormalizeRadian(new Fixed64(-1)); // ~5.2832（2π - 1）
```

---

### `static Fixed64 DegreeToRadian(Fixed64 degree)`

```csharp
public static Fixed64 DegreeToRadian(Fixed64 degree)
```

**功能**：将角度值转换为弧度值。

**参数**：
- `degree`：角度值

**返回值**：对应的弧度值

**示例**：

```csharp
var result = Fixed64.DegreeToRadian(new Fixed64(180)); // π
var result2 = Fixed64.DegreeToRadian(new Fixed64(90));  // π/2
```

---

### `static Fixed64 RadianToDegree(Fixed64 radian)`

```csharp
public static Fixed64 RadianToDegree(Fixed64 radian)
```

**功能**：将弧度值转换为角度值。

**参数**：
- `radian`：弧度值

**返回值**：对应的角度值

**示例**：

```csharp
var result = Fixed64.RadianToDegree(Fixed64.PI); // 180
var result2 = Fixed64.RadianToDegree(Fixed64.Half_PI); // 90
```

## 插值

### `static Fixed64 Lerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)`

```csharp
public static Fixed64 Lerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
```

**功能**：在两个值之间进行线性插值。

**参数**：
- `value1`：起始值
- `value2`：结束值
- `amount`：插值因子，范围通常在 [0, 1] 之间

**返回值**：插值结果

**实现原理**：value1 + (value2 - value1) × amount

**注意事项**：`amount` 不受 [0, 1] 限制，超出范围会进行外推。

**示例**：

```csharp
var result = Fixed64.Lerp(new Fixed64(0), new Fixed64(10), new Fixed64(0.5)); // 5
var result2 = Fixed64.Lerp(new Fixed64(0), new Fixed64(10), Fixed64.One);      // 10
```

---

### `static Fixed64 ClampLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)`

```csharp
public static Fixed64 ClampLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
```

**功能**：在两个值之间进行钳制线性插值。将 `amount` 限制在 [0, 1] 范围内。

**参数**：
- `value1`：起始值
- `value2`：结束值
- `amount`：插值因子

**返回值**：插值结果，不会超出 [value1, value2] 范围

**示例**：

```csharp
var result = Fixed64.ClampLerp(new Fixed64(0), new Fixed64(10), new Fixed64(0.5)); // 5
var result2 = Fixed64.ClampLerp(new Fixed64(0), new Fixed64(10), new Fixed64(2));  // 10（钳制）
```

---

### `static Fixed64 InverseLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)`

```csharp
public static Fixed64 InverseLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
```

**功能**：逆线性插值，计算一个值在两个值之间的插值因子。

**参数**：
- `value1`：起始值
- `value2`：结束值
- `amount`：要计算插值因子的值

**返回值**：插值因子

**实现原理**：(amount - value1) / (value2 - value1)

**注意事项**：如果 value1 等于 value2，返回 0。

**示例**：

```csharp
var result = Fixed64.InverseLerp(new Fixed64(0), new Fixed64(10), new Fixed64(5)); // 0.5
var result2 = Fixed64.InverseLerp(new Fixed64(0), new Fixed64(10), new Fixed64(10)); // 1
```

---

### `static Fixed64 SmoothStep(Fixed64 value1, Fixed64 value2, Fixed64 amount)`

```csharp
public static Fixed64 SmoothStep(Fixed64 value1, Fixed64 value2, Fixed64 amount)
```

**功能**：在两个值之间进行平滑插值。使用三次平滑函数 3t² - 2t³ 处理插值因子。

**参数**：
- `value1`：起始值
- `value2`：结束值
- `amount`：插值因子，范围通常在 [0, 1] 之间

**返回值**：平滑插值结果

**实现原理**：
1. 将 `amount` 钳制到 [0, 1]
2. 使用三次平滑函数：3t² - 2t³
3. 进行线性插值

**示例**：

```csharp
var result = Fixed64.SmoothStep(new Fixed64(0), new Fixed64(10), new Fixed64(0.5)); // 5
// 在起止点附近变化缓慢，中间变化较快
```

## ToString

### `override string ToString()`

```csharp
public override string ToString()
```

**功能**：将定点数转换为字符串表示。

**返回值**：字符串表示

**特殊情况**：

| 值 | 输出 |
|----|------|
| `NaN` | `"NaN"` |
| 正无穷 | `"+∞"` |
| 负无穷 | `"-∞"` |
| 整数（无小数部分） | 整数字符串，如 `"42"` |
| 小数（有小数部分） | 浮点数字符串，如 `"3.14"` |

**示例**：

```csharp
var a = Fixed64.NaN;
Console.WriteLine(a.ToString()); // "NaN"

var b = Fixed64.PositiveInfinity;
Console.WriteLine(b.ToString()); // "+∞"

var c = new Fixed64(42);
Console.WriteLine(c.ToString()); // "42"

var d = new Fixed64(3.14);
Console.WriteLine(d.ToString()); // "3.14"
```

## 与 Fixed32 的关键差异

| 特性 | Fixed64 | Fixed32 |
|------|---------|---------|
| 存储类型 | `Int128` | `long` |
| 原始值类型 | `Int128` | `long` |
| 格式 | Q64.64 | Q32.32 |
| 总位宽 | 128 | 64 |
| 整数位宽 | 64 | 32 |
| 小数位宽 | 64 | 32 |
| 精度 | 约 5.42 × 10⁻²⁰（1/2⁶⁴） | 约 4.65 × 10⁻¹⁰（1/2³²） |
| 表示范围 | 约 ±9.22 × 10¹⁸ | 约 ±2.15 × 10⁹ |
| .NET 版本要求 | .NET 7+ | .NET Standard 兼容 |
| 乘法实现 | 分解四项乘积，小数×小数用 `UInt128` | 直接运算 |
| Sin 泰勒项数 | 13 项（C3~C27） | 较少项数 |
| Tan 泰勒项数 | 13 项（T3~T27） | 较少项数 |
| SinLut 大小 | 205888 项 | 较小 |
| TanLut 大小 | 65537 项 | 较小 |
| `long` 转换 | 显式，通过 `double` 中转 | 显式，直接构造 |
