# FQuaternion

`FQuaternion<T>` 是基于定点数的四元数结构体，用于表示三维空间中的旋转。四元数由四个分量 `(X, Y, Z, W)` 组成，其中 `(X, Y, Z)` 对应虚部（向量部分），`W` 对应实部（标量部分）。

与欧拉角相比，四元数避免了万向节锁（Gimbal Lock）问题，且插值运算更加平滑，因此广泛应用于三维旋转的表示与计算中。

- **命名空间**：`SimplexLab.Fixed`
- **泛型约束**：`where T : struct, IFixed<T>`
- **结构定义**：`public partial struct FQuaternion<T>`

> 本模块基于定点数实现，适用于对确定性计算有严格要求的场景（如物理模拟、网络游戏同步等），避免了浮点数的精度不确定性。

## 结构定义

```csharp
public partial struct FQuaternion<T> where T : struct, IFixed<T>
```

四元数通过 `partial struct` 拆分为多个文件组织：

| 文件 | 说明 |
|------|------|
| `Quaternion.cs` | 基础定义：字段、构造函数、`Identity`、`ToString`、`GetHashCode` |
| `Quaternion.Set.cs` | 设置方法：`Set`、`SetFromToRotation`、`SetLookRotation` |
| `Quaternion.Rotation.cs` | 旋转方法：`LookRotation`、`FromToRotation`、`RotateTowards` |
| `Quaternion.Operation.cs` | 运算符重载：`+`、`-`、`*` |
| `Quaternion.Normalize.cs` | 归一化：实例方法与静态方法 |
| `Quaternion.Lerp.cs` | 插值：`Lerp`、`ClampLerp`、`Slerp`、`ClampSlerp` |
| `Quaternion.Inverse.cs` | 逆四元数 |
| `Quaternion.Euler.cs` | 欧拉角转换 |
| `Quaternion.Dot.cs` | 点乘 |
| `Quaternion.Conjugate.cs` | 共轭 |
| `Quaternion.Compare.cs` | 比较运算与相等判断 |
| `Quaternion.Angle.cs` | 角度计算与轴角构造 |
| `Quaternion.Matrix.cs` | 矩阵转换 |

## 字段

| 字段 | 类型 | 说明 |
|------|------|------|
| `X` | `T` | X 分量（虚部 i） |
| `Y` | `T` | Y 分量（虚部 j） |
| `Z` | `T` | Z 分量（虚部 k） |
| `W` | `T` | W 分量（实部） |

> 四元数的一般形式为 `q = W + Xi + Yj + Zk`，其中 `W` 为标量部分，`(X, Y, Z)` 为向量部分。

## 静态属性

### Identity

```csharp
public static FQuaternion<T> Identity { get; }
```

**描述**：单位四元数，表示无旋转。值为 `(0, 0, 0, 1)`。

**示例**：

```csharp
var identity = FQuaternion<Fixed32>.Identity;
// identity = (0, 0, 0, 1)
```

**备注**：任何四元数与单位四元数相乘，结果不变，即 `q * Identity = q`。

## 构造函数

### FQuaternion(T x, T y, T z, T w)

```csharp
public FQuaternion(T x, T y, T z, T w)
```

**描述**：用四个分量创建四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 分量 |
| `y` | `T` | Y 分量 |
| `z` | `T` | Z 分量 |
| `w` | `T` | W 分量 |

**示例**：

```csharp
var q = new FQuaternion<Fixed32>(
    (Fixed32)0.1,
    (Fixed32)0.2,
    (Fixed32)0.3,
    (Fixed32)0.9
);
```

**备注**：构造函数不会自动归一化。如果需要单位四元数，请在构造后调用 `Normalize()`。

## 实例方法

### Normalize

```csharp
public void Normalize()
```

**描述**：将当前四元数归一化为单位四元数。归一化后四元数的模长为 1。

**示例**：

```csharp
var q = new FQuaternion<Fixed32>(1, 2, 3, 4);
q.Normalize();
// 归一化后 q 的模长为 1
```

**备注**：此方法会修改当前实例。如果四元数的模长为零，则归一化结果为 `Identity`。

