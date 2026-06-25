# Fixed32

`Fixed32` 是一个 32 位定点数结构体，使用 64 位 `long` 存储，其中高 32 位为整数部分，低 32 位为小数部分（Q32.32 格式）。该结构体提供高精度的小数计算，避免浮点数的精度误差，适用于游戏逻辑、物理模拟等需要确定性计算的场景。

- **命名空间**：`SimplexLab.Fixed`
- **存储格式**：Q32.32（1 位符号 + 31 位整数 + 32 位小数）
- **精度**：约 4.65 × 10⁻¹⁰（即 1/2³²）
- **表示范围**：约 -2,147,483,648 ~ +2,147,483,647.999999999

## 结构定义

```csharp
public partial struct Fixed32 : IFixed<Fixed32>
```

`Fixed32` 实现了 `IFixed<Fixed32>` 接口，该接口继承自：
- `IMinMaxValue<Fixed32>` — 提供最大/最小值
- `IComparable` / `IComparable<Fixed32>` — 提供比较功能
- `IEquatable<Fixed32>` — 提供相等性判断

## 构造函数

### Fixed32()

```csharp
public Fixed32()
```

**功能**：创建值为 0 的定点数。

**示例**：

```csharp
var f = new Fixed32();
// f == Fixed32.Zero
```

---

### `Fixed32(int value)`

```csharp
public Fixed32(int value)
```

**功能**：从整数创建定点数。

**参数**：
- `value`：整数值

**示例**：

```csharp
var f = new Fixed32(42);
// f 表示定点数 42
```

---

### `Fixed32(float value)`

```csharp
public Fixed32(float value)
```

**功能**：从浮点数创建定点数。内部转换为 `double` 后处理。

**参数**：
- `value`：浮点数值

**示例**：

```csharp
var f = new Fixed32(3.14f);
```

---

### `Fixed32(double value)`

```csharp
public Fixed32(double value)
```

**功能**：从双精度浮点数创建定点数。将浮点数乘以小数部分乘数（2³²）并四舍五入，限制在有效范围内。

**参数**：
- `value`：双精度浮点数值

**示例**：

```csharp
var f = new Fixed32(3.14159);
```

---

### `Fixed32(Fixed32 other)`

```csharp
public Fixed32(Fixed32 other)
```

**功能**：从另一个定点数创建定点数（拷贝构造）。

**参数**：
- `other`：另一个定点数

**示例**：

```csharp
var a = new Fixed32(10);
var b = new Fixed32(a);
// b == a
```

## 常量属性

| 属性 | 类型 | 近似值 | 说明 |
|------|------|--------|------|
| `MaxValue` | `Fixed32` | ~2,147,483,647 | 定点数能表示的最大值 |
| `MinValue` | `Fixed32` | ~-2,147,483,648 | 定点数能表示的最小值 |
| `AdditiveIdentity` | `Fixed32` | 0 | 加法单位元，x + 0 = x |
| `MultiplicativeIdentity` | `Fixed32` | 1 | 乘法单位元，x × 1 = x |
| `Zero` | `Fixed32` | 0 | 零值 |
| `Half` | `Fixed32` | 0.5 | 二分之一 |
| `One` | `Fixed32` | 1 | 一 |
| `NegativeOne` | `Fixed32` | -1 | 负一 |
| `Two` | `Fixed32` | 2 | 二 |
| `Ln2` | `Fixed32` | ~0.6931 | 自然对数 ln(2) |
| `Ln10` | `Fixed32` | ~2.3026 | 自然对数 ln(10) |
| `NaN` | `Fixed32` | — | 非数字（Not a Number） |
| `Epsilon` | `Fixed32` | ~1.86 × 10⁻⁹ | 最小精度 |
| `E` | `Fixed32` | ~2.71828 | 自然常数 e |
| `PI` | `Fixed32` | ~3.14159 | 圆周率 π |
| `Half_PI` | `Fixed32` | ~1.5708 | π/2 |
| `Quarter_PI` | `Fixed32` | ~0.7854 | π/4 |
| `Two_PI` | `Fixed32` | ~6.28319 | 2π |
| `TPN1` | `Fixed32` | 0.1 | 10⁻¹ |
| `TPN2` | `Fixed32` | 0.01 | 10⁻² |
| `TPN3` | `Fixed32` | 0.001 | 10⁻³ |
| `TPN4` | `Fixed32` | 0.0001 | 10⁻⁴ |
| `N180` | `Fixed32` | 180 | 180 度，用于角度转换 |
| `N360` | `Fixed32` | 360 | 360 度，用于角度转换 |
| `PositiveInfinity` | `Fixed32` | +∞ | 正无穷大 |
| `NegativeInfinity` | `Fixed32` | -∞ | 负无穷大 |
| `DegToRad` | `Fixed32` | ~0.01745 | 角度转弧度系数（π/180） |
| `RadToDeg` | `Fixed32` | ~57.29578 | 弧度转角度系数（180/π） |

**示例**：

```csharp
var pi = Fixed32.PI;
var halfPi = Fixed32.Half_PI;
var degToRad = Fixed32.DegToRad;

// NaN 检测
var nan = Fixed32.NaN;
Console.WriteLine(nan.IsNaN()); // True
```

**注意事项**：
- `NaN` 的内部原始值为 `long.MinValue`（`0x8000000000000000`），任何与 `NaN` 的运算结果均为 `NaN`。
- `PositiveInfinity` 的内部原始值为 `long.MaxValue`（`0x7FFFFFFFFFFFFFFF`）。
- `NegativeInfinity` 的内部原始值为 `long.MinValue + 1`（`0x8000000000000001`）。
- `Epsilon` 的内部原始值为 8，表示定点数比较时的精度阈值。

## 类型转换

### 隐式转换（从其他类型到 Fixed32）

#### `implicit operator Fixed32(byte n)`

```csharp
public static implicit operator Fixed32(byte n)
```

**功能**：将 `byte` 隐式转换为 `Fixed32`。

**参数**：
- `n`：byte 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed32 f = (byte)42; // 隐式转换
```

---

#### `implicit operator Fixed32(short n)`

```csharp
public static implicit operator Fixed32(short n)
```

**功能**：将 `short` 隐式转换为 `Fixed32`。

**参数**：
- `n`：short 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed32 f = (short)-100; // 隐式转换
```

---

#### `implicit operator Fixed32(int n)`

```csharp
public static implicit operator Fixed32(int n)
```

**功能**：将 `int` 隐式转换为 `Fixed32`。

**参数**：
- `n`：int 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed32 f = 42; // 隐式转换，最常用
```

### 显式转换（从其他类型到 Fixed32）

#### `explicit operator Fixed32(long n)`

```csharp
public static explicit operator Fixed32(long n)
```

**功能**：将 `long` 显式转换为 `Fixed32`。

**参数**：
- `n`：long 值

**返回值**：对应的定点数

**注意事项**：`long` 范围可能超出 `Fixed32` 的表示范围，超出部分会被钳制。

**示例**：

```csharp
Fixed32 f = (Fixed32)123456789L;
```

---

#### `explicit operator Fixed32(float n)`

```csharp
public static explicit operator Fixed32(float n)
```

**功能**：将 `float` 显式转换为 `Fixed32`。

**参数**：
- `n`：float 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed32 f = (Fixed32)3.14f;
```

