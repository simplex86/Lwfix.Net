# FVector3

基于定点数的三维向量

## 结构定义

```csharp
public partial struct FVector3<T> where T : struct, IFixed<T>
```

**命名空间**：`SimplexLab.Fixed`

## 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `X` | `T` | X 分量（可读写） |
| `Y` | `T` | Y 分量（可读写） |
| `Z` | `T` | Z 分量（可读写） |
| `Magnitude` | `T` | 向量的长度（只读），计算公式：`√(X² + Y² + Z²)` |
| `SqrMagnitude` | `T` | 向量的长度平方（只读），计算公式：`X² + Y² + Z²` |
| `Normalized` | `FVector3<T>` | 归一化后的向量（只读），长度为 1 的同方向向量 |

> **提示**：当只需要比较向量长度时，优先使用 `SqrMagnitude` 以避免开方运算，提升性能。

## 构造函数

### FVector3(T x, T y, T z)

用指定的 x、y 和 z 分量创建向量。

```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2), Fixed64.FromInt(3));
// v = (1, 2, 3)
```

### FVector3(FVector3\<T\> other)

拷贝构造，创建与指定向量分量相同的向量。

```csharp
var a = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2), Fixed64.FromInt(3));
var b = new FVector3<Fixed64>(a);
// b = (1, 2, 3)
```

## 静态常量

| 常量 | 值 | 说明 |
|------|----|------|
| `Zero` | (0, 0, 0) | 零向量 |
| `One` | (1, 1, 1) | 全1向量 |
| `Up` | (0, 1, 0) | 向上方向 |
| `Down` | (0, -1, 0) | 向下方向 |
| `Left` | (-1, 0, 0) | 向左方向 |
| `Right` | (1, 0, 0) | 向右方向 |
| `Forward` | (0, 0, 1) | 向前方向 |
| `Back` | (0, 0, -1) | 向后方向 |

```csharp
var pos = FVector3<Fixed64>.Forward * Fixed64.FromInt(10);
// pos = (0, 0, 10)
```

## 实例方法

### void Normalize()

将当前向量归一化为单位向量（长度为 1）。如果向量为零向量，则不进行操作。

```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(0), Fixed64.FromInt(3), Fixed64.FromInt(4));
v.Normalize();
// v ≈ (0, 0.6, 0.8)
```

### void Scale(FVector3\<T\> scale)

将当前向量的各分量与 `scale` 对应分量相乘，就地修改当前向量。

```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(2), Fixed64.FromInt(3), Fixed64.FromInt(4));
v.Scale(new FVector3<Fixed64>(Fixed64.FromInt(5), Fixed64.FromInt(6), Fixed64.FromInt(7)));
// v = (10, 18, 28)
```

### override string ToString()

返回向量的字符串表示，格式为 `[X,Y,Z]`。

```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2), Fixed64.FromInt(3));
var s = v.ToString(); // "[1,2,3]"
```

### override int GetHashCode()

返回向量的哈希码，基于 X、Y 和 Z 分量的哈希值组合。

### override bool Equals(object other)

判断当前向量是否与指定对象相等。当 `other` 为 `FVector3<T>` 且各分量相等时返回 `true`。

### bool Equals(FVector3\<T\> other)

判断当前向量是否与指定向量相等，各分量完全相同时返回 `true`。

## 静态方法

### Angle

```csharp
public static T Angle(FVector3<T> from, FVector3<T> to)
```

**描述**：计算两个向量之间的角度（单位：度），返回值范围为 [0, 180]。

**参数**：
- `from`：起始向量
- `to`：目标向量

**返回值**：两向量之间的角度（度），类型为 `T`

**计算公式**：`arccos(dot(from, to) / (|from| × |to|))` 并转换为度

**示例**：
```csharp
var a = FVector3<Fixed64>.Right;    // (1, 0, 0)
var b = FVector3<Fixed64>.Forward;  // (0, 0, 1)
var angle = FVector3<Fixed64>.Angle(a, b);
// angle ≈ 90
```

