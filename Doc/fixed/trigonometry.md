# 三角函数

FMath 类提供了基于定点数的三角函数计算功能，包括正弦、余弦、正切及其反函数，以及角度与弧度的转换工具。所有角度参数均以弧度为单位。

- **命名空间**：`SimplexLab.Fixed`
- **泛型约束**：所有泛型方法的约束为 `where T : struct, IFixed<T>`
- **FMath** 是一个静态分部类，其泛型方法内部委托给对应定点数类型（如 `Fixed32`）的静态实现

## 正弦函数

### Sin

```csharp
public static T Sin<T>(T radian) where T : struct, IFixed<T>
```

计算指定弧度的正弦值。内部使用泰勒展开进行计算，先将角度规范化到 `[0, 2π)`，再利用正弦函数的对称性将角度缩减到 `[0, π/2]` 区间以减少计算误差。

- **参数**：`radian` — 弧度值
- **返回值**：正弦值，范围在 `[-1, 1]` 之间
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
- **示例**：

```csharp
Fixed32 result  = FMath.Sin(Fixed32.PI / 6);  // ≈ 0.5
Fixed32 result2 = FMath.Sin(Fixed32.PI / 2);  // ≈ 1
Fixed32 result3 = FMath.Sin(Fixed32.Zero);     // = 0
```

### FastSin

```csharp
public static T FastSin<T>(T radian) where T : struct, IFixed<T>
```

使用查表法快速计算正弦值，速度比 `Sin` 函数更快，但精度较低。

- **参数**：`radian` — 弧度值
- **返回值**：正弦值，范围在 `[-1, 1]` 之间
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
- **注意**：误差大于 `Sin` 函数，适用于对精度要求不高但需要高性能的场景
- **示例**：

```csharp
Fixed32 result = FMath.FastSin(Fixed32.PI / 4); // ≈ 0.707
```

### Asin

```csharp
public static T Asin<T>(T value) where T : struct, IFixed<T>
```

计算反正弦值。内部通过 `Asin(x) = π/2 - Acos(x)` 实现。

- **参数**：`value` — 输入值，范围在 `[-1, 1]` 之间
- **返回值**：反正弦值，范围在 `[-π/2, π/2]` 之间
- **异常**：当输入值不在 `[-1, 1]` 范围内时抛出 `ArgumentException`
- **示例**：

```csharp
Fixed32 result  = FMath.Asin((Fixed32)1);  // ≈ π/2
Fixed32 result2 = FMath.Asin((Fixed32)0);  // = 0
Fixed32 result3 = FMath.Asin((Fixed32)(-1)); // ≈ -π/2
```

---

## 余弦函数

### Cos

```csharp
public static T Cos<T>(T radian) where T : struct, IFixed<T>
```

计算指定弧度的余弦值。内部通过 `Cos(x) = Sin(x + π/2)` 实现。

- **参数**：`radian` — 弧度值
- **返回值**：余弦值，范围在 `[-1, 1]` 之间
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
- **示例**：

```csharp
Fixed32 result  = FMath.Cos(Fixed32.PI / 3); // ≈ 0.5
Fixed32 result2 = FMath.Cos(Fixed32.Zero);   // = 1
Fixed32 result3 = FMath.Cos(Fixed32.PI);     // ≈ -1
```

### FastCos

```csharp
public static T FastCos<T>(T radian) where T : struct, IFixed<T>
```

使用查表法快速计算余弦值。内部通过 `FastCos(x) = FastSin(x + π/2)` 实现，速度比 `Cos` 函数更快，但精度较低。

- **参数**：`radian` — 弧度值
- **返回值**：余弦值，范围在 `[-1, 1]` 之间
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
- **注意**：误差大于 `Cos` 函数，适用于对精度要求不高但需要高性能的场景
- **示例**：

```csharp
Fixed32 result = FMath.FastCos(Fixed32.PI / 4); // ≈ 0.707
```

### Acos

```csharp
public static T Acos<T>(T value) where T : struct, IFixed<T>
```

计算反余弦值。内部通过 `Atan(Sqrt(1 - x²) / x)` 计算，并根据输入值的符号调整结果。

- **参数**：`value` — 输入值，范围在 `[-1, 1]` 之间
- **返回值**：反余弦值，范围在 `[0, π]` 之间
- **异常**：当输入值不在 `[-1, 1]` 范围内时抛出 `ArgumentOutOfRangeException`
- **特殊情况**：
  - 输入为 `0` → 返回 `π/2`
- **示例**：

```csharp
Fixed32 result  = FMath.Acos((Fixed32)1);   // = 0
Fixed32 result2 = FMath.Acos((Fixed32)0);   // ≈ π/2
Fixed32 result3 = FMath.Acos((Fixed32)(-1)); // ≈ π
```

