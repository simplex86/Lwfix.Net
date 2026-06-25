# FMatrix4x4

基于定点数的 4x4 矩阵

## 结构定义

```csharp
public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
```

**命名空间**：`SimplexLab.Fixed`

### 矩阵布局

```
| M11  M12  M13  M14 |
| M21  M22  M23  M24 |
| M31  M32  M33  M34 |
| M41  M42  M43  M44 |
```

---

## 字段

| 字段 | 类型 | 说明 |
|------|------|------|
| `M11` | `T` | 第1行第1列元素 |
| `M12` | `T` | 第1行第2列元素 |
| `M13` | `T` | 第1行第3列元素 |
| `M14` | `T` | 第1行第4列元素 |
| `M21` | `T` | 第2行第1列元素 |
| `M22` | `T` | 第2行第2列元素 |
| `M23` | `T` | 第2行第3列元素 |
| `M24` | `T` | 第2行第4列元素 |
| `M31` | `T` | 第3行第1列元素 |
| `M32` | `T` | 第3行第2列元素 |
| `M33` | `T` | 第3行第3列元素 |
| `M34` | `T` | 第3行第4列元素 |
| `M41` | `T` | 第4行第1列元素 |
| `M42` | `T` | 第4行第2列元素 |
| `M43` | `T` | 第4行第3列元素 |
| `M44` | `T` | 第4行第4列元素 |

---

## 静态属性

### Zero

```csharp
public static FMatrix4x4<T> Zero { get; }
```

**说明**：零矩阵，所有元素均为 `T.Zero`。

```csharp
var zero = FMatrix4x4<Fixed32>.Zero;
// | 0  0  0  0 |
// | 0  0  0  0 |
// | 0  0  0  0 |
// | 0  0  0  0 |
```

### Identity

```csharp
public static FMatrix4x4<T> Identity { get; }
```

**说明**：单位矩阵，对角线元素为 `T.One`，其余为 `T.Zero`。

```csharp
var identity = FMatrix4x4<Fixed32>.Identity;
// | 1  0  0  0 |
// | 0  1  0  0 |
// | 0  0  1  0 |
// | 0  0  0  1 |
```

---

## 构造函数

### FMatrix4x4(T m11, T m12, ..., T m44)

```csharp
public FMatrix4x4(T m11, T m12, T m13, T m14,
                  T m21, T m22, T m23, T m24,
                  T m31, T m32, T m33, T m34,
                  T m41, T m42, T m43, T m44)
```

**说明**：用16个元素创建 4x4 矩阵。

**参数**：16个类型为 `T` 的参数，分别对应矩阵的16个元素。

**示例**：

```csharp
var m = new FMatrix4x4<Fixed32>(
    1, 0, 0, 0,
    0, 1, 0, 0,
    0, 0, 1, 0,
    5, 3, 2, 1
);
```

### FMatrix4x4(FMatrix4x4\<T\> matrix)

```csharp
public FMatrix4x4(FMatrix4x4<T> matrix)
```

**说明**：拷贝构造函数，创建与给定矩阵元素相同的新矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix4x4<T>` | 要拷贝的源矩阵 |

---

## 实例属性

### 索引器 this[int index]

```csharp
public T this[int index] { get; set; }
```

**说明**：按列主序索引访问矩阵元素。索引范围 0~15。

**列主序映射**：

| 索引 | 字段 | 索引 | 字段 | 索引 | 字段 | 索引 | 字段 |
|------|------|------|------|------|------|------|------|
| 0 | M11 | 4 | M12 | 8 | M13 | 12 | M14 |
| 1 | M21 | 5 | M22 | 9 | M23 | 13 | M24 |
| 2 | M31 | 6 | M32 | 10 | M33 | 14 | M34 |
| 3 | M41 | 7 | M42 | 11 | M43 | 15 | M44 |

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Identity;
var val = m[0];  // M11 = 1
var val2 = m[5]; // M22 = 1
m[12] = new Fixed32(5); // 设置 M14 = 5
```

**备注**：索引超出 0~15 范围时抛出 `IndexOutOfRangeException`。

---

### Determinanted

```csharp
public T Determinanted { get; }
```

