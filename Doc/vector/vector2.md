# FVector2

基于定点数的二维向量

## 结构定义

```csharp
public partial struct FVector2<T> where T : struct, IFixed<T>
```

**命名空间**：`SimplexLab.Fixed`

## 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `X` | `T` | X 分量（可读写） |
| `Y` | `T` | Y 分量（可读写） |
| `Magnitude` | `T` | 向量的长度（只读），计算公式：`√(X² + Y²)` |
| `SqrMagnitude` | `T` | 向量的长度平方（只读），计算公式：`X² + Y²` |
| `Normalized` | `FVector2<T>` | 归一化后的向量（只读），长度为 1 的同方向向量 |

> **提示**：当只需要比较向量长度时，优先使用 `SqrMagnitude` 以避免开方运算，提升性能。

## 构造函数

### FVector2(T x, T y)

用指定的 x 和 y 分量创建向量。

```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4));
// v = (3, 4)
```

### FVector2(FVector2\<T\> other)

拷贝构造，创建与指定向量分量相同的向量。

```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2));
var b = new FVector2<Fixed64>(a);
// b = (1, 2)
```

## 静态常量

| 常量 | 值 | 说明 |
|------|----|------|
| `Zero` | (0, 0) | 零向量 |
| `One` | (1, 1) | 全1向量 |
| `Up` | (0, 1) | 向上方向 |
| `Down` | (0, -1) | 向下方向 |
| `Left` | (-1, 0) | 向左方向 |
| `Right` | (1, 0) | 向右方向 |

```csharp
var pos = FVector2<Fixed64>.Right * Fixed64.FromInt(5);
// pos = (5, 0)
```

## 实例方法

### void Normalize()

将当前向量归一化为单位向量（长度为 1）。如果向量为零向量，则不进行操作。

```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4));
v.Normalize();
// v ≈ (0.6, 0.8)
```

### void Scale(FVector2\<T\> scale)

将当前向量的各分量与 `scale` 对应分量相乘，就地修改当前向量。

```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(2), Fixed64.FromInt(3));
v.Scale(new FVector2<Fixed64>(Fixed64.FromInt(4), Fixed64.FromInt(5)));
// v = (8, 15)
```

### override string ToString()

返回向量的字符串表示，格式为 `[X,Y]`。

```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2));
var s = v.ToString(); // "[1,2]"
```

### override int GetHashCode()

返回向量的哈希码，基于 X 和 Y 分量的哈希值组合。

### override bool Equals(object other)

判断当前向量是否与指定对象相等。当 `other` 为 `FVector2<T>` 且各分量相等时返回 `true`。

### bool Equals(FVector2\<T\> other)

判断当前向量是否与指定向量相等，各分量完全相同时返回 `true`。

## 静态方法

### Angle

```csharp
public static T Angle(FVector2<T> from, FVector2<T> to)
```

**描述**：计算两个向量之间的角度（单位：度），返回值范围为 [0, 180]。

**参数**：
- `from`：起始向量
- `to`：目标向量

**返回值**：两向量之间的角度（度），类型为 `T`

**计算公式**：`arccos(dot(from, to) / (|from| × |to|))` 并转换为度

**示例**：
```csharp
var a = FVector2<Fixed64>.Right;   // (1, 0)
var b = FVector2<Fixed64>.Up;      // (0, 1)
var angle = FVector2<Fixed64>.Angle(a, b);
// angle ≈ 90
```

**注意事项**：如果任一向量为零向量，返回 0。

---

### SignedAngle

```csharp
public static T SignedAngle(FVector2<T> from, FVector2<T> to)
```

**描述**：计算两个向量之间的带符号角度（单位：度）。从 `from` 到 `to` 逆时针旋转为正，顺时针为负。

**参数**：
- `from`：起始向量
- `to`：目标向量

**返回值**：带符号的角度（度），范围为 (-180, 180]

**示例**：
```csharp
var a = FVector2<Fixed64>.Right;   // (1, 0)
var b = FVector2<Fixed64>.Up;      // (0, 1)
var angle = FVector2<Fixed64>.SignedAngle(a, b);
// angle ≈ 90（逆时针）

var c = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(-1));
var angle2 = FVector2<Fixed64>.SignedAngle(a, c);
// angle2 ≈ -45（顺时针）
```

