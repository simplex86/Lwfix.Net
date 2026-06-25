# FRandom

Lwfix.Net 提供了基于定点数的伪随机数生成器，采用**线性同余算法**（Linear Congruential Generator, LCG）。该模块为定点数类型提供统一的随机数生成接口，支持生成 `[0, 1)` 区间的随机浮点值、指定整数区间的随机整数值以及指定定点数区间的随机定点数值。

**命名空间**：`SimplexLab.Fixed`

**模块组成**：

| 类型 | 说明 |
|------|------|
| `IRandom` | 随机数基础接口 |
| `IRandom<T>` | 泛型随机数接口 |
| `FRandomEntryAttribute<T>` | 随机数入口特性 |
| `FRandom` | 随机数管理类（对外统一入口） |
| `FRandom32` | 32位随机数生成器（`Fixed32` 的内部实现） |

---

## IRandom 接口

```csharp
public interface IRandom { }
```

随机数基础接口，为空接口（标记接口）。所有随机数相关接口均继承自该接口，用于在 `FRandom` 中以非泛型方式统一管理不同定点数类型的随机数生成器实例。

### 说明

- 作为 `IRandom<T>` 的父接口，提供类型统一的基类
- 在 `FRandom` 内部以 `Dictionary<Type, IRandom>` 存储各类型的随机数生成器时作为值类型

---

## IRandom\<T\> 接口

```csharp
public interface IRandom<T> : IRandom where T : struct, IFixed<T>
```

泛型随机数接口，定义了定点数类型随机数生成器必须实现的方法。继承自 `IRandom`，并约束泛型参数 `T` 必须为值类型且实现 `IFixed<T>` 接口。

### 类型参数

| 参数 | 约束 | 说明 |
|------|------|------|
| `T` | `struct, IFixed<T>` | 定点数类型，如 `Fixed32` |

### 方法

#### Next

```csharp
T Next()
```

获取 `[0, 1)` 区间内的随机定点数。

**返回值**：`T` — 区间 `[0, 1)` 内的随机定点数，即值大于等于 0 且小于 1。

**示例**：

```csharp
// 获取 [0, 1) 区间的随机数
var value = FRandom.Shared.Next<Fixed32>();
// value 可能的值：0.0, 0.142, 0.587, 0.999 等
```

**备注**：

- 返回值始终满足 `0 ≤ value < 1`
- 每次调用会推进内部状态，产生不同的值

---

#### Next(int min, int max)

```csharp
T Next(int min, int max)
```

获取 `[min, max)` 区间内的随机整数（以定点数类型返回）。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `int` | 区间下界（包含） |
| `max` | `int` | 区间上界（不包含） |

**返回值**：`T` — 区间 `[min, max)` 内的随机整数值，以定点数类型表示。

**示例**：

```csharp
// 获取 [0, 10) 区间的随机整数
var value = FRandom.Shared.Next<Fixed32>(0, 10);
// value 可能的值：0, 1, 2, ..., 9（整数值）

// 获取 [-5, 5) 区间的随机整数
var value2 = FRandom.Shared.Next<Fixed32>(-5, 5);
// value2 可能的值：-5, -4, -3, ..., 4
```

**备注**：

- 返回值为整数值（小数部分为零）
- `min` 必须小于 `max`，否则行为未定义
- 内部实现：先调用 `Next()` 获取 `[0, 1)` 的随机值，再映射到 `[min, max)` 并取整

---

#### Next(T min, T max)

```csharp
T Next(T min, T max)
```

获取 `[min, max)` 区间内的随机定点数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `T` | 区间下界（包含），定点数类型 |
| `max` | `T` | 区间上界（不包含），定点数类型 |

**返回值**：`T` — 区间 `[min, max)` 内的随机定点数值。

**示例**：

```csharp
// 获取 [0, 1) 区间的随机定点数
var value = FRandom.Shared.Next<Fixed32>(Fixed32.Zero, Fixed32.One);
// value 可能的值：0.0, 0.25, 0.5, 0.75 等

// 获取 [-5, 5) 区间的随机定点数
var value2 = FRandom.Shared.Next<Fixed32>(new Fixed32(-5), new Fixed32(5));
// value2 可能的值：-4.7, -2.3, 0.1, 3.8 等
```

**备注**：

- `min` 和 `max` 会被四舍五入（`Round`）到最近整数后再参与计算
- 返回值为整数值（小数部分为零），因为内部实现与 `Next(int, int)` 一致
- `min` 必须小于 `max`，否则行为未定义