---

### Set

```csharp
public void Set(T x, T y, T z, T w)
```

**描述**：设置四元数的四个分量。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | 新的 X 分量 |
| `y` | `T` | 新的 Y 分量 |
| `z` | `T` | 新的 Z 分量 |
| `w` | `T` | 新的 W 分量 |

**示例**：

```csharp
var q = FQuaternion<Fixed32>.Identity;
q.Set((Fixed32)0.1, (Fixed32)0.2, (Fixed32)0.3, (Fixed32)0.9);
// q = (0.1, 0.2, 0.3, 0.9)
```

---

### SetFromToRotation

```csharp
public void SetFromToRotation(FVector3<T> from, FVector3<T> to)
```

**描述**：将当前四元数设置为从方向 `from` 旋转到方向 `to` 的四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FVector3<T>` | 起始方向向量 |
| `to` | `FVector3<T>` | 目标方向向量 |

**示例**：

```csharp
var q = new FQuaternion<Fixed32>();
q.SetFromToRotation(FVector3<Fixed32>.Right, FVector3<Fixed32>.Up);
// q 表示从 X 轴正方向旋转到 Y 轴正方向的旋转
```

**备注**：内部调用 `FromToRotation` 静态方法并赋值给当前实例。

---

### SetLookRotation

```csharp
public void SetLookRotation(FVector3<T> view)
public void SetLookRotation(FVector3<T> view, FVector3<T> upwards)
```

**描述**：将当前四元数设置为朝向指定方向的旋转。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `view` | `FVector3<T>` | 朝向方向（前方向） |
| `upwards` | `FVector3<T>` | 上方向（默认为 `FVector3<T>.Up`） |

**示例**：

```csharp
var q = new FQuaternion<Fixed32>();
q.SetLookRotation(FVector3<Fixed32>.Forward);
// q 朝向 Z 轴正方向

q.SetLookRotation(FVector3<Fixed32>.Forward, FVector3<Fixed32>.Up);
// q 朝向 Z 轴正方向，上方向为 Y 轴正方向
```

**备注**：单参数版本默认使用 `FVector3<T>.Up` 作为上方向。

---

### ToString

```csharp
public override string ToString()
```

**描述**：返回四元数的字符串表示，格式为 `(X, Y, Z, W)`。

**返回值**：格式化后的字符串。

**示例**：

```csharp
var q = FQuaternion<Fixed32>.Identity;
Console.WriteLine(q.ToString()); // 输出: (0, 0, 0, 1)
```

---

### GetHashCode

```csharp
public override int GetHashCode()
```

**描述**：返回四元数的哈希码。

**返回值**：基于四个分量的组合哈希值。

---

### Equals

```csharp
public override bool Equals(object other)
public bool Equals(FQuaternion<T> other)
```

**描述**：判断当前四元数是否与另一个对象相等。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `other` | `object` / `FQuaternion<T>` | 要比较的对象 |

**返回值**：如果相等返回 `true`，否则返回 `false`。

**备注**：泛型版本 `Equals(FQuaternion<T>)` 逐分量比较是否相等。`object` 版本先进行类型检查，再调用泛型版本。

## 静态方法

### Angle

```csharp
public static T Angle(FQuaternion<T> a, FQuaternion<T> b)
```

**描述**：计算两个四元数之间的角度（单位：度）。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `a` | `FQuaternion<T>` | 第一个四元数 |
| `b` | `FQuaternion<T>` | 第二个四元数 |

**返回值**：两个旋转之间的角度，单位为度，类型为 `T`。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Euler(0, 0, 0);
var q2 = FQuaternion<Fixed32>.Euler(0, 90, 0);
var angle = FQuaternion<Fixed32>.Angle(q1, q2);
// angle ≈ 90
```

**备注**：内部通过点积计算，结果范围为 `[0, 180]` 度。如果两个四元数表示相同旋转（点积为 1），返回 0。

---

### AngleAxis

```csharp
public static FQuaternion<T> AngleAxis(T angle, FVector3<T> axis)
```