**注意事项**：符号由二维叉积 `from.X * to.Y - from.Y * to.X` 的符号决定。

---

### Cross

```csharp
public static T Cross(FVector2<T> lhs, FVector2<T> rhs)
```

**描述**：计算二维叉积，返回标量值。二维叉积等于两向量构成的平行四边形的有向面积。

**参数**：
- `lhs`：左侧向量
- `rhs`：右侧向量

**返回值**：标量 `T`，值为 `lhs.X * rhs.Y - lhs.Y * rhs.X`

**示例**：
```csharp
var a = FVector2<Fixed64>.Right;   // (1, 0)
var b = FVector2<Fixed64>.Up;      // (0, 1)
var cross = FVector2<Fixed64>.Cross(a, b);
// cross = 1（正值表示 b 在 a 的逆时针方向）
```

**注意事项**：
- 返回值为正，表示 `rhs` 在 `lhs` 的逆时针方向
- 返回值为负，表示 `rhs` 在 `lhs` 的顺时针方向
- 返回值为零，表示两向量共线
- 与 `FVector3` 的叉积不同，2D 叉积返回标量而非向量

---

### Distance

```csharp
public static T Distance(FVector2<T> a, FVector2<T> b)
```

**描述**：计算两点之间的欧几里得距离。

**参数**：
- `a`：第一个点
- `b`：第二个点

**返回值**：两点之间的距离，类型为 `T`

**计算公式**：`√((a.X - b.X)² + (a.Y - b.Y)²)`

**示例**：
```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2));
var b = new FVector2<Fixed64>(Fixed64.FromInt(4), Fixed64.FromInt(6));
var dist = FVector2<Fixed64>.Distance(a, b);
// dist = 5
```

---

### Dot

```csharp
public static T Dot(FVector2<T> lhs, FVector2<T> rhs)
```

**描述**：计算两个向量的点积（内积）。

**参数**：
- `lhs`：左侧向量
- `rhs`：右侧向量

**返回值**：点积值，类型为 `T`

**计算公式**：`lhs.X * rhs.X + lhs.Y * rhs.Y`

**示例**：
```csharp
var a = FVector2<Fixed64>.Right;   // (1, 0)
var b = FVector2<Fixed64>.Right;   // (1, 0)
var dot = FVector2<Fixed64>.Dot(a, b);
// dot = 1（同向）
```

**注意事项**：
- 点积 > 0：两向量夹角为锐角（大致同向）
- 点积 = 0：两向量正交（垂直）
- 点积 < 0：两向量夹角为钝角（大致反向）

---

### Lerp

```csharp
public static FVector2<T> Lerp(FVector2<T> a, FVector2<T> b, T t)
```

**描述**：在向量 `a` 和 `b` 之间进行线性插值。当 `t = 0` 时返回 `a`，`t = 1` 时返回 `b`。

**参数**：
- `a`：起始向量
- `b`：目标向量
- `t`：插值参数

**返回值**：插值结果向量

**计算公式**：`a + (b - a) * t`

**示例**：
```csharp
var a = FVector2<Fixed64>.Zero;
var b = FVector2<Fixed64>.One;
var mid = FVector2<Fixed64>.Lerp(a, b, Fixed64.FromFraction(1, 2));
// mid ≈ (0.5, 0.5)
```

**注意事项**：此方法不限制 `t` 的范围，`t` 可以小于 0 或大于 1，结果会超出 `[a, b]` 范围。如需限制，请使用 `ClampLerp`。

---

### ClampLerp

```csharp
public static FVector2<T> ClampLerp(FVector2<T> a, FVector2<T> b, T t)
```

**描述**：在向量 `a` 和 `b` 之间进行钳制线性插值。`t` 会被限制在 [0, 1] 范围内。

**参数**：
- `a`：起始向量
- `b`：目标向量
- `t`：插值参数（自动钳制到 [0, 1]）

**返回值**：插值结果向量，保证在 `[a, b]` 范围内

**示例**：
```csharp
var a = FVector2<Fixed64>.Zero;
var b = FVector2<Fixed64>.One;
var result = FVector2<Fixed64>.ClampLerp(a, b, Fixed64.FromInt(2));
// t 被钳制为 1，result = (1, 1)
```

---

### ClampMagnitude

```csharp
public static FVector2<T> ClampMagnitude(FVector2<T> vector, T maxMagnitude)
```