---

#### `explicit operator Fixed32(double n)`

```csharp
public static explicit operator Fixed32(double n)
```

**功能**：将 `double` 显式转换为 `Fixed32`。

**参数**：
- `n`：double 值

**返回值**：对应的定点数

**示例**：

```csharp
Fixed32 f = (Fixed32)3.14159265358979;
```

### 显式转换（从 Fixed32 到其他类型）

#### `explicit operator byte(Fixed32 n)`

```csharp
public static explicit operator byte(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `byte`。

**参数**：
- `n`：定点数

**返回值**：转换后的 byte 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed32(42);
byte b = (byte)f; // 42
```

---

#### `explicit operator short(Fixed32 n)`

```csharp
public static explicit operator short(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `short`。

**参数**：
- `n`：定点数

**返回值**：转换后的 short 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed32(-100);
short s = (short)f; // -100
```

---

#### `explicit operator int(Fixed32 n)`

```csharp
public static explicit operator int(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `int`。

**参数**：
- `n`：定点数

**返回值**：转换后的 int 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed32(3.14);
int i = (int)f; // 3（截断小数部分）
```

---

#### `explicit operator long(Fixed32 n)`

```csharp
public static explicit operator long(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `long`。

**参数**：
- `n`：定点数

**返回值**：转换后的 long 值

**注意事项**：如果值为 `NaN`，返回 0。

**示例**：

```csharp
var f = new Fixed32(42);
long l = (long)f; // 42
```

---

#### `explicit operator float(Fixed32 n)`

```csharp
public static explicit operator float(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `float`。

**参数**：
- `n`：定点数

**返回值**：转换后的 float 值

**注意事项**：如果值为 `NaN`，返回 `float.NaN`。

**示例**：

```csharp
var f = new Fixed32(3.14);
float fl = (float)f; // ~3.14
```

---

#### `explicit operator double(Fixed32 n)`

```csharp
public static explicit operator double(Fixed32 n)
```

**功能**：将 `Fixed32` 显式转换为 `double`。

**参数**：
- `n`：定点数

**返回值**：转换后的 double 值

**注意事项**：
- 如果值为 `NaN`，返回 `double.NaN`
- 如果值为正无穷，返回 `double.PositiveInfinity`
- 如果值为负无穷，返回 `double.NegativeInfinity`

**示例**：

```csharp
var f = new Fixed32(3.14);
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
var f = new Fixed32(42);
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
var f = new Fixed32(-100);
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
var f = new Fixed32(3.14);
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
var f = new Fixed32(42);
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
var f = new Fixed32(3.14);
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
var f = new Fixed32(3.14);
double d = f.ToDouble(); // ~3.14

var inf = Fixed32.PositiveInfinity;
double dInf = inf.ToDouble(); // double.PositiveInfinity
```

---

#### `static Fixed32 FromRaw(long value)`

```csharp
public static Fixed32 FromRaw(long value)
```

**功能**：从原始内部值创建定点数。直接使用给定的 64 位原始值作为内部存储。

**参数**：
- `value`：原始内部值（Q32.32 格式的 64 位整数）

**返回值**：创建的定点数

**注意事项**：此方法绕过常规的类型转换和范围检查，应谨慎使用。

**示例**：

```csharp
// 创建值为 1.0 的定点数（1 << 32 = 4294967296）
var one = Fixed32.FromRaw(4294967296L);
```

---

#### `static long ToRaw(Fixed32 value)`

```csharp
public static long ToRaw(Fixed32 value)
```

**功能**：获取定点数的原始内部存储值。

**参数**：
- `value`：定点数

**返回值**：原始内部值（Q32.32 格式的 64 位整数）

**示例**：

```csharp
var f = new Fixed32(1);
long raw = Fixed32.ToRaw(f); // 4294967296 (即 1 << 32)
```

---

#### `Fixed32 Integral()`

```csharp
public Fixed32 Integral()
```

**功能**：获取当前定点数的整数部分（截断小数部分）。

**返回值**：整数部分的定点数

**注意事项**：如果当前值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed32(3.14);
var integral = f.Integral(); // 3
```

---

#### `static Fixed32 Integral(Fixed32 n)`

```csharp
public static Fixed32 Integral(Fixed32 n)
```

**功能**：获取指定定点数的整数部分。

**参数**：
- `n`：定点数

**返回值**：整数部分的定点数

**注意事项**：如果输入值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed32(-3.14);
var integral = Fixed32.Integral(f); // -3
```

---

#### `Fixed32 Fractional()`

```csharp
public Fixed32 Fractional()
```

**功能**：获取当前定点数的小数部分。

**返回值**：小数部分的定点数

**注意事项**：如果当前值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed32(3.14);
var frac = f.Fractional(); // ~0.14
```

---

#### `static Fixed32 Fractional(Fixed32 n)`

```csharp
public static Fixed32 Fractional(Fixed32 n)
```

**功能**：获取指定定点数的小数部分。

**参数**：
- `n`：定点数

**返回值**：小数部分的定点数

**注意事项**：如果输入值是 `NaN`，返回 `NaN`。

**示例**：

```csharp
var f = new Fixed32(-3.14);
var frac = Fixed32.Fractional(f); // ~0.14（始终为非负）
```

## 算术运算符

### 加法 `operator +`

#### `operator +(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 operator +(Fixed32 a, Fixed32 b)
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
var a = new Fixed32(3);
var b = new Fixed32(4);
var c = a + b; // 7
```

---

#### `operator +(Fixed32 a, int b)`

```csharp
public static Fixed32 operator +(Fixed32 a, int b)
```

**功能**：定点数加整数。

**参数**：
- `a`：定点数
- `b`：整数

**返回值**：相加后的结果

**示例**：

```csharp
var a = new Fixed32(3.5);
var c = a + 2; // 5.5
```

---

#### `operator +(int a, Fixed32 b)`

```csharp
public static Fixed32 operator +(int a, Fixed32 b)
```

**功能**：整数加定点数。

**参数**：
- `a`：整数
- `b`：定点数

**返回值**：相加后的结果

**示例**：

```csharp
var b = new Fixed32(3.5);
var c = 2 + b; // 5.5
```

### 减法 `operator -`

#### `operator -(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 operator -(Fixed32 a, Fixed32 b)
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
var a = new Fixed32(10);
var b = new Fixed32(3);
var c = a - b; // 7
```

---

#### `operator -(Fixed32 a, int b)`

```csharp
public static Fixed32 operator -(Fixed32 a, int b)
```

**功能**：定点数减整数。

**参数**：
- `a`：被减数（定点数）
- `b`：减数（整数）

**返回值**：相减后的结果

**示例**：

```csharp
var a = new Fixed32(5.5);
var c = a - 2; // 3.5
```

---

#### `operator -(int a, Fixed32 b)`

```csharp
public static Fixed32 operator -(int a, Fixed32 b)
```

**功能**：整数减定点数。

**参数**：
- `a`：被减数（整数）
- `b`：减数（定点数）

**返回值**：相减后的结果

**示例**：

```csharp
var b = new Fixed32(3.5);
var c = 10 - b; // 6.5
```

### 乘法 `operator *`

#### `operator *(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 operator *(Fixed32 a, Fixed32 b)
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

**示例**：

```csharp
var a = new Fixed32(3);
var b = new Fixed32(4);
var c = a * b; // 12
```

---

#### `operator *(Fixed32 a, int b)`

```csharp
public static Fixed32 operator *(Fixed32 a, int b)
```

**功能**：定点数乘整数。

**参数**：
- `a`：定点数
- `b`：整数

**返回值**：相乘后的结果

**示例**：

```csharp
var a = new Fixed32(3.5);
var c = a * 2; // 7
```

---

#### `operator *(int a, Fixed32 b)`

```csharp
public static Fixed32 operator *(int a, Fixed32 b)
```

**功能**：整数乘定点数。

**参数**：
- `a`：整数
- `b`：定点数

**返回值**：相乘后的结果

**示例**：

```csharp
var b = new Fixed32(3.5);
var c = 2 * b; // 7
```

### 除法 `operator /`

#### `operator /(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 operator /(Fixed32 a, Fixed32 b)
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

**示例**：

```csharp
var a = new Fixed32(12);
var b = new Fixed32(4);
var c = a / b; // 3
```

---

#### `operator /(Fixed32 a, int b)`

```csharp
public static Fixed32 operator /(Fixed32 a, int b)
```

**功能**：定点数除以整数。

**参数**：
- `a`：被除数
- `b`：除数（整数）

**返回值**：相除后的结果

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var a = new Fixed32(7);
var c = a / 2; // 3.5
```

---

#### `operator /(int a, Fixed32 b)`

```csharp
public static Fixed32 operator /(int a, Fixed32 b)
```

**功能**：整数除以定点数。

**参数**：
- `a`：被除数（整数）
- `b`：除数

**返回值**：相除后的结果

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var b = new Fixed32(2);
var c = 7 / b; // 3.5
```

### 取模 `operator %`

#### `operator %(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 operator %(Fixed32 a, Fixed32 b)
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
var a = new Fixed32(7);
var b = new Fixed32(3);
var c = a % b; // 1
```

---

#### `operator %(Fixed32 a, int b)`

```csharp
public static Fixed32 operator %(Fixed32 a, int b)
```

**功能**：定点数对整数取模。

**参数**：
- `a`：被除数
- `b`：除数（整数）

**返回值**：余数

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var a = new Fixed32(7);
var c = a % 3; // 1
```

---

#### `operator %(int a, Fixed32 b)`

```csharp
public static Fixed32 operator %(int a, Fixed32 b)
```

**功能**：整数对定点数取模。

**参数**：
- `a`：被除数（整数）
- `b`：除数

**返回值**：余数

**异常**：当除数为 0 时，抛出 `DivideByZeroException`

**示例**：

```csharp
var b = new Fixed32(3);
var c = 7 % b; // 1
```

### 取反 `operator -`

#### `operator -(Fixed32 n)`

```csharp
public static Fixed32 operator -(Fixed32 n)
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
var a = new Fixed32(3);
var b = -a; // -3
```

## 比较运算符

### `operator ==`

```csharp
public static bool operator ==(Fixed32 a, Fixed32 b)
```

**功能**：判断两个定点数是否相等。

**返回值**：如果相等返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed32(3);
var b = new Fixed32(3);
bool equal = a == b; // true
```

---

### `operator !=`

```csharp
public static bool operator !=(Fixed32 a, Fixed32 b)
```

**功能**：判断两个定点数是否不相等。

**返回值**：如果不相等返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `true`。

**示例**：

```csharp
var a = new Fixed32(3);
var b = new Fixed32(4);
bool notEqual = a != b; // true
```

---

### `operator >`

```csharp
public static bool operator >(Fixed32 a, Fixed32 b)
```

**功能**：判断第一个定点数是否大于第二个。

**返回值**：如果大于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed32(5);
var b = new Fixed32(3);
bool greater = a > b; // true
```

---

### `operator <`

```csharp
public static bool operator <(Fixed32 a, Fixed32 b)
```

**功能**：判断第一个定点数是否小于第二个。

**返回值**：如果小于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed32(3);
var b = new Fixed32(5);
bool less = a < b; // true
```

---

### `operator >=`

```csharp
public static bool operator >=(Fixed32 a, Fixed32 b)
```

**功能**：判断第一个定点数是否大于等于第二个。

**返回值**：如果大于等于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed32(5);
var b = new Fixed32(5);
bool ge = a >= b; // true
```

---

### `operator <=`

```csharp
public static bool operator <=(Fixed32 a, Fixed32 b)
```

**功能**：判断第一个定点数是否小于等于第二个。

**返回值**：如果小于等于返回 `true`，否则返回 `false`

**注意事项**：如果任何一个操作数是 `NaN`，返回 `false`。

**示例**：

```csharp
var a = new Fixed32(5);
var b = new Fixed32(5);
bool le = a <= b; // true
```

## 数学方法

### 绝对值

#### `Fixed32 Abs()`

```csharp
public Fixed32 Abs()
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
var a = new Fixed32(-3);
var b = a.Abs(); // 3
```

---

#### `static Fixed32 Abs(Fixed32 n)`

```csharp
public static Fixed32 Abs(Fixed32 n)
```

**功能**：获取指定定点数的绝对值。

**参数**：
- `n`：要计算绝对值的定点数

**返回值**：绝对值

**示例**：

```csharp
var result = Fixed32.Abs(new Fixed32(-5)); // 5
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
var a = new Fixed32(-3);
int s = a.Sign(); // -1

var b = Fixed32.Zero;
int s2 = b.Sign(); // 0
```

---

#### `static int Sign(Fixed32 n)`

```csharp
public static int Sign(Fixed32 n)
```

**功能**：获取指定定点数的符号。

**参数**：
- `n`：要获取符号的定点数

**返回值**：正数返回 1，负数返回 -1，零返回 0

**异常**：如果输入值是 `NaN`，抛出 `ArithmeticException`

**示例**：

```csharp
int s = Fixed32.Sign(new Fixed32(42)); // 1
```

---

#### `static bool IsSigns(Fixed32 a, Fixed32 b)`

```csharp
public static bool IsSigns(Fixed32 a, Fixed32 b)
```

**功能**：判断两个定点数的符号是否相同。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：如果符号相同返回 `true`，否则返回 `false`

**示例**：

```csharp
var a = new Fixed32(3);
var b = new Fixed32(5);
bool same = Fixed32.IsSigns(a, b); // true

var c = new Fixed32(-3);
bool diff = Fixed32.IsSigns(a, c); // false
```

### 平方根

#### `Fixed32 Sqrt()`

```csharp
public Fixed32 Sqrt()
```

**功能**：计算当前定点数的平方根。使用二进制搜索算法。

**返回值**：平方根值

**注意事项**：
- 如果当前值是 `NaN`，返回 `NaN`
- 如果当前值是正无穷，返回正无穷
- 如果当前值是 0，返回 0
- 如果当前值是负数，返回 `NaN`（非抛出异常）

**示例**：

```csharp
var a = new Fixed32(16);
var b = a.Sqrt(); // 4

var c = new Fixed32(2);
var d = c.Sqrt(); // ~1.4142
```

---

#### `static Fixed32 Sqrt(Fixed32 n)`

```csharp
public static Fixed32 Sqrt(Fixed32 n)
```

**功能**：计算指定定点数的平方根。

**参数**：
- `n`：要计算平方根的定点数

**返回值**：平方根值

**示例**：

```csharp
var result = Fixed32.Sqrt(new Fixed32(9)); // 3
```

### 立方根

#### `Fixed32 Cbrt()`

```csharp
public Fixed32 Cbrt()
```

**功能**：计算当前定点数的立方根。使用公式 ∛x = e^(ln(x)/3)。

**返回值**：立方根值

**注意事项**：
- 如果当前值是 `NaN`，返回 `NaN`
- 如果当前值是 0，返回 0
- 支持负数的立方根：∛(-x) = -∛(x)

**示例**：

```csharp
var a = new Fixed32(27);
var b = a.Cbrt(); // 3

var c = new Fixed32(-8);
var d = c.Cbrt(); // -2
```

---

#### `static Fixed32 Cbrt(Fixed32 n)`

```csharp
public static Fixed32 Cbrt(Fixed32 n)
```

**功能**：计算指定定点数的立方根。

**参数**：
- `n`：要计算立方根的定点数

**返回值**：立方根值

**示例**：

```csharp
var result = Fixed32.Cbrt(new Fixed32(8)); // 2
```

### 幂运算

#### `Fixed32 Pow(int n)`

```csharp
public Fixed32 Pow(int n)
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
var a = new Fixed32(2);
var b = a.Pow(10); // 1024

var c = new Fixed32(3);
var d = c.Pow(-1); // ~0.3333（即 1/3）
```

---

#### `Fixed32 Pow(Fixed32 n)`

```csharp
public Fixed32 Pow(Fixed32 n)
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
var a = new Fixed32(4);
var b = a.Pow(new Fixed32(0.5)); // 2（即 √4）

var c = new Fixed32(2);
var d = c.Pow(new Fixed32(3)); // 8
```

---

#### `static Fixed32 Pow(Fixed32 m, int n)`

```csharp
public static Fixed32 Pow(Fixed32 m, int n)
```

**功能**：计算指定定点数的整数次幂。

**参数**：
- `m`：底数
- `n`：指数

**返回值**：幂值

**示例**：

```csharp
var result = Fixed32.Pow(new Fixed32(2), 8); // 256
```

---

#### `static Fixed32 Pow(Fixed32 m, Fixed32 n)`

```csharp
public static Fixed32 Pow(Fixed32 m, Fixed32 n)
```

**功能**：计算指定底数的指定次幂。

**参数**：
- `m`：底数
- `n`：指数

**返回值**：幂值

**示例**：

```csharp
var result = Fixed32.Pow(new Fixed32(9), new Fixed32(0.5)); // 3
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
var a = new Fixed32(4);
bool isPow = a.IsPowerOfTwo(); // true

var b = new Fixed32(3);
bool isPow2 = b.IsPowerOfTwo(); // false
```

---

#### `static bool IsPowerOfTwo(Fixed32 value)`

```csharp
public static bool IsPowerOfTwo(Fixed32 value)
```

**功能**：判断指定定点数是否为 2 的幂。

**参数**：
- `value`：要判断的定点数

**返回值**：如果是 2 的幂返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsPowerOfTwo(new Fixed32(8)); // true
```

---

#### `Fixed32 ClosestPowerOfTwo()`

```csharp
public Fixed32 ClosestPowerOfTwo()
```

**功能**：计算最接近当前定点数的 2 的幂。

**返回值**：最接近的 2 的幂

**注意事项**：非正数返回 1。

**示例**：

```csharp
var a = new Fixed32(5);
var b = a.ClosestPowerOfTwo(); // 4

var c = new Fixed32(6);
var d = c.ClosestPowerOfTwo(); // 8
```

---

#### `static Fixed32 ClosestPowerOfTwo(Fixed32 value)`

```csharp
public static Fixed32 ClosestPowerOfTwo(Fixed32 value)
```

**功能**：计算最接近指定定点数的 2 的幂。

**参数**：
- `value`：要计算的定点数

**返回值**：最接近的 2 的幂

**示例**：

```csharp
var result = Fixed32.ClosestPowerOfTwo(new Fixed32(5)); // 4
```

---

#### `Fixed32 NextPowerOfTwo()`

```csharp
public Fixed32 NextPowerOfTwo()
```

**功能**：计算大于当前定点数的最小 2 的幂。

**返回值**：下一个 2 的幂

**示例**：

```csharp
var a = new Fixed32(5);
var b = a.NextPowerOfTwo(); // 8
```

---

#### `static Fixed32 NextPowerOfTwo(Fixed32 value)`

```csharp
public static Fixed32 NextPowerOfTwo(Fixed32 value)
```

**功能**：计算大于指定定点数的最小 2 的幂。

**参数**：
- `value`：要计算的定点数

**返回值**：下一个 2 的幂

**示例**：

```csharp
var result = Fixed32.NextPowerOfTwo(new Fixed32(5)); // 8
```

### 指数函数

#### `Fixed32 Exp()`

```csharp
public Fixed32 Exp()
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
3. 计算 2^k × e^r 得到最终结果

**示例**：

```csharp
var a = new Fixed32(1);
var b = a.Exp(); // ~2.71828（即 e）

var c = new Fixed32(0);
var d = c.Exp(); // 1
```

---

#### `static Fixed32 Exp(Fixed32 m)`

```csharp
public static Fixed32 Exp(Fixed32 m)
```

**功能**：计算 e 的指定定点数次幂。

**参数**：
- `m`：指数

**返回值**：e^m 的值

**示例**：

```csharp
var result = Fixed32.Exp(new Fixed32(2)); // ~7.38906（即 e²）
```

### 对数函数

#### `Fixed32 Log()`

```csharp
public Fixed32 Log()
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
var a = new Fixed32(2.71828);
var b = a.Log(); // ~1.0（即 ln(e)）

var c = new Fixed32(1);
var d = c.Log(); // 0
```

---

#### `static Fixed32 Log(Fixed32 n)`

```csharp
public static Fixed32 Log(Fixed32 n)
```

**功能**：计算指定定点数的自然对数。

**参数**：
- `n`：要计算自然对数的定点数

**返回值**：自然对数值

**示例**：

```csharp
var result = Fixed32.Log(new Fixed32(10)); // ~2.3026
```

---

#### `Fixed32 Log2()`

```csharp
public Fixed32 Log2()
```

**功能**：计算当前定点数的以 2 为底的对数。

**返回值**：以 2 为底的对数值

**注意事项**：
- 如果输入是 `NaN` 或负数，返回 `NaN`
- 如果输入是 0，返回负无穷
- 如果输入是正无穷，返回正无穷

**实现原理**：归一化到 [1, 2) 区间，使用二进制搜索算法计算对数的小数部分。

**示例**：

```csharp
var a = new Fixed32(8);
var b = a.Log2(); // 3

var c = new Fixed32(1);
var d = c.Log2(); // 0
```

---

#### `static Fixed32 Log2(Fixed32 n)`

```csharp
public static Fixed32 Log2(Fixed32 n)
```

**功能**：计算指定定点数的以 2 为底的对数。

**参数**：
- `n`：要计算对数的定点数

**返回值**：以 2 为底的对数值

**示例**：

```csharp
var result = Fixed32.Log2(new Fixed32(16)); // 4
```

---

#### `Fixed32 Log10()`

```csharp
public Fixed32 Log10()
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
var a = new Fixed32(100);
var b = a.Log10(); // 2

var c = new Fixed32(1000);
var d = c.Log10(); // 3
```

---

#### `static Fixed32 Log10(Fixed32 n)`

```csharp
public static Fixed32 Log10(Fixed32 n)
```

**功能**：计算指定定点数的以 10 为底的对数。

**参数**：
- `n`：要计算对数的定点数

**返回值**：以 10 为底的对数值

**示例**：

```csharp
var result = Fixed32.Log10(new Fixed32(100)); // 2
```

### 倒数

#### `Fixed32 Reciprocal()`

```csharp
public Fixed32 Reciprocal()
```

**功能**：计算当前定点数的倒数（1/x）。

**返回值**：倒数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是 0，返回正无穷
- 如果输入是无穷大，返回 0

**示例**：

```csharp
var a = new Fixed32(4);
var b = a.Reciprocal(); // 0.25

var c = new Fixed32(0.5);
var d = c.Reciprocal(); // 2
```

---

#### `static Fixed32 Reciprocal(Fixed32 n)`

```csharp
public static Fixed32 Reciprocal(Fixed32 n)
```

**功能**：计算指定定点数的倒数。

**参数**：
- `n`：要计算倒数的定点数

**返回值**：倒数

**示例**：

```csharp
var result = Fixed32.Reciprocal(new Fixed32(5)); // 0.2
```

## 取整方法

### 四舍五入

#### `Fixed32 Round()`

```csharp
public Fixed32 Round()
```

**功能**：将当前定点数四舍五入到最近的整数。

**返回值**：四舍五入后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷，返回正无穷
- 如果输入是负无穷，返回负无穷
- 小数部分等于 0.5 时，向偶数取整（银行家舍入法）

**示例**：

```csharp
var a = new Fixed32(3.5);
var b = a.Round(); // 4

var c = new Fixed32(2.5);
var d = c.Round(); // 2（向偶数取整）

var e = new Fixed32(3.4);
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
var a = new Fixed32(3.7);
int b = a.RoundToInt(); // 4
```

---

#### `static Fixed32 Round(Fixed32 n)`

```csharp
public static Fixed32 Round(Fixed32 n)
```

**功能**：将指定定点数四舍五入到最近的整数。

**参数**：
- `n`：要四舍五入的定点数

**返回值**：四舍五入后的定点数

**示例**：

```csharp
var result = Fixed32.Round(new Fixed32(3.6)); // 4
```

---

#### `static int RoundToInt(Fixed32 n)`

```csharp
public static int RoundToInt(Fixed32 n)
```

**功能**：将指定定点数四舍五入到最近的整数，并返回 `int` 类型。

**参数**：
- `n`：要四舍五入的定点数

**返回值**：四舍五入后的整数值

**示例**：

```csharp
int result = Fixed32.RoundToInt(new Fixed32(3.6)); // 4
```

### 向下取整

#### `Fixed32 Floor()`

```csharp
public Fixed32 Floor()
```

**功能**：返回小于或等于当前定点数的最大整数。

**返回值**：向下取整后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是负无穷，返回负无穷
- 如果输入是正无穷，返回正无穷

**示例**：

```csharp
var a = new Fixed32(3.7);
var b = a.Floor(); // 3

var c = new Fixed32(-3.2);
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
var a = new Fixed32(3.7);
int b = a.FloorToInt(); // 3
```

---

#### `static Fixed32 Floor(Fixed32 n)`

```csharp
public static Fixed32 Floor(Fixed32 n)
```

**功能**：返回小于或等于指定定点数的最大整数。

**参数**：
- `n`：要向下取整的定点数

**返回值**：向下取整后的定点数

**示例**：

```csharp
var result = Fixed32.Floor(new Fixed32(3.7)); // 3
```

---

#### `static int FloorToInt(Fixed32 n)`

```csharp
public static int FloorToInt(Fixed32 n)
```

**功能**：返回小于或等于指定定点数的最大整数，并返回 `int` 类型。

**参数**：
- `n`：要向下取整的定点数

**返回值**：向下取整后的整数值

**示例**：

```csharp
int result = Fixed32.FloorToInt(new Fixed32(3.7)); // 3
```

### 向上取整

#### `Fixed32 Ceil()`

```csharp
public Fixed32 Ceil()
```

**功能**：返回大于或等于当前定点数的最小整数。

**返回值**：向上取整后的定点数

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷，返回正无穷
- 如果没有小数部分，直接返回原值

**示例**：

```csharp
var a = new Fixed32(3.2);
var b = a.Ceil(); // 4

var c = new Fixed32(-3.7);
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
var a = new Fixed32(3.2);
int b = a.CeilToInt(); // 4
```

---

#### `static Fixed32 Ceil(Fixed32 n)`

```csharp
public static Fixed32 Ceil(Fixed32 n)
```

**功能**：返回大于或等于指定定点数的最小整数。

**参数**：
- `n`：要向上取整的定点数

**返回值**：向上取整后的定点数

**示例**：

```csharp
var result = Fixed32.Ceil(new Fixed32(3.2)); // 4
```

---

#### `static int CeilToInt(Fixed32 n)`

```csharp
public static int CeilToInt(Fixed32 n)
```

**功能**：返回大于或等于指定定点数的最小整数，并返回 `int` 类型。

**参数**：
- `n`：要向上取整的定点数

**返回值**：向上取整后的整数值

**示例**：

```csharp
int result = Fixed32.CeilToInt(new Fixed32(3.2)); // 4
```

## 最值与限制

### 最小值 Min

#### `static Fixed32 Min(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 Min(Fixed32 a, Fixed32 b)
```

**功能**：返回两个定点数中的较小值。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：两个数中的较小值

**示例**：

```csharp
var result = Fixed32.Min(new Fixed32(3), new Fixed32(5)); // 3
```

---

#### `static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c)`

```csharp
public static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c)
```

**功能**：返回三个定点数中的最小值。

**参数**：
- `a`、`b`、`c`：三个定点数

**返回值**：三个数中的最小值

**示例**：

```csharp
var result = Fixed32.Min(new Fixed32(3), new Fixed32(1), new Fixed32(5)); // 1
```

---

#### `static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c, Fixed32 d)`

```csharp
public static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c, Fixed32 d)
```

**功能**：返回四个定点数中的最小值。

**参数**：
- `a`、`b`、`c`、`d`：四个定点数

**返回值**：四个数中的最小值

**示例**：

```csharp
var result = Fixed32.Min(new Fixed32(3), new Fixed32(1), new Fixed32(5), new Fixed32(2)); // 1
```

---

#### `static Fixed32 Min(params Fixed32[] fixeds)`

```csharp
public static Fixed32 Min(params Fixed32[] fixeds)
```

**功能**：返回多个定点数中的最小值。

**参数**：
- `fixeds`：定点数数组

**返回值**：多个数中的最小值

**异常**：如果数组为空，抛出 `ArgumentException`

**注意事项**：如果数组包含 `NaN`，返回 `NaN`。

**示例**：

```csharp
var result = Fixed32.Min(new Fixed32(3), new Fixed32(1), new Fixed32(5), new Fixed32(2), new Fixed32(4)); // 1
```

### 最大值 Max

#### `static Fixed32 Max(Fixed32 a, Fixed32 b)`

```csharp
public static Fixed32 Max(Fixed32 a, Fixed32 b)
```

**功能**：返回两个定点数中的较大值。

**参数**：
- `a`：第一个定点数
- `b`：第二个定点数

**返回值**：两个数中的较大值

**示例**：

```csharp
var result = Fixed32.Max(new Fixed32(3), new Fixed32(5)); // 5
```

---

#### `static Fixed32 Max(Fixed32 a, Fixed32 b, Fixed32 c)`

```csharp
public static Fixed32 Max(Fixed32 a, Fixed32 b, Fixed32 c)
```

**功能**：返回三个定点数中的最大值。

**参数**：
- `a`、`b`、`c`：三个定点数

**返回值**：三个数中的最大值

**示例**：

```csharp
var result = Fixed32.Max(new Fixed32(3), new Fixed32(1), new Fixed32(5)); // 5
```

---

#### `static Fixed32 Max(Fixed32 a, Fixed32 b, Fixed32 c, Fixed32 d)`

```csharp
public static Fixed32 Max(Fixed32 a, Fixed32 b, Fixed32 c, Fixed32 d)
```

**功能**：返回四个定点数中的最大值。

**参数**：
- `a`、`b`、`c`、`d`：四个定点数

**返回值**：四个数中的最大值

**示例**：

```csharp
var result = Fixed32.Max(new Fixed32(3), new Fixed32(1), new Fixed32(5), new Fixed32(2)); // 5
```

---

#### `static Fixed32 Max(params Fixed32[] fixeds)`

```csharp
public static Fixed32 Max(params Fixed32[] fixeds)
```

**功能**：返回多个定点数中的最大值。

**参数**：
- `fixeds`：定点数数组

**返回值**：多个数中的最大值

**异常**：如果数组为空，抛出 `ArgumentException`

**注意事项**：如果数组包含 `NaN`，返回 `NaN`。

**示例**：

```csharp
var result = Fixed32.Max(new Fixed32(3), new Fixed32(1), new Fixed32(5), new Fixed32(2), new Fixed32(4)); // 5
```

### 范围限制

#### `static Fixed32 Clamp(Fixed32 value, Fixed32 min, Fixed32 max)`

```csharp
public static Fixed32 Clamp(Fixed32 value, Fixed32 min, Fixed32 max)
```

**功能**：将定点数限制在指定的最小值和最大值之间。

**参数**：
- `value`：要限制的定点数
- `min`：最小值
- `max`：最大值

**返回值**：限制在 [min, max] 范围内的结果

**示例**：

```csharp
var result1 = Fixed32.Clamp(new Fixed32(5), new Fixed32(0), new Fixed32(10)); // 5
var result2 = Fixed32.Clamp(new Fixed32(-3), new Fixed32(0), new Fixed32(10)); // 0
var result3 = Fixed32.Clamp(new Fixed32(15), new Fixed32(0), new Fixed32(10)); // 10
```

---

#### `static Fixed32 Clamp01(Fixed32 value)`

```csharp
public static Fixed32 Clamp01(Fixed32 value)
```

**功能**：将定点数限制在 [0, 1] 范围内。

**参数**：
- `value`：要限制的定点数

**返回值**：限制在 [0, 1] 范围内的结果

**示例**：

```csharp
var result1 = Fixed32.Clamp01(new Fixed32(0.5)); // 0.5
var result2 = Fixed32.Clamp01(new Fixed32(-1));  // 0
var result3 = Fixed32.Clamp01(new Fixed32(2));   // 1
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
var a = Fixed32.NaN;
bool isNan = a.IsNaN(); // true