**说明**：获取矩阵的行列式值。

**返回值**：行列式值，类型为 `T`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Identity;
var det = m.Determinanted; // 结果为 1
```

---

### Inversed

```csharp
public FMatrix4x4<T> Inversed { get; }
```

**说明**：获取矩阵的逆矩阵。若矩阵为奇异矩阵，则返回所有元素为 `T.PositiveInfinity` 的矩阵。

**返回值**：逆矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Identity;
var inv = m.Inversed; // 仍为单位矩阵
```

---

### Transposed

```csharp
public FMatrix4x4<T> Transposed { get; }
```

**说明**：获取矩阵的转置矩阵。

**返回值**：转置矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Translate(1, 2, 3);
var t = m.Transposed;
```

---

## 实例方法

### ToString()

```csharp
public override string ToString()
```

**说明**：返回矩阵的字符串表示，按行格式化输出。

### GetHashCode()

```csharp
public override int GetHashCode()
```

**说明**：返回矩阵的哈希码，通过对所有16个元素的哈希码进行异或运算得到。

### Frustum

```csharp
public FMatrix4x4<T> Frustum(T left, T right, T bottom, T top, T zNear, T zFar)
```

**说明**：创建透视裁剪矩阵（实例方法）。根据视锥体的六个面参数构建透视投影矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `left` | `T` | 近裁剪面左边界 |
| `right` | `T` | 近裁剪面右边界 |
| `bottom` | `T` | 近裁剪面下边界 |
| `top` | `T` | 近裁剪面上边界 |
| `zNear` | `T` | 近裁剪面距离 |
| `zFar` | `T` | 远裁剪面距离 |

**返回值**：透视裁剪矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Identity;
var frustum = m.Frustum(
    new Fixed32(-1), new Fixed32(1),   // left, right
    new Fixed32(-1), new Fixed32(1),   // bottom, top
    new Fixed32(1),  new Fixed32(100)  // zNear, zFar
);
```

**备注**：此方法为实例方法，但通常以 `Zero` 矩阵调用后返回新矩阵。`zNear` 和 `zFar` 必须为正值且 `zFar > zNear`。

---

## 静态方法

### Determinant

```csharp
public static T Determinant(FMatrix4x4<T> matrix)
```

**说明**：计算 4x4 矩阵的行列式。使用拉普拉斯展开法，沿第一行展开，共需 17 次加法和 28 次乘法。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix4x4<T>` | 目标矩阵 |

**返回值**：行列式值，类型为 `T`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Identity;
var det = FMatrix4x4<Fixed32>.Determinant(m); // 结果为 1
```

---

### Inverse

```csharp
public static FMatrix4x4<T> Inverse(FMatrix4x4<T> matrix)
```

**说明**：计算 4x4 矩阵的逆矩阵。使用伴随矩阵法：`A⁻¹ = adj(A) / det(A)`。先计算行列式，若行列式为零（通过 `IsZero()` 判断），则返回所有元素为 `T.PositiveInfinity` 的矩阵；否则使用行列式的倒数乘以伴随矩阵得到逆矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix4x4<T>` | 目标矩阵 |

**返回值**：逆矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Translate(5, 3, 2);
var inv = FMatrix4x4<Fixed32>.Inverse(m);
// 验证：m * inv 应为单位矩阵
```

**备注**：与 `FMatrix3x3` 的 `Inverse` 不同，4x4 版本使用 `det.IsZero()` 而非 `det == 0` 来判断奇异矩阵，更适合定点数场景。

---

### LookAt

```csharp
public static FMatrix4x4<T> LookAt(FVector3<T> from, FVector3<T> to, FVector3<T> up)
```

**说明**：构建观察矩阵。根据观察位置、目标位置和上方向，内部通过 `FQuaternion<T>.LookRotation` 计算旋转，再调用 `TRS` 生成完整的变换矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `from` | `FVector3<T>` | 观察者位置 |
| `to` | `FVector3<T>` | 目标位置 |
| `up` | `FVector3<T>` | 上方向向量 |