---

## FRandomEntryAttribute\<T\> 特性

```csharp
public class FRandomEntryAttribute<T> : Attribute where T : struct, IFixed<T>
```

随机数入口特性类，继承自 `Attribute`，用于标记和注册定点数类型对应的随机数生成器。

### 类型参数

| 参数 | 约束 | 说明 |
|------|------|------|
| `T` | `struct, IFixed<T>` | 关联的定点数类型 |

### 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `Type` | `Type`（只读） | 获取关联的定点数类型信息，值为 `typeof(T)` |

### 构造函数

```csharp
public FRandomEntryAttribute()
```

创建 `FRandomEntryAttribute<T>` 实例，并自动将 `Type` 属性设置为 `typeof(T)`。

**示例**：

```csharp
// 标记 Fixed32 对应的随机数生成器
[FRandomEntryAttribute<Fixed32>]
internal class FRandom32 : IRandom<Fixed32>
{
    // ...
}
```

**备注**：

- 该特性用于将定点数类型与对应的随机数生成器建立关联
- `Type` 属性在构造时自动赋值，值为泛型参数 `T` 的运行时类型

---

## FRandom 类

```csharp
public class FRandom
```

随机数管理类，提供统一的随机数生成接口。内部维护一个 `Dictionary<Type, IRandom>` 字典，根据泛型类型参数分发到对应的随机数生成器实现。

### 属性

#### Shared

```csharp
public static FRandom Shared { get; }
```

全局共享的随机数生成器实例。该属性为静态只读，在首次访问时自动创建。

**类型**：`FRandom`（静态只读）

**示例**：

```csharp
// 使用全局共享实例
var value = FRandom.Shared.Next<Fixed32>();
```

**备注**：

- 该实例为全局唯一，整个应用程序生命周期内共享同一个实例
- 内部在构造时注册了 `Fixed32` 对应的 `FRandom32` 生成器
- 非线程安全，多线程环境下需自行加锁

---

### 方法

#### Next\<T\>()

```csharp
public T Next<T>() where T : struct, IFixed<T>
```

获取 `[0, 1)` 区间内的随机定点数。

**类型参数**：

| 参数 | 约束 | 说明 |
|------|------|------|
| `T` | `struct, IFixed<T>` | 定点数类型 |

**返回值**：`T` — 区间 `[0, 1)` 内的随机定点数。

**示例**：

```csharp
// 获取 [0, 1) 区间的随机 Fixed32 值
Fixed32 value = FRandom.Shared.Next<Fixed32>();
Console.WriteLine(value.ToDouble()); // 输出如：0.5873
```

**备注**：

- 内部通过 `Get<T>()` 查找对应类型的 `IRandom<T>` 实现并调用其 `Next()` 方法
- 如果类型 `T` 未注册，将返回默认值

---

#### Next\<T\>(int min, int max)

```csharp
public T Next<T>(int min, int max) where T : struct, IFixed<T>
```

获取 `[min, max)` 区间内的随机整数（以定点数类型返回）。

**类型参数**：

| 参数 | 约束 | 说明 |
|------|------|------|
| `T` | `struct, IFixed<T>` | 定点数类型 |

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `int` | 区间下界（包含） |
| `max` | `int` | 区间上界（不包含） |

**返回值**：`T` — 区间 `[min, max)` 内的随机整数值，以定点数类型表示。

**示例**：

```csharp
// 获取 [0, 100) 区间的随机整数
Fixed32 value = FRandom.Shared.Next<Fixed32>(0, 100);
Console.WriteLine(value.ToInt()); // 输出如：42

// 获取 [-10, 10) 区间的随机整数
Fixed32 value2 = FRandom.Shared.Next<Fixed32>(-10, 10);
Console.WriteLine(value2.ToInt()); // 输出如：-3
```

**备注**：

- 返回值为整数值（小数部分为零）
- `min` 必须小于 `max`

---

#### Next\<T\>(T min, T max)

```csharp
public T Next<T>(T min, T max) where T : struct, IFixed<T>
```

获取 `[min, max)` 区间内的随机定点数。

**类型参数**：

| 参数 | 约束 | 说明 |
|------|------|------|
| `T` | `struct, IFixed<T>` | 定点数类型 |

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `T` | 区间下界（包含），定点数类型 |
| `max` | `T` | 区间上界（不包含），定点数类型 |

**返回值**：`T` — 区间 `[min, max)` 内的随机定点数值。

**示例**：