**注意事项**：如果任一向量为零向量，返回 0。

---

### SignedAngle

```csharp
public static T SignedAngle(FVector3<T> from, FVector3<T> to, FVector3<T> axis)
```

**描述**：计算两个向量之间相对于指定旋转轴的带符号角度（单位：度）。

**参数**：
- `from`：起始向量
- `to`：目标向量
- `axis`：旋转轴向量，用于确定角度的符号

**返回值**：带符号的角度（度）

**示例**：
```csharp
var from = FVector3<Fixed64>.Right;    // (1, 0, 0)
var to = FVector3<Fixed64>.Forward;    // (0, 0, 1)
var axis = FVector3<Fixed64>.Up;       // (0, 1, 0)
var angle = FVector3<Fixed64>.SignedAngle(from, to, axis);
// angle ≈ 90（绕 Y 轴逆时针旋转）
```

**注意事项**：
- 与 `FVector2.SignedAngle` 不同，3D 版本需要指定旋转轴，因为在三维空间中角度的符号取决于旋转方向
- 符号由叉积在轴上的投影决定

---

### Cross

```csharp
public static FVector3<T> Cross(FVector3<T> lhs, FVector3<T> rhs)
```

**描述**：计算三维叉积，返回垂直于两输入向量的向量。

**参数**：
- `lhs`：左侧向量
- `rhs`：右侧向量

**返回值**：叉积结果向量 `FVector3<T>`

**计算公式**：
- `X = lhs.Y * rhs.Z - lhs.Z * rhs.Y`
- `Y = lhs.Z * rhs.X - lhs.X * rhs.Z`
- `Z = lhs.X * rhs.Y - lhs.Y * rhs.X`

**示例**：
```csharp
var a = FVector3<Fixed64>.Right;    // (1, 0, 0)
var b = FVector3<Fixed64>.Forward;  // (0, 0, 1)
var cross = FVector3<Fixed64>.Cross(a, b);
// cross ≈ (0, -1, 0) = Down（注意方向）
```

**注意事项**：
- 叉积结果垂直于两输入向量
- 叉积不满足交换律：`Cross(a, b) = -Cross(b, a)`
- 与 `FVector2.Cross` 不同，3D 叉积返回向量而非标量
- 叉积的长度等于两向量构成的平行四边形面积

---

### Distance

```csharp
public static T Distance(FVector3<T> a, FVector3<T> b)
```

**描述**：计算两点之间的欧几里得距离。

**参数**：
- `a`：第一个点
- `b`：第二个点

**返回值**：两点之间的距离，类型为 `T`

**计算公式**：`√((a.X - b.X)² + (a.Y - b.Y)² + (a.Z - b.Z)²)`

**示例**：
```csharp
var a = FVector3<Fixed64>.Zero;
var b = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2), Fixed64.FromInt(2));
var dist = FVector3<Fixed64>.Distance(a, b);
// dist = 3
```

---

### Dot

```csharp
public static T Dot(FVector3<T> lhs, FVector3<T> rhs)
```

**描述**：计算两个向量的点积（内积）。

**参数**：
- `lhs`：左侧向量
- `rhs`：右侧向量

**返回值**：点积值，类型为 `T`

**计算公式**：`lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z`

**示例**：
```csharp
var a = FVector3<Fixed64>.Forward;  // (0, 0, 1)
var b = FVector3<Fixed64>.Forward;  // (0, 0, 1)
var dot = FVector3<Fixed64>.Dot(a, b);
// dot = 1（同向）
```

**注意事项**：
- 点积 > 0：两向量夹角为锐角
- 点积 = 0：两向量正交
- 点积 < 0：两向量夹角为钝角

---

### Lerp

```csharp
public static FVector3<T> Lerp(FVector3<T> a, FVector3<T> b, T t)
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
var a = FVector3<Fixed64>.Zero;
var b = FVector3<Fixed64>.One;
var mid = FVector3<Fixed64>.Lerp(a, b, Fixed64.FromFraction(1, 2));
// mid ≈ (0.5, 0.5, 0.5)
```