var b = new Fixed32(3);
bool isNan2 = b.IsNaN(); // false
```

---

### `static bool IsNaN(Fixed32 value)`

```csharp
public static bool IsNaN(Fixed32 value)
```

**功能**：检测指定定点数是否为 `NaN`。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是 `NaN` 返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsNaN(Fixed32.NaN); // true
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
var a = Fixed32.Zero;
bool isZero = a.IsZero(); // true
```

---

### `static bool IsZero(Fixed32 n)`

```csharp
public static bool IsZero(Fixed32 n)
```

**功能**：检测指定定点数是否为零。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是零返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsZero(new Fixed32(0)); // true
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
var a = Fixed32.MinValue;
bool isMin = a.IsMin(); // true
```

---

### `static bool IsMin(Fixed32 n)`

```csharp
public static bool IsMin(Fixed32 n)
```

**功能**：检测指定定点数是否为最小值。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是最小值返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsMin(Fixed32.MinValue); // true
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
var a = Fixed32.MaxValue;
bool isMax = a.IsMax(); // true
```

---

### `static bool IsMax(Fixed32 n)`

```csharp
public static bool IsMax(Fixed32 n)
```

**功能**：检测指定定点数是否为最大值。

**参数**：
- `n`：要检测的定点数

**返回值**：如果是最大值返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsMax(Fixed32.MaxValue); // true
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
var a = Fixed32.PositiveInfinity;
bool isInf = a.IsInfinity(); // true
```

---

### `static bool IsInfinity(Fixed32 value)`

```csharp
public static bool IsInfinity(Fixed32 value)
```

**功能**：检测指定定点数是否为无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsInfinity(Fixed32.NegativeInfinity); // true
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
var a = Fixed32.PositiveInfinity;
bool isPosInf = a.IsPositiveInfinity(); // true
```