---

## 正切函数

### Tan

```csharp
public static T Tan<T>(T radian) where T : struct, IFixed<T>
```

计算指定弧度的正切值。内部使用泰勒展开进行计算，先将角度规范化到 `[-π/2, π/2]` 范围内，再将角度缩减到 `[0, π/2]` 区间。对于接近 `π/4` 的角度使用余切函数的倒数来提高精度。

- **参数**：`radian` — 弧度值
- **返回值**：正切值
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
  - 输入为 `0` → 返回 `0`
  - 输入为 `π/2` → 返回正无穷大（`MaxValue`）
  - 输入为 `-π/2` → 返回负无穷大（`MinValue`）
  - 输入为 `π/4` → 返回 `1`
- **注意**：
  - 将 `radian` 规范化到 `[-π/2, π/2]` 范围内，其值越接近 `±π/2` 误差越大
  - 经测试，与 `±π/2` 差值小于 `0.0017` 时，误差将大于 `0.1`
- **示例**：

```csharp
Fixed32 result  = FMath.Tan(Fixed32.PI / 4);  // ≈ 1
Fixed32 result2 = FMath.Tan(Fixed32.PI / 6);  // ≈ 0.577
Fixed32 result3 = FMath.Tan(Fixed32.Zero);     // = 0
```

### FastTan

```csharp
public static T FastTan<T>(T radian) where T : struct, IFixed<T>
```

使用查表法快速计算正切值，速度比 `Tan` 函数更快，但精度较低。

- **参数**：`radian` — 弧度值
- **返回值**：正切值
- **特殊情况**：
  - 输入为 `NaN` → 返回 `NaN`
  - 输入为正无穷或负无穷 → 返回 `NaN`
  - 输入为 `0` → 返回 `0`
  - 输入为 `π/2` → 返回正无穷大（`MaxValue`）
  - 输入为 `-π/2` → 返回负无穷大（`MinValue`）
  - 输入为 `π/4` → 返回 `1`
- **注意**：
  - 误差大于 `Tan` 函数，但计算速度比 `Tan` 函数更快
  - 其值越接近 `±π/2` 误差越大
- **示例**：

```csharp
Fixed32 result = FMath.FastTan(Fixed32.PI / 6); // ≈ 0.577
```

### Atan

```csharp
public static T Atan<T>(T value) where T : struct, IFixed<T>
```

计算反正切值。内部使用级数展开进行迭代计算，对于绝对值大于 `1` 的输入先取倒数再计算，最后通过 `π/2 - result` 还原结果。

- **参数**：`value` — 输入值
- **返回值**：反正切值，范围在 `[-π/2, π/2]` 之间
- **特殊情况**：
  - 输入为 `0` → 返回 `0`
- **示例**：

```csharp
Fixed32 result  = FMath.Atan((Fixed32)1);   // ≈ π/4
Fixed32 result2 = FMath.Atan((Fixed32)0);   // = 0
Fixed32 result3 = FMath.Atan((Fixed32)(-1)); // ≈ -π/4
```

### Atan2

```csharp
public static T Atan2<T>(T y, T x) where T : struct, IFixed<T>
```

计算两参数反正切值，考虑象限。内部使用近似公式 `z / (1 + 0.28125 * z²)` 进行计算，可正确处理各象限的角度。

- **参数**：
  - `y` — y 坐标
  - `x` — x 坐标
- **返回值**：反正切值，范围在 `[-π, π]` 之间
- **特殊情况**：
  - `x = 0` 且 `y > 0` → 返回 `π/2`
  - `x = 0` 且 `y = 0` → 返回 `0`
  - `x = 0` 且 `y < 0` → 返回 `-π/2`
- **示例**：

```csharp
Fixed32 result  = FMath.Atan2((Fixed32)1, (Fixed32)1);    // ≈ π/4（第一象限）
Fixed32 result2 = FMath.Atan2((Fixed32)1, (Fixed32)(-1));  // ≈ 3π/4（第二象限）
Fixed32 result3 = FMath.Atan2((Fixed32)(-1), (Fixed32)(-1)); // ≈ -3π/4（第三象限）
Fixed32 result4 = FMath.Atan2((Fixed32)(-1), (Fixed32)1);   // ≈ -π/4（第四象限）
```

---

## 角度与弧度转换

### DegreeToRadian

```csharp
public static T DegreeToRadian<T>(T degree) where T : struct, IFixed<T>
```

将角度值转换为弧度值。计算公式为 `radian = degree × π / 180`。

- **参数**：`degree` — 角度值
- **返回值**：对应的弧度值
- **示例**：