```csharp
// 获取 [0, 1) 区间的随机定点数
Fixed32 value = FRandom.Shared.Next<Fixed32>(Fixed32.Zero, Fixed32.One);
Console.WriteLine(value.ToDouble()); // 输出如：0.7321

// 获取负数区间的随机定点数
Fixed32 value2 = FRandom.Shared.Next<Fixed32>(new Fixed32(-5), new Fixed32(5));
Console.WriteLine(value2.ToDouble()); // 输出如：-2.0
```

**备注**：

- `min` 和 `max` 会被四舍五入到最近整数后再参与计算
- 返回值为整数值（小数部分为零）
- `min` 必须小于 `max`

---

## FRandom32 类

```csharp
internal class FRandom32 : IRandom<Fixed32>
```

32位随机数生成器，为 `Fixed32` 类型提供具体的伪随机数生成实现。采用**线性同余伪随机数生成器**（Linear Congruential Generator, LCG），算法参考《计算机程序设计艺术》卷2第三章。

> **注意**：该类为 `internal`，不对外暴露，仅通过 `FRandom` 间接使用。

### 算法原理

线性同余法使用如下递推公式生成伪随机数序列：

```
s_{n+1} = (a × s_n + b) mod m
```

其中：

| 参数 | 值 | 说明 |
|------|------|------|
| `a`（乘数） | `1103515245` | 线性同余法的乘数 |
| `b`（增量） | `15107` | 线性同余法的增量 |
| `m`（模数） | `838859327` | 线性同余法的模数 |

生成 `[0, 1)` 区间随机数的步骤：

1. 使用线性同余公式更新内部状态 `s`
2. 将新的状态值 `s` 构造为 `Fixed32`
3. 除以模数 `m`，归一化到 `[0, 1)` 区间

> **注意**：计算 `s * a` 时使用 `long`（64位）类型进行中间运算，避免32位整数溢出。

### 构造函数

#### FRandom32()

```csharp
public FRandom32()
```

使用当前 UTC 时间与 Unix 纪元（1970年1月1日）的差值（秒数）作为种子，创建随机数生成器实例。

**示例**：

```csharp
// 内部使用，由 FRandom 自动创建
var random = new FRandom32();
// 种子为当前UTC时间的Unix时间戳（秒）
```

**备注**：

- 种子值为 `DateTime.UtcNow` 与 `new DateTime(1970, 1, 1)` 的差值总秒数
- 不同时间创建的实例将产生不同的随机数序列
- 由于种子精度为秒级，同一秒内创建的多个实例将产生相同的序列

---

#### FRandom32(int seed)

```csharp
public FRandom32(int seed)
```

使用指定种子创建随机数生成器实例。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `seed` | `int` | 随机数种子值 |

**示例**：

```csharp
// 使用固定种子，确保可复现的随机序列
var random = new FRandom32(42);
var v1 = random.Next(); // 每次使用种子42创建实例，v1的值相同

// 使用不同种子
var random2 = new FRandom32(123);
var v2 = random2.Next(); // 与v1不同
```

**备注**：

- 相同种子产生的随机数序列完全相同，适合需要可复现结果的场景（如回放系统）
- 种子为 0 时，首次调用 `Next()` 会返回 `(0 × 1103515245 + 15107) % 838859327 = 15107`

---

### 方法

#### Next()

```csharp
public Fixed32 Next()
```

获取 `[0, 1)` 区间内的随机定点数。

**返回值**：`Fixed32` — 区间 `[0, 1)` 内的随机定点数。

**示例**：

```csharp
var random = new FRandom32(42);
var value = random.Next();
// value 为 [0, 1) 区间的 Fixed32 值

// 多次调用产生不同值
var v1 = random.Next();
var v2 = random.Next();
var v3 = random.Next();
// v1, v2, v3 各不相同
```

**备注**：

- 返回值始终满足 `0 ≤ value < 1`
- 每次调用推进内部状态：`s = (s * a + b) % m`
- 归一化方式：`new Fixed32(s) / m`
- 中间计算使用 `long` 类型防止溢出

---

#### Next(int min, int max)

```csharp
public Fixed32 Next(int min, int max)
```

获取 `[min, max)` 区间内的随机整数（以 `Fixed32` 类型返回）。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `int` | 区间下界（包含） |
| `max` | `int` | 区间上界（不包含） |

**返回值**：`Fixed32` — 区间 `[min, max)` 内的随机整数值。

**示例**：