**返回值**：观察矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var from = new FVector3<Fixed32>(0, 5, -10);
var to = new FVector3<Fixed32>(0, 0, 0);
var up = new FVector3<Fixed32>(0, 1, 0);
var viewMatrix = FMatrix4x4<Fixed32>.LookAt(from, to, up);
```

**备注**：`from` 与 `to` 不应重合，`up` 不应与观察方向平行。

---

### MultiplyPoint

```csharp
public static FVector3<T> MultiplyPoint(FVector3<T> point, FMatrix4x4<T> matrix)
```

**说明**：将三维点与 4x4 矩阵相乘，考虑平移分量（齐次坐标 w=1）。

**计算公式**：

```
x' = point.X * M11 + point.Y * M21 + point.Z * M31 + M41
y' = point.X * M12 + point.Y * M22 + point.Z * M32 + M42
z' = point.X * M13 + point.Y * M23 + point.Z * M33 + M43
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `point` | `FVector3<T>` | 三维点坐标 |
| `matrix` | `FMatrix4x4<T>` | 变换矩阵 |

**返回值**：变换后的三维点，类型为 `FVector3<T>`。

**示例**：

```csharp
var point = new FVector3<Fixed32>(1, 2, 3);
var translateMatrix = FMatrix4x4<Fixed32>.Translate(5, 3, 2);
var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, translateMatrix);
// result = (6, 5, 5)
```

**备注**：此方法包含平移分量，适用于位置点的变换。

---

### MultiplyVector

```csharp
public static FVector3<T> MultiplyVector(FVector3<T> vector, FMatrix4x4<T> matrix)
```

**说明**：将三维向量与 4x4 矩阵相乘，忽略平移分量（齐次坐标 w=0）。适用于方向向量的变换。

**计算公式**：

```
x' = vector.X * M11 + vector.Y * M21 + vector.Z * M31
y' = vector.X * M12 + vector.Y * M22 + vector.Z * M32
z' = vector.X * M13 + vector.Y * M23 + vector.Z * M33
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `vector` | `FVector3<T>` | 三维方向向量 |
| `matrix` | `FMatrix4x4<T>` | 变换矩阵 |

**返回值**：变换后的三维向量，类型为 `FVector3<T>`。

**示例**：

```csharp
var dir = new FVector3<Fixed32>(1, 0, 0);
var rotMatrix = FMatrix4x4<Fixed32>.RotateY(Fixed32.Pi / 2);
var result = FMatrix4x4<Fixed32>.MultiplyVector(dir, rotMatrix);
// result ≈ (0, 0, -1)
```

---

### Ortho

```csharp
public static FMatrix4x4<T> Ortho(T left, T right, T bottom, T top, T zNear, T zFar)
```

**说明**：创建正交投影矩阵。将视锥体内的物体平行投影到近裁剪面上，无透视缩短效果。

**矩阵形式**：

```
| 2/(r-l)    0         0           0       |
|   0      2/(t-b)     0           0       |
|   0        0      -2/(f-n)  -(f+n)/(f-n) |
|   0        0         0           1       |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `left` | `T` | 近裁剪面左边界 |
| `right` | `T` | 近裁剪面右边界 |
| `bottom` | `T` | 近裁剪面下边界 |
| `top` | `T` | 近裁剪面上边界 |
| `zNear` | `T` | 近裁剪面距离 |
| `zFar` | `T` | 远裁剪面距离 |

**返回值**：正交投影矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var ortho = FMatrix4x4<Fixed32>.Ortho(
    new Fixed32(-10), new Fixed32(10),   // left, right
    new Fixed32(-10), new Fixed32(10),   // bottom, top
    new Fixed32(1),   new Fixed32(100)   // zNear, zFar
);
```

**备注**：常用于 2D 游戏或 UI 渲染，以及不需要透视效果的场景。

---

### Perspective

```csharp
public static FMatrix4x4<T> Perspective(T fov, T aspect, T zNear, T zFar)
```

**说明**：创建透视投影矩阵。根据视场角、宽高比和裁剪面距离构建透视投影。

**矩阵形式**：

```
| fax/a  0     0              0           |
|   0   fax    0              0           |
|   0    0  -(f+n)/(f-n)  -2fn/(f-n)     |
|   0    0    -1              0           |