---

### `static bool IsPositiveInfinity(Fixed32 value)`

```csharp
public static bool IsPositiveInfinity(Fixed32 value)
```

**功能**：检测指定定点数是否为正无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是正无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsPositiveInfinity(Fixed32.PositiveInfinity); // true
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
var a = Fixed32.NegativeInfinity;
bool isNegInf = a.IsNegativeInfinity(); // true
```

---

### `static bool IsNegativeInfinity(Fixed32 value)`

```csharp
public static bool IsNegativeInfinity(Fixed32 value)
```

**功能**：检测指定定点数是否为负无穷大。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是负无穷大返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsNegativeInfinity(Fixed32.NegativeInfinity); // true
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
var a = new Fixed32(3);
bool isPos = a.IsPositive(); // true

var b = Fixed32.Zero;
bool isPos2 = b.IsPositive(); // true
```

---

### `static bool IsPositive(Fixed32 value)`

```csharp
public static bool IsPositive(Fixed32 value)
```

**功能**：检测指定定点数是否为正数（包括 0）。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是正数（含 0）返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsPositive(new Fixed32(-3)); // false
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
var a = new Fixed32(-3);
bool isNeg = a.IsNegative(); // true
```

---

### `static bool IsNegative(Fixed32 value)`

```csharp
public static bool IsNegative(Fixed32 value)
```

**功能**：检测指定定点数是否为负数。

**参数**：
- `value`：要检测的定点数

**返回值**：如果是负数返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsNegative(new Fixed32(3)); // false
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
var a = new Fixed32(3.14);
bool isFrac = a.IsFractional(); // true

var b = new Fixed32(3);
bool isFrac2 = b.IsFractional(); // false
```