```csharp
var random = new FRandom32(42);

// 获取 [0, 10) 区间的随机整数
var value = random.Next(0, 10);
// value 可能的值：0, 1, 2, ..., 9

// 获取 [100, 200) 区间的随机整数
var value2 = random.Next(100, 200);
// value2 可能的值：100, 101, ..., 199
```

**备注**：

- 返回值为整数值（小数部分为零），通过 `Integral()` 方法取整
- 内部实现步骤：
  1. 调用 `Next()` 获取 `[0, 1)` 的随机值 `mod`
  2. 计算区间宽度：`max = max - min`
  3. 计算结果：`(mod * max + min).Integral()`
- `min` 必须小于 `max`，否则行为未定义

---

#### Next(Fixed32 min, Fixed32 max)

```csharp
public Fixed32 Next(Fixed32 min, Fixed32 max)
```

获取 `[min, max)` 区间内的随机定点数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `min` | `Fixed32` | 区间下界（包含），定点数类型 |
| `max` | `Fixed32` | 区间上界（不包含），定点数类型 |

**返回值**：`Fixed32` — 区间 `[min, max)` 内的随机定点数值。

**示例**：

```csharp
var random = new FRandom32(42);

// 获取 [0, 1) 区间的随机定点数
var value = random.Next(Fixed32.Zero, Fixed32.One);
// value 可能的值：0, 1 等（整数值）

// 获取负数区间
var value2 = random.Next(new Fixed32(-5), new Fixed32(5));
// value2 可能的值：-5, -4, ..., 4
```

**备注**：

- `min` 和 `max` 参数会先通过 `Round()` 方法四舍五入到最近整数
- 返回值为整数值（小数部分为零），通过 `Integral()` 方法取整
- 内部实现步骤：
  1. 对 `min` 和 `max` 执行 `Round()` 取整
  2. 调用 `Next()` 获取 `[0, 1)` 的随机值 `mod`
  3. 计算区间宽度：`max = max - min`
  4. 计算结果：`(mod * max + min).Integral()`
- `min` 必须小于 `max`，否则行为未定义

---

## 使用指南

### 基本用法

```csharp
using SimplexLab.Fixed;

// 获取全局随机数实例
var random = FRandom.Shared;

// 生成 [0, 1) 区间的随机数
Fixed32 value = random.Next<Fixed32>();

// 生成 [0, 10) 区间的随机整数
Fixed32 intValue = random.Next<Fixed32>(0, 10);

// 生成 [-5, 5) 区间的随机定点数
Fixed32 rangeValue = random.Next<Fixed32>(new Fixed32(-5), new Fixed32(5));
```

### 游戏中的典型应用

```csharp
using SimplexLab.Fixed;

// 暴击判定（30%暴击率）
Fixed32 critChance = new Fixed32(0.3);
Fixed32 roll = FRandom.Shared.Next<Fixed32>();
bool isCrit = roll < critChance;

// 随机伤害（10~50之间）
Fixed32 damage = FRandom.Shared.Next<Fixed32>(10, 50);

// 随机方向偏移
Fixed32 offset = FRandom.Shared.Next<Fixed32>(new Fixed32(-1), new Fixed32(1));
```

### 可复现的随机序列

由于 `FRandom32` 支持指定种子，在需要确定性随机序列的场景（如回放系统、测试等）中，可以通过反射或直接构造 `FRandom32` 实例来使用固定种子：

```csharp
// 使用固定种子创建随机数生成器
var seededRandom = new FRandom32(12345);

// 相同种子始终产生相同序列
var v1 = seededRandom.Next(); // 确定性结果
var v2 = seededRandom.Next(); // 确定性结果
```

---

## 注意事项

1. **线程安全**：`FRandom.Shared` 不是线程安全的，在多线程环境中使用时需要外部加锁。
2. **种子精度**：默认构造函数使用 UTC 时间的 Unix 秒数作为种子，同一秒内创建的多个实例将产生相同的随机序列。
3. **算法局限性**：线性同余法是一种经典的伪随机数生成算法，不具备密码学安全性，不应用于安全敏感场景。
4. **定点数区间方法**：`Next(Fixed32 min, Fixed32 max)` 方法会对 `min` 和 `max` 执行四舍五入操作，且返回值为整数，如需获取带小数的随机定点数，建议使用 `Next()` 获取 `[0, 1)` 的值后自行缩放。
5. **类型注册**：`FRandom` 在构造时仅注册了 `Fixed32` 对应的 `FRandom32`，如果查询未注册的类型，将返回 `null` 并导致异常。
