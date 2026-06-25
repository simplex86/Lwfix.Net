# Lwfix.Net 参考手册

- [Fixed32](#fixed32)
- [Fixed64](#fixed64)
- [FVector2](#fvector2)
- [FVector3](#fvector3)
- [FMatrix3x3](#fmatrix3x3)
- [FMatrix4x4](#fmatrix4x4)
- [FQuaternion](#fquaternion)
- [FRandom](#frandom)

## Fixed32

表示 Q32.32 格式的定点数

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| Fixed32() | 创建值为0的定点数 |
| Fixed32(int) | 从整数创建定点数 |
| Fixed32(float) | 从浮点数创建定点数 |
| Fixed32(double) | 从双精度浮点数创建定点数 |
| **类型转换** | |
| Fixed32 Integral() | 获取整数部分 |
| Fixed32 Fractional() | 获取小数部分 |
| byte ToByte() | 转换为byte |
| short ToShort() | 转换为short |
| int ToInt() | 转换为int |
| long ToLong() | 转换为long |
| float ToFloat() | 转换为float |
| double ToDouble() | 转换为double |
| **运算符** | |
| static Fixed32 operator +(Fixed32 a, Fixed32 b) | 两个定点数相加 |
| static Fixed32 operator +(Fixed32 a, int b) | 定点数加整数 |
| static Fixed32 operator +(int a, Fixed32 b) | 整数加定点数 |
| static Fixed32 operator -(Fixed32 a, Fixed32 b) | 两个定点数相减 |
| static Fixed32 operator -(Fixed32 a, int b) | 定点数减整数 |
| static Fixed32 operator -(int a, Fixed32 b) | 整数减定点数 |
| static Fixed32 operator *(Fixed32 a, Fixed32 b) | 两个定点数相乘 |
| static Fixed32 operator *(Fixed32 a, int b) | 定点数乘整数 |
| static Fixed32 operator *(int a, Fixed32 b) | 整数乘定点数 |
| static Fixed32 operator /(Fixed32 a, Fixed32 b) | 两个定点数相除 |
| static Fixed32 operator /(Fixed32 a, int b) | 定点数除以整数 |
| static Fixed32 operator /(int a, Fixed32 b) | 整数除以定点数 |
| static Fixed32 operator %(Fixed32 a, Fixed32 b) | 计算两个定点数相除的余数 |
| static Fixed32 operator %(Fixed32 a, int b) | 定点数对整数取模 |
| static Fixed32 operator %(int a, Fixed32 b) | 整数对定点数取模 |
| static Fixed32 operator -(Fixed32 n) | 定点数的相反数 |
| **比较** | |
| static bool operator ==(Fixed32 a, Fixed32 b) | 判断两个定点数是否相等 |
| static bool operator !=(Fixed32 a, Fixed32 b) | 判断两个定点数是否不相等 |
| static bool operator >(Fixed32 a, Fixed32 b) | 判断第一个定点数是否大于第二个 |
| static bool operator <(Fixed32 a, Fixed32 b) | 判断第一个定点数是否小于第二个 |
| static bool operator >=(Fixed32 a, Fixed32 b) | 判断第一个定点数是否大于等于第二个 |
| static bool operator >=(Fixed32 a, Fixed32 b) | 判断第一个定点数是否小于等于第二个 |
| **数学方法** | |
| Fixed32 Abs() | 绝对值 |
| int Sign() | 符号（1/0/-1） |
| Fixed32 Sqrt() | 平方根 |
| Fixed32 Cbrt() | 立方根 |
| Fixed32 Pow(int) | 整数幂运算 |
| Fixed32 Pow(Fixed32) | 定点数幂运算 |
| bool IsPowerOfTwo() | 是否为2的幂 |
| Fixed32 ClosestPowerOfTwo() | 最近的2的幂 |
| Fixed32 NextPowerOfTwo() | 下一个2的幂 |
| Fixed32 Exp() | 指数函数（e^n） |
| Fixed32 Log() | 自然对数 |
| Fixed32 Log2() | 以2为底的对数 |
| Fixed32 Log10() | 以10为底的对数 |
| Fixed32 Reciprocal() | 倒数 |
| Fixed32 Round() | 四舍五入 |
| int RoundToInt() | 四舍五入为int |
| Fixed32 Floor() | 向下取整 |
| int FloorToInt() | 向下取整为int |
| Fixed32 Ceil() | 向上取整 |
| int CeilToInt() | 向上取整为int |
| bool IsNaN() | 是否为NaN |
| bool IsZero() | 是否为零 |
| bool IsMin() | 是否为最小值 |
| bool IsMax() | 是否为最大值 |
| bool IsInfinity() | 是否为无穷大 |
| bool IsPositiveInfinity() | 是否为正无穷 |
| bool IsNegativeInfinity() | 是否为负无穷 |
| bool IsPositive() | 是否为正数（含0） |
| bool IsNegative() | 是否为负数 |
| bool IsFractional() | 是否有小数部分 |
| **三角函数** |  |
| static Fixed32 Sin(Fixed32) | 正弦 |
| static Fixed32 FastSin(Fixed32) | 快速正弦（查表法） |
| static Fixed32 Asin(Fixed32) | 反正弦 |
| static Fixed32 Cos(Fixed32) | 余弦 |
| static Fixed32 FastCos(Fixed32) | 快速余弦（查表法） |
| static Fixed32 Acos(Fixed32) | 反余弦 |
| static Fixed32 Tan(Fixed32) | 正切 |
| static Fixed32 FastTan(Fixed32) | 快速正切（查表法） |
| static Fixed32 Atan(Fixed32) | 反正切 |
| static Fixed32 Atan2(Fixed32, Fixed32) | 两参数反正切 |
| **静态函数** | |
| static Fixed32 FromRaw(long) | 从原始值创建定点数 |
| static long ToRaw(Fixed32) | 获取原始存储值 |
| static bool IsSigns(Fixed32, Fixed32) | 符号是否相同 |
| static Fixed32 Min(Fixed32, Fixed32) | 最小值 |
| static Fixed32 Min(Fixed32, Fixed32, Fixed32) | 最小值 |
| static Fixed32 Min(Fixed32, Fixed32, Fixed32, Fixed32) | 最小值 |
| static Fixed32 Max(Fixed32, Fixed32) | 最大值 |
| static Fixed32 Max(Fixed32, Fixed32, Fixed32) | 最大值 |
| static Fixed32 Max(Fixed32, Fixed32, Fixed32, Fixed32) | 最大值 |
| static Fixed32 Clamp(Fixed32, Fixed32, Fixed32) | 限制在[min, max]范围内 |
| static Fixed32 Clamp01(Fixed32) | 限制在[0, 1]范围内 |
| static Fixed32 Lerp(Fixed32, Fixed32, Fixed32) | 线性插值 |
| static Fixed32 ClampLerp(Fixed32, Fixed32, Fixed32) | 钳制线性插值 |
| static Fixed32 InverseLerp(Fixed32, Fixed32, Fixed32) | 逆线性插值 |
| static Fixed32 SmoothStep(Fixed32, Fixed32, Fixed32) | 平滑插值 |
| static Fixed32 MoveTowards(Fixed32, Fixed32, Fixed32) | 移向目标值 |
| static Fixed32 MoveTowardsAngle(Fixed32, Fixed32, Fixed32) | 移向目标角度 |
| static Fixed32 Repeat(Fixed32, Fixed32) | 重复值 |
| static Fixed32 DeltaAngle(Fixed32, Fixed32) | 角度差 |
| static Fixed32 SmoothDamp(Fixed32, Fixed32, ref Fixed32, Fixed32) | 平滑阻尼 |
| static Fixed32 SmoothDamp(Fixed32, Fixed32, ref Fixed32, Fixed32, Fixed32) | 平滑阻尼 |
| static Fixed32 SmoothDamp(Fixed32, Fixed32, ref Fixed32, Fixed32, Fixed32, Fixed32) | 平滑阻尼 |
| static Fixed32 NormalizeRadian(Fixed32) | 弧度规范化到[0, 2π] |
| static Fixed32 DegreeToRadian(Fixed32) | 角度转弧度 |
| static Fixed32 RadianToDegree(Fixed32) | 弧度转角度 |

## Fixed64

表示 Q64.64 格式的定点数

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| Fixed64() | 创建值为0的定点数 |
| Fixed64(int) | 从整数创建定点数 |
| Fixed64(float) | 从浮点数创建定点数 |
| Fixed64(double) | 从双精度浮点数创建定点数 |
| **类型转换** | |
| Fixed64 Integral() | 获取整数部分 |
| Fixed64 Fractional() | 获取小数部分 |
| byte ToByte() | 转换为byte |
| short ToShort() | 转换为short |
| int ToInt() | 转换为int |
| long ToLong() | 转换为long |
| float ToFloat() | 转换为float |
| double ToDouble() | 转换为double |
| **运算符** | |
| static Fixed64 operator +(Fixed64 a, Fixed64 b) | 两个定点数相加 |
| static Fixed64 operator +(Fixed64 a, int b) | 定点数加整数 |
| static Fixed64 operator +(int a, Fixed64 b) | 整数加定点数 |
| static Fixed64 operator -(Fixed64 a, Fixed64 b) | 两个定点数相减 |
| static Fixed64 operator -(Fixed64 a, int b) | 定点数减整数 |
| static Fixed64 operator -(int a, Fixed64 b) | 整数减定点数 |
| static Fixed64 operator *(Fixed64 a, Fixed64 b) | 两个定点数相乘 |
| static Fixed64 operator *(Fixed64 a, int b) | 定点数乘整数 |
| static Fixed64 operator *(int a, Fixed64 b) | 整数乘定点数 |
| static Fixed64 operator /(Fixed64 a, Fixed64 b) | 两个定点数相除 |
| static Fixed64 operator /(Fixed64 a, int b) | 定点数除以整数 |
| static Fixed64 operator /(int a, Fixed64 b) | 整数除以定点数 |
| static Fixed64 operator %(Fixed64 a, Fixed64 b) | 计算两个定点数相除的余数 |
| static Fixed64 operator %(Fixed64 a, int b) | 定点数对整数取模 |
| static Fixed64 operator %(int a, Fixed64 b) | 整数对定点数取模 |
| static Fixed64 operator -(Fixed64 n) | 定点数的相反数 |
| **比较** | |
| static bool operator ==(Fixed64 a, Fixed64 b) | 判断两个定点数是否相等 |
| static bool operator !=(Fixed64 a, Fixed64 b) | 判断两个定点数是否不相等 |
| static bool operator >(Fixed64 a, Fixed64 b) | 判断第一个定点数是否大于第二个 |
| static bool operator <(Fixed64 a, Fixed64 b) | 判断第一个定点数是否小于第二个 |
| static bool operator >=(Fixed64 a, Fixed64 b) | 判断第一个定点数是否大于等于第二个 |
| static bool operator >=(Fixed64 a, Fixed64 b) | 判断第一个定点数是否小于等于第二个 |
| **数学方法** | |
| Fixed64 Abs() | 绝对值 |
| int Sign() | 符号（1/0/-1） |
| Fixed64 Sqrt() | 平方根 |
| Fixed64 Cbrt() | 立方根 |
| Fixed64 Pow(int) | 整数幂运算 |
| Fixed64 Pow(Fixed64) | 定点数幂运算 |
| bool IsPowerOfTwo() | 是否为2的幂 |
| Fixed64 ClosestPowerOfTwo() | 最近的2的幂 |
| Fixed64 NextPowerOfTwo() | 下一个2的幂 |
| Fixed64 Exp() | 指数函数（e^n） |
| Fixed64 Log() | 自然对数 |
| Fixed64 Log2() | 以2为底的对数 |
| Fixed64 Log10() | 以10为底的对数 |
| Fixed64 Reciprocal() | 倒数 |
| Fixed64 Round() | 四舍五入 |
| int RoundToInt() | 四舍五入为int |
| Fixed64 Floor() | 向下取整 |
| int FloorToInt() | 向下取整为int |
| Fixed64 Ceil() | 向上取整 |
| int CeilToInt() | 向上取整为int |
| bool IsNaN() | 是否为NaN |
| bool IsZero() | 是否为零 |
| bool IsMin() | 是否为最小值 |
| bool IsMax() | 是否为最大值 |
| bool IsInfinity() | 是否为无穷大 |
| bool IsPositiveInfinity() | 是否为正无穷 |
| bool IsNegativeInfinity() | 是否为负无穷 |
| bool IsPositive() | 是否为正数（含0） |
| bool IsNegative() | 是否为负数 |
| bool IsFractional() | 是否有小数部分 |
| **三角函数** |  |
| static Fixed64 Sin(Fixed64) | 正弦 |
| static Fixed64 FastSin(Fixed64) | 快速正弦（查表法） |
| static Fixed64 Asin(Fixed64) | 反正弦 |
| static Fixed64 Cos(Fixed64) | 余弦 |
| static Fixed64 FastCos(Fixed64) | 快速余弦（查表法） |
| static Fixed64 Acos(Fixed64) | 反余弦 |
| static Fixed64 Tan(Fixed64) | 正切 |
| static Fixed64 FastTan(Fixed64) | 快速正切（查表法） |
| static Fixed64 Atan(Fixed64) | 反正切 |
| static Fixed64 Atan2(Fixed64, Fixed64) | 两参数反正切 |
| **静态函数** | |
| static Fixed64 FromRaw(Int128) | 从原始值创建定点数 |
| static Int128 ToRaw(Fixed64) | 获取原始存储值 |
| static bool IsSigns(Fixed64, Fixed64) | 符号是否相同 |
| static Fixed64 Min(Fixed64, Fixed64) | 最小值 |
| static Fixed64 Min(Fixed64, Fixed64, Fixed64) | 最小值 |
| static Fixed64 Min(Fixed64, Fixed64, Fixed64, Fixed64) | 最小值 |
| static Fixed64 Max(Fixed64, Fixed64) | 最大值 |
| static Fixed64 Max(Fixed64, Fixed64, Fixed64) | 最大值 |
| static Fixed64 Max(Fixed64, Fixed64, Fixed64, Fixed64) | 最大值 |
| static Fixed64 Clamp(Fixed64, Fixed64, Fixed64) | 限制在[min, max]范围内 |
| static Fixed64 Clamp01(Fixed64) | 限制在[0, 1]范围内 |
| static Fixed64 Lerp(Fixed64, Fixed64, Fixed64) | 线性插值 |
| static Fixed64 ClampLerp(Fixed64, Fixed64, Fixed64) | 钳制线性插值 |
| static Fixed64 InverseLerp(Fixed64, Fixed64, Fixed64) | 逆线性插值 |
| static Fixed64 SmoothStep(Fixed64, Fixed64, Fixed64) | 平滑插值 |
| static Fixed64 MoveTowards(Fixed64, Fixed64, Fixed64) | 移向目标值 |
| static Fixed64 MoveTowardsAngle(Fixed64, Fixed64, Fixed64) | 移向目标角度 |
| static Fixed64 Repeat(Fixed64, Fixed64) | 重复值 |
| static Fixed64 DeltaAngle(Fixed64, Fixed64) | 角度差 |
| static Fixed64 SmoothDamp(Fixed64, Fixed64, ref Fixed64, Fixed64) | 平滑阻尼 |
| static Fixed64 SmoothDamp(Fixed64, Fixed64, ref Fixed64, Fixed64, Fixed64) | 平滑阻尼 |
| static Fixed64 SmoothDamp(Fixed64, Fixed64, ref Fixed64, Fixed64, Fixed64, Fixed64) | 平滑阻尼 |
| static Fixed64 NormalizeRadian(Fixed64) | 弧度规范化到[0, 2π] |
| static Fixed64 DegreeToRadian(Fixed64) | 角度转弧度 |
| static Fixed64 RadianToDegree(Fixed64) | 弧度转角度 |

> [!WARNING]  
> Fixed64 使用 `Int128` / `UInt128` 类型，需要在 .Net 7 及更高版本才被支持。

## FVector2

基于定点数的二维向量

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| FVector2(T, T) | 构造二维向量 |
| **属性** | |
| T Magnitude { get; } | 向量长度 |
| T SqrMagnitude { get; } | 向量长度平方 |
| FVector2 Normalized { get; } | 归一化向量 |
| **成员函数** | |
| void Normalize() | 归一化（原地） |
| **静态函数** | |
| static T Angle(FVector2, FVector2) | 两向量夹角（度） |
| static T Cross(FVector2, FVector2) | 叉乘 |
| static T Distance(FVector2, FVector2) | 两点距离 |
| static T Dot(FVector2, FVector2) | 点乘 |
| static FVector2 Lerp(FVector2, FVector2, T) | 线性插值 |
| static FVector2 ClampLerp(FVector2, FVector2, T) | 钳制线性插值 |
| static FVector2 ClampMagnitude(FVector2, T) | 限制向量长度 |
| static FVector2 Max(FVector2, FVector2) | 各分量取最大值 |
| static FVector2 Min(FVector2, FVector2) | 各分量取最小值 |
| static FVector2 Normalize(FVector2) | 归一化 |
| static FVector2 Reflect(FVector2, FVector2) | 反射 |
| static FVector2 Scale(FVector2, FVector2) | 缩放 |
| static FVector2 MoveTowards(FVector2, FVector, T) | 移向目标 |
| static FVector2 Perpendicular(FVector2) | 垂直向量（仅2D） |
| static FVector2 Rotate(FVector2, T) | 旋转向量（仅2D） |
| static T SignedAngle(FVector2, FVector2) | 带符号角度（仅2D） |

## FVector3

基于定点数的三维向量

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| FVector3(T, T, T) | 构造三维向量 |
| **属性** | |
| T Magnitude { get; } | 向量长度 |
| T SqrMagnitude { get; } | 向量长度平方 |
| FVector3 Normalized { get; } | 归一化向量 |
| **成员函数** | |
| void Normalize() | 归一化（原地） |
| **静态函数** | |
| static T Angle(FVector3, FVector3) | 两向量夹角（度） |
| static T Cross(FVector3, FVector3) | 叉乘 |
| static T Distance(FVector3, FVector3) | 两点距离 |
| static T Dot(FVector3, FVector3) | 点乘 |
| static FVector3 Lerp(FVector3, FVector3, T) | 线性插值 |
| static FVector3 ClampLerp(FVector3, FVector3, T) | 钳制线性插值 |
| static FVector3 ClampMagnitude(FVector3, T) | 限制向量长度 |
| static FVector3 Max(FVector3, FVector3) | 各分量取最大值 |
| static FVector3 Min(FVector3, FVector3) | 各分量取最小值 |
| static FVector3 Normalize(FVector3) | 归一化 |
| static FVector3 Reflect(FVector3, FVector3) | 反射 |
| static FVector3 Scale(FVector3, FVector3) | 缩放 |
| static FVector3 MoveTowards(FVector3, FVector3, T) | 移向目标 |
| static FVector3 Slerp(FVector3, FVector3, T) | 球面插值（仅3D） |
| static FVector3 ClampSlerp(FVector3, FVector3, T) | 钳制球面插值（仅3D） |
| static T SignedAngle(FVector3, FVector3, FVector3) | 带符号角度（仅3D） |
| static void OrthoNormalize(ref FVector3, ref FVector3) | 正交归一化（仅3D） |
| static void OrthoNormalize(ref FVector3, ref FVector3, ref FVector3) | 正交归一化（仅3D） |
| static FVector3 Project(FVector3, FVector3) | 投影（仅3D） |
| static FVector3 ProjectOnPlane(FVector3, FVector3) | 平面投影（仅3D） |
| static FVector3 RotateTowards(FVector3, FVector3, T, T) | 旋转到目标（仅3D） |

## FMatrix3x3

基于定点数的 3x3 矩阵

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| FMatrix3x3(T, T, T, T, T, T, T, T, T) | 构造3x3矩阵 |
| **静态属性** | |
| static FMatrix3x3 Zero { get; } | 零矩阵 |
| static FMatrix3x3 Identity { get; } | 单位矩阵 |
| **静态函数** | |
| static T Determinant(FMatrix3x3) | 行列式 |
| static FMatrix3x3 Inverse(FMatrix3x3) | 逆矩阵 |
| static FMatrix3x3 Transpose(FMatrix3x3) | 转置矩阵 |
| static FMatrix3x3 Translate(T, T) | 平移矩阵（3x3） |
| static FMatrix3x3 Rotate(T) | 旋转矩阵（3x3） |
| static FMatrix3x3 Scale(T, T) | 缩放矩阵（3x3） |
| static FMatrix3x3 TR(T, T, T) | 平移旋转矩阵（3x3） |
| static FMatrix3x3 TRS(T tx, T ty, T rad, T sx, T sy) | 平移旋转缩放矩阵（3x3） |
| static FVector2 MultiplyPoint(FVector2 p, FMatrix3x3) | 点与3x3矩阵相乘 |
| static FVector2 MultiplyVector(FVector2, FMatrix3x3) | 向量与3x3矩阵相乘 |
| static FMatrix3x3 LookAt(FVector3, FVector3) | 观察矩阵（3x3） |
| static FMatrix3x3 AngleAxis(T, FVector3) | 绕轴旋转矩阵（3x3） |

## FMatrix4x4

基于定点数的 4x4 矩阵

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| FMatrix4x4(T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) | 构造4x4矩阵 |
| **静态属性** | |
| static FMatrix4x4 Zero { get; } | 零矩阵 |
| static FMatrix4x4 Identity { get; } | 单位矩阵 |
| **静态函数** | |
| static T Determinant(FMatrix4x4) | 行列式 |
| static FMatrix4x4 Inverse(FMatrix4x4) | 逆矩阵 |
| static FMatrix4x4 Transpose(FMatrix4x4) | 转置矩阵 |
| static FMatrix4x4 Translate(T, T, T) | 平移矩阵（4x4） |
| static FMatrix4x4 Translate(FVector3 translation) | 平移矩阵（4x4） |
| static FMatrix4x4 Rotate(FQuaternion<T> rotation) | 旋转矩阵（4x4） |
| static FMatrix4x4 RotateX/Y/Z(T) | 绕轴旋转矩阵（4x4） |
| static FMatrix4x4 Scale(T, T, T) | 缩放矩阵（4x4） |
| static FMatrix4x4 Scale(FVector3) | 缩放矩阵（4x4） |
| static FMatrix4x4 TRS(FVector3, FQuaternion, FVector3) | 平移旋转缩放矩阵（4x4） |
| static FVector3 MultiplyPoint(FVector3, FMatrix4x4) | 点与4x4矩阵相乘 |
| static FVector3 MultiplyVector(FVector3, FMatrix4x4) | 向量与4x4矩阵相乘 |
| static FMatrix4x4 LookAt(FVector3, FVector3, FVector3) | 观察矩阵（4x4） |
| static FMatrix4x4 Ortho(T, T, T, T, T, T) | 正交投影矩阵（4x4） |
| static FMatrix4x4 Perspective(T, T, T, T) | 透视投影矩阵（4x4） |
| static FMatrix4x4 AngleAxis(T, FVector3) | 绕轴旋转矩阵（4x4） |


## FQuaternion

基于定点数的四元数

| 函数 | 说明 |
|----------|----------|
| **构造函数** | |
| FQuaternion(T, T, T, T) | 构造四元数 |
| **成员函数** | |
| void Normalize() | 归一化（原地） |
| void Set(T, T, T, T) | 设置分量 |
| **静态属性** | |
| static FQuaternion Identity { get; } | 单位四元数 |
| **静态函数** | |
| static T Angle(FQuaternion, FQuaternion) | 两四元数间角度 |
| static FQuaternion AngleAxis(T, FVector3) | 绕轴旋转四元数 |
| static FQuaternion Conjugate(FQuaternion) | 共轭四元数 |
| static T Dot(FQuaternion, FQuaternion) | 点乘 |
| static FQuaternion Euler(T, T, T) | 从欧拉角创建 |
| static FQuaternion Euler(FVector3 eulerAngles) | 从欧拉角向量创建 |
| static FQuaternion Inverse(FQuaternion) | 逆四元数 |
| static FQuaternion Lerp(FQuaternion, FQuaternion, T) | 线性插值 |
| static FQuaternion ClampLerp(FQuaternion, FQuaternion, T) | 钳制线性插值 |
| static FQuaternion Slerp(FQuaternion, FQuaternion, T) | 球面插值 |
| static FQuaternion ClampSlerp(FQuaternion, FQuaternion, T) | 钳制球面插值 |
| static FQuaternion Normalize(FQuaternion) | 归一化 |
| static FQuaternion LookRotation(FVector3) | 朝向指定方向 |
| static FQuaternion LookRotation(FVector3, FVector3) | 朝向指定方向（含up） |
| static FQuaternion FromToRotation(FVector3, FVector3) | 从from到to的旋转 |
| static FQuaternion RotateTowards(FQuaternion, FQuaternion, T) | 受控转向 |
| static FQuaternion FromMatrix(FMatrix3x3) | 从3x3矩阵构造 |
| static FMatrix4x4 ToMatrix(FQuaternion) | 转换为4x4矩阵 |

## FRandom

基于定点数的随机数生成器

| 函数 | 说明 |
|----------|----------|
| **成员函数** | |
| T Next<T>() | $[0, 1)$ 区间随机数 |
| T Next<T>(int, int) | $[min, max)$ 区间随机整数 |
| T Next<T>(T, T) | $[min, max)$ 区间随机定点数 |
| **静态属性** | |
| static FRandom Shared { get; } | 全局共享实例 |