```csharp
Fixed32 rad  = FMath.DegreeToRadian((Fixed32)180); // ≈ π
Fixed32 rad2 = FMath.DegreeToRadian((Fixed32)90);   // ≈ π/2
Fixed32 rad3 = FMath.DegreeToRadian((Fixed32)45);   // ≈ π/4
```

### RadianToDegree

```csharp
public static T RadianToDegree<T>(T radian) where T : struct, IFixed<T>
```

将弧度值转换为角度值。计算公式为 `degree = radian × 180 / π`。

- **参数**：`radian` — 弧度值
- **返回值**：对应的角度值
- **示例**：

```csharp
Fixed32 deg  = FMath.RadianToDegree(Fixed32.PI);     // ≈ 180
Fixed32 deg2 = FMath.RadianToDegree(Fixed32.PI / 2); // ≈ 90
```

### NormalizeRadian

```csharp
public static T NormalizeRadian<T>(T radian) where T : struct, IFixed<T>
```

将弧度值规范化到 `[0, 2π)` 范围内。对于超出该范围的弧度值，通过取模运算将其映射回标准区间。

- **参数**：`radian` — 弧度值
- **返回值**：规范化后的弧度值，范围在 `[0, 2π)` 之间
- **示例**：

```csharp
Fixed32 norm  = FMath.NormalizeRadian(Fixed32.PI * 3);    // ≈ π
Fixed32 norm2 = FMath.NormalizeRadian(Fixed32.PI / 2);    // ≈ π/2（已在范围内，不变）
Fixed32 norm3 = FMath.NormalizeRadian(-Fixed32.PI / 2);   // ≈ 3π/2
```

---

## IFixed\<T\> 接口三角方法

`IFixed<T>` 接口定义了三角函数的抽象静态方法，所有实现该接口的定点数类型（如 `Fixed32`）都必须提供具体实现。FMath 的泛型方法正是委托给这些接口方法。

### 接口定义

```csharp
public interface IFixed<T> : IMinMaxValue<T>, IComparable, IComparable<T>, IEquatable<T>
    where T : IFixed<T>
{
    // 正弦
    abstract static T Sin(T radian);
    abstract static T FastSin(T radian);
    abstract static T Asin(T value);

    // 余弦
    abstract static T Cos(T radian);
    abstract static T FastCos(T radian);
    abstract static T Acos(T value);

    // 正切
    abstract static T Tan(T radian);
    abstract static T FastTan(T radian);
    abstract static T Atan(T value);
    abstract static T Atan2(T y, T x);

    // 工具
    abstract static T NormalizeRadian(T radian);
    abstract static T DegreeToRadian(T degree);
    abstract static T RadianToDegree(T radian);
}
```

### Fixed32 上的实例方法

`Fixed32` 结构体实现了 `IFixed<Fixed32>` 接口，因此上述方法均以静态方法的形式直接在 `Fixed32` 上可用。以下为 `Fixed32` 特有的调用方式：

```csharp
// 正弦
Fixed32 s1 = Fixed32.Sin(Fixed32.PI / 6);       // ≈ 0.5
Fixed32 s2 = Fixed32.FastSin(Fixed32.PI / 6);   // ≈ 0.5（精度较低）
Fixed32 s3 = Fixed32.Asin((Fixed32)1);           // ≈ π/2

// 余弦
Fixed32 c1 = Fixed32.Cos(Fixed32.PI / 3);        // ≈ 0.5
Fixed32 c2 = Fixed32.FastCos(Fixed32.PI / 3);    // ≈ 0.5（精度较低）
Fixed32 c3 = Fixed32.Acos((Fixed32)0);           // ≈ π/2

// 正切
Fixed32 t1 = Fixed32.Tan(Fixed32.PI / 4);        // ≈ 1
Fixed32 t2 = Fixed32.FastTan(Fixed32.PI / 4);    // ≈ 1（精度较低）
Fixed32 t3 = Fixed32.Atan((Fixed32)1);           // ≈ π/4
Fixed32 t4 = Fixed32.Atan2((Fixed32)1, (Fixed32)1); // ≈ π/4

// 工具
Fixed32 r1 = Fixed32.NormalizeRadian(Fixed32.PI * 3); // ≈ π
Fixed32 r2 = Fixed32.DegreeToRadian((Fixed32)180);     // ≈ π
Fixed32 r3 = Fixed32.RadianToDegree(Fixed32.PI);       // ≈ 180
```

### FMath 与 Fixed32 直接调用的关系

FMath 的泛型方法是 `Fixed32` 静态方法的上层封装，二者在功能上完全等价：