---

### `static bool IsFractional(Fixed32 value)`

```csharp
public static bool IsFractional(Fixed32 value)
```

**功能**：检测指定定点数是否有小数部分。

**参数**：
- `value`：要检测的定点数

**返回值**：如果有小数部分返回 `true`，否则返回 `false`

**示例**：

```csharp
bool result = Fixed32.IsFractional(new Fixed32(3.14)); // true
```

## 三角函数

### 正弦 Sin

#### `static Fixed32 Sin(Fixed32 radian)`

```csharp
public static Fixed32 Sin(Fixed32 radian)
```

**功能**：计算指定弧度的正弦值。使用泰勒级数展开（11 阶）。

**参数**：
- `radian`：弧度值

**返回值**：正弦值，范围在 [-1, 1] 之间

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`

**示例**：

```csharp
var sin0 = Fixed32.Sin(Fixed32.Zero);       // 0
var sinPi2 = Fixed32.Sin(Fixed32.Half_PI);  // 1
var sinPi = Fixed32.Sin(Fixed32.PI);        // 0
```

---

#### `static Fixed32 FastSin(Fixed32 radian)`

```csharp
public static Fixed32 FastSin(Fixed32 radian)
```

**功能**：使用查表法快速计算正弦值。速度比 `Sin` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：正弦值，范围在 [-1, 1] 之间

**注意事项**：
- 如果输入是 `NaN`，返回 `NaN`
- 如果输入是正无穷或负无穷，返回 `NaN`
- 误差大于 `Sin` 函数

**示例**：

```csharp
var result = Fixed32.FastSin(Fixed32.Half_PI); // ~1（精度略低）
```

---

#### `static Fixed32 Asin(Fixed32 value)`

```csharp
public static Fixed32 Asin(Fixed32 value)
```

**功能**：计算指定值的反正弦值。

**参数**：
- `value`：正弦值，范围在 [-1, 1] 之间

**返回值**：反正弦值，范围在 [-π/2, π/2] 之间

**异常**：当输入值不在 [-1, 1] 范围内时，抛出 `ArgumentOutOfRangeException`

**示例**：

```csharp
var result = Fixed32.Asin(Fixed32.One); // π/2
```

### 余弦 Cos

#### `static Fixed32 Cos(Fixed32 radian)`

```csharp
public static Fixed32 Cos(Fixed32 radian)
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
var cos0 = Fixed32.Cos(Fixed32.Zero);   // 1
var cosPi2 = Fixed32.Cos(Fixed32.Half_PI); // 0
var cosPi = Fixed32.Cos(Fixed32.PI);    // -1
```

---

#### `static Fixed32 FastCos(Fixed32 radian)`

```csharp
public static Fixed32 FastCos(Fixed32 radian)
```

**功能**：使用快速正弦函数计算余弦值。速度比 `Cos` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：余弦值，范围在 [-1, 1] 之间

**注意事项**：误差大于 `Cos` 函数。

**示例**：

```csharp
var result = Fixed32.FastCos(Fixed32.Zero); // ~1
```

---

#### `static Fixed32 Acos(Fixed32 value)`

```csharp
public static Fixed32 Acos(Fixed32 value)
```

**功能**：计算指定值的反余弦值。

**参数**：
- `value`：余弦值，范围在 [-1, 1] 之间

**返回值**：反余弦值，范围在 [0, π] 之间

**异常**：当输入值不在 [-1, 1] 范围内时，抛出 `ArgumentOutOfRangeException`

**示例**：

```csharp
var result = Fixed32.Acos(Fixed32.Zero); // π/2
var result2 = Fixed32.Acos(Fixed32.One);  // 0
```

### 正切 Tan

#### `static Fixed32 Tan(Fixed32 radian)`

```csharp
public static Fixed32 Tan(Fixed32 radian)
```

**功能**：计算指定弧度的正切值。使用泰勒级数展开（21 阶）。

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
- 经测试，与 ±π/2 差值小于 0.0017 时，误差将大于 0.1

**示例**：

```csharp
var tan0 = Fixed32.Tan(Fixed32.Zero);         // 0
var tanPi4 = Fixed32.Tan(Fixed32.Quarter_PI); // 1
```

---

#### `static Fixed32 FastTan(Fixed32 radian)`

```csharp
public static Fixed32 FastTan(Fixed32 radian)
```

**功能**：使用查表法快速计算正切值。速度比 `Tan` 快，但精度较低。

**参数**：
- `radian`：弧度值

**返回值**：正切值

**注意事项**：误差大于 `Tan` 函数，但计算速度更快。

**示例**：

```csharp
var result = Fixed32.FastTan(Fixed32.Quarter_PI); // ~1
```

---

#### `static Fixed32 Atan(Fixed32 z)`

```csharp
public static Fixed32 Atan(Fixed32 z)
```

**功能**：计算指定值的反正切值。

**参数**：
- `z`：正切值

**返回值**：反正切值，范围在 [-π/2, π/2] 之间

**注意事项**：如果输入是 0，返回 0。

**示例**：

```csharp
var result = Fixed32.Atan(Fixed32.One); // π/4
```

---

#### `static Fixed32 Atan2(Fixed32 y, Fixed32 x)`

```csharp
public static Fixed32 Atan2(Fixed32 y, Fixed32 x)
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
var result = Fixed32.Atan2(new Fixed32(1), new Fixed32(1)); // π/4
var result2 = Fixed32.Atan2(new Fixed32(1), new Fixed32(-1)); // 3π/4
```

## Unity 风格工具方法

### `static Fixed32 MoveTowards(Fixed32 current, Fixed32 target, Fixed32 maxDelta)`

```csharp
public static Fixed32 MoveTowards(Fixed32 current, Fixed32 target, Fixed32 maxDelta)
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
var current = new Fixed32(0);
var target = new Fixed32(10);
var maxDelta = new Fixed32(3);