**注意事项**：此方法不限制 `t` 的范围。如需限制，请使用 `ClampLerp`。

---

### ClampLerp

```csharp
public static FVector3<T> ClampLerp(FVector3<T> a, FVector3<T> b, T t)
```

**描述**：在向量 `a` 和 `b` 之间进行钳制线性插值。`t` 会被限制在 [0, 1] 范围内。

**参数**：
- `a`：起始向量
- `b`：目标向量
- `t`：插值参数（自动钳制到 [0, 1]）

**返回值**：插值结果向量，保证在 `[a, b]` 范围内

**示例**：
```csharp
var a = FVector3<Fixed64>.Zero;
var b = FVector3<Fixed64>.One;
var result = FVector3<Fixed64>.ClampLerp(a, b, Fixed64.FromInt(-1));
// t 被钳制为 0，result = (0, 0, 0)
```

---

### Slerp

```csharp
public static FVector3<T> Slerp(FVector3<T> from, FVector3<T> to, T t)
```

**描述**：在两个向量之间进行球面线性插值（Spherical Linear Interpolation）。与 `Lerp` 不同，`Slerp` 沿球面弧线插值，保持旋转的均匀性。

**参数**：
- `from`：起始向量（应为单位向量）
- `to`：目标向量（应为单位向量）
- `t`：插值参数

**返回值**：球面插值结果向量

**示例**：
```csharp
var from = FVector3<Fixed64>.Right;    // (1, 0, 0)
var to = FVector3<Fixed64>.Forward;    // (0, 0, 1)
var mid = FVector3<Fixed64>.Slerp(from, to, Fixed64.FromFraction(1, 2));
// mid ≈ (0.707, 0, 0.707)，45度方向
```

**注意事项**：
- 此方法仅存在于 `FVector3` 中，`FVector2` 没有对应方法
- 输入向量应为归一化的单位向量
- 此方法不限制 `t` 的范围。如需限制，请使用 `ClampSlerp`

---

### ClampSlerp

```csharp
public static FVector3<T> ClampSlerp(FVector3<T> from, FVector3<T> to, T t)
```

**描述**：在两个向量之间进行钳制球面线性插值。`t` 会被限制在 [0, 1] 范围内。

**参数**：
- `from`：起始向量（应为单位向量）
- `to`：目标向量（应为单位向量）
- `t`：插值参数（自动钳制到 [0, 1]）

**返回值**：球面插值结果向量

**示例**：
```csharp
var from = FVector3<Fixed64>.Right;
var to = FVector3<Fixed64>.Forward;
var result = FVector3<Fixed64>.ClampSlerp(from, to, Fixed64.FromInt(2));
// t 被钳制为 1，result = to
```

**注意事项**：此方法仅存在于 `FVector3` 中。

---

### ClampMagnitude

```csharp
public static FVector3<T> ClampMagnitude(FVector3<T> vector, T maxMagnitude)
```

**描述**：将向量的长度限制在指定的最大值内。如果向量长度超过 `maxMagnitude`，则返回同方向但长度为 `maxMagnitude` 的向量；否则返回原向量。

**参数**：
- `vector`：要限制的向量
- `maxMagnitude`：最大长度

**返回值**：长度不超过 `maxMagnitude` 的向量

**示例**：
```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(0), Fixed64.FromInt(3), Fixed64.FromInt(4));
// |v| = 5
var clamped = FVector3<Fixed64>.ClampMagnitude(v, Fixed64.FromInt(2));
// clamped ≈ (0, 1.2, 1.6)，长度为 2
```

---

### Max

```csharp
public static FVector3<T> Max(FVector3<T> lhs, FVector3<T> rhs)
```

**描述**：返回各分量取最大值组成的新向量。

**参数**：
- `lhs`：第一个向量
- `rhs`：第二个向量

**返回值**：各分量为 `lhs` 和 `rhs` 对应分量最大值的新向量

