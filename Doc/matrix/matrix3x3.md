# FMatrix3x3

基于定点数的 3x3 矩阵

## 结构定义

```csharp
public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
```

**命名空间**：`SimplexLab.Fixed`

### 矩阵布局

```
| M11  M12  M13 |
| M21  M22  M23 |
| M31  M32  M33 |
```

---

## 字段

| 字段 | 类型 | 说明 |
|------|------|------|
| `M11` | `T` | 第1行第1列元素 |
| `M12` | `T` | 第1行第2列元素 |
| `M13` | `T` | 第1行第3列元素 |
| `M21` | `T` | 第2行第1列元素 |
| `M22` | `T` | 第2行第2列元素 |
| `M23` | `T` | 第2行第3列元素 |
| `M31` | `T` | 第3行第1列元素 |
| `M32` | `T` | 第3行第2列元素 |
| `M33` | `T` | 第3行第3列元素 |

---

## 静态属性

### Zero

```csharp
public static FMatrix3x3<T> Zero { get; }
```

**说明**：零矩阵，所有元素均为 `T.Zero`。

```csharp
var zero = FMatrix3x3<Fixed32>.Zero;
// | 0  0  0 |
// | 0  0  0 |
// | 0  0  0 |
```

### Identity

```csharp
public static FMatrix3x3<T> Identity { get; }
```

**说明**：单位矩阵，对角线元素为 `T.One`，其余为 `T.Zero`。

```csharp
var identity = FMatrix3x3<Fixed32>.Identity;
// | 1  0  0 |
// | 0  1  0 |
// | 0  0  1 |
```

---

## 构造函数

### FMatrix3x3(T m11, T m12, T m13, T m21, T m22, T m23, T m31, T m32, T m33)

```csharp
public FMatrix3x3(T m11, T m12, T m13, T m21, T m22, T m23, T m31, T m32, T m33)
```

**说明**：用9个元素创建 3x3 矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `m11` | `T` | 第1行第1列 |
| `m12` | `T` | 第1行第2列 |
| `m13` | `T` | 第1行第3列 |
| `m21` | `T` | 第2行第1列 |
| `m22` | `T` | 第2行第2列 |
| `m23` | `T` | 第2行第3列 |
| `m31` | `T` | 第3行第1列 |
| `m32` | `T` | 第3行第2列 |
| `m33` | `T` | 第3行第3列 |

**示例**：

```csharp
var m = new FMatrix3x3<Fixed32>(
    new Fixed32(1), new Fixed32(2), new Fixed32(3),
    new Fixed32(4), new Fixed32(5), new Fixed32(6),
    new Fixed32(7), new Fixed32(8), new Fixed32(9)
);
```

### FMatrix3x3(FMatrix3x3\<T\> matrix)

```csharp
public FMatrix3x3(FMatrix3x3<T> matrix)
```

**说明**：拷贝构造函数，创建与给定矩阵元素相同的新矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix3x3<T>` | 要拷贝的源矩阵 |

**示例**：

```csharp
var original = FMatrix3x3<Fixed32>.Identity;
var copy = new FMatrix3x3<Fixed32>(original);
```

---

## 实例属性

### Determinant

```csharp
public T Determinant { get; }
```

**说明**：获取矩阵的行列式值。行列式用于判断矩阵是否可逆——当行列式为零时，矩阵为奇异矩阵，不可逆。

**返回值**：行列式值，类型为 `T`。

**示例**：

```csharp
var m = FMatrix3x3<Fixed32>.Identity;
var det = m.Determinant; // 结果为 1
```

---

## 实例方法

### ToString()

```csharp
public override string ToString()
```

**说明**：返回矩阵的字符串表示，按行格式化输出。

**返回值**：格式化后的矩阵字符串。

### GetHashCode()

```csharp
public override int GetHashCode()
```

**说明**：返回矩阵的哈希码，通过对所有9个元素的哈希码进行异或运算得到。

**返回值**：哈希码值。

---

## 静态方法

### Determinate

```csharp
public static T Determinate(FMatrix3x3<T> matrix)
```

**说明**：计算 3x3 矩阵的行列式。使用 Sarrus 法则（对角线展开法）计算。

**数学公式**：

```
det = M11·M22·M33 - M11·M23·M32
    - M12·M21·M33 + M12·M23·M31
    + M13·M21·M32 - M13·M22·M31
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix3x3<T>` | 目标矩阵 |

**返回值**：行列式值，类型为 `T`。

**示例**：

```csharp
var m = FMatrix3x3<Fixed32>.Identity;
var det = FMatrix3x3<Fixed32>.Determinate(m); // 结果为 1
```

**备注**：实例属性 `Determinant` 是此方法的语法糖。

---

### Inverse

```csharp
public static FMatrix3x3<T> Inverse(FMatrix3x3<T> matrix)
```

**说明**：计算 3x3 矩阵的逆矩阵。使用伴随矩阵法：`A⁻¹ = adj(A) / det(A)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix3x3<T>` | 目标矩阵 |

**返回值**：逆矩阵。若矩阵为奇异矩阵（行列式为零），则返回所有元素为 `T.PositiveInfinity` 的矩阵。

**示例**：

```csharp
var m = FMatrix3x3<Fixed32>.Identity;
var inv = FMatrix3x3<Fixed32>.Inverse(m);
// 逆矩阵仍为单位矩阵