var result1 = Fixed32.MoveTowards(current, target, maxDelta); // 3
var result2 = Fixed32.MoveTowards(new Fixed32(9), target, maxDelta); // 10（到达目标）
```

---

### `static Fixed32 MoveTowardsAngle(Fixed32 current, Fixed32 target, Fixed32 maxDelta)`

```csharp
public static Fixed32 MoveTowardsAngle(Fixed32 current, Fixed32 target, Fixed32 maxDelta)
```

**功能**：将当前角度向目标角度移动，考虑角度环绕。

**参数**：
- `current`：当前角度（度）
- `target`：目标角度（度）
- `maxDelta`：最大移动角度

**返回值**：移动后的角度

**示例**：

```csharp
var current = new Fixed32(350);
var target = new Fixed32(10);
var maxDelta = new Fixed32(5);

var result = Fixed32.MoveTowardsAngle(current, target, maxDelta); // 355（沿最短路径移动）
```

---

### `static Fixed32 Repeat(Fixed32 t, Fixed32 length)`

```csharp
public static Fixed32 Repeat(Fixed32 t, Fixed32 length)
```

**功能**：将值限制在 [0, length) 范围内，超出则循环。

**参数**：
- `t`：输入值
- `length`：范围长度

**返回值**：重复后的值，范围在 [0, length) 之间

**示例**：

```csharp
var result1 = Fixed32.Repeat(new Fixed32(5), new Fixed32(3)); // 2
var result2 = Fixed32.Repeat(new Fixed32(-1), new Fixed32(3)); // 2
var result3 = Fixed32.Repeat(new Fixed32(3), new Fixed32(3));  // 0
```

---

### `static Fixed32 DeltaAngle(Fixed32 current, Fixed32 target)`

```csharp
public static Fixed32 DeltaAngle(Fixed32 current, Fixed32 target)
```

**功能**：计算两个角度之间的最小差值。

**参数**：
- `current`：当前角度（度）
- `target`：目标角度（度）

**返回值**：角度差，范围在 [-180, 180] 之间

**示例**：

```csharp
var result1 = Fixed32.DeltaAngle(new Fixed32(10), new Fixed32(350)); // -20
var result2 = Fixed32.DeltaAngle(new Fixed32(10), new Fixed32(30));  // 20
```

---

### `static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed, Fixed32 deltaTime)`

```csharp
public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed, Fixed32 deltaTime)
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
var current = new Fixed32(0);
var target = new Fixed32(100);
var velocity = Fixed32.Zero;
var smoothTime = new Fixed32(0.3);
var maxSpeed = Fixed32.MaxValue;
var deltaTime = new Fixed32(0.016); // ~60fps