**描述**：将向量的长度限制在指定的最大值内。如果向量长度超过 `maxMagnitude`，则返回同方向但长度为 `maxMagnitude` 的向量；否则返回原向量。

**参数**：
- `vector`：要限制的向量
- `maxMagnitude`：最大长度

**返回值**：长度不超过 `maxMagnitude` 的向量

**示例**：
```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4));
// |v| = 5
var clamped = FVector2<Fixed64>.ClampMagnitude(v, Fixed64.FromInt(2));
// clamped ≈ (1.2, 1.6)，长度为 2
```

---

### Max

```csharp
public static FVector2<T> Max(FVector2<T> lhs, FVector2<T> rhs)
```

**描述**：返回各分量取最大值组成的新向量。

**参数**：
- `lhs`：第一个向量
- `rhs`：第二个向量

**返回值**：各分量为 `lhs` 和 `rhs` 对应分量最大值的新向量

**示例**：
```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(5));
var b = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(2));
var result = FVector2<Fixed64>.Max(a, b);
// result = (3, 5)
```

---

### Min

```csharp
public static FVector2<T> Min(FVector2<T> lhs, FVector2<T> rhs)
```

**描述**：返回各分量取最小值组成的新向量。

**参数**：
- `lhs`：第一个向量
- `rhs`：第二个向量

**返回值**：各分量为 `lhs` 和 `rhs` 对应分量最小值的新向量

**示例**：
```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(5));
var b = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(2));
var result = FVector2<Fixed64>.Min(a, b);
// result = (1, 2)
```

---

### Multiply3

```csharp
public static FVector2<T> Multiply3(FVector2<T> a, FVector2<T> b, FVector2<T> c)
```

**描述**：计算二维向量的三重积（Triple Product）。二维三重积利用叉积的标量结果与第三个向量组合，生成新向量。

**参数**：
- `a`：第一个向量
- `b`：第二个向量
- `c`：第三个向量

**返回值**：三重积结果向量

**计算公式**：设 `z = a.X * b.Y - a.Y * b.X`（即 `Cross(a, b)`），则结果为 `(-z * c.Y, z * c.X)`

**示例**：
```csharp
var a = FVector2<Fixed64>.Right;
var b = FVector2<Fixed64>.Up;
var c = new FVector2<Fixed64>(Fixed64.FromInt(2), Fixed64.FromInt(3));
var result = FVector2<Fixed64>.Multiply3(a, b, c);
// Cross(a, b) = 1，result = (-3, 2)
```

**注意事项**：三重积在碰撞检测和物理模拟中常用于计算支撑点。

---

### Normalize

```csharp
public static FVector2<T> Normalize(FVector2<T> v)
```

**描述**：返回向量的归一化结果（单位向量）。如果向量为零向量，返回 `Zero`。

**参数**：
- `v`：要归一化的向量

**返回值**：长度为 1 的同方向向量，或零向量

**示例**：
```csharp
var v = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4));
var n = FVector2<Fixed64>.Normalize(v);
// n ≈ (0.6, 0.8)
```

---

### Perpendicular

```csharp
public static FVector2<T> Perpendicular(FVector2<T> v)
```

**描述**：返回垂直于原向量的新向量（逆时针旋转 90 度）。

**参数**：
- `v`：原向量

**返回值**：逆时针旋转 90 度后的向量 `(-v.Y, v.X)`

**示例**：
```csharp
var v = FVector2<Fixed64>.Right;  // (1, 0)
var p = FVector2<Fixed64>.Perpendicular(v);
// p = (0, 1) = Up
```

**注意事项**：此方法仅存在于 `FVector2` 中，`FVector3` 没有对应方法，因为三维空间中垂直方向不唯一。

---

### Reflect

```csharp
public static FVector2<T> Reflect(FVector2<T> direction, FVector2<T> normal)
```

**描述**：根据法线反射入射方向向量。常用于光线反射、碰撞反弹等场景。

**参数**：
- `direction`：入射方向向量
- `normal`：反射面的法线向量（应为单位向量）

**返回值**：反射后的方向向量

**计算公式**：`direction - 2 * dot(normal, direction) * normal`

**示例**：
```csharp
var dir = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(-1));
var normal = FVector2<Fixed64>.Up;  // (0, 1)
var reflected = FVector2<Fixed64>.Reflect(dir, normal);
// reflected ≈ (1, 1)（Y 分量被反转）
```