**示例**：
```csharp
var a = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(5), Fixed64.FromInt(3));
var b = new FVector3<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(2), Fixed64.FromInt(6));
var result = FVector3<Fixed64>.Max(a, b);
// result = (3, 5, 6)
```

---

### Min

```csharp
public static FVector3<T> Min(FVector3<T> lhs, FVector3<T> rhs)
```

**描述**：返回各分量取最小值组成的新向量。

**参数**：
- `lhs`：第一个向量
- `rhs`：第二个向量

**返回值**：各分量为 `lhs` 和 `rhs` 对应分量最小值的新向量

**示例**：
```csharp
var a = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(5), Fixed64.FromInt(3));
var b = new FVector3<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(2), Fixed64.FromInt(6));
var result = FVector3<Fixed64>.Min(a, b);
// result = (1, 2, 3)
```

---

### Multiply3

```csharp
public static FVector3<T> Multiply3(FVector3<T> a, FVector3<T> b, FVector3<T> c)
```

**描述**：计算三维向量的三重积（Triple Product），即 `Cross(c, Cross(a, b))`。

**参数**：
- `a`：第一个向量
- `b`：第二个向量
- `c`：第三个向量

**返回值**：三重积结果向量

**计算公式**：`Cross(c, Cross(a, b))`

**示例**：
```csharp
var a = FVector3<Fixed64>.Right;
var b = FVector3<Fixed64>.Up;
var c = FVector3<Fixed64>.Forward;
var result = FVector3<Fixed64>.Multiply3(a, b, c);
// Cross(a, b) = Forward，Cross(c, Forward) = -Right
// result ≈ (-1, 0, 0)
```

**注意事项**：三重积在碰撞检测和物理模拟中常用于计算支撑点。

---

### Normalize

```csharp
public static FVector3<T> Normalize(FVector3<T> v)
```

**描述**：返回向量的归一化结果（单位向量）。如果向量为零向量，返回 `Zero`。

**参数**：
- `v`：要归一化的向量

**返回值**：长度为 1 的同方向向量，或零向量

**示例**：
```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(0), Fixed64.FromInt(3), Fixed64.FromInt(4));
var n = FVector3<Fixed64>.Normalize(v);
// n ≈ (0, 0.6, 0.8)
```

---

### OrthoNormalize（双参数）

```csharp
public static void OrthoNormalize(ref FVector3<T> normal, ref FVector3<T> tangent)
```

**描述**：将法线和切线向量正交化并归一化。法线向量被归一化，切线向量通过 Gram-Schmidt 过程被正交化后归一化。

**参数**：
- `normal`：法线向量（引用传递，会被修改为归一化的单位向量）
- `tangent`：切线向量（引用传递，会被修改为与法线正交的单位向量）

**示例**：
```csharp
var normal = new FVector3<Fixed64>(Fixed64.FromInt(0), Fixed64.FromInt(2), Fixed64.FromInt(0));
var tangent = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(1), Fixed64.FromInt(0));
FVector3<Fixed64>.OrthoNormalize(ref normal, ref tangent);
// normal = (0, 1, 0)
// tangent ≈ (1, 0, 0)（与 normal 正交且归一化）
```

**注意事项**：此方法仅存在于 `FVector3` 中。

---

### OrthoNormalize（三参数）

```csharp
public static void OrthoNormalize(ref FVector3<T> normal, ref FVector3<T> tangent, ref FVector3<T> binormal)
```

**描述**：将法线、切线和副法线向量正交化并归一化，形成正交坐标系。

**参数**：
- `normal`：法线向量（引用传递，会被归一化）
- `tangent`：切线向量（引用传递，会被正交化后归一化）
- `binormal`：副法线向量（引用传递，会被设为 `Cross(normal, tangent)` 并归一化）