其中 fax = 1/tan(fov/2)，a = aspect
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `fov` | `T` | 垂直视场角（角度制，内部自动转换为弧度） |
| `aspect` | `T` | 宽高比（width / height） |
| `zNear` | `T` | 近裁剪面距离（正值） |
| `zFar` | `T` | 远裁剪面距离（正值，大于 zNear） |

**返回值**：透视投影矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var persp = FMatrix4x4<Fixed32>.Perspective(
    new Fixed32(60),  // 60度视场角
    new Fixed32(16) / 9,  // 16:9 宽高比
    new Fixed32(1),   // 近裁剪面
    new Fixed32(1000) // 远裁剪面
);
```

**备注**：`fov` 参数为角度制（非弧度），内部会通过 `T.DegreeToRadian()` 转换。`zNear` 和 `zFar` 必须为正值。

---

### Translate(T x, T y, T z)

```csharp
public static FMatrix4x4<T> Translate(T x, T y, T z)
```

**说明**：创建 3D 平移矩阵。

**矩阵形式**：

```
| 1  0  0  0 |
| 0  1  0  0 |
| 0  0  1  0 |
| x  y  z  1 |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴平移量 |
| `y` | `T` | Y 轴平移量 |
| `z` | `T` | Z 轴平移量 |

**返回值**：平移矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var t = FMatrix4x4<Fixed32>.Translate(5, 3, 2);
```

---

### Translate(FVector3\<T\> translation)

```csharp
public static FMatrix4x4<T> Translate(FVector3<T> translation)
```

**说明**：使用向量创建 3D 平移矩阵，等价于 `Translate(translation.X, translation.Y, translation.Z)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `translation` | `FVector3<T>` | 平移向量 |

**返回值**：平移矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var offset = new FVector3<Fixed32>(5, 3, 2);
var t = FMatrix4x4<Fixed32>.Translate(offset);
```

---

### Rotate

```csharp
public static FMatrix4x4<T> Rotate(FQuaternion<T> rotation)
```

**说明**：根据四元数创建 3D 旋转矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `rotation` | `FQuaternion<T>` | 旋转四元数 |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var quat = FQuaternion<Fixed32>.Euler(0, Fixed32.Pi / 4, 0);
var rotMatrix = FMatrix4x4<Fixed32>.Rotate(quat);
```

**备注**：传入的四元数应为单位四元数。

---

### RotateX(T radians)

```csharp
public static FMatrix4x4<T> RotateX(T radians)
```

**说明**：创建绕 X 轴旋转的矩阵（绕原点）。

**矩阵形式**：

```
| 1   0    0   0 |
| 0   c    s   0 |
| 0  -s    c   0 |
| 0   0    0   1 |

其中 c = cos(radians)，s = sin(radians)
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var rx = FMatrix4x4<Fixed32>.RotateX(Fixed32.Pi / 4); // 绕X轴旋转45度
```

---

### RotateX(T radians, FVector3\<T\> centerPoint)

```csharp
public static FMatrix4x4<T> RotateX(T radians, FVector3<T> centerPoint)
```

**说明**：创建绕 X 轴旋转的矩阵，以指定点为旋转中心。

**矩阵形式**：

```
| 1   0    0   0 |
| 0   c    s   0 |
| 0  -s    c   0 |
| 0   y'   z'  1 |

其中 y' = centerPoint.Y * (1-c) + centerPoint.Z * s
     z' = centerPoint.Z * (1-c) - centerPoint.Y * s
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |
| `centerPoint` | `FVector3<T>` | 旋转中心点 |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var center = new FVector3<Fixed32>(5, 0, 0);
var rx = FMatrix4x4<Fixed32>.RotateX(Fixed32.Pi / 4, center);
```

---

### RotateY(T radians)

```csharp
public static FMatrix4x4<T> RotateY(T radians)
```

**说明**：创建绕 Y 轴旋转的矩阵（绕原点）。

**矩阵形式**：

```
|  c   0  -s   0 |
|  0   1   0   0 |
|  s   0   c   0 |
|  0   0   0   1 |

其中 c = cos(radians)，s = sin(radians)
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var ry = FMatrix4x4<Fixed32>.RotateY(Fixed32.Pi / 3); // 绕Y轴旋转60度
```