**描述**：创建绕指定轴旋转指定角度的四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `angle` | `T` | 旋转角度（单位：度） |
| `axis` | `FVector3<T>` | 旋转轴（将被自动归一化） |

**返回值**：表示该旋转的四元数。

**示例**：

```csharp
// 绕 Y 轴旋转 90 度
var q = FQuaternion<Fixed32>.AngleAxis(
    (Fixed32)90,
    FVector3<Fixed32>.Up
);
```

**备注**：旋转轴会在内部被归一化。角度以度为单位，内部转换为弧度进行计算。公式为：

```
q = (axis * sin(angle/2), cos(angle/2))
```

---

### Conjugate

```csharp
public static FQuaternion<T> Conjugate(FQuaternion<T> value)
```

**描述**：返回四元数的共轭。共轭四元数将向量部分取反。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `value` | `FQuaternion<T>` | 原始四元数 |

**返回值**：共轭四元数 `(−X, −Y, −Z, W)`。

**示例**：

```csharp
var q = new FQuaternion<Fixed32>(1, 2, 3, 4);
var conj = FQuaternion<Fixed32>.Conjugate(q);
// conj = (-1, -2, -3, 4)
```

**备注**：对于单位四元数，共轭等于逆。共轭的几何意义是反方向旋转。

---

### Dot

```csharp
public static T Dot(FQuaternion<T> a, FQuaternion<T> b)
```

**描述**：计算两个四元数的点积。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `a` | `FQuaternion<T>` | 第一个四元数 |
| `b` | `FQuaternion<T>` | 第二个四元数 |

**返回值**：点积值 `a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z`。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Identity;
var q2 = FQuaternion<Fixed32>.Identity;
var dot = FQuaternion<Fixed32>.Dot(q1, q2);
// dot = 1（两个相同单位四元数的点积为 1）
```

**备注**：点积为 1 表示两个四元数表示相同的旋转；点积为 -1 表示旋转相反。点积的绝对值可用于判断两个旋转的相似程度。

---

### Euler

```csharp
public static FQuaternion<T> Euler(T x, T y, T z)
public static FQuaternion<T> Euler(FVector3<T> eulerAngles)
```

**描述**：从欧拉角创建四元数。旋转顺序为 Z-X-Y（即先绕 Z 轴旋转，再绕 X 轴旋转，最后绕 Y 轴旋转）。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | 绕 X 轴的旋转角度（度） |
| `y` | `T` | 绕 Y 轴的旋转角度（度） |
| `z` | `T` | 绕 Z 轴的旋转角度（度） |
| `eulerAngles` | `FVector3<T>` | 欧拉角向量（X, Y, Z 分量分别为绕各轴的旋转角度，单位：度） |

**返回值**：对应的四元数。

**示例**：

```csharp
// 绕 Y 轴旋转 90 度
var q1 = FQuaternion<Fixed32>.Euler(0, 90, 0);

// 使用向量形式
var angles = new FVector3<Fixed32>(0, 90, 0);
var q2 = FQuaternion<Fixed32>.Euler(angles);
```

**备注**：角度以度为单位，内部通过 `DegToRad` 转换为弧度后计算。向量版本等价于 `Euler(eulerAngles.X, eulerAngles.Y, eulerAngles.Z)`。

---

### Inverse

```csharp
public static FQuaternion<T> Inverse(FQuaternion<T> rotation)
```

**描述**：返回四元数的逆。逆四元数表示与原四元数相反的旋转。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `rotation` | `FQuaternion<T>` | 原始四元数 |

**返回值**：逆四元数。

**示例**：

```csharp
var q = FQuaternion<Fixed32>.AngleAxis(45, FVector3<Fixed32>.Up);
var inv = FQuaternion<Fixed32>.Inverse(q);
// q * inv = Identity
```

**备注**：逆四元数 = 共轭 / 模长的平方。对于单位四元数，逆等于共轭。`q * Inverse(q) = Identity`。

---

### Lerp

```csharp
public static FQuaternion<T> Lerp(FQuaternion<T> from, FQuaternion<T> to, T t)
```

**描述**：对两个四元数进行线性插值。结果会自动归一化。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FQuaternion<T>` | 起始四元数 |
| `to` | `FQuaternion<T>` | 目标四元数 |
| `t` | `T` | 插值参数，通常在 `[0, 1]` 范围内 |