**示例**：
```csharp
var normal = new FVector3<Fixed64>(Fixed64.FromInt(0), Fixed64.FromInt(1), Fixed64.FromInt(0));
var tangent = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(1), Fixed64.FromInt(0));
var binormal = FVector3<Fixed64>.Zero;
FVector3<Fixed64>.OrthoNormalize(ref normal, ref tangent, ref binormal);
// normal = (0, 1, 0)
// tangent ≈ (1, 0, 0)
// binormal ≈ (0, 0, 1) 或 (0, 0, -1)
```

**注意事项**：此方法仅存在于 `FVector3` 中。

---

### Project

```csharp
public static FVector3<T> Project(FVector3<T> vector, FVector3<T> normal)
```

**描述**：将向量投影到指定方向上。

**参数**：
- `vector`：要投影的向量
- `normal`：投影方向（不需要是单位向量）

**返回值**：投影结果向量，与 `normal` 同方向

**计算公式**：`normal * Dot(vector, normal) / Dot(normal, normal)`

**示例**：
```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4), Fixed64.FromInt(0));
var dir = FVector3<Fixed64>.Right;  // (1, 0, 0)
var proj = FVector3<Fixed64>.Project(v, dir);
// proj = (3, 0, 0)
```

**注意事项**：
- 此方法仅存在于 `FVector3` 中
- 如果 `normal` 为零向量，返回 `Zero`

---

### ProjectOnPlane

```csharp
public static FVector3<T> ProjectOnPlane(FVector3<T> vector, FVector3<T> planeNormal)
```

**描述**：将向量投影到由法线定义的平面上，即从向量中移除沿法线方向的分量。

**参数**：
- `vector`：要投影的向量
- `planeNormal`：平面的法线向量

**返回值**：投影到平面上的向量

**计算公式**：`vector - Project(vector, planeNormal)`

**示例**：
```csharp
var v = new FVector3<Fixed64>(Fixed64.FromInt(3), Fixed64.FromInt(4), Fixed64.FromInt(0));
var planeNormal = FVector3<Fixed64>.Up;  // (0, 1, 0)，即 XZ 平面
var proj = FVector3<Fixed64>.ProjectOnPlane(v, planeNormal);
// proj = (3, 0, 0)，Y 分量被移除
```

**注意事项**：
- 此方法仅存在于 `FVector3` 中
- `planeNormal` 应为单位向量

---

### Reflect

```csharp
public static FVector3<T> Reflect(FVector3<T> direction, FVector3<T> normal)
```

**描述**：根据法线反射入射方向向量。常用于光线反射、碰撞反弹等场景。

**参数**：
- `direction`：入射方向向量
- `normal`：反射面的法线向量（应为单位向量）

**返回值**：反射后的方向向量

**计算公式**：`direction - 2 * dot(normal, direction) * normal`

**示例**：
```csharp
var dir = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(-1), Fixed64.FromInt(0));
var normal = FVector3<Fixed64>.Up;  // (0, 1, 0)
var reflected = FVector3<Fixed64>.Reflect(dir, normal);
// reflected ≈ (1, 1, 0)（Y 分量被反转）
```

**注意事项**：`normal` 应该是归一化的单位向量，否则反射结果不正确。

---

### Scale

```csharp
public static FVector3<T> Scale(FVector3<T> a, FVector3<T> b)
```

**描述**：将两个向量的对应分量相乘，返回新向量。

**参数**：
- `a`：第一个向量
- `b`：第二个向量

**返回值**：各分量为 `a` 和 `b` 对应分量乘积的新向量

**示例**：
```csharp
var a = new FVector3<Fixed64>(Fixed64.FromInt(2), Fixed64.FromInt(3), Fixed64.FromInt(4));
var b = new FVector3<Fixed64>(Fixed64.FromInt(5), Fixed64.FromInt(6), Fixed64.FromInt(7));
var result = FVector3<Fixed64>.Scale(a, b);
// result = (10, 18, 28)
```

**注意事项**：与实例方法 `Scale` 不同，此静态方法不修改原向量，而是返回新向量。

---

### MoveTowards

```csharp
public static FVector3<T> MoveTowards(FVector3<T> current, FVector3<T> target, T maxDistanceDelta)
```