// 每帧调用
current = Fixed32.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);
```

---

### `static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed)`

```csharp
public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed)
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
var current = new Fixed32(0);
var target = new Fixed32(100);
var velocity = Fixed32.Zero;
current = Fixed32.SmoothDamp(current, target, ref velocity, new Fixed32(0.3), Fixed32.MaxValue);
```

---

### `static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime)`

```csharp
public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime)
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
var current = new Fixed32(0);
var target = new Fixed32(100);
var velocity = Fixed32.Zero;
current = Fixed32.SmoothDamp(current, target, ref velocity, new Fixed32(0.3));
```

## 角度/弧度转换

### `static Fixed32 NormalizeRadian(Fixed32 radian)`

```csharp
public static Fixed32 NormalizeRadian(Fixed32 radian)
```

**功能**：将弧度值规范化到 [0, 2π] 范围内。

**参数**：
- `radian`：弧度值

**返回值**：规范化后的弧度值

**示例**：

```csharp
var result1 = Fixed32.NormalizeRadian(new Fixed32(7)); // ~0.7168（7 - 2π）
var result2 = Fixed32.NormalizeRadian(new Fixed32(-1)); // ~5.2832（2π - 1）
```

---

### `static Fixed32 DegreeToRadian(Fixed32 degree)`

```csharp
public static Fixed32 DegreeToRadian(Fixed32 degree)
```

**功能**：将角度值转换为弧度值。

**参数**：
- `degree`：角度值

**返回值**：对应的弧度值

**示例**：

```csharp
var result = Fixed32.DegreeToRadian(new Fixed32(180)); // π
var result2 = Fixed32.DegreeToRadian(new Fixed32(90));  // π/2
```

---

### `static Fixed32 RadianToDegree(Fixed32 radian)`

```csharp
public static Fixed32 RadianToDegree(Fixed32 radian)
```

**功能**：将弧度值转换为角度值。

**参数**：
- `radian`：弧度值

**返回值**：对应的角度值

**示例**：

```csharp
var result = Fixed32.RadianToDegree(Fixed32.PI); // 180
var result2 = Fixed32.RadianToDegree(Fixed32.Half_PI); // 90
```

## 插值

### `static Fixed32 Lerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)`

```csharp
public static Fixed32 Lerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
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
var result = Fixed32.Lerp(new Fixed32(0), new Fixed32(10), new Fixed32(0.5)); // 5
var result2 = Fixed32.Lerp(new Fixed32(0), new Fixed32(10), Fixed32.One);      // 10
```

---

### `static Fixed32 ClampLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)`

```csharp
public static Fixed32 ClampLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
```

**功能**：在两个值之间进行钳制线性插值。将 `amount` 限制在 [0, 1] 范围内。

**参数**：
- `value1`：起始值
- `value2`：结束值
- `amount`：插值因子

**返回值**：插值结果，不会超出 [value1, value2] 范围

**示例**：

```csharp
var result = Fixed32.ClampLerp(new Fixed32(0), new Fixed32(10), new Fixed32(0.5)); // 5
var result2 = Fixed32.ClampLerp(new Fixed32(0), new Fixed32(10), new Fixed32(2));  // 10（钳制）
```

---

### `static Fixed32 InverseLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)`

```csharp
public static Fixed32 InverseLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
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
var result = Fixed32.InverseLerp(new Fixed32(0), new Fixed32(10), new Fixed32(5)); // 0.5
var result2 = Fixed32.InverseLerp(new Fixed32(0), new Fixed32(10), new Fixed32(10)); // 1
```

---

### `static Fixed32 SmoothStep(Fixed32 value1, Fixed32 value2, Fixed32 amount)`

```csharp
public static Fixed32 SmoothStep(Fixed32 value1, Fixed32 value2, Fixed32 amount)
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
var result = Fixed32.SmoothStep(new Fixed32(0), new Fixed32(10), new Fixed32(0.5)); // 5
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
var a = Fixed32.NaN;
Console.WriteLine(a.ToString()); // "NaN"

var b = Fixed32.PositiveInfinity;
Console.WriteLine(b.ToString()); // "+∞"

var c = new Fixed32(42);
Console.WriteLine(c.ToString()); // "42"

var d = new Fixed32(3.14);
Console.WriteLine(d.ToString()); // "3.14"
```