---

### RotateY(T radians, FVector3\<T\> centerPoint)

```csharp
public static FMatrix4x4<T> RotateY(T radians, FVector3<T> centerPoint)
```

**说明**：创建绕 Y 轴旋转的矩阵，以指定点为旋转中心。

**矩阵形式**：

```
|  c   0  -s   0 |
|  0   1   0   0 |
|  s   0   c   0 |
|  x'  0   z'  1 |

其中 x' = centerPoint.X * (1-c) - centerPoint.Z * s
     z' = centerPoint.Z * (1-c) + centerPoint.X * s
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |
| `centerPoint` | `FVector3<T>` | 旋转中心点 |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

---

### RotateZ(T radians)

```csharp
public static FMatrix4x4<T> RotateZ(T radians)
```

**说明**：创建绕 Z 轴旋转的矩阵（绕原点）。

**矩阵形式**：

```
|  c   s   0   0 |
| -s   c   0   0 |
|  0   0   1   0 |
|  0   0   0   1 |

其中 c = cos(radians)，s = sin(radians)
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var rz = FMatrix4x4<Fixed32>.RotateZ(Fixed32.Pi / 6); // 绕Z轴旋转30度
```

---

### RotateZ(T radians, FVector3\<T\> centerPoint)

```csharp
public static FMatrix4x4<T> RotateZ(T radians, FVector3<T> centerPoint)
```

**说明**：创建绕 Z 轴旋转的矩阵，以指定点为旋转中心。

**矩阵形式**：

```
|  c   s   0   0 |
| -s   c   0   0 |
|  0   0   1   0 |
|  x'  y'  0   1 |

其中 x' = centerPoint.X * (1-c) + centerPoint.Y * s
     y' = centerPoint.Y * (1-c) - centerPoint.X * s
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `radians` | `T` | 旋转角度（弧度制） |
| `centerPoint` | `FVector3<T>` | 旋转中心点 |

**返回值**：旋转矩阵，类型为 `FMatrix4x4<T>`。

---

### Scale(T x, T y, T z)

```csharp
public static FMatrix4x4<T> Scale(T x, T y, T z)
```

**说明**：创建 3D 缩放矩阵。

**矩阵形式**：

```
| x  0  0  0 |
| 0  y  0  0 |
| 0  0  z  0 |
| 0  0  0  1 |
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴缩放因子 |
| `y` | `T` | Y 轴缩放因子 |
| `z` | `T` | Z 轴缩放因子 |

**返回值**：缩放矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var s = FMatrix4x4<Fixed32>.Scale(2, 2, 2); // 均匀放大2倍
```

---

### Scale(FVector3\<T\> scale)

```csharp
public static FMatrix4x4<T> Scale(FVector3<T> scale)
```

**说明**：使用向量创建 3D 缩放矩阵，等价于 `Scale(scale.X, scale.Y, scale.Z)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `scale` | `FVector3<T>` | 缩放向量 |

**返回值**：缩放矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var scaleVec = new FVector3<Fixed32>(2, 3, 4);
var s = FMatrix4x4<Fixed32>.Scale(scaleVec);
```

---

### Scale(T x, T y, T z, FVector3\<T\> centerPoint)

```csharp
public static FMatrix4x4<T> Scale(T x, T y, T z, FVector3<T> centerPoint)
```

**说明**：创建以指定点为中心的缩放矩阵。通过在缩放前后平移来实现在任意中心点的缩放。

**矩阵形式**：

```
|  x    0    0    0 |
|  0    y    0    0 |
|  0    0    z    0 |
| tx   ty   tz    1 |

其中 tx = centerPoint.X * (1-x)
     ty = centerPoint.Y * (1-y)
     tz = centerPoint.Z * (1-z)
```

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `x` | `T` | X 轴缩放因子 |
| `y` | `T` | Y 轴缩放因子 |
| `z` | `T` | Z 轴缩放因子 |
| `centerPoint` | `FVector3<T>` | 缩放中心点 |