**返回值**：插值后的四元数。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Euler(0, 0, 0);
var q2 = FQuaternion<Fixed32>.Euler(0, 90, 0);
var mid = FQuaternion<Fixed32>.Lerp(q1, q2, (Fixed32)0.5);
// mid 约为绕 Y 轴旋转 45 度
```

**备注**：`t` 不被钳制，超出 `[0, 1]` 范围时会进行外推。如果需要钳制，请使用 `ClampLerp`。线性插值的旋转速度不恒定，如需匀速旋转请使用 `Slerp`。

---

### ClampLerp

```csharp
public static FQuaternion<T> ClampLerp(FQuaternion<T> from, FQuaternion<T> to, T t)
```

**描述**：对两个四元数进行钳制线性插值。`t` 被限制在 `[0, 1]` 范围内。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FQuaternion<T>` | 起始四元数 |
| `to` | `FQuaternion<T>` | 目标四元数 |
| `t` | `T` | 插值参数，将被钳制到 `[0, 1]` |

**返回值**：插值后的四元数。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Euler(0, 0, 0);
var q2 = FQuaternion<Fixed32>.Euler(0, 90, 0);
var result = FQuaternion<Fixed32>.ClampLerp(q1, q2, (Fixed32)1.5);
// t 被钳制为 1，结果等于 q2
```

**备注**：内部通过 `T.Clamp01(t)` 将 `t` 限制在 `[0, 1]`，然后调用 `Lerp`。

---

### Slerp

```csharp
public static FQuaternion<T> Slerp(FQuaternion<T> from, FQuaternion<T> to, T t)
```

**描述**：对两个四元数进行球面线性插值（Spherical Linear Interpolation）。插值在四元数所在的超球面上进行，保证角速度恒定。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FQuaternion<T>` | 起始四元数 |
| `to` | `FQuaternion<T>` | 目标四元数 |
| `t` | `T` | 插值参数，通常在 `[0, 1]` 范围内 |

**返回值**：球面插值后的四元数。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Euler(0, 0, 0);
var q2 = FQuaternion<Fixed32>.Euler(0, 90, 0);
var mid = FQuaternion<Fixed32>.Slerp(q1, q2, (Fixed32)0.5);
// mid 为绕 Y 轴旋转 45 度，角速度恒定
```

**备注**：`t` 不被钳制，超出 `[0, 1]` 范围时会进行外推。如果两个四元数表示相同旋转（点积为 1），直接返回 `from`。如需钳制请使用 `ClampSlerp`。球面插值比线性插值计算量更大，但旋转速度更均匀。

---

### ClampSlerp

```csharp
public static FQuaternion<T> ClampSlerp(FQuaternion<T> from, FQuaternion<T> to, T t)
```

**描述**：对两个四元数进行钳制球面线性插值。`t` 被限制在 `[0, 1]` 范围内。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FQuaternion<T>` | 起始四元数 |
| `to` | `FQuaternion<T>` | 目标四元数 |
| `t` | `T` | 插值参数，将被钳制到 `[0, 1]` |