**注意事项**：`normal` 应该是归一化的单位向量，否则反射结果不正确。

---

### Rotate

```csharp
public static FVector2<T> Rotate(FVector2<T> vector, T radians)
```

**描述**：将向量旋转指定弧度。正值为逆时针旋转，负值为顺时针旋转。

**参数**：
- `vector`：要旋转的向量
- `radians`：旋转弧度（逆时针为正）

**返回值**：旋转后的向量

**计算公式**：`(X * cos(θ) - Y * sin(θ), X * sin(θ) + Y * cos(θ))`

**示例**：
```csharp
var v = FVector2<Fixed64>.Right;  // (1, 0)
var pi = Fixed64.Pi;
var rotated = FVector2<Fixed64>.Rotate(v, pi / 2);
// rotated ≈ (0, 1) = Up（逆时针旋转 90 度）
```

**注意事项**：此方法仅存在于 `FVector2` 中。`FVector3` 使用 `RotateTowards` 方法进行旋转。

---

### Scale

```csharp
public static FVector2<T> Scale(FVector2<T> a, FVector2<T> b)
```

**描述**：将两个向量的对应分量相乘，返回新向量。

**参数**：
- `a`：第一个向量
- `b`：第二个向量

**返回值**：各分量为 `a` 和 `b` 对应分量乘积的新向量

**示例**：
```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(2), Fixed64.FromInt(3));
var b = new FVector2<Fixed64>(Fixed64.FromInt(4), Fixed64.FromInt(5));
var result = FVector2<Fixed64>.Scale(a, b);
// result = (8, 15)
```

**注意事项**：与实例方法 `Scale` 不同，此静态方法不修改原向量，而是返回新向量。

---

### MoveTowards

```csharp
public static FVector2<T> MoveTowards(FVector2<T> current, FVector2<T> target, T maxDistanceDelta)
```

**描述**：将当前点向目标点移动，移动距离不超过 `maxDistanceDelta`。如果距离小于 `maxDistanceDelta`，则直接返回目标点。

**参数**：
- `current`：当前位置
- `target`：目标位置
- `maxDistanceDelta`：每步最大移动距离

**返回值**：移动后的位置

**示例**：
```csharp
var current = FVector2<Fixed64>.Zero;
var target = new FVector2<Fixed64>(Fixed64.FromInt(10), Fixed64.FromInt(0));
var next = FVector2<Fixed64>.MoveTowards(current, target, Fixed64.FromInt(3));
// next = (3, 0)，向目标移动了 3 个单位
```

**注意事项**：当 `maxDistanceDelta` 为负值时，将远离目标移动。当距离为零或已到达目标时返回 `target`。

---

## 运算符

| 运算符 | 签名 | 说明 |
|--------|------|------|
| `+` | `FVector2<T> +(FVector2<T> a, FVector2<T> b)` | 向量加法，各分量相加 |
| `-` | `FVector2<T> -(FVector2<T> a, FVector2<T> b)` | 向量减法，各分量相减 |
| `*` | `FVector2<T> *(FVector2<T> a, FVector2<T> b)` | 对应分量相乘 |
| `/` | `FVector2<T> /(FVector2<T> a, FVector2<T> b)` | 对应分量相除 |
| `-` | `FVector2<T> -(FVector2<T> a)` | 取反，各分量取负 |
| `*` | `FVector2<T> *(FVector2<T> a, T d)` | 向量乘以标量 |
| `*` | `FVector2<T> *(T d, FVector2<T> a)` | 标量乘以向量 |
| `/` | `FVector2<T> /(FVector2<T> a, T d)` | 向量除以标量 |
| `==` | `bool ==(FVector2<T> lhs, FVector2<T> rhs)` | 判断两向量是否相等 |
| `!=` | `bool !=(FVector2<T> lhs, FVector2<T> rhs)` | 判断两向量是否不等 |

```csharp
var a = new FVector2<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2));
var b = new FVector2<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4));

var sum = a + b;           // (4, 6)
var diff = b - a;          // (2, 2)
var comp = a * b;          // (3, 8)
var neg = -a;              // (-1, -2)
var scaled = a * Fixed64.FromInt(2);  // (2, 4)
var div = b / Fixed64.FromInt(2);     // (1.5, 2)
```