**描述**：将当前点向目标点移动，移动距离不超过 `maxDistanceDelta`。如果距离小于 `maxDistanceDelta`，则直接返回目标点。

**参数**：
- `current`：当前位置
- `target`：目标位置
- `maxDistanceDelta`：每步最大移动距离

**返回值**：移动后的位置

**示例**：
```csharp
var current = FVector3<Fixed64>.Zero;
var target = new FVector3<Fixed64>(Fixed64.FromInt(10), Fixed64.FromInt(0), Fixed64.FromInt(0));
var next = FVector3<Fixed64>.MoveTowards(current, target, Fixed64.FromInt(3));
// next = (3, 0, 0)，向目标移动了 3 个单位
```

**注意事项**：当 `maxDistanceDelta` 为负值时，将远离目标移动。当距离为零或已到达目标时返回 `target`。

---

### RotateTowards

```csharp
public static FVector3<T> RotateTowards(FVector3<T> current, FVector3<T> target, T maxRadiansDelta, T maxMagnitudeDelta)
```

**描述**：将当前向量向目标向量旋转，同时限制旋转角度和长度变化。这是 `FVector3` 特有的方法，`FVector2` 使用更简单的 `Rotate` 方法。

**参数**：
- `current`：当前向量
- `target`：目标向量
- `maxRadiansDelta`：最大旋转弧度
- `maxMagnitudeDelta`：最大长度变化量

**返回值**：旋转后的向量

**示例**：
```csharp
var current = FVector3<Fixed64>.Right;    // (1, 0, 0)
var target = FVector3<Fixed64>.Forward;   // (0, 0, 1)
var pi = Fixed64.Pi;
var result = FVector3<Fixed64>.RotateTowards(current, target, pi / 4, Fixed64.FromInt(1));
// 向 target 方向旋转最多 45 度，长度变化不超过 1
```

**注意事项**：
- 此方法仅存在于 `FVector3` 中，`FVector2` 使用 `Rotate` 方法
- 内部使用 `FQuaternion<T>.AngleAxis` 实现旋转
- 当 `maxRadiansDelta` 为 0 时不旋转，为负值时旋转远离目标

---

## 运算符

| 运算符 | 签名 | 说明 |
|--------|------|------|
| `+` | `FVector3<T> +(FVector3<T> a, FVector3<T> b)` | 向量加法，各分量相加 |
| `-` | `FVector3<T> -(FVector3<T> a, FVector3<T> b)` | 向量减法，各分量相减 |
| `*` | `FVector3<T> *(FVector3<T> a, FVector3<T> b)` | 对应分量相乘 |
| `/` | `FVector3<T> /(FVector3<T> a, FVector3<T> b)` | 对应分量相除 |
| `-` | `FVector3<T> -(FVector3<T> a)` | 取反，各分量取负 |
| `*` | `FVector3<T> *(FVector3<T> a, T d)` | 向量乘以标量 |
| `*` | `FVector3<T> *(T d, FVector3<T> a)` | 标量乘以向量 |
| `/` | `FVector3<T> /(FVector3<T> a, T d)` | 向量除以标量 |
| `==` | `bool ==(FVector3<T> lhs, FVector3<T> rhs)` | 判断两向量是否相等 |
| `!=` | `bool !=(FVector3<T> lhs, FVector3<T> rhs)` | 判断两向量是否不等 |

```csharp
var a = new FVector3<Fixed64>(Fixed64.FromInt(1), Fixed64.FromInt(2), Fixed64.FromInt(3));
var b = new FVector3<Fixed64>(Fixed64.FromInt(4), Fixed64.FromInt(5), Fixed64.FromInt(6));

var sum = a + b;           // (5, 7, 9)
var diff = b - a;          // (3, 3, 3)
var comp = a * b;          // (4, 10, 18)
var neg = -a;              // (-1, -2, -3)
var scaled = a * Fixed64.FromInt(2);  // (2, 4, 6)
var div = b / Fixed64.FromInt(2);     // (2, 2.5, 3)
```