**返回值**：球面插值后的四元数。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Euler(0, 0, 0);
var q2 = FQuaternion<Fixed32>.Euler(0, 90, 0);
var result = FQuaternion<Fixed32>.ClampSlerp(q1, q2, (Fixed32)(-0.5));
// t 被钳制为 0，结果等于 q1
```

**备注**：内部通过 `T.Clamp01(t)` 将 `t` 限制在 `[0, 1]`，然后调用 `Slerp`。

---

### Normalize

```csharp
public static FQuaternion<T> Normalize(FQuaternion<T> q)
```

**描述**：返回归一化后的四元数副本。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `q` | `FQuaternion<T>` | 待归一化的四元数 |

**返回值**：归一化后的单位四元数。如果输入四元数的模长为零，返回 `Identity`。

**示例**：

```csharp
var q = new FQuaternion<Fixed32>(1, 2, 3, 4);
var normalized = FQuaternion<Fixed32>.Normalize(q);
// normalized 为 q 的单位四元数版本
```

**备注**：此方法不修改原始四元数。归一化公式为 `q / |q|`，其中 `|q| = sqrt(Dot(q, q))`。

---

### LookRotation

```csharp
public static FQuaternion<T> LookRotation(FVector3<T> forward)
public static FQuaternion<T> LookRotation(FVector3<T> forward, FVector3<T> upwards)
```

**描述**：创建具有指定前方向和上方向的旋转四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `forward` | `FVector3<T>` | 前方向（视线方向） |
| `upwards` | `FVector3<T>` | 上方向（默认为 `FVector3<T>.Up`） |

**返回值**：朝向指定方向的四元数。

**示例**：

```csharp
// 朝向 X 轴正方向
var q1 = FQuaternion<Fixed32>.LookRotation(FVector3<Fixed32>.Right);

// 朝向 Z 轴正方向，上方向为 Y 轴
var q2 = FQuaternion<Fixed32>.LookRotation(
    FVector3<Fixed32>.Forward,
    FVector3<Fixed32>.Up
);
```

**备注**：内部通过 `FMatrix3x3<T>.LookAt` 构造旋转矩阵，再通过 `FromMatrix` 转换为四元数。`forward` 和 `upwards` 不能平行或为零向量。

---

### FromToRotation

```csharp
public static FQuaternion<T> FromToRotation(FVector3<T> from, FVector3<T> to)
```

**描述**：创建从方向 `from` 旋转到方向 `to` 的四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FVector3<T>` | 起始方向 |
| `to` | `FVector3<T>` | 目标方向 |

**返回值**：表示该旋转的四元数。

**示例**：

```csharp
var q = FQuaternion<Fixed32>.FromToRotation(
    FVector3<Fixed32>.Right,
    FVector3<Fixed32>.Up
);
// q 表示从 X 轴正方向旋转到 Y 轴正方向
```

**备注**：内部通过叉积和点积计算旋转轴和旋转角度，结果会自动归一化。

---

### RotateTowards

```csharp
public static FQuaternion<T> RotateTowards(FQuaternion<T> from, FQuaternion<T> to, T maxDegreesDelta)
```

**描述**：从 `from` 向 `to` 旋转，但旋转角度不超过 `maxDegreesDelta` 度。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FQuaternion<T>` | 起始四元数 |
| `to` | `FQuaternion<T>` | 目标四元数 |
| `maxDegreesDelta` | `T` | 最大旋转角度（度） |

**返回值**：旋转后的四元数。如果 `from` 与 `to` 之间的角度为零，返回 `to`。

**示例**：

```csharp
var current = FQuaternion<Fixed32>.Euler(0, 0, 0);
var target = FQuaternion<Fixed32>.Euler(0, 90, 0);
// 每次最多旋转 30 度
var next = FQuaternion<Fixed32>.RotateTowards(current, target, (Fixed32)30);
```

**备注**：此方法基于 `Slerp` 实现，保证匀速旋转。当 `maxDegreesDelta` 大于两四元数之间的角度时，直接返回 `to`。常用于平滑转向的实现。

---

### FromMatrix

```csharp
public static FQuaternion<T> FromMatrix(FMatrix3x3<T> matrix)
```

**描述**：从 3×3 旋转矩阵构造四元数。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix3x3<T>` | 3×3 旋转矩阵 |

**返回值**：对应的四元数。

**示例**：

```csharp
var matrix = FMatrix3x3<Fixed32>.LookAt(
    FVector3<Fixed32>.Forward,
    FVector3<Fixed32>.Up
);
var q = FQuaternion<Fixed32>.FromMatrix(matrix);
```

**备注**：使用 Shepperd 方法，根据矩阵对角线元素选择最优分支计算，避免数值精度问题。矩阵必须是正交旋转矩阵才能得到正确结果。

---

### ToMatrix