**返回值**：缩放矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var center = new FVector3<Fixed32>(5, 0, 0);
var s = FMatrix4x4<Fixed32>.Scale(2, 2, 2, center);
// 以 (5, 0, 0) 为中心放大2倍
```

---

### Scale(FVector3\<T\> scale, FVector3\<T\> centerPoint)

```csharp
public static FMatrix4x4<T> Scale(FVector3<T> scale, FVector3<T> centerPoint)
```

**说明**：使用向量创建以指定点为中心的缩放矩阵，等价于 `Scale(scale.X, scale.Y, scale.Z, centerPoint)`。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `scale` | `FVector3<T>` | 缩放向量 |
| `centerPoint` | `FVector3<T>` | 缩放中心点 |

**返回值**：缩放矩阵，类型为 `FMatrix4x4<T>`。

---

### TRS

```csharp
public static FMatrix4x4<T> TRS(FVector3<T> translation, FQuaternion<T> rotation, FVector3<T> scale)
```

**说明**：创建平移旋转缩放组合矩阵，等价于 `Scale(scale) * Rotate(rotation) * Translate(translation)`。这是 3D 图形中最常用的物体变换矩阵。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `translation` | `FVector3<T>` | 平移向量 |
| `rotation` | `FQuaternion<T>` | 旋转四元数 |
| `scale` | `FVector3<T>` | 缩放向量 |

**返回值**：平移旋转缩放组合矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var pos = new FVector3<Fixed32>(10, 5, 0);
var rot = FQuaternion<Fixed32>.Euler(0, Fixed32.Pi / 4, 0);
var scl = new FVector3<Fixed32>(2, 2, 2);
var trs = FMatrix4x4<Fixed32>.TRS(pos, rot, scl);
```

**备注**：变换顺序为**先平移 → 再旋转 → 最后缩放**（`S * R * T`），与 Unity 的 `Matrix4x4.TRS` 行为一致。

---

### AngleAxis

```csharp
public static FMatrix4x4<T> AngleAxis(T angle, FVector3<T> axis)
```

**说明**：创建绕任意轴旋转的 4x4 旋转矩阵。使用 Rodrigues 旋转公式。

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