| FMath 泛型调用 | Fixed32 直接调用 |
|---|---|
| `FMath.Sin<T>(radian)` | `Fixed32.Sin(radian)` |
| `FMath.FastSin<T>(radian)` | `Fixed32.FastSin(radian)` |
| `FMath.Asin<T>(value)` | `Fixed32.Asin(value)` |
| `FMath.Cos<T>(radian)` | `Fixed32.Cos(radian)` |
| `FMath.FastCos<T>(radian)` | `Fixed32.FastCos(radian)` |
| `FMath.Acos<T>(value)` | `Fixed32.Acos(value)` |
| `FMath.Tan<T>(radian)` | `Fixed32.Tan(radian)` |
| `FMath.FastTan<T>(radian)` | `Fixed32.FastTan(radian)` |
| `FMath.Atan<T>(value)` | `Fixed32.Atan(value)` |
| `FMath.Atan2<T>(y, x)` | `Fixed32.Atan2(y, x)` |
| `FMath.NormalizeRadian<T>(radian)` | `Fixed32.NormalizeRadian(radian)` |
| `FMath.DegreeToRadian<T>(degree)` | `Fixed32.DegreeToRadian(degree)` |
| `FMath.RadianToDegree<T>(radian)` | `Fixed32.RadianToDegree(radian)` |

FMath 泛型方法的优势在于支持任意实现了 `IFixed<T>` 接口的定点数类型，而不仅限于 `Fixed32`。

---

## 实现细节

### 正弦（Sin / FastSin）

- **Sin**：使用泰勒展开 `sin(x) = x - x³/3! + x⁵/5! - x⁷/7! + x⁹/9! - x¹¹/11!`，展开到第 11 阶。先将角度规范化到 `[0, 2π)`，再利用象限对称性将角度缩减到 `[0, π/2]` 区间，以减少泰勒展开的截断误差。
- **FastSin**：使用预计算的正弦查找表（LUT），通过将 `[0, π/2]` 区间离散化为等距采样点，根据输入角度查表取最近值。速度更快但精度受限于查找表的分辨率。

### 余弦（Cos / FastCos）

- **Cos**：利用余弦与正弦的相位关系，`Cos(x) = Sin(x + π/2)`，直接调用 `Sin` 函数实现。
- **FastCos**：同理，`FastCos(x) = FastSin(x + π/2)`，直接调用 `FastSin` 函数实现。

### 正切（Tan / FastTan）

- **Tan**：使用泰勒展开 `tan(x) = x + x³/3 + 2x⁵/15 + 17x⁷/315 + ...`，展开到第 21 阶。先将角度规范化到 `[-π/2, π/2]`，再缩减到 `[0, π/2]` 区间。对于大于 `π/4` 的角度，使用余切函数的倒数来提高精度。
- **FastTan**：使用预计算的正切查找表（LUT），查表取最近值。

### 反余弦（Acos）

- **Acos**：通过公式 `Acos(x) = Atan(Sqrt(1 - x²) / x)` 计算。当输入为负数时，结果需要加上 `π` 以保证返回值在 `[0, π]` 范围内。

### 反正弦（Asin）

- **Asin**：利用 `Asin(x) = π/2 - Acos(x)` 的关系，直接调用 `Acos` 函数实现。

### 反正切（Atan）

- **Atan**：使用级数展开进行迭代计算。对于绝对值大于 `1` 的输入，先取倒数 `z = 1/z`，计算后再通过 `π/2 - result` 还原。迭代最多 30 次，当项值趋近于零时提前终止。

### 两参数反正切（Atan2）

- **Atan2**：使用近似公式 `z / (1 + 0.28125 × z²)` 计算，其中 `z = y/x`。该近似公式在精度和性能之间取得了较好的平衡。根据 `x` 和 `y` 的符号判断象限，对结果进行相应的调整。

---

## 精度与性能说明

| 方法 | 计算方式 | 精度 | 性能 |
|---|---|---|---|
| `Sin` | 泰勒展开（11 阶） | 高 | 中 |
| `FastSin` | 查表法（LUT） | 中 | 快 |
| `Cos` | 泰勒展开（11 阶，通过 Sin） | 高 | 中 |
| `FastCos` | 查表法（LUT，通过 FastSin） | 中 | 快 |
| `Tan` | 泰勒展开（21 阶） | 高 | 慢 |
| `FastTan` | 查表法（LUT） | 中 | 快 |
| `Asin` | 通过 Acos 间接计算 | 高 | 中 |
| `Acos` | 通过 Atan + Sqrt 间接计算 | 高 | 中 |
| `Atan` | 级数迭代（最多 30 次） | 高 | 中 |
| `Atan2` | 近似公式 | 中 | 快 |

> **建议**：在游戏逻辑等对性能敏感且精度要求不高的场景中，优先使用 `Fast*` 系列方法；在物理模拟等对精度要求较高的场景中，使用标准方法。