```csharp
public static FMatrix4x4<T> ToMatrix(FQuaternion<T> quat)
```

**描述**：将四元数转换为 4×4 旋转矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `quat` | `FQuaternion<T>` | 四元数 |

**返回值**：对应的 4×4 旋转矩阵。

**示例**：

```csharp
var q = FQuaternion<Fixed32>.Euler(0, 45, 0);
var matrix = FQuaternion<Fixed32>.ToMatrix(q);
// matrix 为绕 Y 轴旋转 45 度的 4×4 矩阵
```

**备注**：输出矩阵的平移部分为零，第四行和第四列除 `M44 = 1` 外均为 0。输入四元数应为单位四元数。

## 运算符

### operator +

```csharp
public static FQuaternion<T> operator +(FQuaternion<T> lhs, FQuaternion<T> rhs)
```

**描述**：四元数加法，逐分量相加。

**示例**：

```csharp
var q1 = new FQuaternion<Fixed32>(1, 2, 3, 4);
var q2 = new FQuaternion<Fixed32>(5, 6, 7, 8);
var result = q1 + q2;
// result = (6, 8, 10, 12)
```

**备注**：四元数加法在旋转运算中较少直接使用，主要用于插值计算的中间步骤。

---

### operator -

```csharp
public static FQuaternion<T> operator -(FQuaternion<T> lhs, FQuaternion<T> rhs)
```

**描述**：四元数减法，逐分量相减。

**示例**：

```csharp
var q1 = new FQuaternion<Fixed32>(5, 6, 7, 8);
var q2 = new FQuaternion<Fixed32>(1, 2, 3, 4);
var result = q1 - q2;
// result = (4, 4, 4, 4)
```

---

### operator *（标量乘法）

```csharp
public static FQuaternion<T> operator *(FQuaternion<T> quaternion, T t)
public static FQuaternion<T> operator *(T t, FQuaternion<T> quaternion)
```

**描述**：四元数与标量的乘法，每个分量乘以标量。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `quaternion` | `FQuaternion<T>` | 四元数 |
| `t` | `T` | 标量 |

**返回值**：缩放后的四元数。

**示例**：

```csharp
var q = new FQuaternion<Fixed32>(1, 2, 3, 4);
var result1 = q * (Fixed32)2;
// result1 = (2, 4, 6, 8)

var result2 = (Fixed32)2 * q;
// result2 = (2, 4, 6, 8)
```

**备注**：两个重载分别支持 `四元数 * 标量` 和 `标量 * 四元数`，结果相同。

---

### operator *（四元数乘法）

```csharp
public static FQuaternion<T> operator *(FQuaternion<T> lhs, FQuaternion<T> rhs)
```

**描述**：四元数乘法，用于组合两个旋转。注意四元数乘法不满足交换律。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `lhs` | `FQuaternion<T>` | 左操作数（后应用的旋转） |
| `rhs` | `FQuaternion<T>` | 右操作数（先应用的旋转） |

**返回值**：组合旋转后的四元数。

**示例**：

```csharp
var rotX = FQuaternion<Fixed32>.Euler(45, 0, 0);  // 绕 X 轴旋转 45 度
var rotY = FQuaternion<Fixed32>.Euler(0, 90, 0);   // 绕 Y 轴旋转 90 度
var combined = rotY * rotX;
// 先绕 X 轴旋转 45 度，再绕 Y 轴旋转 90 度
```

**备注**：`lhs * rhs` 表示先应用 `rhs` 的旋转，再应用 `lhs` 的旋转。四元数乘法不满足交换律：`q1 * q2 ≠ q2 * q1`。

乘法公式：

```
W = w1*w2 - x1*x2 - y1*y2 - z1*z2
X = w1*x2 + x1*w2 + y1*z2 - z1*y2
Y = w1*y2 + y1*w2 + z1*x2 - x1*z2
Z = w1*z2 + z1*w2 + x1*y2 - y1*x2
```

---

### operator *（四元数乘向量）

```csharp
public static FVector3<T> operator *(FQuaternion<T> rotation, FVector3<T> point)
```