**返回值**：绕轴旋转矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
// 绕 (1,1,0) 方向旋转 45 度
var axis = new FVector3<Fixed32>(1, 1, 0).Normalize();
var m = FMatrix4x4<Fixed32>.AngleAxis(Fixed32.Pi / 4, axis);
```

**备注**：`axis` 应为单位向量。第4行和第4列中平移分量和透视分量均为默认值（M41=M42=M43=0, M14=M24=M34=0, M44=1）。

---

### Transpose

```csharp
public static FMatrix4x4<T> Transpose(FMatrix4x4<T> matrix)
```

**说明**：计算矩阵的转置矩阵，将行与列互换。

**参数**：

| 参数 | 类型 | 说明 |
|------|------|------|
| `matrix` | `FMatrix4x4<T>` | 目标矩阵 |

**返回值**：转置矩阵，类型为 `FMatrix4x4<T>`。

**示例**：

```csharp
var m = FMatrix4x4<Fixed32>.Translate(1, 2, 3);
var t = FMatrix4x4<Fixed32>.Transpose(m);
```

---

## 运算符

### operator +

```csharp
public static FMatrix4x4<T> operator +(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
```

**说明**：矩阵加法，对应元素相加。

### operator -

```csharp
public static FMatrix4x4<T> operator -(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
```

**说明**：矩阵减法，对应元素相减。

### operator *

```csharp
public static FMatrix4x4<T> operator *(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
```

**说明**：矩阵乘法，遵循标准矩阵乘法规则。

**计算公式**：

```
result[i,j] = Σ(lhs[i,k] * rhs[k,j])  (k = 1..4)
```

**示例**：

```csharp
var t = FMatrix4x4<Fixed32>.Translate(5, 3, 2);
var r = FMatrix4x4<Fixed32>.RotateY(Fixed32.Pi / 4);
var combined = r * t; // 先平移后旋转
```

### operator ==

```csharp
public static bool operator ==(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
```

**说明**：判断两个矩阵是否相等，所有16个元素均相等时返回 `true`。

### operator !=

```csharp
public static bool operator !=(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
```

**说明**：判断两个矩阵是否不相等。

---

# FMatrix3x3 与 FMatrix4x4 差异对比

| 特性 | FMatrix3x3\<T\> | FMatrix4x4\<T\> |
|------|-----------------|-----------------|
| **维度** | 3x3（9个元素） | 4x4（16个元素） |
| **主要用途** | 2D 仿射变换、3D 旋转 | 3D 仿射变换、投影、相机 |
| **索引器** | 无 | 有，列主序 `this[int index]` |
| **行列式属性** | `Determinant` | `Determinanted` |
| **逆矩阵属性** | 无（仅静态方法） | `Inversed` |
| **转置属性** | 无（仅静态方法） | `Transposed` |
| **平移** | `Translate(T x, T y)` | `Translate(T x, T y, T z)` + `Translate(FVector3<T>)` |
| **旋转** | `Rotate(T radians)`（2D弧度） | `Rotate(FQuaternion<T>)` + `RotateX/Y/Z`（含指定中心点重载） |
| **缩放** | `Scale(T x, T y)` | `Scale(T x, T y, T z)` + 向量重载 + 指定中心点重载 |
| **组合变换** | `TR`、`TRS`（参数式） | `TRS`（向量+四元数式） |
| **观察矩阵** | `LookAt(forward, upwards)` | `LookAt(from, to, up)` |
| **点变换** | `MultiplyPoint(FVector2, matrix)` → `FVector2` | `MultiplyPoint(FVector3, matrix)` → `FVector3` |
| **向量变换** | `MultiplyVector(FVector2, matrix)` → `FVector2` | `MultiplyVector(FVector3, matrix)` → `FVector3` |
| **投影** | 无 | `Ortho`、`Perspective`、`Frustum` |
| **绕轴旋转** | `AngleAxis(T, FVector3)` | `AngleAxis(T, FVector3)` |
| **奇异矩阵判断** | `det == 0` | `det.IsZero()` |
| **奇异矩阵返回** | 所有元素为 `PositiveInfinity` | 所有元素为 `PositiveInfinity` |

---

## 常见使用场景

### 2D 物体变换（FMatrix3x3）

```csharp
// 创建一个先平移、再旋转、最后缩放的变换矩阵
var transform = FMatrix3x3<Fixed32>.TRS(
    tx: new Fixed32(100),  // X 平移
    ty: new Fixed32(50),   // Y 平移
    radians: Fixed32.Pi / 6,  // 旋转30度
    sx: new Fixed32(2),    // X 缩放
    sy: new Fixed32(2)     // Y 缩放
);

// 变换一个点
var point = new FVector2<Fixed32>(3, 4);
var transformed = FMatrix3x3<Fixed32>.MultiplyPoint(point, transform);
```

### 3D 相机矩阵（FMatrix4x4）

```csharp
// 观察矩阵
var view = FMatrix4x4<Fixed32>.LookAt(
    from: new FVector3<Fixed32>(0, 5, -10),
    to: new FVector3<Fixed32>(0, 0, 0),
    up: new FVector3<Fixed32>(0, 1, 0)
);

// 透视投影矩阵
var projection = FMatrix4x4<Fixed32>.Perspective(
    fov: new Fixed32(60),
    aspect: new Fixed32(16) / 9,
    zNear: new Fixed32(1),
    zFar: new Fixed32(1000)
);

// MVP 矩阵
var model = FMatrix4x4<Fixed32>.TRS(
    translation: new FVector3<Fixed32>(0, 0, 0),
    rotation: FQuaternion<Fixed32>.Identity,
    scale: FVector3<Fixed32>.One
);
var mvp = model * view * projection;
```

### 绕任意点旋转（FMatrix4x4）

```csharp
// 绕点 (5, 0, 0) 绕 Y 轴旋转 90 度
var center = new FVector3<Fixed32>(5, 0, 0);
var rotMatrix = FMatrix4x4<Fixed32>.RotateY(Fixed32.Pi / 2, center);

// 变换一个点
var point = new FVector3<Fixed32>(6, 0, 0);
var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, rotMatrix);
```