// 奇异矩阵示例
var singular = new FMatrix3x3<Fixed32>(
    1, 2, 3,
    4, 5, 6,
    7, 8, 9  // 第3行 = 第1行 + 第2行，行列式为0
);
var singularInv = FMatrix3x3<Fixed32>.Inverse(singular);
// 所有元素为 PositiveInfinity
```

**备注**：调用前应先检查行列式是否为零，以避免得到无效结果。

---

### LookAt

```csharp
public static FMatrix3x3<T> LookAt(FVector3<T> forward, FVector3<T> upwards)
```

**说明**：根据前方向和上方向构建观察矩阵（旋转矩阵）。内部对 `forward` 和 `upwards` 进行叉积运算来构建正交基。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `forward` | `FVector3<T>` | 前方向向量（会自动归一化） |
| `upwards` | `FVector3<T>` | 上方向向量（用于确定朝向） |

**返回值**：观察旋转矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var forward = new FVector3<Fixed32>(0, 0, 1);
var up = new FVector3<Fixed32>(0, 1, 0);
var viewMatrix = FMatrix3x3<Fixed32>.LookAt(forward, up);
```

**备注**：`forward` 和 `upwards` 不应平行，否则叉积结果为零向量，导致矩阵无效。

---

### MultiplyPoint

```csharp
public static FVector2<T> MultiplyPoint(FVector2<T> point, FMatrix3x3<T> matrix)
```

**说明**：将二维点与 3x3 矩阵相乘，考虑平移分量（即齐次坐标中 w=1 的情况）。

**计算公式**：

```
x' = point.X * M11 + point.Y * M21 + M31
y' = point.X * M12 + point.Y * M22 + M32
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `point` | `FVector2<T>` | 二维点坐标 |
| `matrix` | `FMatrix3x3<T>` | 变换矩阵 |

**返回值**：变换后的二维点，类型为 `FVector2<T>`。

**示例**：

```csharp
var point = new FVector2<Fixed32>(1, 2);
var translateMatrix = FMatrix3x3<Fixed32>.Translate(3, 4);
var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, translateMatrix);
// result = (4, 6)
```

**备注**：与 `MultiplyVector` 不同，此方法包含矩阵的平移分量（第3行 M31、M32）。

---

### MultiplyVector

```csharp
public static FVector2<T> MultiplyVector(FVector2<T> vector, FMatrix3x3<T> matrix)
```

**说明**：将二维向量与 3x3 矩阵相乘，忽略平移分量（即齐次坐标中 w=0 的情况）。适用于方向向量的变换。

**计算公式**：

```
x' = vector.X * M11 + vector.Y * M21
y' = vector.X * M12 + vector.Y * M22
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `vector` | `FVector2<T>` | 二维方向向量 |
| `matrix` | `FMatrix3x3<T>` | 变换矩阵 |

**返回值**：变换后的二维向量，类型为 `FVector2<T>`。

**示例**：

```csharp
var dir = new FVector2<Fixed32>(1, 0);
var rotMatrix = FMatrix3x3<Fixed32>.Rotate(Fixed32.Pi / 2); // 旋转90度
var result = FMatrix3x3<Fixed32>.MultiplyVector(dir, rotMatrix);
// result ≈ (0, 1)
```

**备注**：方向向量变换不应受平移影响，因此使用此方法而非 `MultiplyPoint`。

---

### Translate

```csharp
public static FMatrix3x3<T> Translate(T x, T y)
```

**说明**：创建 2D 平移矩阵。

**矩阵形式**：

```
| 1  0  0 |
| 0  1  0 |
| x  y  1 |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴平移量 |
| `y` | `T` | Y 轴平移量 |

**返回值**：平移矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var t = FMatrix3x3<Fixed32>.Translate(3, 4);
// | 1  0  0 |
// | 0  1  0 |
// | 3  4  1 |
```

---

### Rotate

```csharp
public static FMatrix3x3<T> Rotate(T radians)
```

**说明**：创建 2D 旋转矩阵（绕原点旋转）。

**矩阵形式**：

```
|  cos   sin  0 |
| -sin   cos  0 |
|   0     0   1 |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制），正值为逆时针旋转 |

**返回值**：旋转矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
// 旋转 90 度（π/2 弧度）
var r = FMatrix3x3<Fixed32>.Rotate(Fixed32.Pi / 2);
// | 0   1  0 |
// | -1  0  0 |
// | 0   0  1 |
```

**备注**：角度单位为弧度，可使用 `T.DegreeToRadian()` 将角度转换为弧度。

---

### Scale

```csharp
public static FMatrix3x3<T> Scale(T x, T y)
```