**描述**：用四元数旋转向量。等价于将旋转应用到点上。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `rotation` | `FQuaternion<T>` | 旋转四元数 |
| `point` | `FVector3<T>` | 待旋转的向量 |

**返回值**：旋转后的向量。

**示例**：

```csharp
var rotation = FQuaternion<Fixed32>.Euler(0, 90, 0);  // 绕 Y 轴旋转 90 度
var point = FVector3<Fixed32>.Right;                    // X 轴正方向
var rotated = rotation * point;
// rotated 约为 Z 轴正方向（或负方向，取决于坐标系）
```

**备注**：此运算的几何意义是将向量 `point` 按四元数 `rotation` 所表示的旋转进行变换。内部使用优化的公式避免完整四元数乘法的开销。

---

### operator ==

```csharp
public static bool operator ==(FQuaternion<T> lhs, FQuaternion<T> rhs)
```

**描述**：判断两个四元数是否表示相同的旋转。

**返回值**：如果两个四元数的点积为 1，返回 `true`；否则返回 `false`。

**示例**：

```csharp
var q1 = FQuaternion<Fixed32>.Identity;
var q2 = FQuaternion<Fixed32>.Identity;
var equal = q1 == q2; // true
```

**备注**：使用点积判断相等，而非逐分量比较。这意味着两个分量值不同但表示相同旋转的四元数（如 `q` 和 `-q`）可能不被判定为相等。精确的逐分量比较请使用 `Equals` 方法。

---

### operator !=

```csharp
public static bool operator !=(FQuaternion<T> lhs, FQuaternion<T> rhs)
```

**描述**：判断两个四元数是否表示不同的旋转。

**返回值**：如果两个四元数的点积不为 1，返回 `true`；否则返回 `false`。

## 常见用法

### 创建旋转

```csharp
// 方式一：从欧拉角创建
var q1 = FQuaternion<Fixed32>.Euler(30, 45, 60);

// 方式二：从轴角创建
var q2 = FQuaternion<Fixed32>.AngleAxis(90, FVector3<Fixed32>.Up);

// 方式三：从方向创建
var q3 = FQuaternion<Fixed32>.LookRotation(FVector3<Fixed32>.Forward);

// 方式四：从两个方向创建
var q4 = FQuaternion<Fixed32>.FromToRotation(
    FVector3<Fixed32>.Right,
    FVector3<Fixed32>.Up
);
```

### 组合旋转

```csharp
var rotX = FQuaternion<Fixed32>.Euler(45, 0, 0);
var rotY = FQuaternion<Fixed32>.Euler(0, 90, 0);
var combined = rotY * rotX;  // 先绕 X 轴旋转 45 度，再绕 Y 轴旋转 90 度
```

### 旋转向量

```csharp
var rotation = FQuaternion<Fixed32>.Euler(0, 90, 0);
var point = FVector3<Fixed32>.Right;
var rotatedPoint = rotation * point;
```

### 平滑插值

```csharp
var from = FQuaternion<Fixed32>.Euler(0, 0, 0);
var to = FQuaternion<Fixed32>.Euler(0, 90, 0);

// 线性插值（速度不均匀）
var midLerp = FQuaternion<Fixed32>.Lerp(from, to, (Fixed32)0.5);

// 球面插值（速度均匀，推荐）
var midSlerp = FQuaternion<Fixed32>.Slerp(from, to, (Fixed32)0.5);
```

### 受控转向

```csharp
var current = FQuaternion<Fixed32>.Euler(0, 0, 0);
var target = FQuaternion<Fixed32>.Euler(0, 90, 0);

// 每帧最多旋转 30 度
var next = FQuaternion<Fixed32>.RotateTowards(current, target, (Fixed32)30);
```

### 矩阵转换

```csharp
// 四元数转 4×4 矩阵
var q = FQuaternion<Fixed32>.Euler(0, 45, 0);
var matrix = FQuaternion<Fixed32>.ToMatrix(q);

// 3×3 矩阵转四元数
var q2 = FQuaternion<Fixed32>.FromMatrix(someMatrix3x3);
```