**说明**：创建 2D 缩放矩阵。

**矩阵形式**：

```
| x  0  0 |
| 0  y  0 |
| 0  0  1 |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴缩放因子 |
| `y` | `T` | Y 轴缩放因子 |

**返回值**：缩放矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var s = FMatrix3x3<Fixed32>.Scale(2, 3);
// | 2  0  0 |
// | 0  3  0 |
// | 0  0  1 |
```

**备注**：缩放因子为负值时会产生镜像翻转效果。

---

### TR

```csharp
public static FMatrix3x3<T> TR(T x, T y, T radians)
```

**说明**：创建平移旋转组合矩阵，等价于 `Rotate(radians) * Translate(x, y)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴平移量 |
| `y` | `T` | Y 轴平移量 |
| `radians` | `T` | 旋转角度（弧度制） |

**返回值**：平移旋转组合矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var tr = FMatrix3x3<Fixed32>.TR(5, 3, Fixed32.Pi / 4);
// 先平移 (5, 3)，再旋转 45 度
```

**备注**：变换顺序为**先平移后旋转**（矩阵乘法从右到左：`R * T`）。

---

### TRS

```csharp
public static FMatrix3x3<T> TRS(T tx, T ty, T radians, T sx, T sy)
```

**说明**：创建平移旋转缩放组合矩阵，等价于 `Scale(sx, sy) * Rotate(radians) * Translate(tx, ty)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `tx` | `T` | X 轴平移量 |
| `ty` | `T` | Y 轴平移量 |
| `radians` | `T` | 旋转角度（弧度制） |
| `sx` | `T` | X 轴缩放因子 |
| `sy` | `T` | Y 轴缩放因子 |

**返回值**：平移旋转缩放组合矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var trs = FMatrix3x3<Fixed32>.TRS(5, 3, Fixed32.Pi / 4, 2, 2);
// 先平移 (5, 3)，再旋转 45 度，最后缩放 (2, 2)
```

**备注**：变换顺序为**先平移 → 再旋转 → 最后缩放**（`S * R * T`），这是 2D 游戏中常用的物体变换顺序。

---

### AngleAxis

```csharp
public static FMatrix3x3<T> AngleAxis(T angle, FVector3<T> axis)
```

**说明**：创建绕任意轴旋转的 3x3 旋转矩阵。使用 Rodrigues 旋转公式。

**数学公式**：

```
M = uu^T + cos(θ)(I - uu^T) + sin(θ)S

其中 u = (x, y, z) 为单位轴向量，S 为斜对称矩阵
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `angle` | `T` | 旋转角度（弧度制） |
| `axis` | `FVector3<T>` | 旋转轴向量（应为单位向量） |

**返回值**：绕轴旋转矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
// 绕 Y 轴旋转 90 度
var axis = new FVector3<Fixed32>(0, 1, 0);
var m = FMatrix3x3<Fixed32>.AngleAxis(Fixed32.Pi / 2, axis);
```

**备注**：`axis` 应为单位向量，否则旋转结果可能不符合预期。

---

### Transpose

```csharp
public static FMatrix3x3<T> Transpose(FMatrix3x3<T> matrix)
```

**说明**：计算矩阵的转置矩阵，将行与列互换。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix3x3<T>` | 目标矩阵 |

**返回值**：转置矩阵，类型为 `FMatrix3x3<T>`。

**示例**：

```csharp
var m = new FMatrix3x3<Fixed32>(1, 2, 3, 4, 5, 6, 7, 8, 9);
var t = FMatrix3x3<Fixed32>.Transpose(m);
// | 1  4  7 |
// | 2  5  8 |
// | 3  6  9 |
```

---

## 运算符

### operator +

```csharp
public static FMatrix3x3<T> operator +(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
```

**说明**：矩阵加法，对应元素相加。

**示例**：

```csharp
var a = FMatrix3x3<Fixed32>.Identity;
var b = FMatrix3x3<Fixed32>.Identity;
var sum = a + b; // 对角线元素为 2
```

### operator -

```csharp
public static FMatrix3x3<T> operator -(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
```

**说明**：矩阵减法，对应元素相减。

### operator *

```csharp
public static FMatrix3x3<T> operator *(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
```

**说明**：矩阵乘法，遵循标准矩阵乘法规则。

**计算公式**：

```
result[i,j] = Σ(lhs[i,k] * rhs[k,j])  (k = 1..3)
```

**示例**：

```csharp
var t = FMatrix3x3<Fixed32>.Translate(5, 3);
var r = FMatrix3x3<Fixed32>.Rotate(Fixed32.Pi / 4);
var combined = r * t; // 先平移后旋转
```

### operator ==

```csharp
public static bool operator ==(FMatrix3x3<T> a, FMatrix3x3<T> b)
```

**说明**：判断两个矩阵是否相等，所有9个元素均相等时返回 `true`。

### operator !=

```csharp
public static bool operator !=(FMatrix3x3<T> a, FMatrix3x3<T> b)
```

**说明**：判断两个矩阵是否不相等。